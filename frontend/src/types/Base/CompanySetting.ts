import { UniformFileNaming, btnGroupItem } from '../System/Form'

export interface CompanyVO extends UniformFileNaming {
  id: number
  company_name: string
  city: string
  address: string
  manager: string
  contact_tel: string
}

export interface DataProps {
  tableData: CompanyVO[]
  showDialog: boolean
  dialogForm: CompanyVO
  btnList: btnGroupItem[]
  authorityList: string[]
}
