import { createApp } from 'vue'
import MessageComponent from './message.vue'
import { MessageOptions } from '@/types/System/HookComponent'
// Use vuetify
import { vuetify } from '@/plugins/vuetify/index'

// Use the createApp of vue3 and the mount and unmount methods to create a mounted instance
export default {
  name: 'message',
  message: (options: MessageOptions) => {
    const mountNode = document.createElement('div')
    mountNode.className = 'messageItems'
    mountNode.style.top = `${ getNewMsgTop() }px`
    mountNode.style.zIndex = '9999'
    document.body.appendChild(mountNode)
    const app = createApp(MessageComponent, {
      ...options,
      removeComponent: () => {
        setTimeout(
          () => {
            mountNode.style.opacity = '0'
            setTimeout(() => {
              app.unmount()
              document.body.removeChild(mountNode)
              resetMsgTop()
            }, 200)
          },
          options.shutDelay ? options.shutDelay - 200 : 1300
        )
      }
    }).use(vuetify)
    return app.mount(mountNode)
  }
}

function resetMsgTop() {
  const messageDomList: any = document.body.getElementsByClassName('messageItems')
  for (let i = 0; i < messageDomList.length; i++) {
    messageDomList[i].style.top = `${ getNewMsgTop(i) }px`
  }
}

// Get message top
function getNewMsgTop(index = -1) {
  const messageDomList = document.body.querySelectorAll('.messageItems')
  let top = 10
  if (index < 0) {
    for (let i = 0; i < messageDomList.length; i++) {
      top += messageDomList[i].clientHeight + 10
    }
  } else {
    for (let i = 0; i < messageDomList.length; i++) {
      if (i < index) {
        top += messageDomList[i].clientHeight + 10
      } else {
        break
      }
    }
  }
  return top
}
