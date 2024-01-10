export interface UniformFileNaming {
  creator?: string
  create_time?: string
  last_update_time?: string
  tenant_id?: number
}

// VxeTable Required paging parameters
export interface TablePage {
  total: number
  pageIndex: number
  pageSize: number
  searchObjects?: SearchObject[]
}

export interface NavListOptions {
  title: string
  labelKey: string
  indexKey: string
  indexValue: string
}

// API Required paging parameters
export interface PageConfigProps {
  pageIndex: number
  pageSize: number
  sqlTitle?: string
  searchObjects?: any
  total?: number
}

export interface VxeTableRow {
  _X_ROW_KEY?: string
}

export interface SearchObject {
  name?: string,
  operator?: SearchOperator,
  text?: string,
  value?: string
}

export enum SearchOperator {
  INCLUDE = 6,
  EQUAL = 1
}

export interface btnGroupItem {
  name: string
  icon: string
  code: string
  click: () => void
}