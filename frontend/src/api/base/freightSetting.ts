import http from '@/utils/http/request'
import { PageConfigProps } from '@/types/System/Form'
import { FreightVO } from '@/types/Base/Freight'

// Find Data by Pagination
export const getFreightList = (data: PageConfigProps) => http({
    url: '/freightfee/list',
    method: 'post',
    data
  })

// get data all
export const getFreightAll = () => http({
    url: '/freightfee/all',
    method: 'get',
    hideLoading: true
  })

// Add a new freightfee
export const addFreight = (data: FreightVO) => http({
    url: '/freightfee',
    method: 'post',
    data
  })

// Update freightfee
export const updateFreight = (data: FreightVO) => http({
    url: '/freightfee',
    method: 'put',
    data
  })

// Delete freightfee
export const deleteFreight = (id: number) => http({
    url: '/freightfee',
    method: 'delete',
    params: {
      id
    }
  })

// Excel Import
export const excelImport = (data: Array<FreightVO>) => http({
    url: '/freightfee/excel',
    method: 'post',
    data
  })
