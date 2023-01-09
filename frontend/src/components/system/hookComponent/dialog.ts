import { createApp } from 'vue'
import DialogComponent from './dialog.vue'
import { DialogOptions } from '@/types/System/HookComponent'
// Use vuetify
import { vuetify } from '@/plugins/vuetify/index'
import i18n from '@/languages/i18n'

// Use the createApp of vue3 and the mount and unmount methods to create a mounted instance
export default {
  name: 'dialog',
  dialog: (options: DialogOptions) => {
    const mountNode = document.createElement('div')
    mountNode.style.zIndex = '9000'
    document.body.appendChild(mountNode)
    if (!options.confirmText) {
      options.confirmText = i18n.global.t('system.hookComponent.dialog.defaultConfirm')
    }
    if (!options.cancleText) {
      options.cancleText = i18n.global.t('system.hookComponent.dialog.defaultClose')
    }
    if (!options.title) {
      options.title = i18n.global.t('system.hookComponent.dialog.defaultTitle')
    }
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
}
