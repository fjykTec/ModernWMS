import { createApp } from 'vue'
import './style.css' // Global Styles
import { vuetify } from '@/plugins/vuetify/index'
import i18n from './languages/i18n'

import App from './App.vue'

// import router
import { router } from './router'
import { store } from './store/index'
import hookComponent from '@/components/common/function/index'

const app = createApp(App)

app.use(router)
app.use(store)
app.use(vuetify)
app.use(i18n)
app.use(hookComponent)

app.mount('#app')
