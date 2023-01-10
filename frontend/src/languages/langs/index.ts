import zhCN from 'vxe-table/lib/locale/lang/zh-CN'
import enUS from 'vxe-table/lib/locale/lang/en-US'

import en from './enUs'
import cn from './cnZh'

export default {
  zh_CN: {
    ...cn,
    ...zhCN
  },
  en_US: {
    ...en,
    ...enUS
  }
}
