import { UniformFileNaming } from '../System/Form'

export interface DeliveryManagementVO extends UniformFileNaming {
  id: number
  dispatch_no?: string
  dispatch_status?: number
  qty?: number
  weight?: number
  volume?: number
  customer_id?: number
  customer_name?: string
  picked_qty?: number
  detailList: DeliveryManagementDetailListVO[]
}

export interface DeliveryManagementDetailListVO {
  id: number
  qty: number
  sku_id?: number
  spu_code?: string
  spu_name?: string
  sku_code?: string
  sku_name?: string
}

export interface DeliveryManagementDetailVO extends DeliveryManagementVO {
  id: number
  spu_code?: string
  spu_name?: string
  sku_code?: string
  unpackage_qty?: number
  unweighing_qty?: number
  spu_description?: string
  bar_code?: string
  qty?: number
  unpicked_qty?: number
  picked_qty?: number
  weight?: number
  volume?: number
  weight_unit?: number
  is_todo: boolean
}

export interface addRequestVO {
  customer_id: number
  customer_name: string
  sku_id: number
  qty: number
}

export interface ConfirmOrderVO {
  spu_code: string
  sku_code: string
  qty: number
  confirm: boolean
  pick_list: ConfirmOrderPickListVO[]
}

export interface ConfirmOrderPickListVO {
  warehouse_name: string
  location_name: string
  pick_qty: number
}

export interface PackageVO {
  id?: number
  dispatch_no?: string
  dispatch_status?: number
  package_qty?: number
  picked_qty?: number
}

export interface WeighVO {
  id?: number
  dispatch_no?: string
  dispatch_status?: number
  weighing_qty?: number
  weighing_weight?: number
  picked_qty?: number
}

export interface DeliveryVO {
  id?: number
  dispatch_no?: string
  dispatch_status?: number
  picked_qty?: number
}

export interface SignInVO {
  id?: number
  dispatch_no?: string
  dispatch_status?: number
  damage_qty?: number
}

export interface CancleOrderVO {
  dispatch_no?: string
  dispatch_status?: number
}

export interface SetCarrierVO {
  id: number
  dispatch_no: string
  dispatch_status: number
  freightfee_id: number
  carrier: string
  waybill_no: string
}

export interface ConfirmItem {
  id: number
  dispatch_no: string
  dispatch_status: number
  spu_name: string
  spu_code: string
  sku_code: string
  maxQty: number
  qty: number
  picked_qty?: number
  weight?: number
  weight_unit?: string
}

export interface DeliveryBatchAllocationVO {
  id: number
  dateFrom: string
  dateTo: string
  allocationRule: string
  is_valid: boolean
}
