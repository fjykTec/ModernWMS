import XEUtils from 'xe-utils'
import i18n from '@/languages/i18n'

export const formatIsValid = (value: boolean) => (value ? i18n.global.t('system.combobox.yesOrNo.yes') : i18n.global.t('system.combobox.yesOrNo.no'))

export const formatDate = (value: any, format?: string) => {
  const date = new Date(value)
  if (!value || !date || XEUtils.toDateString(date, 'yyyy-MM-dd') === '1900-01-01') {
    return ''
  }
  return XEUtils.toDateString(date, format || 'yyyy-MM-dd HH:mm:ss')
}

export const formatString = (value: any): string => {
  if (value == null) return ''
  return value?.toString() || ''
}