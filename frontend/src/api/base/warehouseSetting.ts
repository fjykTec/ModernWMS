import http from '@/utils/http/request'
import { PageConfigProps } from '@/types/System/Form'
import { GoodsLocationVO, WarehouseAreaVO, WarehouseVO } from '@/types/Base/Warehouse'

/**
 * Warehouse Setting
 */
// Find Data by Pagination
export const getWarehouseList = (data: PageConfigProps) => http({
  url: '/warehouse/list',
  method: 'post',
  data
})

// Add a new warehouse
export const addWarehouse = (data: WarehouseVO) => http({
  url: '/warehouse',
  method: 'post',
  data
})

// Update warehouse
export const updateWarehouse = (data: WarehouseVO) => http({
  url: '/warehouse',
  method: 'put',
  data
})

// Delete warehouse
export const deleteWarehouse = (id: number) => http({
  url: '/warehouse',
  method: 'delete',
  params: {
    id
  }
})
// Get warehouse information
export const getWarehouse = (id: number) => http({
  url: '/warehouse',
  method: 'get',
  params: {
    id
  }
})
// Get the combobox item with warehouse
export const getWarehouseSelect = () => http({
  url: '/warehouse/select-item',
  method: 'get'
})

// Excel Import
export const excelImport = (data: Array<WarehouseVO>) => http({
  url: '/warehouse/excel',
  method: 'post',
  data
})

// Get the combobox item with warehouse area
export const getWarehouseAreaSelect = (warehouse_id: number) => http({
  url: '/warehousearea/all',
  method: 'get',
  params: {
    warehouse_id
  }
})

/**
 * Warehouse Area Setting
 */
// Find Data by Pagination
export const getWarehouseAreaList = (data: PageConfigProps) => http({
  url: '/warehousearea/list',
  method: 'post',
  data
})

// Add a new warehousearea
export const addWarehouseArea = (data: WarehouseAreaVO) => http({
  url: '/warehousearea',
  method: 'post',
  data
})

// Update warehousearea
export const updateWarehouseArea = (data: WarehouseAreaVO) => http({
  url: '/warehousearea',
  method: 'put',
  data
})

// Delete warehousearea
export const deleteWarehouseArea = (id: number) => http({
  url: '/warehousearea',
  method: 'delete',
  params: {
    id
  }
})

/**
 * Goods Location Setting
 */
// Find Data by Pagination
export const getGoodsLocationList = (data: PageConfigProps) => http({
  url: '/goodslocation/list',
  method: 'post',
  data
})

// Add a new goodslocation
export const addGoodsLocation = (data: GoodsLocationVO) => http({
  url: '/goodslocation',
  method: 'post',
  data
})
// Get all library location information
export const getGoodsLocation = () => http({
  url: '/goodslocation/all',
  method: 'get'
})

// Update goodslocation
export const updateGoodsLocation = (data: GoodsLocationVO) => http({
  url: '/goodslocation',
  method: 'put',
  data
})

// Delete goodslocation
export const deleteGoodsLocation = (id: number) => http({
  url: '/goodslocation',
  method: 'delete',
  params: {
    id
  }
})

export const getWarehouseProduct = (data) => http({
  url: '/stock/qrcode-list',
  method: 'post',
  data
})
