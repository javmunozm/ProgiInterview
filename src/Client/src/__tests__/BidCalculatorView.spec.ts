import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import { createPinia } from 'pinia'
import { createRouter, createMemoryHistory } from 'vue-router'
import BidCalculatorView from '../views/BidCalculatorView.vue'

function createTestRouter() {
  return createRouter({
    history: createMemoryHistory(),
    routes: [{ path: '/', component: BidCalculatorView }],
  })
}

describe('BidCalculatorView', () => {
  it('renders the form with price input and type selector', () => {
    const wrapper = mount(BidCalculatorView, {
      global: {
        plugins: [createPinia(), createTestRouter()],
      },
    })

    expect(wrapper.find('input#price').exists()).toBe(true)
    expect(wrapper.find('select#type').exists()).toBe(true)
    expect(wrapper.text()).toContain('Bid Calculation Tool')
  })

  it('has Common as default vehicle type', () => {
    const wrapper = mount(BidCalculatorView, {
      global: {
        plugins: [createPinia(), createTestRouter()],
      },
    })

    const select = wrapper.find('select#type')
    expect((select.element as HTMLSelectElement).value).toBe('Common')
  })

  it('shows both Common and Luxury options', () => {
    const wrapper = mount(BidCalculatorView, {
      global: {
        plugins: [createPinia(), createTestRouter()],
      },
    })

    const options = wrapper.findAll('select#type option')
    expect(options).toHaveLength(2)
    expect(options[0].text()).toBe('Common')
    expect(options[1].text()).toBe('Luxury')
  })
})
