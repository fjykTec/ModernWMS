/*
 * @Author: yanguoping 125722066@qq.com
 * @Date: 2023-10-12 10:29:48
 * @LastEditors: yanguoping 125722066@qq.com
 * @LastEditTime: 2024-05-09 09:12:32
 * @FilePath: \frontend\src\utils\dataVerification\tableRule.ts
 * @Description: 这是默认设置,请设置`customMade`, 打开koroFileHeader查看配置 进行设置: https://github.com/OBKoro1/koro1FileHeader/wiki/%E9%85%8D%E7%BD%AE
 */
import i18n from '@/languages/i18n'

// Judge whether it is integer
export function isInteger(rule: any, value: any) {
  if (rule.hasOwnProperty('cellValue')) {
    value = rule.cellValue
    rule = rule.rule
  }
  return new Promise((resolve, reject) => {
    let validText = ''    
    if (value === '' || value == null) {
      resolve(true)
    } else {
      let re = /^-?[0-9]\d*$/
      let max = 2147483647
      let min = -2147483648
      const length = rule.length || 32
      if (length === 8) {
        max = 255
        min = 0
      } else if (length === 16) {
        max = 32767
        min = -32768
      }
      if (rule.validNumerical === 'nonNegative') {
        re = /^[0-9]\d*$/
        validText = '>=0'
      } else if (rule.validNumerical === 'greaterThanZero') {
        re = /^[1-9]\d*$/
        validText = '>0'
      } else if (rule.validNumerical === 'noZero') {
        re = /^[-]?[1-9][0-9]*$/
        validText = '!=0'
      }
      const rsCheck = re.test(value)
      if (!rsCheck) {
        reject(new Error(`${ i18n.global.t('system.checkText.mustInput') }${ validText }${ i18n.global.t('system.checkText.integer') }`))
      } else if (value > max || value < min) {
        reject(new Error(`${ i18n.global.t('system.checkText.mustInput') }${ min }-${ max }${ i18n.global.t('system.checkText.integer') }`))
      } else {
        resolve(true)
      }
    }
  })
}

// Judge whether it is decimal
export function isDecimal(rule: any, value: any): any {
  if (rule.hasOwnProperty('cellValue')) {
    value = rule.cellValue
    rule = rule.rule
  }
  return new Promise((resolve, reject) => {
    if (value === '' || value == null) {
      resolve(true)
    } else {
      let validText = ''
      let reg = /^[-]?\d+(\.\d+)?$/
      if (rule.validNumerical === 'nonNegative') {
        reg = /^\d+(\.\d+)?$/
        validText = '>=0'
      } else if (rule.validNumerical === 'greaterThanZero') {
        reg = /^[+]{0,1}([1-9][0-9]{0,})$|^[+]{0,1}([1-9][0-9]{0,}\.\d+)$|^[+]{0,1}([0]\.(?!0+$)\d{1,})$/
        validText = '>0'
      }
      const rsCheck = reg.test(value)
      if (!rsCheck) {
        reject(new Error(`${ i18n.global.t('system.checkText.pleaseEnter') }${ validText }${ i18n.global.t('system.checkText.decimal') }`))
      } else {
        const length = rule.length || 18
        const decimalLength = rule.decimalLength || 2
        const arr = value.toString().split('.')
        if (arr.length > 0 && arr[0].length > length - decimalLength) {
          reject(new Error(`${ i18n.global.t('system.checkText.pleaseEnter') }${ length - decimalLength }${ i18n.global.t('system.checkText.inputIntMsg') }`))
        } else if (arr.length > 1 && arr[1].length > decimalLength) {
          reject(new Error(`${ i18n.global.t('system.checkText.pleaseEnter') }${ decimalLength }${ i18n.global.t('system.checkText.inputDecimalMsg') }`))
        } else {
          resolve(true)
        }
      }
    }
  })
}
