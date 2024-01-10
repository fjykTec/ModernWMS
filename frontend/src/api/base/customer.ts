import http from '@/utils/http/request'
import { PageConfigProps } from '@/types/System/Form'
import { CustomerVO, CustomerExcelVO } from '@/types/Base/Customer'

// Get all
export const getCustomerAll = () => http({
    url: '/customer/all',
    method: 'get'
  })

// Find Data by Pagination
export const getCustomerList = (data: PageConfigProps) => http({
    url: '/customer/list',
    method: 'post',
    data
  })

// Add a new form
export const addCustomer = (data: CustomerVO) => http({
    url: '/customer',
    method: 'post',
    data
  })

// Update form
export const updateCustomer = (data: CustomerVO) => http({
    url: '/customer',
    method: 'put',
    data
  })

// Delete form
export const deleteCustomer = (id: number) => http({
    url: '/customer',
    method: 'delete',
    params: {
      id
    }
  })

// Excel Import
export const excelImport = (data: Array<CustomerExcelVO>) => http({
    url: '/customer/excel',
    method: 'post',
    data
  })
