import VXETable from 'vxe-table'
import XEUtils from 'xe-utils'
import VXETablePluginExportXLSX from 'vxe-table-plugin-export-xlsx'
import 'vxe-table/lib/style.css'

/**
 * Format date.
 * @param {String} format: default value is 'yyyy-MM-dd HH:mm:ss'
 */
VXETable.formats.add('formatDate', ({ cellValue }, format) => {
  const date = new Date(cellValue)
  if (!cellValue || !date || XEUtils.toDateString(date, 'yyyy-MM-dd') === '1900-01-01' || XEUtils.toDateString(date, 'yyyy-MM-dd') === '1000-01-01') {
    return ''
  }
  return XEUtils.toDateString(date, format || 'yyyy-MM-dd HH:mm:ss')
})

VXETable.use(VXETablePluginExportXLSX)

export { VXETable }
