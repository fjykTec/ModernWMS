import { RouteMeta } from 'vue-router'

export interface CustomerRouterProps {
  name: string
  module?: string
  path: string
  directory: string
  redirect: string
  component: any
  children?: CustomerRouterProps[]
  meta?: RouteMeta
}
