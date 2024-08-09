import http from '@/utils/http/request'
import { PageConfigProps } from '@/types/System/Form'
import {
  addRequestVO,
  ConfirmOrderVO,
  PackageVO,
  WeighVO,
  DeliveryVO,
  SignInVO,
  CancleOrderVO,
  SetCarrierVO
} from '@/types/DeliveryManagement/DeliveryManagement'

// Get Pre shipment
export const getShipment = (data: PageConfigProps) => http({
    url: '/dispatchlist/advanced-list',
    method: 'post',
    data
  })

// Get Pre shipment
export const getPreShipment = (data: PageConfigProps) => http({
    url: '/dispatchlist/advanced-list',
    method: 'post',
    data: {
      ...data,
      sqlTitle: 'dispatch_status=0'
    }
  })

// Get new shipment
export const getNewShipment = (data: PageConfigProps) => http({
    url: '/dispatchlist/advanced-list',
    method: 'post',
    data: {
      ...data,
      sqlTitle: 'dispatch_status=1'
    }
  })

// Get new shipment
export const getGoodsToBePicked = (data: PageConfigProps) => http({
    url: '/dispatchlist/list',
    method: 'post',
    data: {
      ...data,
      sqlTitle: 'dispatch_status=2'
    }
  })

// Get To Be Packaged
export const getToBePackaged = (data: PageConfigProps) => http({
    url: '/dispatchlist/list',
    method: 'post',
    data: {
      ...data,
      sqlTitle: 'to_package'
    }
  })

// Get picked
export const getPicked = (data: PageConfigProps) => http({
    url: '/dispatchlist/list',
    method: 'post',
    data: {
      ...data,
      sqlTitle: 'dispatch_status=3'
    }
  })

// Get Packaged
export const getPackaged = (data: PageConfigProps) => http({
    url: '/dispatchlist/list',
    method: 'post',
    data: {
      ...data,
      // sqlTitle: 'dispatch_status=4'
      sqlTitle: 'package'
    }
  })

// Get To Be Weight
export const getToBeWeighed = (data: PageConfigProps) => http({
    url: '/dispatchlist/list',
    method: 'post',
    data: {
      ...data,
      sqlTitle: 'to_weight'
    }
  })

// Get Weight
export const getWeighed = (data: PageConfigProps) => http({
    url: '/dispatchlist/list',
    method: 'post',
    data: {
      ...data,
      // sqlTitle: 'dispatch_status=5'
      sqlTitle: 'weight'
    }
  })

// Get To Be Delivery
export const getToBeDelivery = (data: PageConfigProps) => http({
    url: '/dispatchlist/list',
    method: 'post',
    data: {
      ...data,
      sqlTitle: 'to_delivery'
    }
  })

// Get Delivery
export const getDelivery = (data: PageConfigProps) => http({
    url: '/dispatchlist/list',
    method: 'post',
    data: {
      ...data,
      sqlTitle: 'delivery'
    }
  })

// Get Sign In
export const getSignIn = (data: PageConfigProps) => http({
    url: '/dispatchlist/list',
    method: 'post',
    data: {
      ...data,
      sqlTitle: 'dispatch_status=7'
    }
  })

// Add
export const addShipment = (data: addRequestVO[]) => http({
    url: '/dispatchlist',
    method: 'post',
    data
  })

// Delete
export const delShipment = (dispatch_no: string) => http({
    url: '/dispatchlist',
    method: 'delete',
    params: {
      dispatch_no
    }
  })

// Get order confirmation information
export const getConfirmOrderInfoAndStock = (dispatch_no: string) => http({
    url: '/dispatchlist/confirm-check',
    method: 'get',
    params: {
      dispatch_no
    }
  })

// Confirm orders and create dispatchpicklist
export const confirmOrder = (data: ConfirmOrderVO[]) => http({
    url: '/dispatchlist/confirm-order',
    method: 'post',
    data
  })
// Confirm pickingDetail
export const confirmPickingDetail = (data: number[]) => http({
  url: '/dispatchlist/confirm-pick-detail',
  method: 'post',
  data
})
// Confirm picking
export const confirmPicking = (dispatch_no: string) => http({
    url: '/dispatchlist/confirm-pick-dispatchlistno',
    method: 'put',
    params: {
      dispatch_no
    }
  })

// Pack
export const handlePackage = (data: PackageVO[]) => http({
    url: '/dispatchlist/package',
    method: 'post',
    data
  })

// Weigh
export const handleWeigh = (data: WeighVO[]) => http({
    url: '/dispatchlist/weight',
    method: 'post',
    data
  })

// Delivery
export const handleDelivery = (data: DeliveryVO[]) => http({
    url: '/dispatchlist/delivery',
    method: 'post',
    data
  })

// Sign in
export const handleSignIn = (data: SignInVO[]) => http({
    url: '/dispatchlist/sign',
    method: 'post',
    data
  })

// Undo to previous step by detail
export const cancelOrderByDetail = (id: number) => http({
    url: '/dispatchlist/cancel-order',
    method: 'put',
    params: {
      id
    }
  })

// Undo to previous step by dispatch
export const cancelOrderByDispatch = (data: CancleOrderVO) => http({
    url: '/dispatchlist/cancel-order',
    method: 'post',
    data
  })

// Set carrier
export const setCarrier = (data: SetCarrierVO[]) => http({
    url: '/dispatchlist/freightfee',
    method: 'post',
    data
  })

// Set carrier
export const viewInventoryDetails = (dispatch_id: number) => http({
    url: '/dispatchlist/pick-list',
    method: 'get',
    params: {
      dispatch_id
    }
  })

// Set carrier
export const viewDeliveryMainDetail = (dispatch_no: string) => http({
    url: '/dispatchlist/by-dispatch_no',
    method: 'get',
    params: {
      dispatch_no
    }
  })
