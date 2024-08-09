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
  stock_age_from?: string
  stock_age_to?: string
  expiry_date_from?: string
  expiry_date_to?: string
}
// Get stock Age
export const list = (data: ListProps) => http({
  url: '/stock/stock-age-list',
  method: 'post',
  data
})
