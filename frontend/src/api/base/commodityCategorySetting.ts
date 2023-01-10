import http from '@/utils/http/request'
import { CategoryVO } from '@/types/Base/CommodityCategorySetting'

// Get all
export const getCategoryAll = () => http({
    url: '/category/all',
    method: 'get'
  })

// Add a new form
export const addCategory = (data: CategoryVO) => http({
    url: '/category',
    method: 'post',
    data
  })

// Update form
export const updateCategory = (data: CategoryVO) => http({
    url: '/category',
    method: 'put',
    data
  })

// Delete form
export const deleteCategory = (id: number) => http({
    url: '/category',
    method: 'delete',
    params: {
      id
    }
  })
