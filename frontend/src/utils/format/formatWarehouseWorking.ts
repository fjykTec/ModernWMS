import i18n from '@/languages/i18n'
import { PROCESS_JOB_COMBINE, FREEZE_JOB_FREEZE, TAKING_JOB_FINISH } from '@/constant/warehouseWorking'
import { MoveStatus } from '@/types/WarehouseWorking/WarehouseMove'
import { AdjustJobType } from '@/types/WarehouseWorking/WarehouseAdjust'

export const formatProcessJobType = (value: boolean) => (value === PROCESS_JOB_COMBINE
    ? i18n.global.t('wms.warehouseWorking.warehouseProcessing.process_combine')
    : i18n.global.t('wms.warehouseWorking.warehouseProcessing.process_split'))

export const formatFreezeJobType = (value: boolean) => (value === FREEZE_JOB_FREEZE
    ? i18n.global.t('wms.warehouseWorking.warehouseFreeze.freeze')
    : i18n.global.t('wms.warehouseWorking.warehouseFreeze.unfreeze'))

export const formatMoveStatus = (value: number) => (value === MoveStatus.UNADJUST
    ? i18n.global.t('wms.warehouseWorking.warehouseMove.unadjust')
    : i18n.global.t('wms.warehouseWorking.warehouseMove.adjusted'))

export const formatTakingJobStatus = (value: boolean) => (value === TAKING_JOB_FINISH
    ? i18n.global.t('wms.warehouseWorking.warehouseTaking.finish')
    : i18n.global.t('wms.warehouseWorking.warehouseTaking.unfinish'))

export const formatAdjustJobType = (value: number) => {
  switch (value) {
    case AdjustJobType.TAKE:
      return i18n.global.t('wms.warehouseWorking.warehouseAdjust.warehouseTake')
    case AdjustJobType.PROCESS_COMBINE:
      return i18n.global.t('wms.warehouseWorking.warehouseAdjust.processCombine')
    case AdjustJobType.PROCESS_SPLIT:
      return i18n.global.t('wms.warehouseWorking.warehouseAdjust.processSplit')
    case AdjustJobType.MOVE:
      return i18n.global.t('wms.warehouseWorking.warehouseAdjust.warehouseMove')
    default:
      return ''
  }
}
