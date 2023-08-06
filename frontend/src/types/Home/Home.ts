// test interface
export interface SideBarMenu {
  icon?: string
  lable: string
  routerPath?: string
  showDetail?: boolean
  children?: SideBarMenu[]
}

export interface SideBarDataProps {
  menuList: SideBarMenu[]
}