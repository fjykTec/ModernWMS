import http from '@/utils/http/request'
import { OwnerOfCargoVO } from '@/types/Base/OwnerOfCargo'
import { PageConfigProps } from '@/types/System/Form'

// Get all
export const getOwnerOfCargoAll = () => http({
    url: '/goodsowner/all',
    method: 'get'
  })

// Get data by page
export const getOwnerOfCargoByPage = (data: PageConfigProps) => http({
    url: '/goodsowner/list',
    method: 'post',
    data
  })

// Add a new form
export const addOwnerOfCargo = (data: OwnerOfCargoVO) => http({
    url: '/goodsowner',
    method: 'post',
    data
  })

// Update form
export const updateOwnerOfCargo = (data: OwnerOfCargoVO) => http({
    url: '/goodsowner',
    method: 'put',
    data
  })

// Delete form
export const deleteOwnerOfCargo = (id: number) => http({
    url: '/goodsowner',
    method: 'delete',
    params: {
      id
    }
  })

// import form
export const excelImport = (data: OwnerOfCargoVO[]) => http({
    url: '/goodsowner/excel',
    method: 'post',
    data
  })
