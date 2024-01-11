import { UserStateProps, MenuItem } from '@/types/System/Store'

export const user = {
  namespaced: true,
  state: {
    userInfo: {},
    token: '',
    // String used to refresh token
    refreshToken: '',
    // The time when the token expires
    expirationTime: '',
    // Token validity period
    effectiveMinutes: '',
    // Is refreshToken currently in progress
    isRefreshingToken: false,
    // Menu permissions
    menulist: [
      // {
      //   name: 'testmenu',
      //   path: '/testmenu',
      //   directory: 'testmenu/testmenu'
      // }
    ]
  },
  mutations: {
    setUserInfo(state: UserStateProps, userInfo: any) {
      state.userInfo = userInfo
    },
    resetUserInfo(state: UserStateProps, userInfo = {}) {
      state.userInfo = { ...state.userInfo, ...userInfo }
    },
    setToken(state: UserStateProps, token: string) {
      state.token = token
    },
    setExpirationTime(state: UserStateProps, expirationTime: number) {
      state.expirationTime = expirationTime
    },
    setIsRefreshingToken(state: UserStateProps, isRefreshingToken: boolean) {
      state.isRefreshingToken = isRefreshingToken
    },
    setRefreshToken(state: UserStateProps, refreshToken: string) {
      state.refreshToken = refreshToken
    },
    setEffectiveMinutes(state: UserStateProps, effectiveMinutes: number) {
      state.effectiveMinutes = effectiveMinutes
    },
    setUserMenuList(state: UserStateProps, menulist: MenuItem[]) {
      state.menulist = menulist
    }
  },
  actions: {},
  getters: {
    userInfo(state: UserStateProps) {
      return state.userInfo
    },
    token(state: UserStateProps) {
      return state.token
    },
    expirationTime(state: UserStateProps) {
      return state.expirationTime
    },
    isRefreshingToken(state: UserStateProps) {
      return state.isRefreshingToken
    },
    refreshToken(state: UserStateProps) {
      return state.refreshToken
    },
    effectiveMinutes(state: UserStateProps) {
      return state.effectiveMinutes
    },
    menulist(state: UserStateProps) {
      return state.menulist
    }
  }
}
