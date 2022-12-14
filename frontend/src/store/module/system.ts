interface stateProps {
  language: string
  openedMenus: string[]
}

export const system = {
  namespaced: true,
  state: {
    language: '',
    openedMenus: []
  },
  mutations: {
    setLanguage(state: stateProps, lang: string) {
      state.language = lang
    },
    addOpenedMenu(state: stateProps, menuName: string) {
      if (!state.openedMenus.includes(menuName)) {
        state.openedMenus.push(menuName)
      }
    },
    delOpenedMenu(state: stateProps, menuName: string) {
      const menuIndex = state.openedMenus.findIndex((item) => item === menuName)
      if (menuIndex > -1) {
        state.openedMenus.splice(menuIndex, 1)
      }
    },
    clearOpenedMenu(state: stateProps) {
      state.openedMenus = []
    }
  },
  actions: {},
  getters: {
    language(state: stateProps) {
      return state.language
    },
    openedMenus(state: stateProps) {
      return state.openedMenus
    }
  }
}
