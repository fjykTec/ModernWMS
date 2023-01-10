import { VxeTablePropTypes } from 'vxe-table'
import { UniformFileNaming } from '../System/Form'

export interface CategoryVO extends UniformFileNaming {
  id: number
  parent_id?: number
  category_name: string
  is_valid?: boolean
}

export interface DataProps {
  tableData: CategoryVO[]
  tableTreeConfig: VxeTablePropTypes.TreeConfig
  showDialog: boolean
  dialogForm: CategoryVO
}
