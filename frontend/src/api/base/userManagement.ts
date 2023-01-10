import http from '@/utils/http/request'
import { PageConfigProps } from '@/types/System/Form'
import { UserVO, ChangePwdAPIParams, ImportVO } from '@/types/Base/UserManagement'

// Find Data by Pagination
export const getSelectItem = () => http({
    url: '/user/select-item',
    method: 'get'
  })

// Find Data by Pagination
export const getUserList = (data: PageConfigProps) => http({
    url: '/user/list',
    method: 'post',
    data
  })

// Add a new user
export const addUser = (data: UserVO) => http({
    url: '/user',
    method: 'post',
    data
  })

  // Register a new user
export const registerUser = (data: UserVO) => http({
    url: '/user/register',
    method: 'post',
    data
  })

// Update user
export const updateUser = (data: UserVO) => http({
    url: '/user',
    method: 'put',
    data
  })

// Delete user
export const deleteUser = (id: number) => http({
    url: '/user',
    method: 'delete',
    params: {
      id
    }
  })

// Reset password
export const resetPassword = (id_list: number[]) => http({
    url: '/user/reset-pwd',
    method: 'post',
    data: {
      id_list
    }
  })

// Change password
export const changePassword = (data: ChangePwdAPIParams) => http({
    url: '/user/change-pwd',
    method: 'post',
    data
  })

// Import form
export const excelImport = (data: ImportVO[]) => http({
    url: '/user/excel',
    method: 'post',
    data
  })
