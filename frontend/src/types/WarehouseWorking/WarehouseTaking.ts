import { UniformFileNaming } from '../System/Form'

export interface WarehouseTakingVO extends UniformFileNaming {
  id: number
  job_code: string
  job_status: boolean
  sku_id: number
  goods_owner_id: number
  goods_location_id: number
  book_qty: number
  counted_qty: number
  difference_qty: number
  spu_code: string
  spu_name: string
  sku_code: string
  series_number: string
  warehouse_name: string
  location_name: string
  adjust_status: boolean
  handler: string
  handle_time: string,
  price: number,
  expiry_date: string,
  putaway_date:string
}
