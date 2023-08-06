import http from '@/utils/http/request'
import { PageConfigProps } from '@/types/System/Form'
import { WarehouseAdjustVO } from '@/types/WarehouseWorking/WarehouseAdjust'

// Get list
export const getStockAdjustList = (data: PageConfigProps) => http({
    url: '/stockadjust/list',
    method: 'post',
    data
  })

// Get all
export const getStockAdjustAll = () => http({
    url: '/stockadjust/all',
    method: 'get'
  })

// Get one
export const getStockAdjustOne = (id: number) => http({
    url: '/stockadjust',
    method: 'get',
    params: {
      id
    }
  })

// Add a new form
export const addStockAdjust = (data: WarehouseAdjustVO) => http({
    url: '/stockadjust',
    method: 'post',
    data
  })

// Delete form
export const deleteStockAdjust = (id: number) => http({
    url: '/stockadjust',
    method: 'delete',
    params: {
      id
    }
  })

  // Delete form
export const confirmStockAdjust = (id: number) => http({
    url: '/stockadjust/confirm',
    method: 'put',
    params: {
      id
    }
  })
