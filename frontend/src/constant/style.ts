import { store } from '@/store'

interface pageHasElement {
  hasPager?: boolean
  hasTab?: boolean
  hasOperateBtn?: boolean
  hasToolBar?: boolean
}

export const primaryColor = '#9C27B0'
export const primaryLightColor = '#ECE7F6'
export const lightGrey = '#999999'
export const errorColor = '#BA2828'

export const SYSTEM_HEIGHT = {
  HEADER: 60,
  TAB: 70,
  OPERATE_BAR: 52,
  VXE_PAGER: 48,
  SELECT_TABLE: 500,
  TOOLBAR: 40
}

// The height of the content card
export const computedCardHeight = ({ hasTab = true, hasOperateBtn = true }: pageHasElement) => {
  const clientHeight = store.getters['system/clientHeight']
  const EXTRA_MARGIN = 100

  let res = clientHeight - SYSTEM_HEIGHT.HEADER - EXTRA_MARGIN

  if (hasTab) {
    res -= SYSTEM_HEIGHT.TAB
  }
  if (hasOperateBtn) {
    res -= SYSTEM_HEIGHT.OPERATE_BAR
  }

  return `${ res }px`
}

// The height of the table
export const computedTableHeight = ({ hasPager = true, hasTab = true, hasOperateBtn = true }: pageHasElement) => {
  const clientHeight = store.getters['system/clientHeight']
  const EXTRA_MARGIN = 100

  let res = clientHeight - SYSTEM_HEIGHT.HEADER - EXTRA_MARGIN

  if (hasPager) {
    res -= SYSTEM_HEIGHT.VXE_PAGER
  }
  if (hasTab) {
    res -= SYSTEM_HEIGHT.TAB
  }
  if (hasOperateBtn) {
    res -= SYSTEM_HEIGHT.OPERATE_BAR
  }

  return `${ res }px`
}

// The height of the table container in select modal.
export const computedSelectTableSearchHeight = ({ hasPager = true, hasToolBar = false }: pageHasElement) => {
  let res = SYSTEM_HEIGHT.SELECT_TABLE

  if (hasPager) {
    res += SYSTEM_HEIGHT.VXE_PAGER
  }
  if (hasToolBar) {
    res += SYSTEM_HEIGHT.TOOLBAR
  }

  return `${ res }px`
}
