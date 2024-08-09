import { UniformFileNaming } from '../System/Form'

export interface WarehouseMoveVO extends UniformFileNaming {
  id: number
  job_code: string
  move_status: MoveStatus
  sku_id: number
  orig_goods_location_id: number
  dest_googs_location_id: number
  qty: number
  goods_owner_id: number
  handler: string
  handle_time: string
  orig_goods_warehouse: string
  orig_goods_location_name: string
  dest_googs_warehouse: string
  dest_googs_location_name: string
  spu_code: string
  spu_name: string
  sku_code: string
  sku_name: string
  series_number: string,
  price: number,
  expiry_date: string,
  putaway_date: string
}

export enum MoveStatus {
  UNADJUST = 0,
  ADJUSTED = 1
}
