import '@/assets/css/hookComponent.css' // import hookComponent style

// Use import.meta.globEager to read the files in the components folder, distinguished by the suffix ts
const componentsList: any = import.meta.globEager('../hookComponent/**')

const List: any = {}
export default function (app: any) {
  Object.keys(componentsList).forEach((key) => {
    // Filter out ts suffix
    if (key.includes('.ts')) {
      // Assignment function component, thrown later, imported for use
      List[`$${ componentsList[key].default.name }`] = componentsList[key].default

      // Define function components into global variables, and use them in script in vue through proxy
      app.config.globalProperties[`$${ componentsList[key].default.name }`] = componentsList[key].default
    }
  })
}

// Export function components for import
export const funComponentList = List
