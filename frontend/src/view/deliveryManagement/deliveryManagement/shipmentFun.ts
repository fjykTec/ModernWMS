import i18n from '@/languages/i18n'

// Get the status display according to the key value
export function getShipmentState(state: number) {
  switch (state) {
    case 0:
      return i18n.global.t('wms.deliveryManagement.preShipment')
    case 1:
      return i18n.global.t('wms.deliveryManagement.newShipment')
    case 2:
      return i18n.global.t('wms.deliveryManagement.goodsToBePicked')
    case 3:
      return i18n.global.t('wms.deliveryManagement.picked')
    case 4:
      return i18n.global.t('wms.deliveryManagement.packaged')
    case 5:
      return i18n.global.t('wms.deliveryManagement.weighed')
    case 6:
      return i18n.global.t('wms.deliveryManagement.outOfWarehouse')
    case 7:
      return i18n.global.t('wms.deliveryManagement.signedIn')
  }
}