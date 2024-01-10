<!--
 @name : Detail.vue
 @Description : 
 @author : saal
 @date : 2023/10/9
-->
<template>
  <div class="warehouse-info VWms-card">
    <Transition name="slide-fade">
      <div v-if="props.presentData?.type === 'warehouse'">
        <div class="header">
          <p class="main-title">{{ props.presentData?.warehouse_name }}</p>
          <p class="sub-title">{{ props.presentData?.address }}</p>
        </div>
        <div class="text-grid">
          <p>负责人:</p>
          <p>{{ props.presentData?.manager }}</p>
          <p>联系电话:</p>
          <p>{{ props.presentData?.contact_tel }}</p>
          <p>Email:</p>
          <p>{{ props.presentData?.email }}</p>
        </div>
        <v-divider></v-divider>
        <div class="data-grid">
          <div class="column-center data-container">
            <p class="data-label">库区数</p>
            <p class="data-number">{{ props.presentData?.factory_number }}</p>
          </div>
          <div class="column-center data-container">
            <p class="data-label">库存量</p>
            <p class="data-number">{{ props.presentData?.warehouse_product_number }}</p>
          </div>
        </div>
      </div>
    </Transition>
    <Transition name="slide-fade">
      <div v-show="props.presentData?.type === 'factory'">
        <div class="header">
          <p class="main-title">{{ props.presentData?.area_name }}</p>
          <p class="sub-title">{{ formatAreaProperty(props.presentData?.area_property) }}</p>
        </div>
        <v-divider></v-divider>
        <div class="data-grid">
          <div class="column-center data-container">
            <p class="data-label">总货架数</p>
            <p class="data-number">{{ props.presentData?.shelf_number }}</p>
          </div>
          <div class="column-center data-container">
            <p class="data-label">总库位数</p>
            <p class="data-number">{{ props.presentData?.shelf_item_number }}</p>
          </div>
          <div class="column-center data-container">
            <p class="data-label">剩余库位</p>
            <p class="data-number">{{ props.presentData?.surplus_number }}</p>
          </div>
          <div class="column-center data-container">
            <p class="data-label">总货物数</p>
            <p class="data-number">{{ props.presentData?.factory_product_number }}</p>
          </div>
        </div>
      </div>
    </Transition>
    <Transition name="slide-fade">
      <div v-show="props.presentData?.type === 'shelf'">
        <div class="flex-center-between">
          <div class="header">
            <p class="main-title">{{ props.presentData?.shelf_name }}</p>
          </div>
          <p><span class="em">货架规格: </span>{{ props.presentData?.layer }} * {{ props.presentData?.column }}</p>
        </div>
        <v-divider></v-divider>
        <div class="data-grid">
          <div class="column-center data-container">
            <p class="data-label">闲置库位</p>
            <p class="data-number">{{ props.presentData?.surplus_number }}</p>
          </div>
          <div class="column-center data-container">
            <p class="data-label">总货物数</p>
            <p class="data-number">{{ props.presentData?.shelf_product_number }}</p>
          </div>
        </div>
      </div>
    </Transition>
    <Transition name="slide-fade">
      <div v-show="props.presentData?.type === 'shelfItem'">
        <div class="header">
          <p class="main-title">{{ props.presentData?.tag_number }}号库位</p>
        </div>
        <div class="text-grid">
          <p>层号:</p>
          <p>{{ props.presentData?.layer_number }}</p>
          <p>货架编码:</p>
          <p>{{ props.presentData?.location_name }}</p>
        </div>
        <v-divider></v-divider>
        <div>
          <div v-if="props.presentData?.product?.length === 0">空库位</div>
          <div v-else class="product-info-container">
            <vxe-table
              align="center"
              :data="props.presentData?.product"
              border
              height="160"
              :row-config="{isHover: true}"
            >
              <vxe-column type="seq" width="60"></vxe-column>
              <vxe-column field="spu_name" title="货物名称"></vxe-column>
              <vxe-column field="sku_name" title="规格名称"></vxe-column>
              <vxe-column field="qty" title="数量"></vxe-column>
            </vxe-table>
          </div>
        </div>
      </div>
    </Transition>
  </div>
</template>

<script setup lang="ts">
import { formatAreaProperty } from '@/utils/format/formatWarehouse'

const props = defineProps(['presentData'])
</script>

<style scoped lang="less">
.column-center {
  display: flex;
  flex-direction: column;
  align-items: center;

}
.warehouse-info {
  height: 300px;
  .header {
    .main-title {
      font-weight: 700;
      font-size: 18px;
      margin-bottom: 5px;
    }
    .sub-title {
      font-size: 14px;
      color: #64676b;
    }
  }
  .text-grid {
    display: grid;
    grid-template-columns: 80px 1fr;
    grid-gap: 5px;
    margin-top: 5px;
  }
  .data-grid {
    display: grid;
    margin: 0 20px;
    grid-template-columns: repeat(2, 1fr);
    grid-gap: 15px;
    justify-content: center;
    align-items: center;
    .data-container {
      background-color: white;
      border-radius: 5px;
      padding: 5px;
    }
    .data-label{
      color: #606060;
    }
    .data-number {
      font-size: 18px;
      font-weight: 600;
      color: #2a2a2a;
    }
  }
  .product-info-container {
    margin: 10px 0;
    background-color: #FFFFFF;
    border-radius: 5px;
    text-align: center;
  }
}
.slide-fade-enter-active {
  transition: all 0.4s ease-out;
}
.slide-fade-enter-from{
  transform: translateX(300px);
  opacity: 0;
}
</style>