import http from '@/utils/http/request'
import { PageConfigProps } from '@/types/System/Form'
import { StockAsnVO, SortingVo, PutawayVo } from '@/types/WMS/StockAsn'

export const getStockAsnList = (data: PageConfigProps) => http({
    url: '/asn/list',
    method: 'post',
    data
  })

export const addAsn = (data: StockAsnVO) => http({
    url: '/asn',
    method: 'post',
    data
  })

export const updateAsn = (data: StockAsnVO) => http({
    url: '/asn',
    method: 'put',
    data
  })

export const deleteAsn = (id: number) => http({
    url: '/asn',
    method: 'delete',
    params: {
      id
    }
  })

export const confirmAsn = (id: number) => http({
    url: `/asn/confirm/${ id }`,
    method: 'put'
  })
export const confirmAsnCancel = (id: number) => http({
    url: `/asn/confirm-cancel/${ id }`,
    method: 'put'
  })

export const unloadAsn = (id: number) => http({
    url: `/asn/unload/${ id }`,
    method: 'put'
  })
export const unloadAsnCancel = (id: number) => http({
    url: `/asn/unload-cancel/${ id }`,
    method: 'put'
  })

export const sortedAsn = (id: number) => http({
    url: `/asn/sorted/${ id }`,
    method: 'put'
  })
export const sortedAsnCancel = (id: number) => http({
    url: `/asn/sorted-cancel/${ id }`,
    method: 'put'
  })

export const sortingAsn = (data: SortingVo) => http({
    url: '/asn/sorting',
    method: 'put',
    data
  })

export const putawayAsn = (data: PutawayVo) => http({
    url: '/asn/putaway',
    method: 'put',
    data
  })

export const getSkuInfo = (id: number) => http({
    url: '/spu/sku',
    method: 'get',
    params: {
      sku_id: id
    }
  })
