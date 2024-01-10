import http from '@/utils/http/request'

interface ListProps {
  pageIndex: number
  pageSize: number
  spu_code?: string
  spu_name?: string
  sku_code?: string
  sku_name?: string
  warehouse_name?: string
  customer_name?: string
  delivery_date_from?: string
  delivery_date_to?: string
}

export const list = (data: ListProps) => http({
    url: '/stock/delivery-list',
    method: 'post',
    data
  })
