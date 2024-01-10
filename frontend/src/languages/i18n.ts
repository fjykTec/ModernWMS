import zhCN from 'vxe-table/lib/locale/lang/zh-CN'
import enUS from 'vxe-table/lib/locale/lang/en-US'
import zhTW from 'vxe-table/lib/locale/lang/zh-TW'

import { createI18n } from 'vue-i18n'
import { getSelectedLang } from './method/index'

import en from './langsJson/en.json'
import cn from './langsJson/cn.json'
import tw from './langsJson/tw.json'

const i18n = createI18n({
  legacy: false,
  globalInjection: true,
  locale: getStorageLang(),
  messages: {
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
})

// get language in storage or default
function getStorageLang() {
  const lang = localStorage.getItem('language')
  if (lang) {
    return getSelectedLang(lang)
  }
  return 'en_US'
}

export default i18n
