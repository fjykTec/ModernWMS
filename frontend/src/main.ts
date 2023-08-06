import { createApp } from 'vue'
import './style.css' // Global Styles
import { VXETable } from '@/plugins/VXETable/index'
import { vuetify } from '@/plugins/vuetify/index'
import i18n from './languages/i18n'
import App from './App.vue'

// import router
import { router } from './router'
import { store } from './store/index'
import hookComponent from '@/components/system/index'

const app = createApp(App)

VXETable.setup({
  i18n: (key, args) => i18n.global.t(key, args)
})

app.use(router)
app.use(store)
app.use(vuetify)
app.use(i18n)
app.use(hookComponent)
app.use(VXETable)

app.mount('#app')
