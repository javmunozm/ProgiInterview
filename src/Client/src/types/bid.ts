export interface Fee {
  name: string
  amount: number
  rule: string
  calculation: string
}

export interface BidCalculation {
  vehiclePrice: number
  vehicleType: string
  fees: Fee[]
  totalPrice: number
}
