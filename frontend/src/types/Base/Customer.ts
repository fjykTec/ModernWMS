import { UniformFileNaming, TablePage } from '../System/Form'

export interface CustomerVO extends UniformFileNaming {
  id: number
  customer_name: string
  city: string
  address: string
  manager: string
  email: string
  contact_tel: string
  is_valid: boolean
}
export interface CustomerExcelVO {
  customer_name: string
  city: string
  address: string
  manager: string
  email: string
  contact_tel: string
  _XID: string
  errorMsg: string
}
export interface DataProps {
  tableData: CustomerVO[]
  tablePage: TablePage
  showDialog: boolean
  dialogForm: CustomerVO
}
