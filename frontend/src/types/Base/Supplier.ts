import { UniformFileNaming, TablePage } from '../System/Form'

export interface SupplierVO extends UniformFileNaming {
  id: number
  supplier_name: string
  city: string
  address: string
  manager: string
  email: string
  contact_tel: string
  is_valid: boolean
}

export interface SupplierExcelVO {
  supplier_name: string
  city: string
  address: string
  manager: string
  email: string
  contact_tel: string
}

export interface DataProps {
  tableData: SupplierVO[]
  tablePage: TablePage
  showDialog: boolean
  dialogForm: SupplierVO
}
