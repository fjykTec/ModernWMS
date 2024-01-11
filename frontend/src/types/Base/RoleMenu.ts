import { NavListOptions, UniformFileNaming, VxeTableRow, btnGroupItem } from '../System/Form'

export interface RoleMenuVO extends UniformFileNaming {
  userrole_id?: number
  role_name?: string
  detailList: RoleMenuDetailVo[]
}

export interface RoleMenuDetailVo extends VxeTableRow {
  id: number
  menu_id?: number
  menu_name?: string
  menu_actions_authority: string[]
}

export interface DataProps {
  activeRoleMenuForm: RoleMenuVO
  roleList: RoleMenuVO[]
  showDialog: boolean
  dialogForm: RoleMenuVO
  navListOptions: NavListOptions
  btnList: btnGroupItem[]
  editMenuDialogCurrentRow: RoleMenuDetailVo
}
