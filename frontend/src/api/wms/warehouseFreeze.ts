import http from '@/utils/http/request'
import { PageConfigProps } from '@/types/System/Form'
import { WarehouseFreezeVO } from '@/types/WarehouseWorking/WarehouseFreeze'

// Get list
export const getStockFreezeList = (data: PageConfigProps) => http({
    url: '/stockfreeze/list',
    method: 'post',
    data
  })

// Get all
export const getStockFreezeAll = () => http({
    url: '/stockfreeze/all',
    method: 'get'
  })

// Get one
export const getStockFreezeOne = (id: number) => http({
    url: '/stockfreeze',
    method: 'get',
    params: {
      id
    }
  })

// Add a new form
export const addStockFreeze = (data: WarehouseFreezeVO) => http({
    url: '/stockfreeze',
    method: 'post',
    data
  })

// Delete form
export const deleteStockFreeze = (id: number) => http({
    url: '/stockfreeze',
    method: 'delete',
    params: {
      id
    }
  })
