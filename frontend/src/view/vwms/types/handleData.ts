import _ from 'lodash'
import {
  factoryDataType,
  factoryInfoType,
  formatDataType,
  productInfoType,
  shelfDataType,
  shelfItemInfoType
} from '@/view/vwms/types/types'

export const handlePostJson = (warehouseAreaInfo: factoryInfoType[], goodsLocationInfo: shelfItemInfoType[], productInfo: productInfoType[]) => {
  const data: formatDataType = {
    area: [],
  }
  warehouseAreaInfo.forEach(areaItem => {
    const warehouseProduct = goodsLocationInfo.filter(item => item.warehouse_area_id == areaItem.id)
    const shelfList = Object.values(_.groupBy(warehouseProduct, 'shelf_number'))
    const shelves: shelfDataType[] = []
    shelfList.forEach(item => {
      const { layer, column } = getShelfSpec(item)
      const shelf:shelfDataType = {
        shelf_name: item[0].shelf_number,
        layer,
        column,
        shelfItems: getShelfProduct(item, productInfo)
      }
      shelves.push(shelf)
    })
    const areaItemInfo:factoryDataType = {
      ...areaItem,
      shelves
    }
    data.area.push(areaItemInfo)
  })
  return data
}

export const getShelfSpec = (shelfItemList) => {
  const maxLayerObj: any = _.maxBy(shelfItemList, 'layer_number')
  const maxTagNumber = parseInt(maxLayerObj.tag_number)
  let layer = parseInt(maxLayerObj.layer_number)
  let column = Math.ceil(maxTagNumber / layer)
  if (layer < 3 && column < 3) {
    layer = 3
    column = 3
  }
  return { layer, column }
}

const getShelfProduct = (shelfItemList, productInfo) => _.flatMap(shelfItemList, (shelfItem) => {
  const product = _.filter(productInfo, { goods_location_id: shelfItem.id })
  return { ...shelfItem, product: [...product] }
})

interface groupsDataType{
  name:string,
  value:number
}
export const getGroupsData = (data:any, attr:string, val:string):groupsDataType[] => {
  const groups = _.groupBy(data, attr)
  return _.map(groups, (items, spu_name) => {
    const total_price = _.sumBy(items, val)
    return { value: total_price, name: spu_name }
  })
}
