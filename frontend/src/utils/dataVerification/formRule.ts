import i18n from '@/languages/i18n'

// Judge whether it is integer
export function IsInteger(value: string | number, validNumerical: 'nonNegative' | 'greaterThanZero' | 'noZero', length?: number): string {
  if (typeof value === 'number') {
    value = String(value)
  }
  let validText = ''
  if (value === '' || value == null) {
    return ''
  }
  let re = /^-?[0-9]\d*$/
  let max = 2147483647
  let min = -2147483648
  length = length || 32
  if (length === 8) {
    max = 255
    min = 0
  } else if (length === 16) {
    max = 32767
    min = -32768
  }
  switch (validNumerical) {
    case 'nonNegative':
      re = /^[0-9]\d*$/
      validText = '>=0'
      break
    case 'greaterThanZero':
      re = /^[1-9]\d*$/
      validText = '>0'
      break
    case 'noZero':
      re = /^[-]?[1-9][0-9]*$/
      validText = '!=0'
      break
  }
  const rsCheck = re.test(value)
  if (!rsCheck) {
    return `${ i18n.global.t('system.checkText.mustInput') }${ validText }${ i18n.global.t('system.checkText.integer') }`
  }
  if (Number(value) > max || Number(value) < min) {
    return `${ i18n.global.t('system.checkText.mustInput') }${ min }-${ max }${ i18n.global.t('system.checkText.integer') }`
  }
  return ''
}

// Judge whether it is decimal
export function IsDecimal(
  value: string | number,
  validNumerical: 'nonNegative' | 'greaterThanZero',
  length?: number,
  decimalLength?: number
): string {
  if (typeof value === 'number') {
    value = String(value)
  }
  if (value === '' || value == null) {
    return ''
  }
  let validText = ''
  let reg = /^[-]?\d+(\.\d+)?$/
  switch (validNumerical) {
    case 'nonNegative':
      reg = /^\d+(\.\d+)?$/
      validText = '>=0'
      break
    case 'greaterThanZero':
      reg = /^[+]{0,1}([1-9][0-9]{0,})$|^[+]{0,1}([1-9][0-9]{0,}\.\d+)$|^[+]{0,1}([0]\.(?!0+$)\d{1,})$/
      validText = '>0'
      break
  }
  const rsCheck = reg.test(value)
  if (!rsCheck) {
    return `${ i18n.global.t('system.checkText.pleaseEnter') }${ validText }${ i18n.global.t('system.checkText.decimal') }`
  }
  length = length || 18
  decimalLength = decimalLength || 2
  const arr = value.toString().split('.')
  if (arr.length > 0 && arr[0].length > length - decimalLength) {
    return `${ i18n.global.t('system.checkText.pleaseEnter') }${ length - decimalLength }${ i18n.global.t('system.checkText.inputIntMsg') }`
  }
  if (arr.length > 1 && arr[1].length > decimalLength) {
    return `${ i18n.global.t('system.checkText.pleaseEnter') }${ decimalLength }${ i18n.global.t('system.checkText.inputDecimalMsg') }`
  }
  return ''
}

// Judge string length
export function StringLength(value: string, min: number, max: number) {
  if (value && (value.length < min || value.length > max)) {
    return `${ i18n.global.t('system.checkText.lengthValid') }${ min }-${ max }`
  }
  return ''
}
