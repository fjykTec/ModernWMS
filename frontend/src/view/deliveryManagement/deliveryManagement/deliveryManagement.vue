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
            <v-window-item value="tabShipment">
              <tabShipment ref="tabShipmentRef" />
            </v-window-item>
            <v-window-item value="tabPreShipment">
              <tabPreShipment ref="tabPreShipmentRef" />
            </v-window-item>
            <v-window-item value="tabNewShipment">
              <tabNewShipment ref="tabNewShipmentRef" />
            </v-window-item>
            <v-window-item value="tabGoodsToBePicked">
              <tabGoodsToBePicked ref="tabGoodsToBePickedRef" />
            </v-window-item>
            <v-window-item value="tabPicked">
              <tabPicked ref="tabPickedRef" />
            </v-window-item>
            <!-- <v-window-item value="tabToBePackaged">
              <tabToBePackaged ref="tabToBePackagedRef" />
            </v-window-item> -->
            <v-window-item value="tabPackaged">
              <tabPackaged ref="tabPackagedRef" />
            </v-window-item>
            <!-- <v-window-item value="tabToBeWeighed">
              <tabToBeWeighed ref="tabToBeWeighedRef" />
            </v-window-item> -->
            <v-window-item value="tabWeighed">
              <tabWeighed ref="tabWeighedRef" />
            </v-window-item>
            <!-- <v-window-item value="tabToBeDelivered">
              <tabToBeDelivered ref="tabToBeDeliveredRef" />
            </v-window-item> -->
            <v-window-item value="tabDelivered">
              <tabDelivered ref="tabDeliveredRef" />
            </v-window-item>
            <v-window-item value="tabSignIn">
              <tabSignIn ref="tabSignInRef" />
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
import tabShipment from './tabShipment.vue'
import tabPreShipment from './tabPreShipment.vue'
import tabNewShipment from './tabNewShipment.vue'
import tabGoodsToBePicked from './tabGoodsToBePicked.vue'
import tabPicked from './tabPicked.vue'
// import tabToBePackaged from './tabToBePackaged.vue'
import tabPackaged from './tabPackaged.vue'
// import tabToBeWeighed from './tabToBeWeighed.vue'
import tabWeighed from './tabWeighed.vue'
// import tabToBeDelivered from './tabToBeDelivered.vue'
import tabDelivered from './tabDelivered.vue'
import tabSignIn from './tabSignIn.vue'

const tabShipmentRef = ref()
const tabPreShipmentRef = ref()
const tabNewShipmentRef = ref()
const tabGoodsToBePickedRef = ref()
const tabPickedRef = ref()
const tabToBePackagedRef = ref()
const tabPackagedRef = ref()
const tabToBeWeighedRef = ref()
const tabWeighedRef = ref()
const tabToBeDeliveredRef = ref()
const tabDeliveredRef = ref()
const tabSignInRef = ref()

