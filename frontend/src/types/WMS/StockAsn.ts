/*
 * @Author: yanguoping 125722066@qq.com
 * @Date: 2024-03-27 10:59:36
 * @LastEditors: yanguoping 125722066@qq.com
 * @LastEditTime: 2024-05-08 17:20:34
 * @FilePath: \frontend\src\types\WMS\StockAsn.ts
 * @Description: 这是默认设置,请设置`customMade`, 打开koroFileHeader查看配置 进行设置: https://github.com/OBKoro1/koro1FileHeader/wiki/%E9%85%8D%E7%BD%AE
 */
import { UniformFileNaming } from '../System/Form'

export interface StockAsnVO extends UniformFileNaming {
  id?: number
  asn_no?: string
  spu_id?: number
  supplier_name?: string
  supplier_id?: number
  is_valid?: boolean
  spu_code?: string
  spu_name?: string
  sku_code?: string
  sku_name?: string
  origin?: string
  sku_id?: number
  asn_qty?: number
  price?: number
  asn_batch?: string
  estimated_arrival_time?: string
  asn_status?: number
  weight?: number
  volume?: number
  goods_owner_id?: number
  goods_owner_name?: string
  creator?: string
  create_time?: string
  last_update_time?: string
  detailList: StockAsnDetailVO[]
}

export interface StockAsnDetailVO {
  id?: number
  asnmaster_id?: number
  asn_status?: number
  spu_id?: number
  spu_code?: string
  spu_name?: string
  sku_id?: number
  sku_code?: string
  sku_name?: string
  origin?: string
  length_unit?: number
  volume_unit?: number
  weight_unit?: number
  asn_qty?: number
  price?: number
  actual_qty?: number
  weight?: number
  volume?: number
  supplier_id?: number
  supplier_name?: string
  is_valid?: boolean
  is_check?; boolean
}

export interface PutawayVo {
  asn_id: number
  goods_location_id: number
  putaway_qty: number
  location_name: string
}

export interface SortingVo {
  asn_id: number
  sorted_qty: number,
  expiry_date: string,
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

export interface UpdateSortingVo {
  id: number
  asn_id: number
  sorted_qty: number
  series_number: string
  creator: string
  create_time: string
  last_update_time: string
  is_valid: boolean
  tenant_id: number
}
