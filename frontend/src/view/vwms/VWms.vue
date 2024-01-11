<!--
 @name : VWms.vue
 @Description : 
 @author : saal
 @date : 2023/10/6
-->
<template>
  <div id="vwms" class="VWms-container">
    <Transition name="fade">
      <loading-page v-show="loading" :progress="loadingProgress" />
    </Transition>

    <div class="VWms-header">
      <div class="tooltip">
        <v-btn icon variant="text" @click="handleShowRightContainer">
          <v-tooltip activator="parent" location="bottom">{{ showRightContainer ? '收起' : '展开' }}右栏</v-tooltip>
          <v-icon :icon="`mdi-arrow-expand-${showRightContainer?'right':'left'}`"></v-icon>
        </v-btn>

        <v-btn icon variant="text" @click="toggleFullScreen">
          <v-tooltip activator="parent" location="bottom">全屏</v-tooltip>
          <v-icon :icon="`mdi-arrow-${fullScreen?'collapse':'expand'}`"></v-icon>
        </v-btn>

        <v-btn icon variant="text" @click="handleBreak">
          <v-tooltip activator="parent" location="bottom">回退</v-tooltip>
          <v-icon icon="mdi-reply"></v-icon>
        </v-btn>
        <v-btn v-if="presentData?.type === 'shelf' || presentData?.type === 'shelfItem'" @click="handleShelfGrid">
          {{ showShelfGrid ? '关闭' : '打开' }}货架二维视图
        </v-btn>
      </div>
    </div>
    <div class="VWMSIframe">
      <iframe
        ref="VWmsIframe"
        height="100%"
        src="/unity/index.html"
        width="100%"
      />
    </div>
    <div>
      <transition name="slide-fade-grid">
        <shelf-grid
          v-if="presentData?.type === 'shelf' && showShelfGrid || presentData?.type === 'shelfItem' && showShelfGrid"
          :select-index="selectShelfItemTagNumber"
          :shelf-grid-data="shelfGridData"
          @selectShelfItem="handleShelfGridSelect"
        ></shelf-grid>
      </transition>
    </div>
    <transition name="slide-fade-right">
      <div v-if="showRightContainer" class="right-container">
        <Detail :present-data="presentData"></Detail>
        <Bar :bar-data="barData"></Bar>
        <Pie :pie-data="pieData"></Pie>
      </div>
    </transition>
  </div>
</template>

<script lang="ts" setup>
import { useRoute } from 'vue-router'
import { onMounted, reactive, ref, watch } from 'vue'
import _ from 'lodash'
import screenfull from 'screenfull'
import LoadingPage from '@/view/vwms/LoadingPage.vue'
import { getGroupsData, handlePostJson } from '@/view/vwms/types/handleData'
import {
  factoryDataType,
  factoryInfoType,
  factoryShowDataType,
  formatDataType,
  productDataType,
  productInfoType,
  shelfDataType,
  shelfGridDataType,
  shelfItemDataType,
  shelfItemInfoType,
  shelfItemShowDataType,
  shelfShowDataType,
  warehouseInfoType,
  warehouseShowDataType
} from '@/view/vwms/types/types'
import Detail from '@/view/vwms/Detail.vue'
import Bar from '@/view/vwms/chat/Bar.vue'
import Pie from '@/view/vwms/chat/Pie.vue'
import ShelfGrid from '@/view/vwms/ShelfGrid.vue'
import { factoryData, productData, shelfItemData, warehouseData } from '@/view/vwms/data/data'

const VWmsIframe = ref()
const route = useRoute()
const loading = ref(true)
const warehouseId = parseInt(route.query.warehouseId as string)

onMounted(async () => {
  window.addEventListener('message', unityWatch)
})
const showRightContainer = ref(false)
const handleShowRightContainer = () => {
  showRightContainer.value = !showRightContainer.value
}
const showShelfGrid = ref(true)
const handleShelfGrid = () => {
  showShelfGrid.value = !showShelfGrid.value
}
const fullScreen = ref(false)
const toggleFullScreen = () => {
  if (screenfull.isEnabled) {
    const vwms = document.getElementById('vwms')
    screenfull.toggle(vwms!)
    fullScreen.value = !fullScreen.value
  }
}
const handleBreak = () => {
  VWmsIframe.value.contentWindow.parent.postMessage({
    guid: '',
    event: 'returnToUpper'
  }, '*')
}