const tabsConfig = [
  {
    value: 'tabShipment',
    icon: 'mdi-clipboard-list',
    tabName: i18n.global.t('wms.deliveryManagement.shipment')
  },
  {
    value: 'tabPreShipment',
    icon: 'mdi-list-box-outline',
    tabName: i18n.global.t('wms.deliveryManagement.preShipment')
  },
  {
    value: 'tabNewShipment',
    icon: 'mdi-list-box',
    tabName: i18n.global.t('wms.deliveryManagement.newShipment')
  },
  {
    value: 'tabGoodsToBePicked',
    icon: 'mdi-dolly',
    tabName: i18n.global.t('wms.deliveryManagement.goodsToBePicked')
  },
  {
    value: 'tabPicked',
    icon: 'mdi-human-dolly',
    tabName: i18n.global.t('wms.deliveryManagement.picked')
  },
  // {
  //   value: 'tabToBePackaged',
  //   icon: 'mdi-package-variant',
  //   tabName: i18n.global.t('wms.deliveryManagement.toBePackaged')
  // },
  {
    value: 'tabPackaged',
    icon: 'mdi-package-variant',
    // icon: 'mdi-package-variant-closed-check',
    tabName: i18n.global.t('wms.deliveryManagement.packaged')
  },
  // {
  //   value: 'tabToBeWeighed',
  //   icon: 'mdi-basket-fill',
  //   tabName: i18n.global.t('wms.deliveryManagement.toBeWeighed')
  // },
  {
    value: 'tabWeighed',
    icon: 'mdi-basket-fill',
    // icon: 'mdi-weight',
    tabName: i18n.global.t('wms.deliveryManagement.weighed')
  },
  // {
  //   value: 'tabToBeDelivered',
  //   icon: 'mdi-send-outline',
  //   tabName: i18n.global.t('wms.deliveryManagement.toBeDelivered')
  // },
  {
    value: 'tabDelivered',
    icon: 'mdi-send-outline',
    // icon: 'mdi-send',
    tabName: i18n.global.t('wms.deliveryManagement.outOfWarehouse')
  },
  {
    value: 'tabSignIn',
    icon: 'mdi-check-circle',
    tabName: i18n.global.t('wms.deliveryManagement.signedIn')
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
        case 'tabShipment':
          // Tips：Must be write the nextTick so that can get DOM!!
          if (tabShipmentRef?.value?.getShipment) {
            tabShipmentRef.value.getShipment()
          }
          break
        case 'tabPreShipment':
          // Tips：Must be write the nextTick so that can get DOM!!
          if (tabPreShipmentRef?.value?.getPreShipment) {
            tabPreShipmentRef.value.getPreShipment()
          }
          break
        case 'tabNewShipment':
          // Tips：Must be write the nextTick so that can get DOM!!
          if (tabNewShipmentRef?.value?.getNewShipment) {
            tabNewShipmentRef.value.getNewShipment()
          }
          break
        case 'tabGoodsToBePicked':
          // Tips：Must be write the nextTick so that can get DOM!!
          if (tabGoodsToBePickedRef?.value?.getGoodsToBePicked) {
            tabGoodsToBePickedRef.value.getGoodsToBePicked()
          }
          break
        case 'tabToBePackaged':
          // Tips：Must be write the nextTick so that can get DOM!!
          if (tabToBePackagedRef?.value?.getToBePackaged) {
            tabToBePackagedRef.value.getToBePackaged()
          }
          break
        case 'tabPackaged':
          // Tips：Must be write the nextTick so that can get DOM!!
          if (tabPackagedRef?.value?.getPackaged) {
            tabPackagedRef.value.getPackaged()
          }
          break
        case 'tabToBeWeighed':
          // Tips：Must be write the nextTick so that can get DOM!!
          if (tabToBeWeighedRef?.value?.getToBeWeighed) {
            tabToBeWeighedRef.value.getToBeWeighed()
          }
          break
        case 'tabWeighed':
          // Tips：Must be write the nextTick so that can get DOM!!
          if (tabWeighedRef?.value?.getWeighed) {
            tabWeighedRef.value.getWeighed()
          }
          break
        case 'tabPicked':
          // Tips：Must be write the nextTick so that can get DOM!!
          if (tabPickedRef?.value?.getPicked) {
            tabPickedRef.value.getPicked()
          }
          break
        case 'tabToBeDelivered':
          // Tips：Must be write the nextTick so that can get DOM!!
          if (tabToBeDeliveredRef?.value?.getToBeDelivery) {
            tabToBeDeliveredRef.value.getToBeDelivery()
          }
          break
        case 'tabDelivered':
          // Tips：Must be write the nextTick so that can get DOM!!
          if (tabDeliveredRef?.value?.getDelivery) {
            tabDeliveredRef.value.getDelivery()
          }
          break
        case 'tabSignIn':
          // Tips：Must be write the nextTick so that can get DOM!!
          if (tabSignInRef?.value?.getSignIn) {
            tabSignInRef.value.getSignIn()
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
