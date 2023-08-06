import http from '@/utils/http/request'
import { CompanyVO } from '@/types/Base/CompanySetting'

// Get all
export const getCompanyAll = () => http({
    url: '/company/all',
    method: 'get'
  })

// Add a new form
export const addCompany = (data: CompanyVO) => http({
    url: '/company',
    method: 'post',
    data
  })

// Update form
export const updateCompany = (data: CompanyVO) => http({
    url: '/company',
    method: 'put',
    data
  })

// Delete form
export const deleteCompany = (id: number) => http({
    url: '/company',
    method: 'delete',
    params: {
      id
    }
  })