const handleShelfGridSelect = (index) => {
  selectObjectList.push({
    itemName: 'shelfItem',
    itemId: index
  })
  VWmsIframe.value.contentWindow.parent.postMessage({
    guid: `shelfItem-${ index }`,
    event: 'selectShelfItem'
  }, '*')
}

const unityWatch = ({ data }) => {
  const {
    event,
    guid
  } = data
  const allowEvent = ['loadSuccess', 'select', 'selectBreak', 'loadingProgress']
  const unityEventHandleMap = {
    loadSuccess: handleLoadSuccess,
    select: handleSelect,
    selectBreak: handleSelectBreak,
    loadingProgress: handleLoadingProgress
  }
  if (allowEvent.includes(event)) {
    unityEventHandleMap[event](guid)
  }
}
const handleLoadSuccess = async () => {
  await initData()
  startPostJson()
  setTimeout(() => {
    loading.value = false
    showRightContainer.value = true
  }, 2500)
}

interface selectObjectType {
  itemName: string,
  itemId: number
}

const selectObjectList = reactive<selectObjectType[]>([])
const presentData = ref<warehouseShowDataType | factoryShowDataType | shelfShowDataType | shelfItemShowDataType>()

const handleSelect = (guid: string) => {
  const itemName = guid.split('-')[0]
  const itemId = parseInt(guid.split('-')[1])
  const allowSelectItemName = ['factory', 'shelf', 'shelfItem']
  if (allowSelectItemName.includes(itemName)) {
    selectObjectList.push({
      itemName,
      itemId
    })
  }
}

const handleSelectBreak = () => {
  if (selectObjectList.length > 1) {
    if (selectObjectList.length > 4) {
      selectObjectList.splice(2, selectObjectList.length - 2)
    } else {
      selectObjectList.pop()
    }
  }
}
const loadingProgress = ref()
const handleLoadingProgress = (progress) => {
  loadingProgress.value = Math.ceil(progress * 100)
}

const warehouseInfo = ref<warehouseInfoType>(warehouseData)
const factoryInfo = ref<factoryInfoType[]>(factoryData)
const shelfItemInfo = ref<shelfItemInfoType[]>(shelfItemData)
const productInfo = ref<productInfoType[]>(productData)
const formatData = ref<factoryDataType[]>([])
const selectFactoryData = ref<factoryDataType>()
const selectShelfName = ref()
const selectShelfItemTagNumber = ref()
const barData = ref()
const pieData = ref()

// 仓库 名称 地址 负责人 联系电话 email 库区数 总货物数
// 饼 存货类别占比(商品的类别) 柱 仓库库存大小
const getWarehouseData = (): warehouseShowDataType => {
  getWarehouseChatData()
  return {
    ...warehouseInfo.value!,
    factory_number: factoryInfo.value.length,
    warehouse_product_number: _.sumBy(productInfo.value, 'qty'),
    type: 'warehouse'
  }
}
// 仓库的图形数据
const getWarehouseChatData = () => {
  const barGroupsData = getGroupsData(productInfo.value, 'spu_name', 'qty')
    .slice(0, 6)
  const barTitle = '库存商品量'
  barData.value = {
    barTitle,
    barGroupsData
  }

  const pieGroupsData = _.map(formatData.value, (obj) => {
    const totalQty = _.sumBy(obj.shelves, (shelf) => _.sumBy(shelf.shelfItems, (item) => _.sumBy(item.product, 'qty')))
    return {
      name: obj.area_name,
      value: totalQty
    }
  })

  const xData = _.map(pieGroupsData, 'name')
  const yData = _.map(pieGroupsData, 'value')
  const pieTitle = '库区容量'
  pieData.value = {
    pieTitle,
    xData,
    yData
  }
}

