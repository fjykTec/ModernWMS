import { UniformFileNaming } from '../System/Form'

export interface WarehouseFreezeVO extends UniformFileNaming {
  id: number
  job_code: string
  job_type: boolean
  sku_id: number
  goods_owner_id: number
  goods_location_id: number
  handler: string
  handle_time: string
  warehouse_name: string
  location_name: string
  spu_code: string
  spu_name: string
  sku_code: string
  series_number: string
}
