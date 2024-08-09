import { createApp } from 'vue'
import './style.css' // Global Styles
import print from 'vue3-print-nb'
import { setup } from 'yk-vue-plugin-hiprint'
import DataVVue3 from '@kjgl77/datav-vue3'
import { VXETable } from '@/plugins/VXETable/index'
import { vuetify } from '@/plugins/vuetify/index'
import i18n from './languages/i18n'
import App from './App.vue'
import '@/assets/fonts/iconfont.css'

// import router
import { router } from './router'
import { store } from './store/index'
import hookComponent from '@/components/system/index'

import VxeDateColumn from '@/components/table/vxe-date-column.vue'

const app = createApp(App)
app.config.globalProperties.hiprint = setup()

VXETable.setup({
  i18n: (key, args) => i18n.global.t(key, args)
})

app.use(print)
app.use(router)
app.use(store)
app.use(vuetify)
app.use(i18n)
app.use(hookComponent)
app.use(VXETable)
app.use(DataVVue3)

// 自定义组件挂载
app.component('VxeDateColumn', VxeDateColumn)

app.mount('#app')
