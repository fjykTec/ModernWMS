import { UniformFileNaming } from '../System/Form'

export interface WarehouseProcessingVO extends UniformFileNaming {
  id: number
  job_code: string
  job_type: boolean
  process_status: boolean
  processor: string
  process_time: string
  source_detail_list?: Array<WarehouseProcessingDetailVO>
  target_detail_list?: Array<WarehouseProcessingDetailVO>
  adjust_status?: boolean
  detailList?: Array<any>
}

export interface WarehouseProcessingDetailVO extends UniformFileNaming {
  id: number
  stock_process_id: number
  sku_id: number
  goods_owner_id: number
  goods_location_id: number
  qty: number | string
  is_source: boolean
  spu_code: string
  spu_name: string
  sku_code: string
  unit: string
  is_update_stock: boolean
  qty_available?: number
  location_name?: string
}
