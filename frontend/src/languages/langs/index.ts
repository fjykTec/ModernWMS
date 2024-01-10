import zhCN from 'vxe-table/lib/locale/lang/zh-CN'
import enUS from 'vxe-table/lib/locale/lang/en-US'
import zhTW from 'vxe-table/lib/locale/lang/zh-TW'

import en from '../langsJson/en.json'
import cn from '../langsJson/cn.json'
import tw from '../langsJson/tw.json'

export default {
  zh_CN: {
    ...cn,
    ...zhCN
  },
  en_US: {
    ...en,
    ...enUS
  },
  zh_TW: {
    ...tw,
    ...zhTW
  }
}
