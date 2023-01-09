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
            <v-window-item value="tabStockLocation">
              <tabStockLocation ref="tabStockLocationRef" />
            </v-window-item>
            <v-window-item value="tabStock">
              <tabStock ref="tabStockRef" />
            </v-window-item>
          </v-window>
        </v-card-text>
      </v-card>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { ref, reactive, onMounted, watch, nextTick } from 'vue'
import i18n from '@/languages/i18n'
import tabStockLocation from './tabStockLocation.vue'
import tabStock from './tabStock.vue'

const tabStockLocationRef = ref()
const tabStockRef = ref()

const tabsConfig = [
  {
    value: 'tabStockLocation',
    icon: 'mdi-database',
    tabName: i18n.global.t('wms.stockManagement.stockLocation')
  },
  {
    value: 'tabStock',
    icon: 'mdi-warehouse',
    tabName: i18n.global.t('wms.stockManagement.stock')
  }
]

const data = reactive({
  activeTab: '',
  isLoadstockLocation: false,
  isLoadstock: false
})

const method = reactive({
  changeTabs: (e: any): void => {
    nextTick(() => {
      switch (e) {
        case 'tabStockLocation':
          // Tips：Must be write the nextTick so that can get DOM!!
          if (tabStockLocationRef?.value?.getStockLocationList) {
            tabStockLocationRef.value.getStockLocationList()
          }
          break
        case 'tabStock':
          if (tabStockRef?.value?.getStockList) {
            tabStockRef.value.getStockList()
          }
          break
      }
    })
  }
})

onMounted(() => { })
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