// 库区 名称 类型 货架数 库位数 剩余库位 已用空间
// 饼  存货类别占比 柱 货架库存大小
const getFactoryData = (id: number): factoryShowDataType => {
  const factory = _.filter(formatData.value, { id })[0]
  selectFactoryData.value = factory
  const factoryShelf = factory.shelves
  const factoryShelfItemNumber = _.sumBy(factoryShelf, shelf => shelf.layer * shelf.column)
  const usedFactoryShelfItem = _.flatMap(factoryShelf, shelf => _.flatMap(shelf.shelfItems, shelfItem => shelfItem.product))
  getFactoryChatData(usedFactoryShelfItem, factoryShelf)
  return {
    area_name: factory.area_name,
    area_property: factory.area_property,
    shelf_number: factoryShelf.length,
    shelf_item_number: factoryShelfItemNumber,
    surplus_number: factoryShelfItemNumber - usedFactoryShelfItem.length,
    factory_product_number: _.sumBy(usedFactoryShelfItem, 'qty'),
    type: 'factory'
  }
}

// 库区的图形数据
const getFactoryChatData = (usedFactoryShelfItem, factoryShelf) => {
  const barGroupsData = getGroupsData(usedFactoryShelfItem, 'spu_name', 'qty')
    .slice(0, 6)
  const barTitle = '库区库存商品量'
  barData.value = {
    barTitle,
    barGroupsData
  }
  const pieGroupsData = _.map(factoryShelf, obj => {
    const totalQty = _.sumBy(obj.shelfItems, (item: shelfItemDataType) => _.sumBy(item.product, 'qty'))
    return {
      name: obj.shelf_name,
      value: totalQty
    }
  })

  const xData = _.map(pieGroupsData, 'name')
  const yData = _.map(pieGroupsData, 'value')
  const pieTitle = '货架容量'
  pieData.value = {
    pieTitle,
    xData,
    yData
  }
}

const shelfGridData = ref<shelfGridDataType>()
// 货架 名称 规格 闲置位 总货物数
// 饼 存货类别占比 柱 库位存量大小
const getShelfData = (id: number): shelfShowDataType => {
  const shelf: shelfDataType = selectFactoryData.value?.shelves.filter(shelf => shelf.shelf_name === `${ id }`)[0] as shelfDataType
  const shelfProduct: productDataType[] = _.flatMap(shelf.shelfItems, shelfItem => shelfItem.product)
  selectShelfName.value = shelf.shelf_name
  getShelfChatData(shelfProduct, shelf)
  // 货架二维图数据
  shelfGridData.value = {
    shelf_name: `${ shelf.shelf_name }号货架`,
    layer: shelf.layer,
    column: shelf.column,
    products: shelf.shelfItems.map(shelfItem => {
      if (shelfItem.product.length > 0) return parseInt(shelfItem.tag_number)
      return 0
    })
  }

  return {
    shelf_name: `${ shelf.shelf_name }号货架`,
    layer: shelf.layer,
    column: shelf.column,
    surplus_number: shelf.layer * shelf.column - _.sumBy(shelf.shelfItems, shelfItem => shelfItem.product.length),
    shelf_product_number: _.sumBy(shelfProduct, 'qty'),
    type: 'shelf'
  }
}

// 货架的图形数据
const getShelfChatData = (shelfProduct, shelf) => {
  const barGroupsData = getGroupsData(shelfProduct, 'sku_name', 'qty')
  const barTitle = '货架商品规格量'
  barData.value = {
    barTitle,
    barGroupsData
  }
  const pieGroupsData = _.map(shelf.shelfItems, (item) => {
    const totalQty = _.sumBy(item.product, 'qty')
    return {
      name: item.location_name,
      value: totalQty
    }
  })

  const xData = _.map(pieGroupsData, 'name')
  const yData = _.map(pieGroupsData, 'value')
  const pieTitle = '库位容量'
  pieData.value = {
    pieTitle,
    xData,
    yData
  }
}

