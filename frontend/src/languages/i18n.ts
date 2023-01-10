import { createI18n } from 'vue-i18n'
import messages from './langs/index'
import { getSelcectedLang } from './method/index'

const i18n = createI18n({
  legacy: false,
  globalInjection: true,
  locale: getStorageLang(),
  messages
})

// get language in storage or default
function getStorageLang() {
  const lang = localStorage.getItem('language')
  if (lang) {
    return getSelcectedLang(lang)
  }
  return 'en_US'
}

export default i18n
