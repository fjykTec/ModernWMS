import { StateProps } from '@/types/System/Store'

export const system = {
  namespaced: true,
  state: {
    language: '',
    openedMenus: [],
    // Window Size
    clientWidth: 0,
    clientHeight: 0,
    currentRouterPath: '',
    refreshFlag: false
  },
  mutations: {
    setCurrentRouterPath(state: StateProps, path: string) {
      state.currentRouterPath = path
    },
    setRefreshFlag(state: StateProps, flag: boolean) {
      state.refreshFlag = flag
    },
    setLanguage(state: StateProps, lang: string) {
      state.language = lang
    },
    addOpenedMenu(state: StateProps, menuName: string) {
      if (!state.openedMenus.includes(menuName)) {
        state.openedMenus.push(menuName)
      }
    },
    delOpenedMenu(state: StateProps, menuName: string) {
      const menuIndex = state.openedMenus.findIndex((item) => item === menuName)
      if (menuIndex > -1) {
        state.openedMenus.splice(menuIndex, 1)
      }
    },
    clearOpenedMenu(state: StateProps) {
      state.openedMenus = []
    },
    setClientWidth(state: StateProps, clientWidth: number) {
      state.clientWidth = clientWidth
    },
    setClientHeight(state: StateProps, clientHeight: number) {
      state.clientHeight = clientHeight
    }
  },
  actions: {},
  getters: {
    currentRouterPath(state: StateProps) {
      return state.currentRouterPath
    },
    language(state: StateProps) {
      return state.language
    },
    openedMenus(state: StateProps) {
      return state.openedMenus
    },
    clientWidth(state: StateProps) {
      return state.clientWidth
    },
    clientHeight(state: StateProps) {
      return state.clientHeight
    },
    refreshFlag(state: StateProps) {
      return state.refreshFlag
    }
  }
}
