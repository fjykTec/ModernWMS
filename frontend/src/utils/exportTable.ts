import * as XLSX from 'xlsx'
import { hookComponent } from '@/components/system'
import i18n from '@/languages/i18n'
/**
 * Export table
 * Default type is 'xlsx'
 */
export const exportData = ({ table, filename, columnFilterMethod, mode = 'all' }: IExportTable): void => {
  try {
    // 1.Get table header
    const columns = table?.getColumns()
    const theadDOM = table.$el.querySelector('.body--wrapper>.vxe-table--header')
    const theadDOMCopy = theadDOM.cloneNode(true)
    const headerRow = theadDOMCopy.querySelector('.vxe-header--row')
    const headerColumn = theadDOMCopy.querySelectorAll('.vxe-header--column')

    // 2.Get table data
    const bodyDOM = table.$el.querySelector('.body--wrapper>.vxe-table--body')
    const bodyDOMCopy = bodyDOM.cloneNode(true)
    const rowDOMList = bodyDOMCopy.querySelectorAll('.vxe-body--row')

    // 3.Filter header in table
    for (let i = 0; i < columns.length; i++) {
      const column = columns[i]
      if (columnFilterMethod && !columnFilterMethod({ column })) {
       headerRow.removeChild(headerColumn[i])
      }
    }

    // 4.Filter table data in table
    for (let j = 0; j < rowDOMList.length; j++) {
      const row = rowDOMList[j]
      const columnDOMList = row.querySelectorAll('.vxe-body--column')

      for (let i = 0; i < columns.length; i++) {
        const column = columns[i]
        if (columnFilterMethod && !columnFilterMethod({ column })) {
          row.removeChild(columnDOMList[i])
        }
      }
    }

    // 5.Combine table header and data
    const combineDOM = document.createElement('div')
    combineDOM.appendChild(theadDOMCopy)
    if (mode === 'all') {
      combineDOM.appendChild(bodyDOMCopy)
    }

    // 6.Export
    const workBook = XLSX.utils.table_to_book(combineDOM as HTMLElement)
    XLSX.writeFile(workBook, `${ filename }.xlsx`)
  } catch (error) {
    hookComponent.$message({
      type: 'error',
      content: `${ i18n.global.t('system.page.export') }${ i18n.global.t('system.tips.fail') }`
    })
  }
}

interface IExportTable {
  table: any
  filename?: string
  exceptIndex?: Array<number>
  mode?: 'all' | 'header'
  columnFilterMethod?: FilterMEthod
}

type FilterMEthod = ({ column, row }: any) => boolean
