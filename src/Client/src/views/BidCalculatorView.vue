<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import { calculateBid } from '../api/bid-api'
import type { BidCalculation } from '../types/bid'
import VehiclePriceInput from '../components/VehiclePriceInput.vue'

const vehiclePrice = ref<number | null>(null)
const vehicleType = ref<string>('Common')
const result = ref<BidCalculation | null>(null)
const errors = ref<string[]>([])
const priceError = ref<string>('')
const loading = ref(false)

let debounceTimer: ReturnType<typeof setTimeout> | null = null
let requestId = 0

const currency = new Intl.NumberFormat('en-US', {
  style: 'currency',
  currency: 'USD',
  minimumFractionDigits: 2,
  maximumFractionDigits: 2,
})

const hasInput = computed(
  () => vehiclePrice.value !== null && !isNaN(vehiclePrice.value) && vehiclePrice.value > 0
)

async function compute() {
  if (!hasInput.value) {
    result.value = null
    errors.value = []
    priceError.value = ''
    loading.value = false
    return
  }

  const currentRequest = ++requestId
  loading.value = true
  errors.value = []
  priceError.value = ''

  try {
    const response = await calculateBid(vehiclePrice.value as number, vehicleType.value)
    if (currentRequest !== requestId) return
    result.value = response
  } catch (e: any) {
    if (currentRequest !== requestId) return
    const payload = e?.response?.data?.errors
    const priceMessages: string[] = []
    const generalMessages: string[] = []

    if (Array.isArray(payload) && payload.length > 0) {
      for (const item of payload) {
        const message = item.errorMessage ?? item.ErrorMessage ?? String(item)
        const property = item.propertyName ?? item.PropertyName ?? ''
        if (/vehiclePrice/i.test(property)) priceMessages.push(message)
        else generalMessages.push(message)
      }
    } else {
      generalMessages.push('Calculation failed. Please try again.')
    }

    priceError.value = priceMessages[0] ?? ''
    errors.value = generalMessages
    result.value = null
  } finally {
    if (currentRequest === requestId) loading.value = false
  }
}

function onInvalidPriceKey() {
  priceError.value = 'Only digits and a decimal point are allowed.'
}

watch([vehiclePrice, vehicleType], () => {
  if (debounceTimer) clearTimeout(debounceTimer)
  debounceTimer = setTimeout(compute, 300)
})

function getFeeLabel(name: string): string {
  const labels: Record<string, string> = {
    Basic: 'Basic Buyer Fee',
    Special: "Seller's Special Fee",
    Association: 'Association Fee',
    Storage: 'Storage Fee',
  }
  return labels[name] ?? name
}

function formatCurrency(value: number): string {
  return currency.format(value)
}
</script>

<template>
  <main class="page">
    <section class="calculator">
      <header class="header">
        <span class="badge">Bid Tool</span>
        <h1>Bid Calculation Tool</h1>
        <p class="subtitle">Estimate the all-in cost of a vehicle purchased at auction.</p>
      </header>

      <div class="form" role="group" aria-label="Vehicle inputs">
        <VehiclePriceInput
          v-model="vehiclePrice"
          :error="priceError"
          @invalid-input="onInvalidPriceKey"
        />

        <div class="field">
          <label for="type">Vehicle Type</label>
          <div class="select-wrap">
            <select id="type" v-model="vehicleType">
              <option value="Common">Common</option>
              <option value="Luxury">Luxury</option>
            </select>
            <span class="chev" aria-hidden="true">▾</span>
          </div>
        </div>
      </div>

      <transition name="fade">
        <div v-if="errors.length > 0" class="error" role="alert">
          <span class="error-icon" aria-hidden="true">!</span>
          <ul v-if="errors.length > 1">
            <li v-for="(message, i) in errors" :key="i">{{ message }}</li>
          </ul>
          <span v-else>{{ errors[0] }}</span>
        </div>
      </transition>

      <transition name="fade">
        <div v-if="loading && !result" class="loading">
          <span class="spinner" aria-hidden="true"></span>
          <span>Calculating…</span>
        </div>
      </transition>

      <transition name="fade">
        <div v-if="result" class="results" :class="{ 'is-refreshing': loading }">
          <div class="results-head">
            <h2>Summary</h2>
            <span class="pill">{{ result.vehicleType }}</span>
          </div>

          <table class="summary-table">
            <thead>
              <tr>
                <th scope="col">Line item</th>
                <th scope="col" class="amount-col">Amount</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>Vehicle Price</td>
                <td class="amount">{{ formatCurrency(result.vehiclePrice) }}</td>
              </tr>
              <tr v-for="fee in result.fees" :key="fee.name">
                <td>{{ getFeeLabel(fee.name) }}</td>
                <td class="amount">{{ formatCurrency(fee.amount) }}</td>
              </tr>
            </tbody>
            <tfoot>
              <tr>
                <td>Total</td>
                <td class="amount total">{{ formatCurrency(result.totalPrice) }}</td>
              </tr>
            </tfoot>
          </table>

          <div class="results-head breakdown-head">
            <h2>How each fee is calculated</h2>
          </div>

          <table class="breakdown-table" data-testid="fee-breakdown">
            <thead>
              <tr>
                <th scope="col">Segment</th>
                <th scope="col">Rule</th>
                <th scope="col">Calculation</th>
                <th scope="col" class="amount-col">Amount</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="fee in result.fees" :key="`breakdown-${fee.name}`">
                <td class="segment">{{ getFeeLabel(fee.name) }}</td>
                <td class="rule">{{ fee.rule }}</td>
                <td class="formula"><code>{{ fee.calculation }}</code></td>
                <td class="amount">{{ formatCurrency(fee.amount) }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </transition>

      <transition name="fade">
        <div v-if="!hasInput && !result && errors.length === 0" class="empty">
          Enter a vehicle price above to see the fee breakdown.
        </div>
      </transition>
    </section>
  </main>
</template>
