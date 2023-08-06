import http from '@/utils/http/request'
import { RoleMenuVO } from '@/types/Base/RoleMenu'

// Get user authority
export const getUserAuthority = (userrole_id: number) => http({
    url: '/rolemenu/authority',
    method: 'get',
    params: {
      userrole_id
    }
  })

// Get all
export const getRoleMenuAll = () => http({
    url: '/rolemenu/all',
    method: 'get'
  })

// Get all menu setting
export const getMenus = () => http({
    url: '/rolemenu/menus',
    method: 'get'
  })

// Get form by id
export const getRoleMenuById = (userrole_id: number) => http({
    url: '/rolemenu',
    method: 'get',
    params: {
      userrole_id
    }
  })

// Get form by id
export const addRoleMenu = (data: RoleMenuVO) => http({
    url: '/rolemenu',
    method: 'post',
    data
  })

// Update form
export const updateRoleMenu = (data: RoleMenuVO) => http({
    url: '/rolemenu',
    method: 'put',
    data
  })

// Delete form
export const deleteRoleMenu = (userrole_id: number) => http({
    url: '/rolemenu',
    method: 'delete',
    params: {
      userrole_id
    }
  })
