<script setup lang="ts">
import { computed, ref, watch } from 'vue'

interface Props {
  modelValue: number | null
  error?: string
  label?: string
  id?: string
  placeholder?: string
}

const props = withDefaults(defineProps<Props>(), {
  error: '',
  label: 'Vehicle Base Price',
  id: 'price',
  placeholder: '0.00',
})

const emit = defineEmits<{
  'update:modelValue': [value: number | null]
  'invalid-input': [rejected: string]
}>()

const VALID_PATTERN = /^\d*(\.\d*)?$/
const ALLOWED_KEY_PATTERN = /^[0-9.]$/
const ALLOWED_CONTROL_KEYS = new Set([
  'Backspace', 'Delete', 'Tab', 'Escape', 'Enter', 'Home', 'End',
  'ArrowLeft', 'ArrowRight', 'ArrowUp', 'ArrowDown',
])

const inputEl = ref<HTMLInputElement | null>(null)
const displayText = ref(props.modelValue === null ? '' : String(props.modelValue))
const isComposing = ref(false)

watch(
  () => props.modelValue,
  (next) => {
    const incoming = next === null || Number.isNaN(next) ? '' : String(next)
    const currentAsNumber = displayText.value === '' ? null : Number(displayText.value)
    if (currentAsNumber !== next) displayText.value = incoming
  }
)

const errorId = computed(() => `${props.id}-error`)
const hasError = computed(() => props.error.length > 0)

function onKeydown(event: KeyboardEvent) {
  if (event.ctrlKey || event.metaKey || event.altKey) return
  if (ALLOWED_CONTROL_KEYS.has(event.key)) return
  if (event.key === 'Dead' || event.key.length !== 1) {
    event.preventDefault()
    emit('invalid-input', event.key)
    return
  }
  if (!ALLOWED_KEY_PATTERN.test(event.key)) {
    event.preventDefault()
    emit('invalid-input', event.key)
    return
  }
  if (event.key === '.' && (event.target as HTMLInputElement).value.includes('.')) {
    event.preventDefault()
    emit('invalid-input', event.key)
  }
}

function onCompositionStart() {
  isComposing.value = true
}

function onCompositionEnd(event: CompositionEvent) {
  isComposing.value = false
  sanitize((event.target as HTMLInputElement).value)
}

function onBeforeInput(event: InputEvent) {
  if (event.data === null || event.data === '') return
  if (!VALID_PATTERN.test(event.data)) {
    event.preventDefault()
    emit('invalid-input', event.data)
  }
}

function onInput(event: Event) {
  if (isComposing.value) return
  sanitize((event.target as HTMLInputElement).value)
}

function onPaste(event: ClipboardEvent) {
  const text = event.clipboardData?.getData('text') ?? ''
  if (!VALID_PATTERN.test(text)) {
    event.preventDefault()
    emit('invalid-input', text)
  }
}

function sanitize(raw: string) {
  if (VALID_PATTERN.test(raw)) {
    displayText.value = raw
    emit('update:modelValue', raw === '' ? null : Number(raw))
    return
  }

  emit('invalid-input', raw)
  if (inputEl.value) inputEl.value.value = displayText.value
}
</script>

<template>
  <div class="price-input">
    <label :for="id">{{ label }}</label>
    <div class="input-wrap" :class="{ 'has-error': hasError }">
      <span class="prefix" aria-hidden="true">$</span>
      <input
        :id="id"
        ref="inputEl"
        :value="displayText"
        type="text"
        inputmode="decimal"
        :placeholder="placeholder"
        autocomplete="off"
        spellcheck="false"
        :aria-invalid="hasError"
        :aria-describedby="hasError ? errorId : undefined"
        @keydown="onKeydown"
        @beforeinput="onBeforeInput"
        @input="onInput"
        @paste="onPaste"
        @compositionstart="onCompositionStart"
        @compositionend="onCompositionEnd"
      />
    </div>
    <p v-if="hasError" :id="errorId" class="field-error" role="alert">{{ error }}</p>
  </div>
</template>
