import http from '@/utils/http/request'
import { UserRoleVO } from '@/types/Base/UserRoleSetting'

// Get all
export const getUserRoleAll = () => http({
    url: '/userrole/all',
    method: 'get'
  })

// Add a new form
export const addUserRole = (data: UserRoleVO) => http({
    url: '/userrole',
    method: 'post',
    data
  })

// Update form
export const updateUserRole = (data: UserRoleVO) => http({
    url: '/userrole',
    method: 'put',
    data
  })

// Delete form
export const deleteUserRole = (id: number) => http({
    url: '/userrole',
    method: 'delete',
    params: {
      id
    }
  })
