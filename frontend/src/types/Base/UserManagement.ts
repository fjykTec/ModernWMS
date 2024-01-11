import { UniformFileNaming, TablePage, btnGroupItem } from '../System/Form'

export interface UserVO extends UniformFileNaming {
  id: number
  user_num: string
  user_name: string
  contact_tel?: string
  user_role?: string
  sex?: string
  auth_string?: string
  email?: string
  is_valid: boolean
}

export interface ImportVO {
  user_num: string
  user_name: string
  contact_tel: string
  user_role: string
  sex: string
  is_valid: boolean
}

export interface DataProps {
  showDialogImport: boolean
  tableData: UserVO[]
  searchForm: {
    user_num: string
    user_name: string
    user_role: string
  }
  tablePage: TablePage
  showDialog: boolean
  dialogForm: UserVO
  timer: any
  btnList: btnGroupItem[]
  authorityList: string[]
}

export interface ChangePwdAPIParams {
  id: number
  old_password: string
  new_password: string
}
