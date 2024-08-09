import http from '@/utils/http/request'
import { PageConfigProps } from '@/types/System/Form'
import { StockAsnVO, SortingVo, PutawayVo } from '@/types/WMS/StockAsn'

export const listNew = (data: PageConfigProps) => http({
    url: '/asn/asnmaster/list',
    method: 'post',
    data
  })

export const addAsnNew = (data: StockAsnVO) => http({
    url: '/asn/asnmaster',
    method: 'post',
    data
  })

export const updateAsnNew = (data: StockAsnVO) => http({
    url: '/asn/asnmaster',
    method: 'put',
    data
  })

export const confirmArrival = (data: { id: number; arrival_time: string }[]) => http({
    url: '/asn/confirm',
    method: 'put',
    data
  })

export const confirmUnload = (data: { id: number; unloadTime: string; unloadPerson: string; unloadPersonID: number }[]) => http({
    url: '/asn/unload',
    method: 'put',
    data
  })

export const unconfirmArrival = (idList: number[]) => http({
    url: '/asn/confirm-cancel',
    method: 'put',
    data: idList
  })

export const editSorting = (data: { asn_id: number; series_number: string; sorted_qty: number }[]) => http({
    url: '/asn/sorting',
    method: 'put',
    data
  })

export const modifySorting = (data: { asn_id: number; series_number: string; sorted_qty: number }[]) => http({
    url: '/asn/sorting-modify',
    method: 'put',
    data
  })

export const getSorting = (id: number) => http({
    url: '/asn/sorting',
    method: 'get',
    params: {
      asn_id: id
    }
  })

export const getGrouding = (id: number) => http({
    url: '/asn/pending-putaway',
    method: 'get',
    params: {
      id
    }
  })

export const confirmPutaway = (
  data: {
    asn_id: number
    goods_owner_id: number
    series_number: string
    goods_location_id: number
    putaway_qty: number
  }[]
) => http({
    url: '/asn/putaway',
    method: 'put',
    data
  })

export const confirmSorted = (id: number) => http({
    url: '/asn/sorted',
    method: 'put',
    data: [id]
  })

export const revokeUnload = (idList: number[]) => http({
    url: '/asn/unload-cancel',
    method: 'put',
    data: idList
  })

export const revokeSorting = (idList: number[]) => http({
    url: '/asn/sorted-cancel',
    method: 'put',
    data: idList
  })

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

export const deleteAsnByID = (id: number) => http({
    url: '/asn/asnmaster',
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

  export const getPrintAsnList = (data: number[]) => http({
    url: '/asn/print-sn',
    method: 'post',
    data
  })
