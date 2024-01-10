import { UniformFileNaming } from '../System/Form'

export interface WarehouseVO extends UniformFileNaming {
  id?: number
  warehouse_name: string
  city: string
  address: string
  contact_tel?: string
  email?: string
  manager?: string
  is_valid?: boolean
}

export interface WarehouseAreaVO extends UniformFileNaming {
  id: number
  warehouse_id?: number
  parent_id?: number
  warehouse_name: string
  area_name: string
  area_property: AreaProperty
  is_valid: boolean
}

export interface GoodsLocationVO extends UniformFileNaming {
  id: number
  warehouse_id?: number
  warehouse_area_id?: number
  warehouse_name: string
  warehouse_area_name: string
  warehouse_area_property?: number
  location_name: string
  location_length?: number
  location_width?: number
  location_heigth?: number
  location_volume?: number
  location_load?: number
  roadway_number?: string
  shelf_number?: string
  layer_number?: string
  tag_number?: string
  is_valid: boolean
}

export enum AreaProperty {
  'picking_area' = 1,
  'stocking_area' = 2,
  'receiving_area' = 3,
  'return_area' = 4,
  'defective_area' = 5,
  'inventory_area' = 6
}
