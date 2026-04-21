import axios from 'axios'
import type { BidCalculation } from '../types/bid'

const httpClient = axios.create({
  baseURL: '/api',
  headers: { 'Content-Type': 'application/json' },
})

export async function calculateBid(
  vehiclePrice: number,
  vehicleType: string
): Promise<BidCalculation> {
  const { data } = await httpClient.get<BidCalculation>('/bid/calculate', {
    params: { vehiclePrice, vehicleType },
  })
  return data
}
