import http from '@/utils/http/request'
import { PageConfigProps } from '@/types/System/Form'
import { WarehouseTakingVO } from '@/types/WarehouseWorking/WarehouseTaking'

// Get list
export const getStockTakingList = (data: PageConfigProps) => http({
    url: '/stocktaking/list',
    method: 'post',
    data
  })

// Get one
export const getStockTakingOne = (id: number) => http({
    url: '/stocktaking',
    method: 'get',
    params: {
      id
    }
  })

// Add a new form
export const addStockTaking = (data: WarehouseTakingVO) => http({
    url: '/stocktaking',
    method: 'post',
    data
  })

// Delete form
export const deleteStockTaking = (id: number) => http({
    url: '/stocktaking',
    method: 'delete',
    params: {
      id
    }
  })

// Confirm stock taking
export const confirmStockTaking = (data: WarehouseTakingVO) => http({
    url: '/stocktaking',
    method: 'put',
    data: {
      id: data.id,
      counted_qty: data.counted_qty
    }
  })

// Confirm stock adjust
export const confirmAdjustment = (id: number) => http({
    url: '/stocktaking/adjustment-confirm',
    method: 'put',
    params: {
      id
    }
  })
