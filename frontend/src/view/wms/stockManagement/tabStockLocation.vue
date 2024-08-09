<template>
  <div class="operateArea">
    <v-row no-gutters>
      <!-- Operate Btn -->
      <v-col cols="3" class="col">
        <!-- <tooltip-btn icon="mdi-refresh" :tooltip-text="$t('system.page.refresh')" @click="method.refresh"></tooltip-btn>
        <tooltip-btn icon="mdi-export-variant" :tooltip-text="$t('system.page.export')" @click="method.exportTable"> </tooltip-btn> -->
        <BtnGroup :authority-list="data.authorityList" :btn-list="data.btnList" />
      </v-col>

      <!-- Search Input -->
      <v-col cols="9">
        <v-row no-gutters @keyup.enter="method.sureSearch">
          <v-col cols="4"></v-col>
          <v-col cols="4"></v-col>
          <v-col cols="4">
            <v-text-field
              v-model="data.searchForm.location_name"
              clearable
              hide-details
              density="comfortable"
              class="searchInput ml-5 mt-1"
              :label="$t('wms.stockLocation.location_name')"
              variant="solo"
            >
            </v-text-field>
          </v-col>
        </v-row>
      </v-col>
    </v-row>
  </div>

  <!-- Table -->
  <div
    class="mt-5"
    :style="{
      height: cardHeight
    }"
  >
    <vxe-table ref="xTableStockLocation" :column-config="{ minWidth: '100px' }" :data="data.tableData" :height="tableHeight" align="center">
      <template #empty>
        {{ i18n.global.t('system.page.noData') }}
      </template>
      <vxe-column type="seq" width="60"></vxe-column>
      <vxe-column type="checkbox" width="50"></vxe-column>
      <vxe-column field="warehouse_name" :title="$t('wms.stockLocation.warehouse_name')"></vxe-column>
      <vxe-column field="location_name" :title="$t('wms.stockLocation.location_name')"></vxe-column>
      <vxe-column field="spu_code" :title="$t('wms.stockLocation.spu_code')">
        <template #default="{ row }">
          <div :class="'text-decoration-none'" @click="method.showSkuInfo(row)"> {{ row.sku_code }}</div>
        </template>
      </vxe-column>
      <vxe-column field="spu_name" :title="$t('wms.stockLocation.spu_name')"></vxe-column>
      <vxe-column field="sku_code" :title="$t('wms.stockLocation.sku_code')"></vxe-column>
      <vxe-column field="sku_name" :title="$t('wms.stockLocation.sku_name')"></vxe-column>
      <vxe-column field="series_number" :title="$t('wms.stockLocation.series_number')"></vxe-column>
      <vxe-column field="qty" :title="$t('wms.stockLocation.qty')"></vxe-column>
      <vxe-column field="qty" :title="$t('wms.stockLocation.qty')"></vxe-column>
      <vxe-column field="qty_available" :title="$t('wms.stockLocation.qty_available')"></vxe-column>
      <vxe-column field="qty_locked" :title="$t('wms.stockLocation.qty_locked')"></vxe-column>
      <vxe-column field="price" :title="$t('wms.stockAsnInfo.price')"></vxe-column>
      <vxe-date-column field="expiry_date" :title="$t('wms.stockAsnInfo.expiry_date')"> </vxe-date-column>
      <vxe-date-column field="putaway_date" :title="$t('wms.stockAsnInfo.putaway_date')"> </vxe-date-column>
    </vxe-table>
    <custom-pager
      :current-page="data.tablePage.pageIndex"
      :page-size="data.tablePage.pageSize"
      perfect
      :total="data.tablePage.total"
      :page-sizes="PAGE_SIZE"
      :layouts="PAGE_LAYOUT"
      @page-change="method.handlePageChange"
    >
    </custom-pager>
  </div>
  <skuInfo :show-dialog="data.showDialogShowInfo" :sku_id="data.sku_id" @close="method.closeDialogShowInfo" />
</template>

<script lang="ts" setup>
import { computed, ref, reactive, watch, onMounted } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight } from '@/constant/style'
import { StockLocationVO } from '@/types/WMS/StockManagement'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { hookComponent } from '@/components/system'
import { DEBOUNCE_TIME } from '@/constant/system'
import { setSearchObject, getMenuAuthorityList } from '@/utils/common'
import { SearchObject, btnGroupItem } from '@/types/System/Form'
import { getStockLocationList } from '@/api/wms/stockManagement'
import i18n from '@/languages/i18n'
import customPager from '@/components/custom-pager.vue'
import skuInfo from './sku-info.vue'
import { exportData } from '@/utils/exportTable'
import BtnGroup from '@/components/system/btnGroup.vue'

const xTableStockLocation = ref()

const data = reactive({
  sku_id: 0,
  showDialog: false,
  showDialogShowInfo: false,
  searchForm: {
    location_name: ''
  },
  activeTab: null,
  tableData: ref<StockLocationVO[]>([]),
  tablePage: reactive({
    total: 0,
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE,
    searchObjects: ref<Array<SearchObject>>([])
  }),
  timer: ref<any>(null),
  btnList: [] as btnGroupItem[],
  // Menu operation permissions
  authorityList: getMenuAuthorityList()
})

const method = reactive({
  closeDialogShowInfo: () => {
    data.showDialogShowInfo = false
  },
  showSkuInfo(row: StockLocationVO) {
    data.sku_id = row.sku_id
    data.showDialogShowInfo = true
  },
  // Refresh data
  refresh: () => {
    method.getStockLocationList()
  },
  getStockLocationList: async () => {
    const { data: res } = await getStockLocationList(data.tablePage)
    if (!res.isSuccess) {
      hookComponent.$message({
        type: 'error',
        content: res.errorMessage
      })
      return
    }
    data.tableData = res.data.rows
    data.tablePage.total = res.data.totals
  },
  handlePageChange: ref<VxePagerEvents.PageChange>(({ currentPage, pageSize }) => {
    data.tablePage.pageIndex = currentPage
    data.tablePage.pageSize = pageSize

    method.getStockLocationList()
  }),
  exportTable: () => {
    const $table = xTableStockLocation.value
    exportData({
      table: $table,
      filename: i18n.global.t('wms.stockManagement.stockLocation'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },
  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.getStockLocationList()
  }
})

onMounted(() => {
  data.btnList = [
    {
      name: i18n.global.t('system.page.refresh'),
      icon: 'mdi-refresh',
      code: '',
      click: method.refresh
    },
    {
      name: i18n.global.t('system.page.export'),
      icon: 'mdi-export-variant',
      code: 'stock-export',
      click: method.exportTable
    }
  ]
})

const cardHeight = computed(() => computedCardHeight({}))
const tableHeight = computed(() => computedTableHeight({}))

defineExpose({
  getStockLocationList: method.getStockLocationList
})
watch(
  () => data.searchForm,
  () => {
    // debounce
    if (data.timer) {
      clearTimeout(data.timer)
    }
    data.timer = setTimeout(() => {
      data.timer = null
      method.sureSearch()
    }, DEBOUNCE_TIME)
  },
  {
    deep: true
  }
)
</script>

<style lang="less" scoped>
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
