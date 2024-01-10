<template>
  <div class="operateArea">
    <v-row no-gutters>
      <!-- Operate Btn -->
      <v-col cols="3" class="col">
        <!-- <tooltip-btn icon="mdi-refresh" :tooltip-text="$t('system.page.refresh')" @click="method.refresh"></tooltip-btn>
        <tooltip-btn icon="mdi-export-variant" :tooltip-text="$t('system.page.export')" @click="method.exportTable"> </tooltip-btn> -->
        <!-- new version -->
        <BtnGroup :authority-list="data.authorityList" :btn-list="data.btnList" />
      </v-col>

      <!-- Search Input -->
      <v-col cols="9">
        <v-row no-gutters @keyup.enter="method.sureSearch">
          <v-col cols="4"></v-col>
          <v-col cols="4"></v-col>
          <v-col cols="4">
            <v-text-field
              v-model="data.searchForm.spu_name"
              clearable
              hide-details
              density="comfortable"
              class="searchInput ml-5 mt-1"
              :label="$t('wms.stockList.spu_name')"
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
    <vxe-table ref="xTableWarehouse" :column-config="{ minWidth: '100px' }" :data="data.tableData" :height="tableHeight" align="center">
      <template #empty>
        {{ i18n.global.t('system.page.noData') }}
      </template>
      <vxe-column type="seq" width="60"></vxe-column>
      <vxe-column type="checkbox" width="50"></vxe-column>
      <vxe-column field="spu_code" :title="$t('wms.stockList.spu_code')"></vxe-column>
      <vxe-column field="spu_name" :title="$t('wms.stockList.spu_name')"></vxe-column>
      <vxe-column field="sku_code" :title="$t('wms.stockList.sku_code')">
        <template #default="{ row }">
          <div :class="'text-decoration-none'" @click="method.showSkuInfo(row)"> {{ row.sku_code }}</div>
        </template>
      </vxe-column>
      <vxe-column field="qty" :title="$t('wms.stockList.qty')"></vxe-column>
      <vxe-column field="qty_available" :title="$t('wms.stockList.qty_available')"></vxe-column>
      <vxe-column field="qty_locked" :title="$t('wms.stockList.qty_locked')"></vxe-column>
      <vxe-column field="qty_frozen" :title="$t('wms.stockList.qty_frozen')"></vxe-column>
      <vxe-column field="qty_asn" :title="$t('wms.stockList.qty_asn')"></vxe-column>
      <vxe-column field="qty_to_unload" :title="$t('wms.stockList.qty_to_unload')"></vxe-column>
      <vxe-column field="qty_to_sort" :title="$t('wms.stockList.qty_to_sort')"></vxe-column>
      <vxe-column field="qty_sorted" :title="$t('wms.stockList.qty_sorted')"></vxe-column>
      <vxe-column field="shortage_qty" :title="$t('wms.stockList.shortage_qty')"></vxe-column>
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
import { StockVO } from '@/types/WMS/StockManagement'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { hookComponent } from '@/components/system'
import { DEBOUNCE_TIME } from '@/constant/system'
import { setSearchObject, getMenuAuthorityList } from '@/utils/common'
import { SearchObject, btnGroupItem } from '@/types/System/Form'
import { getStockList } from '@/api/wms/stockManagement'
import i18n from '@/languages/i18n'
import customPager from '@/components/custom-pager.vue'
import skuInfo from './sku-info.vue'
import { exportData } from '@/utils/exportTable'
import BtnGroup from '@/components/system/btnGroup.vue'

const xTableWarehouse = ref()

const data = reactive({
  sku_id: 0,
  showDialog: false,
  showDialogShowInfo: false,
  searchForm: {
    spu_name: ''
  },
  activeTab: null,
  tableData: ref<StockVO[]>([]),
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
  showSkuInfo(row: StockVO) {
    data.sku_id = row.sku_id
    data.showDialogShowInfo = true
  },
  sumNum: (list: any[], field: string) => {
    let count = 0
    list.forEach((item) => {
      count += Number(item[field])
    })
    return count
  },
  // footerMethod:ref<VxeTablePropTypes.FooterMethod>({ columns, data }) => {
  //   columns.map((column, columnIndex) => {
  //     if (columnIndex === 0) {
  //       return '合计'
  //     }
  //     if (['qty', 'qty_available'].includes(column.field)) {
  //       return method.sumNum(data, column.field)
  //     }
  //     return null
  //   })
  // },
  // Refresh data
  refresh: () => {
    method.getStockList()
  },
  getStockList: async () => {
    const { data: res } = await getStockList(data.tablePage)
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

    method.getStockList()
  }),
  exportTable: () => {
    const $table = xTableWarehouse.value
    exportData({
      table: $table,
      filename: i18n.global.t('wms.stockManagement.stock'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },
  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.getStockList()
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
      code: 'area-export',
      click: method.exportTable
    }
  ]
})

const cardHeight = computed(() => computedCardHeight({}))
const tableHeight = computed(() => computedTableHeight({}))

defineExpose({
  getStockList: method.getStockList
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
