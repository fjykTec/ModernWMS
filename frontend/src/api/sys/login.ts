import http from '@/utils/http/request'
import { LoginParams } from '../../types/System/UserModel'

export const login = (data: LoginParams) => http({
    url: '/login',
    method: 'post',
    data
  })

// Get user menu dynamically
export const getUserAuthority = (userrole_id: number) => http({
    url: '/rolemenu/authority',
    method: 'get',
    params: {
      userrole_id
    }
  })
