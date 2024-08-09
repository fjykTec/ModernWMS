import i18n from '@/languages/i18n'

export const actionDict: any = {
  companySetting: ['save', 'delete', 'export'],
  userRoleSetting: ['save', 'delete', 'export'],
  roleMenu: [],
  userManagement: ['save', 'delete', 'import', 'export', 'resetPwd'],
  commodityCategorySetting: ['save', 'delete', 'export'],
  commodityManagement: ['save', 'delete', 'export', 'saftyStock', 'printQrCode', 'printBarCode'],
  supplier: ['save', 'delete', 'import', 'export'],
  print: ['save', 'delete', 'export'],
  warehouseSetting: [
    'warehouse-save',
    'warehouse-delete',
    'warehouse-import',
    'warehouse-export',
    'area-save',
    'area-delete',
    'area-export',
    'location-save',
    'location-delete',
    'location-export',
    'location-printBarCode',
    'location-printQrCode'
  ],
  ownerOfCargo: ['save', 'delete', 'import', 'export'],
  freightSetting: ['save', 'delete', 'import', 'export'],
  customer: ['save', 'delete', 'import', 'export'],

  stockAsn: [
    'notice-save',
    'notice-delete',
    'notice-export',
    'notice-printQrCode',
    'putOnTheShelf-printQrCode',
    'delivered-confirm',
    'delivered-export',
    'unloaded-confirm',
    'unloaded-delete',
    'unloaded-export',
    'sorted-editCount',
    'sorted-confirm',
    'sorted-delete',
    'sorted-export',
    'putOnTheShelf-editArrival',
    'putOnTheShelf-delete',
    'putOnTheShelf-export',
    'detail-export'
  ],

  stockManagement: ['area-export', 'stock-export'],
  saftyStock: ['export'],
  asnStatistic: ['export'],
  deliveryStatistic: ['export'],
  stockageStatistic: ['export'],

  warehouseProcessing: ['split', 'group', 'confirmOpeartion', 'confirmAdjust', 'delete', 'export'],
  warehouseMove: ['save', 'delete', 'export', 'confirm'],
  warehouseFreeze: ['freeze', 'unfreeze', 'export'],
  warehouseAdjust: ['export'],
  warehouseTaking: ['save', 'delete', 'export', 'confirmOpeartion', 'confirmAdjust'],

  deliveryManagement: [
    'invoice-save',
    'invoice-confirm',
    'invoice-revoke',
    'invoice-delete',
    'invoice-export',
    'invoice-printQrCode',
    'picked-confirm',
    'picked-revoke',
    'picked-export',
    'packaged-package',
    'packaged-export',
    'packaged-revoke',
    'weighed-weigh',
    'weighed-export',
    'weighed-revoke',
    'delivered-delivery',
    'delivered-setCarrier',
    'delivered-signIn',
    'delivered-export',
    'signedIn-export'
  ]
}

// Get operation name
export const getActionName = (item: string, menu_name?: string) => {
  let titleKey = item

  let titleFun = ''
  if (titleKey.includes('-')) {
    titleFun = `${ i18n.global.t(`base.roleMenu.opeartionFunctionName.${ menu_name }.${ titleKey.split('-')[0] }`) }-`

    titleKey = titleKey.split('-')[1]
  }

  return `${ titleFun }${ i18n.global.t(`base.roleMenu.operationTitle.${ titleKey }`) }`
}
