import { createRouter, createWebHistory } from 'vue-router'
import BidCalculatorView from '../views/BidCalculatorView.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'calculator',
      component: BidCalculatorView,
    },
  ],
})

export default router
