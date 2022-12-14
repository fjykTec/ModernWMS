// interface UserInfo {}

export const user = {
  namespaced: true,
  state: {
    // TODO userInfo 类
    userInfo: {},
    token: '',
    // 用于刷新token用的串
    refreshToken: '',
    // token失效的时间
    expirationTime: '',
    // token有效期
    effectiveMinutes: '',
    // 当前是否正在refreshToken
    isRefreshingToken: false,
    // 菜单权限
    // 目前用于测试
    menulist: [
      // {
      //   name: 'testmenu',
      //   path: '/testmenu',
      //   directory: 'testmenu/testmenu'
      // }
    ]
  },
  mutations: {
    setUserInfo(state: any, userInfo: any) {
      // 这里的 `state` 对象是模块的局部状态
      state.userInfo = userInfo
    },
    resetUserInfo(state: any, userInfo = {}) {
      state.userInfo = { ...state.userInfo, ...userInfo }
    },
    setToken(state: any, token: string) {
      // 这里的 `state` 对象是模块的局部状态
      state.token = token
    },
    setExpirationTime(state: any, expirationTime: number) {
      // 这里的 `state` 对象是模块的局部状态
      state.expirationTime = expirationTime
    },
    setIsRefreshingToken(state: any, isRefreshingToken: boolean) {
      // 这里的 `state` 对象是模块的局部状态
      state.isRefreshingToken = isRefreshingToken
    },
    setRefreshToken(state: any, refreshToken: string) {
      // 这里的 `state` 对象是模块的局部状态
      state.refreshToken = refreshToken
    },
    setEffectiveMinutes(state: any, effectiveMinutes: number) {
      // 这里的 `state` 对象是模块的局部状态
      state.effectiveMinutes = effectiveMinutes
    }
  },
  actions: {},
  getters: {
    userInfo(state: any) {
      return state.userInfo
    },
    token(state: any) {
      return state.token
    },
    expirationTime(state: any) {
      return state.expirationTime
    },
    isRefreshingToken(state: any) {
      return state.isRefreshingToken
    },
    refreshToken(state: any) {
      return state.refreshToken
    },
    effectiveMinutes(state: any) {
      return state.effectiveMinutes
    },
    menulist(state: any) {
      return state.menulist
    }
  }
}
