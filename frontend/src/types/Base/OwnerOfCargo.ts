import { UniformFileNaming, TablePage, btnGroupItem } from '../System/Form'

export interface OwnerOfCargoVO extends UniformFileNaming {
  id: number
  goods_owner_name: string
  city: string
  address: string
  contact_tel: string
  manager: string
}

export interface ImportVO {
  goods_owner_name: string
  city: string
  address: string
  contact_tel: string
  manager: string
}

export interface DataProps {
  tableData: OwnerOfCargoVO[]
  showDialog: boolean
  showDialogImport: boolean
  dialogForm: OwnerOfCargoVO
  searchForm: {
    goods_owner_name: string
  }
  tablePage: TablePage
  timer: any
  btnList: btnGroupItem[]
  authorityList: string[]
}
