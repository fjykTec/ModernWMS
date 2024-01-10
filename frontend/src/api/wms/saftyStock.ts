import http from '@/utils/http/request'
import { PageConfigProps } from '@/types/System/Form'

export const list = (data: PageConfigProps) => http({
    url: '/stock/safety-list',
    method: 'post',
    data
  })
