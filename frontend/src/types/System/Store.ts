// user
export interface MenuItem {
  id: number
  menu_name: string
  module: string
  vue_path: string
  vue_path_detail: string
  vue_directory: string
}

export interface UserStateProps {
  userInfo: any
  token: string
  refreshToken: string
  expirationTime: number
  effectiveMinutes: number
  isRefreshingToken: boolean
  menulist: MenuItem[]
}

export interface StateProps {
  language: string
  currentRouterPath: string
  openedMenus: string[]
  clientWidth: number
  clientHeight: number
  refreshFlag: boolean
}