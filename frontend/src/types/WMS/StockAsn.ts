import { UniformFileNaming } from '../System/Form'

export interface StockAsnVO extends UniformFileNaming {
  id: number
  asn_no: string
  asn_status: number
  spu_id: number
  spu_code: string
  spu_name: string
  sku_id: number
  sku_code: string
  sku_name: string
  origin: string
  length_unit: number
  volume_unit: number
  weight_unit: number
  asn_qty: number
  actual_qty: number
  sorted_qty: number
  shortage_qty: number
  more_qty: number
  damage_qty: number
  weight: number
  volume: number
  supplier_id: number
  supplier_name: string
  goods_owner_id: number
  goods_owner_name: string
  is_valid: boolean
}

export interface PutawayVo {
  asn_id: number
  goods_location_id: number
  putaway_qty: number
  location_name: string
}

export interface SortingVo {
  asn_id: number
  sorted_qty: number
}

export interface SkuInfoVo {
  spu_id: number
  spu_code: string
  spu_name: string
  category_id: number
  category_name: string
  spu_description: string
  bar_code: string
  supplier_id: number
  supplier_name: string
  brand: string
  origin: string
  length_unit: number
  volume_unit: number
  weight_unit: number
  sku_id: number
  sku_code: string
  sku_name: string
  weight: number
  lenght: number
  width: number
  height: number
  volume: number
  unit: string
  cost: number
  price: number
}
