import http from '@/utils/http/request'

interface params {
  ifall?: boolean
  iftodo?: boolean
}
export const getNotify = (data: params) => {
  return http({
    url: '/OA/SMNotifyBoard/home-notifyboard-todo',
    method: 'get',
    data
  })
}
