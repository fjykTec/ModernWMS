import http from '@/utils/http/request'
import { LoginParams } from './model/userModel'

export const login = (data: LoginParams) => {
  return http({
    url: '/login',
    method: 'post',
    data
  })
}
