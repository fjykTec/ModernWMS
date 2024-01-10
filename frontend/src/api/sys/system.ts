import http from '@/utils/http/request'
import { PageConfigProps } from '@/types/System/Form'

// Get Log
export const getActionLog = (data: PageConfigProps) => http({
    url: '/actionlog/list',
    method: 'post',
    data
  })