// 货物 层号 库位号 库位编码
// 列表 商品名称 规格名称 数量 可用数量 货主
const getShelfItemData = (id: number): shelfItemShowDataType | any => {
  const shelf = _.filter(selectFactoryData.value?.shelves, { shelf_name: selectShelfName.value })[0]
  const shelfItem = _.filter(shelf.shelfItems, { tag_number: `${ id }` })[0]
  selectShelfItemTagNumber.value = id
  if (shelfItem) {
    return {
      ...shelfItem,
      type: 'shelfItem'
    }
  }
  return {
    tag_number: id,
    location_name: '无',
    layer_number: Math.ceil(id / shelf.column),
    product: [],
    type: 'shelfItem'
  }
}

watch(selectObjectList, () => {
  const targetData = selectObjectList[selectObjectList.length - 1]
  const handleMap = {
    playground: getWarehouseData,
    factory: getFactoryData,
    shelf: getShelfData,
    shelfItem: getShelfItemData
  }
  console.log(targetData)
  presentData.value = handleMap[targetData.itemName](targetData.itemId)
})

const initData = async () => {
  // try {
  //   const promises = [
  //     await getWarehouse(warehouseId),
  //     await getWarehouseAreaSelect(warehouseId),
  //     await getGoodsLocation(),
  //     await getWarehouseProduct({ warehouse_id: warehouseId })
  //   ]
  //   const responses: any = await Promise.all(promises)
  //   warehouseInfo.value = responses[0].data.data
  //   factoryInfo.value = responses[1].data.data
  //   shelfItemInfo.value = responses[2].data.data
  //   productInfo.value = responses[3].data.data
  // } catch (error) {
  //   loading.value = true
  // }
  selectObjectList.push({
    itemName: 'playground',
    itemId: warehouseId
  })
  const data: formatDataType = handlePostJson(factoryInfo.value, shelfItemInfo.value, productInfo.value)
  formatData.value = data.area
}

const startPostJson = () => {
  const data: formatDataType = handlePostJson(factoryInfo.value, shelfItemInfo.value, productInfo.value)
  try {
    VWmsIframe.value.contentWindow.parent.postMessage({
      guid: JSON.stringify(data),
      event: 'sendData'
    }, '*')
  } catch (error) {
    console.log(error)
  }
}

</script>

<style lang="less" scoped>
* {
  box-sizing: border-box;
}

.VWms-container {
  letter-spacing: 1px;
  width: 100%;
  height: 100%;
  overflow: hidden;
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: #317972;
}

:deep(.v-divider) {
  margin: 10px 0;
}

.VWms-header {
  position: absolute;
  top: 10px;
  left: 10px;
  z-index: 10;
  display: flex;
  align-items: center;
  background-color: rgba(255, 255, 255, 0.9);
  padding: 5px;
  border-radius: 4px;
}

.VWMSIframe {
  width: 100%;
  height: 100%;
  border: none;
  position: absolute;
}

.right-container {
  display: grid;
  grid-template-columns: 400px;
  grid-gap: 15px;
  position: absolute;
  right: 0;
  height: 100%;
  overflow: auto;
  padding: 10px;
  justify-content: center

}

.flex-center {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

::-webkit-scrollbar {
  display: none
}

.VWms-card {
  border-radius: 5px;
  padding: 15px;
  width: 100%;
  background-color: rgba(255, 255, 255, 0.9);
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

.slide-fade-grid-enter-active {
  transition: all 0.3s ease-out;
}

.slide-fade-grid-leave-active {
  transition: all 0.5s ease-out;
}

.slide-fade-grid-enter-from,
.slide-fade-grid-leave-to {
  transform: translateX(-200px);
  opacity: 0;
}

.slide-fade-right-enter-active {
  transition: all 0.3s ease-out;
}

.slide-fade-right-leave-active {
  transition: all 0.5s ease-out;
}

.slide-fade-right-enter-from,
.slide-fade-right-leave-to {
  transform: translateX(200px);
  opacity: 0;
}
</style>