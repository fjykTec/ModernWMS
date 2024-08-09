import { MenuItem } from '@/types/System/Store'
import { CustomerRouterProps } from '@/types/System/Router'
import { SideBarMenu } from '@/types/Home/Home'
import { store } from '@/store'
import i18n from '@/languages/i18n'

// Convert menu permissions to data required for dynamic routing
export function menusToRouter(menuList: MenuItem[]): CustomerRouterProps[] {
  const result: CustomerRouterProps[] = []
  for (const menu of menuList) {
    result.push({
      name: menu.vue_path,
      module: menu.module,
      path: `/${ menu.vue_path }`,
      directory: menu.vue_directory,
      redirect: '',
      component: null
    })
  }
  return result
}

// Convert the menu permission returned from the back end to the sidebar
export function menusToSideBar(): SideBarMenu[] {
  const result: SideBarMenu[] = [
    { icon: GetModuleAndIcon('homepage'), lable: i18n.global.t('router.sideBar.homepage'), routerPath: 'homepage', showDetail: false }
  ]
  const menuList: MenuItem[] = store.getters['user/menulist']

  for (const menu of menuList) {
    // Get the module index and check whether this group exists
    const moduleIndex = result.findIndex((item) => item.lable === i18n.global.t(`router.sideBar.${ menu.module }`))
    const lable = GetMenuNameAndModule(menu.vue_path)
    if (lable) {
      // Primary menu
      if (!menu.module) {
        result.push({
          lable,
          icon: GetModuleAndIcon(menu.vue_path),
          routerPath: menu.vue_path
        })
        continue
      }
      // Secondary menu
      if (moduleIndex > -1) {
        result[moduleIndex].children?.push({
          lable,
          icon: GetModuleAndIcon(menu.vue_path),
          routerPath: menu.vue_path
        })
      } else {
        result.push({
          lable: i18n.global.t(`router.sideBar.${ menu.module }`),
          icon: GetModuleAndIcon(menu.module),
          showDetail: false,
          children: [
            {
              lable,
              icon: GetModuleAndIcon(menu.vue_path),
              routerPath: menu.vue_path
            }
          ]
        })
      }
    }
  }
  result.push({ icon: GetModuleAndIcon('homepage'), lable: i18n.global.t('router.sideBar.vwms'), routerPath: 'vwms', showDetail: false })
  // result.push({ icon: GetModuleAndIcon('homepage'), lable: i18n.global.t('router.sideBar.largeScreen'), routerPath: 'largeScreen', showDetail: false })
  return result
}

// Get the menu name, module and icon
function GetMenuNameAndModule(path: string): string {
  switch (path) {
    case 'homepage':
      return i18n.global.t('router.sideBar.homepage')
    case 'ownerOfCargo':
      return i18n.global.t('router.sideBar.ownerOfCargo')
    case 'menuBasicSettings':
      return i18n.global.t('router.sideBar.menuBasicSettings')
    case 'userManagement':
      return i18n.global.t('router.sideBar.userManagement')
    case 'commodityCategorySetting':
      return i18n.global.t('router.sideBar.commodityCategorySetting')
    case 'commodityManagement':
      return i18n.global.t('router.sideBar.commodityManagement')
    case 'userRoleSetting':
      return i18n.global.t('router.sideBar.userRoleSetting')
    case 'companySetting':
      return i18n.global.t('router.sideBar.companySetting')
    case 'freightSetting':
      return i18n.global.t('router.sideBar.freightSetting')
    case 'warehouseSetting':
      return i18n.global.t('router.sideBar.warehouseSetting')
    case 'customer':
      return i18n.global.t('router.sideBar.customer')
    case 'print':
      return i18n.global.t('router.sideBar.print')
    case 'supplier':
      return i18n.global.t('router.sideBar.supplier')
    case 'roleMenu':
      return i18n.global.t('router.sideBar.roleMenu')
    case 'stockManagement':
      return i18n.global.t('router.sideBar.stockManagement')
    case 'warehouseProcessing':
      return i18n.global.t('router.sideBar.warehouseProcessing')
    case 'warehouseMove':
      return i18n.global.t('router.sideBar.warehouseMove')
    case 'warehouseFreeze':
      return i18n.global.t('router.sideBar.warehouseFreeze')
    case 'warehouseAdjust':
      return i18n.global.t('router.sideBar.warehouseAdjust')
    case 'warehouseTaking':
      return i18n.global.t('router.sideBar.warehouseTaking')
    case 'deliveryManagement':
      return i18n.global.t('router.sideBar.deliveryManagement')
    case 'stockAsn':
      return i18n.global.t('router.sideBar.stockAsn')
    case 'saftyStock':
      return i18n.global.t('router.sideBar.saftyStock')
    case 'deliveryStatistic':
      return i18n.global.t('router.sideBar.deliveryStatistic')
    case 'asnStatistic':
      return i18n.global.t('router.sideBar.asnStatistic')
    case 'stockageStatistic':
      return i18n.global.t('router.sideBar.stockageStatistic')
      case 'largeScreen':
        return i18n.global.t('router.sideBar.largeScreen')
    default:
      return ''
  }
}
function GetModuleAndIcon(name: string) {
  switch (name) {
    case 'baseModule':
      return 'cog'
    case 'homepage':
      return 'home'
    case 'stockManagement':
      return 'warehouse'
    case 'stockAsn':
    case 'asnStatistic':
      return 'home-silo'
    case 'warehouseWorkingModule':
      return 'account-hard-hat-outline '
    case 'deliveryManagement':
    case 'deliveryStatistic':
      return 'cube-send'
    case 'companySetting':
      return 'office-building'
    case 'roleMenu':
      return 'menu'
    case 'userRoleSetting':
      return 'account-box'
    case 'userManagement':
      return 'account'
    case 'commodityCategorySetting':
      return 'format-list-bulleted-type'
    case 'commodityManagement':
      return 'cart-minus'
    case 'ownerOfCargo':
      return 'account-credit-card'
    case 'freightSetting':
      return 'cash-multiple'
    case 'warehouseSetting':
      return 'warehouse'
    case 'warehouseProcessing':
      return 'vector-combine'
    case 'warehouseMove':
      return 'file-document-arrow-right-outline'
    case 'warehouseFreeze':
      return 'lock-outline'
    case 'warehouseAdjust':
      return 'image-auto-adjust'
    case 'warehouseTaking':
      return 'ballot-recount-outline'
    case 'customer':
      return 'account-box-outline'
    case 'print':
      return 'printer-outline'
    case 'supplier':
      return 'account-badge'
    case 'statisticAnalysis':
      return 'chart-pie'
    case 'saftyStock':
      return 'alarm-light'
    case 'stockageStatistic':
      return 'calendar-month'
      case 'largeScreen':
      return 'monitor-screenshot'
    default:
      return ''
  }
}
