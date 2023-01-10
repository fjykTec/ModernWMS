import { AreaProperty } from '@/types/Base/Warehouse'
import i18n from '@/languages/i18n'

export const formatAreaProperty = (value: number) => {
  switch (value) {
    case AreaProperty.picking_area:
      return i18n.global.t('base.warehouseSetting.picking_area')
    case AreaProperty.stocking_area:
      return i18n.global.t('base.warehouseSetting.stocking_area')
    case AreaProperty.receiving_area:
      return i18n.global.t('base.warehouseSetting.receiving_area')
    case AreaProperty.return_area:
      return i18n.global.t('base.warehouseSetting.return_area')
    case AreaProperty.defective_area:
      return i18n.global.t('base.warehouseSetting.defective_area')
    case AreaProperty.inventory_area:
      return i18n.global.t('base.warehouseSetting.inventory_area')
    default:
      return ''
  }
}
