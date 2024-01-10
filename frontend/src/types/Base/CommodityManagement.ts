import { VxeTablePropTypes } from 'vxe-table'
import { UniformFileNaming, TablePage, btnGroupItem } from '../System/Form'

export interface CommodityVO extends UniformFileNaming {
  id: number
  spu_code: string
  spu_name: string
  category_id?: number
  category_name?: string
  spu_description: string
  supplier_id?: number
  supplier_name?: string
  brand: string
  origin?: string
  length_unit: number
  volume_unit: number
  weight_unit: number
  detailList?: CommodityDetailVO[]
}

export interface CommodityDetailVO {
  id: number
  sku_code: string
  sku_name: string
  unit: string
  cost?: number
  price?: number
  weight?: number
  lenght?: number
  width?: number
  height?: number
  volume?: number
  detailList: UpdateSaftyStockReqBodyVO[]
}

export interface CommodityDetailJoinMainVO {
  sku_id: number
  spu_id: number
  spu_code: string
  spu_name: string
  sku_code: string
  sku_name: string
  supplier_id: number
  supplier_name: string
  brand: string
  origin: string
  unit: string
}

export interface DataProps {
  tableData: CommodityVO[]
  tableTreeConfig: VxeTablePropTypes.TreeConfig
  showDialog: boolean
  dialogForm: CommodityVO
  tablePage: TablePage
  searchForm: {
    spu_code: string
    spu_name: string
    category_name: string
  }
  timer: any
  btnList: btnGroupItem[]
  authorityList: string[]
  selectRowData: any[]
}

export interface UpdateSaftyStockReqBodyVO {
  id?: number
  sku_id?: number
  warehouse_id?: number
  warehouse_name: string
  safety_stock_qty: number
}
