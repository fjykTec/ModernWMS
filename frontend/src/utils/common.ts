import XEUtils from 'xe-utils'
import { useRoute } from 'vue-router'
import { SearchObject, SearchOperator } from '@/types/System/Form'
import { store } from '@/store'

export const setSearchObject = (searchForm: any, preciseSearchCols: string[] = []) => {
  const searchObjects: Array<SearchObject> = []
  try {
    for (const key in searchForm) {
      const str = key as string
      const searchValue = searchForm[str as keyof typeof searchForm]
      let operator = SearchOperator.INCLUDE
      if (preciseSearchCols.includes(key)) {
        operator = SearchOperator.EQUAL
      }

      if (searchValue && searchValue.trim() !== '') {
        searchObjects.push({
          name: key,
          operator,
          text: searchValue,
          value: searchValue
        })
      }
    }
    return searchObjects
  } catch (error) {
    return searchObjects
  }
}

// Remove item what value is 'null' or '' in array
export const removeArrayNull = (array: any) => {
  for (const obj of array) {
    Object.keys(obj).forEach((item) => {
      if (XEUtils.isString(obj[item])) {
        obj[item] = obj[item].replace(/(^\s*)|(\s*$)/g, '')
      }
      if (obj[item] === '' || obj[item] === null) {
        delete obj[item]
      }
    })
  }
  return array
}

// Remove item what value is 'null' or '' in object
export const removeObjectNull = (obj: any) => {
  const copy = JSON.parse(JSON.stringify(obj))
  Object.keys(copy).forEach((item) => {
    if (XEUtils.isString(copy[item])) {
      copy[item] = copy[item].replace(/(^\s*)|(\s*$)/g, '')
    }
    if (copy[item] === '' || copy[item] == null) {
      delete copy[item]
    }
  })
  return copy
}

// Obtain menu operation permissions
export const getMenuAuthorityList = () => {
  let AuthorityList: string[] = []

  const route = useRoute()

  const menu_name = route.path.substring(1)

  const menu_list: any[] = store.getters['user/menulist']

  const filter = menu_list.filter((item: any) => item.menu_name === menu_name)

  // Obtain permission list based on route
  if (filter.length > 0) {
    AuthorityList = filter[0].menu_actions
  } else {
    AuthorityList = []
  }

  return AuthorityList
}

// Retrieve data from cache in browser
export const getStorage = (key: string) => {
  // printCommidity...
  const value = localStorage.getItem(key)
  if (value) {
    return JSON.parse(value)
  }
  return null
}

// Store data in the browser's cache
export const setStorage = (key: string, value: any) => {
  localStorage.setItem(key, JSON.stringify(value))
}

// 获取菜单的查询条件设置
export const getMenuSearchSetting = (menu_name: string) => {
  const allSetting = getStorage('menu_search_setting')

  if (allSetting && allSetting[menu_name]) {
    return allSetting[menu_name]
  }

  return []
}

// 生成uuid方法
export const generateUUID = (): string => {
  let d = new Date().getTime()
  const uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, (c) => {
    const r = (d + Math.random() * 16) % 16 | 0
    d = Math.floor(d / 16)
    return (c === 'x' ? r : (r & 0x3) | 0x8).toString(16)
  })
  return uuid
}