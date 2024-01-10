import http from '@/utils/http/request'
import { PageConfigProps } from '@/types/System/Form'
import { PrintSolutionVO, PrintSolutionGetByPathVo } from '@/types/Base/PrintSolution'

// Find Data by Pagination
export const getPrintSolutionList = (data: PageConfigProps) => http({
  url: '/PrintSolution/list',
  method: 'post',
  data
})
// Get user menu form
export const addPrintSolution = (data: PrintSolutionVO) => http({
  url: '/PrintSolution',
  method: 'post',
  data
})
// Update form
export const updatePrintSolution = (data: PrintSolutionVO) => http({
  url: '/PrintSolution',
  method: 'put',
  data
})
// Delete form
export const deletePrintSolution = (id: number) => http({
  url: '/PrintSolution',
  method: 'delete',
  params: {
    id
  }
})
export const listByPath = (data: PrintSolutionGetByPathVo) => http({
  url: '/PrintSolution/get-by-path',
  method: 'post',
  data
})