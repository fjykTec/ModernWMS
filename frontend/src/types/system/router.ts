export interface CustomerRouterProps {
  name: string
  path: string
  directory: string
  redirect: string
  component: any
  children: CustomerRouterProps[]
}