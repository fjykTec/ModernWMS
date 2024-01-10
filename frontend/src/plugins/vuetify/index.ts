// Vuetify
import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
import { aliases, mdi } from 'vuetify/iconsets/mdi'
// Translations provided by Vuetify
import { zhHans, zhHant, en } from 'vuetify/locale'
import { getSelcectedLangForVuetify } from './method/index'

import 'material-design-icons-iconfont/dist/material-design-icons.css'
import 'vuetify/dist/vuetify.min.css'
import '@mdi/font/css/materialdesignicons.css'

const vuetify = createVuetify({
  locale: {
    locale: getStorageLang(),
    messages: { zhHans, zhHant, en }
  },
  components,
  directives,
  icons: {
    defaultSet: 'mdi',
    aliases,
    sets: {
      mdi
    }
  }
})

// get language in storage or default
function getStorageLang() {
  const lang = localStorage.getItem('language')
  if (lang) {
    return getSelcectedLangForVuetify(lang)
  }
  return 'en'
}

export { vuetify }
