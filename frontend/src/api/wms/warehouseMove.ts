import http from '@/utils/http/request'
import { PageConfigProps } from '@/types/System/Form'
import { WarehouseMoveVO } from '@/types/WarehouseWorking/WarehouseMove'

// Get list
export const getStockMoveList = (data: PageConfigProps) => http({
    url: '/stockmove/list',
    method: 'post',
    data
  })

// Get all
export const getStockMoveAll = () => http({
    url: '/stockmove/all',
    method: 'get'
  })

// Get one
export const getStockMoveOne = (id: number) => http({
    url: '/stockmove',
    method: 'get',
    params: {
      id
    }
  })

// Add a new form
export const addStockMove = (data: WarehouseMoveVO) => http({
    url: '/stockmove',
    method: 'post',
    data
  })

// Delete form
export const deleteStockMove = (id: number) => http({
    url: '/stockmove',
    method: 'delete',
    params: {
      id
    }
  })

// Confirm move
export const confirmMove = (id: number) => http({
    url: '/stockmove',
    method: 'put',
    params: {
      id
    }
  })
