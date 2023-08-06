import '@/assets/css/hookComponent.css' // import hookComponent style
import { Components } from '@/types/System/HookComponent'

// Use import.meta.globEager to read the files in the components folder, distinguished by the suffix ts
const componentsList: any = import.meta.globEager('./hookComponent/**')

const List: any = {}
export default function (app: any) {
  Object.keys(componentsList).forEach((key) => {
    // Filter out ts suffix
    if (key.includes('.ts')) {
      const hookObj = componentsList[key].default
      // Assignment function component, thrown later, imported for use
      List[`$${ hookObj.name }`] = hookObj[hookObj.name]
      // Define function components into global variables, and use them in script in vue through proxy
      app.config.globalProperties[`$${ hookObj.name }`] = hookObj[hookObj.name]
    }
  })
}

// Export function components for import
export const hookComponent: Components = List
