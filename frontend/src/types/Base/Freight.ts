import { UniformFileNaming } from '../System/Form'

export interface FreightVO extends UniformFileNaming{
  id?: number
  carrier: string
  departure_city: string
  arrival_city: string
  price_per_weight: number
  price_per_volume: number
  min_payment: number
  is_valid?: boolean
}