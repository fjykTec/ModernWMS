<!-- Warehouse Setting -->
<template>
  <div class="container">
    <div>
      <v-tabs v-model="data.activeTab" stacked @update:model-value="method.changeTabs">
        <v-tab v-for="(item, index) of tabsConfig" :key="index" :value="item.value">
          <v-icon>{{ item.icon }}</v-icon>
          <p class="tabItemTitle">{{ item.tabName }}</p>
        </v-tab>
      </v-tabs>

      <!-- Main Content -->
      <v-card class="mt-5">
        <v-card-text>
          <v-window v-model="data.activeTab">
            <v-window-item value="tabWarehouse">
              <tab-warehouse ref="tabWarehouseRef" />
            </v-window-item>
            <v-window-item value="tabReservoir">
              <tab-reservoir ref="tabReservoirRef" />
            </v-window-item>
            <v-window-item value="tabLocation">
              <tab-location ref="tabLocationRef" />
            </v-window-item>
          </v-window>
        </v-card-text>
      </v-card>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { ref, reactive, onMounted, nextTick } from 'vue'
import i18n from '@/languages/i18n'
import tabWarehouse from './tab-warehouse.vue'
import tabReservoir from './tab-reservoir.vue'
import tabLocation from './tab-location.vue'

const tabWarehouseRef = ref()
const tabReservoirRef = ref()
const tabLocationRef = ref()

const tabsConfig = [
  {
    value: 'tabWarehouse',
    icon: 'mdi-warehouse',
    tabName: i18n.global.t('base.warehouseSetting.warehouseSetting')
  },
  {
    value: 'tabReservoir',
    icon: 'mdi-texture-box',
    tabName: i18n.global.t('base.warehouseSetting.reservoirSetting')
  },
  {
    value: 'tabLocation',
    icon: 'mdi-library-shelves ',
    tabName: i18n.global.t('base.warehouseSetting.locationSetting')
  }
]

const data = reactive({
  activeTab: '',
  isLoadWarehouseData: false,
  isLoadReservoirData: false,
  isLoadLocationData: false
})

const method = reactive({
  changeTabs: (e: any): void => {
    nextTick(() => {
      switch (e) {
        case 'tabWarehouse':
          // Tips：Must be write the nextTick so that can get DOM!!
          if (tabWarehouseRef?.value?.getWarehouseList) {
            tabWarehouseRef.value.getWarehouseList()
          }
          break
        case 'tabReservoir':
          if (tabReservoirRef?.value?.getWarehouseAreaList) {
            tabReservoirRef.value.getWarehouseAreaList()
          }
          break
        case 'tabLocation':
          if (tabLocationRef?.value?.getGoodsLocationList) {
            tabLocationRef.value.getGoodsLocationList()
          }
          break
      }
    })
  }
})

onMounted(() => {})
</script>

<style scoped lang="less">
.operateArea {
  width: 100%;
  min-width: 760px;
  display: flex;
  align-items: center;
  border-radius: 10px;
  padding: 0 10px;
}

.col {
  display: flex;
  align-items: center;
}
</style>
