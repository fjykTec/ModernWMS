import { createApp } from 'vue'
import DialogComponent from './dialog.vue'
import { DialogOptions } from '@/types/system/hookComponent'
// Use vuetify
import { vuetify } from '@/plugins/vuetify/index'

// Use the createApp of vue3 and the mount and unmount methods to create a mounted instance
export default function dialog(options: DialogOptions) {
  const mountNode = document.createElement('div')
  document.body.appendChild(mountNode)
  const app = createApp(DialogComponent, {
    ...options,
    visible: true,
    removeComponent: () => {
      app.unmount()
      document.body.removeChild(mountNode)
    }
  }).use(vuetify)
  return app.mount(mountNode)
}
