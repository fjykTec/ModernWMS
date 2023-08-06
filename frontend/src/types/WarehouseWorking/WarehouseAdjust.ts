import { UniformFileNaming } from '../System/Form'

export interface WarehouseAdjustVO extends UniformFileNaming {
  id: number
  job_type?: AdjustJobType
  job_code: string
  sku_id: number
  goods_owner_id: number
  goods_location_id: number
  qty: number
  is_update_stock: boolean
  source_table_id: number
  spu_code?: string
  spu_name?: string
  sku_code?: string
  warehouse_name?: string
  location_name?: string
}

export enum AdjustJobType {
  UNKNOW = 0,
  TAKE = 1,
  PROCESS_COMBINE = 2,
  PROCESS_SPLIT = 3,
  MOVE = 4
}
