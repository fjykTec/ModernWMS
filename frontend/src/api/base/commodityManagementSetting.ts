import http from '@/utils/http/request'
import { PageConfigProps } from '@/types/System/Form'
import { CommodityVO, UpdateSaftyStockReqBodyVO } from '@/types/Base/CommodityManagement'

// Find Data by Pagination
export const getSpuList = (data: PageConfigProps) => http({
    url: '/spu/list',
    method: 'post',
    data
  })

// Add a new form
export const addSpu = (data: CommodityVO) => http({
    url: '/spu',
    method: 'post',
    data
  })

// Update form
export const updateSpu = (data: CommodityVO) => http({
    url: '/spu',
    method: 'put',
    data
  })

// Delete form
export const deleteSpu = (id: number) => http({
    url: '/spu',
    method: 'delete',
    params: {
      id
    }
  })

// Update safety stock
export const updateSaftyStock = (data: { sku_id: number; detailList: UpdateSaftyStockReqBodyVO[] }) => http({
    url: '/spu/sku-safety-stock',
    method: 'put',
    data
  })
