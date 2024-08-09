<!-- Warehouse Freeze -->
<template>
  <div class="container">
    <div>
      <!-- Main Content -->
      <v-card class="mt-5">
        <v-card-text>
          <v-window v-model="data.activeTab">
            <v-window-item>
              <div class="operateArea">
                <v-row no-gutters>
                  <!-- Operate Btn -->
                  <v-col cols="3" class="col">
                    <BtnGroup :authority-list="data.authorityList" :btn-list="btnList" />
                  </v-col>

                  <!-- Search Input -->
                  <v-col cols="9">
                    <search-group
                      ref="searchGroupRef"
                      v-model="data.searchForm"
                      :menu-name="data.menu_name"
                      i18n-prefix="wms.stockAsnInfo"
                      @sure-search="method.sureSearch"
                    />
                  </v-col>
                </v-row>
              </div>

              <!-- Table -->
              <div
                v-if="data.currentRender === 'table'"
                class="mt-5"
                :style="{
                  height: cardHeight
                }"
              >
                <vxe-table
                  ref="xTableStockLocation"
                  :column-config="{ minWidth: '100px' }"
                  :data="data.tableData"
                  :height="tableHeight"
                  align="center"
                >
                  <template #empty>
                    {{ i18n.global.t('system.page.noData') }}
                  </template>
                  <vxe-column type="seq" width="60"></vxe-column>
                  <!-- <vxe-column type="checkbox" width="50"></vxe-column> -->
                  <vxe-column field="asn_no" :title="$t('wms.stockAsnInfo.asn_no')"></vxe-column>
                  <vxe-column field="spu_code" :title="$t('wms.stockAsnInfo.spu_code')"></vxe-column>
                  <vxe-column field="spu_name" :title="$t('wms.stockAsnInfo.spu_name')"></vxe-column>
                  <vxe-column field="sku_code" :title="$t('wms.stockAsnInfo.sku_code')">
                    <template #default="{ row }">
                      <div :class="'text-decoration-none'" @click="method.showSkuInfo(row)"> {{ row.sku_code }}</div>
                    </template>
                  </vxe-column>
                  <vxe-column field="sku_name" :title="$t('wms.stockAsnInfo.sku_name')"></vxe-column>
                  <vxe-column field="goods_owner_name" :title="$t('wms.stockAsnInfo.goods_owner_name')"></vxe-column>
                  <vxe-column field="supplier_name" :title="$t('wms.stockAsnInfo.supplier_name')"></vxe-column>
                  <vxe-column field="asn_qty" :title="$t('wms.stockAsnInfo.asn_qty')"></vxe-column>
                  <vxe-column field="weight" :title="$t('wms.stockAsnInfo.weight')"></vxe-column>
                  <vxe-column field="volume" :title="$t('wms.stockAsnInfo.volume')"></vxe-column>
                  <vxe-column field="actual_qty" :title="$t('wms.stockAsnInfo.actual_qty')"></vxe-column>
                  <vxe-column field="sorted_qty" :title="$t('wms.stockAsnInfo.sorted_qty')"></vxe-column>
                  <vxe-column field="shortage_qty" :title="$t('wms.stockAsnInfo.shortage_qty')"></vxe-column>
                  <vxe-column field="more_qty" :title="$t('wms.stockAsnInfo.more_qty')"></vxe-column>
                  <vxe-column field="damage_qty" :title="$t('wms.stockAsnInfo.damage_qty')"></vxe-column>
                  <vxe-column field="price" :title="$t('wms.stockAsnInfo.price')"></vxe-column>
                  <vxe-date-column field="expiry_date" :title="$t('wms.stockAsnInfo.expiry_date')"></vxe-date-column>
                  <!-- <vxe-column field="operate" :title="$t('system.page.operate')" width="160" :resizable="false" show-overflow>
                    <template #default="{ row }">
                      <tooltip-btn
                        :flat="true"
                        icon="mdi-eye-outline"
                        :tooltip-text="$t('system.page.view')"
                        @click="method.viewRow(row)"
                      ></tooltip-btn>
                    </template>
                  </vxe-column> -->
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
              <div
                v-else
                class="mt-5"
                :style="{
                  height: cardHeight
                }"
              >
                <Chart type="asn" :chart-data="data.tableData" />
              </div>

              <skuInfo :show-dialog="data.showDialogShowInfo" :form="data.dialogForm" @close="method.closeDialogShowInfo" />
            </v-window-item>
          </v-window>
        </v-card-text>
      </v-card>
    </div>
  </div>

  <view-detail-dialog ref="ViewDetailDialogRef" />
</template>

<script lang="ts" setup>
import { computed, ref, reactive, watch, onMounted } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight } from '@/constant/style'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { DEBOUNCE_TIME } from '@/constant/system'
import { getMenuAuthorityList, setSearchObject } from '@/utils/common'
import { SearchObject, btnGroupItem } from '@/types/System/Form'
import i18n from '@/languages/i18n'
import customPager from '@/components/custom-pager.vue'
import { exportData } from '@/utils/exportTable'
import BtnGroup from '@/components/system/btnGroup.vue'
import { hookComponent } from '@/components/system'
import skuInfo from '@/view/wms/stockAsn/sku-info.vue'
import { StockAsnVO } from '@/types/WMS/StockAsn'
import { getStockAsnList } from '@/api/wms/stockAsn'
import ViewDetailDialog from '@/view/wms/stockAsn/view-detail-dialog.vue'
import SearchGroup from '@/components/system/search-group.vue'
import Chart from '../components/Chart.vue'

const xTableStockLocation = ref()
const ViewDetailDialogRef = ref()
const searchGroupRef = ref()

const data = reactive({
  showDialog: false,
  showDialogShowInfo: false,
  searchForm: {
    supplier_name: '',
    sku_name: '',
    spu_code: '',
    spu_name: '',
    sku_code: '',
    goods_owner_name: ''
  },
  activeTab: null,
  tableData: ref<StockAsnVO[]>([]),
  dialogForm: ref<StockAsnVO>({
    id: 0,
    asn_no: '',
    asn_batch: '',
    estimated_arrival_time: '',
    // asn_status: 0,
    // weight: 0,
    // volume: 0,
    // goods_owner_id: 0,
    // goods_owner_name: '',
    // creator: '',
    // create_time: '',
    // last_update_time: '',
    detailList: []
  }),
  tablePage: reactive({
    total: 0,
    sqlTitle: 'asn_status:4',
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE,
    searchObjects: ref<Array<SearchObject>>([])
  }),
  timer: ref<any>(null),
  btnList: [] as btnGroupItem[],
  // Menu operation permissions
  authorityList: getMenuAuthorityList(),
  // Local search criteria settings
  menu_name: 'asnStatistic',
  currentRender: 'table'
})

const method = reactive({
  // Switch Chart Mode
  switchRenderMode: () => {
    if (data.currentRender === 'chart') {
      data.currentRender = 'table'
    } else {
      data.currentRender = 'chart'
    }
  },
  // Set Search
  handleSetSearch: () => {
    searchGroupRef.value.openDialog()
  },
  // View Rows
  // viewRow: (row: StockAsnVO) => {
  //   ViewDetailDialogRef.value.openDialog(row.id)
  // },
  closeDialogShowInfo: () => {
    data.showDialogShowInfo = false
  },
  showSkuInfo(row: StockAsnVO) {
    data.dialogForm = JSON.parse(JSON.stringify(row))
    data.showDialogShowInfo = true
  },
  // Refresh data
  refresh: () => {
    method.getStockAsnList()
  },
  getStockAsnList: async () => {
    const { data: res } = await getStockAsnList(data.tablePage)
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

    method.getStockAsnList()
  }),
  exportTable: () => {
    const $table = xTableStockLocation.value
    exportData({
      table: $table,
      filename: i18n.global.t('wms.stockAsn.tabReceiptDetails'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },
  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.getStockAsnList()
  }
})

onMounted(() => {
  // data.btnList = [
  //   {
  //     name: i18n.global.t('system.page.refresh'),
  //     icon: 'mdi-refresh',
  //     code: '',
  //     click: method.refresh
  //   },
  //   {
  //     name: i18n.global.t('system.page.export'),
  //     icon: 'mdi-export-variant',
  //     code: 'export',
  //     click: method.exportTable
  //   },
  //   {
  //     name: i18n.global.t('system.page.setSearch'),
  //     icon: 'mdi-cog',
  //     code: '',
  //     click: method.handleSetSearch
  //   },
  //   {
  //     name: i18n.global.t('system.page.chart'),
  //     icon: 'mdi-chart-bar',
  //     code: '',
  //     click: method.switchRenderMode
  //   }
  // ]
  // method.refresh()
})

const cardHeight = computed(() => computedCardHeight({ hasTab: false }))
const tableHeight = computed(() => computedTableHeight({ hasTab: false }))
const btnList = computed(() => [
  {
    name: i18n.global.t('system.page.refresh'),
    icon: 'mdi-refresh',
    code: '',
    click: method.refresh
  },
  {
    name: i18n.global.t('system.page.export'),
    icon: 'mdi-export-variant',
    code: 'export',
    click: method.exportTable
  },
  {
    name: i18n.global.t('system.page.setSearch'),
    icon: 'mdi-cog',
    code: '',
    click: method.handleSetSearch
  },
  {
    name: data.currentRender !== 'chart' ? i18n.global.t('system.page.chart') : i18n.global.t('system.page.table'),
    icon: data.currentRender !== 'chart' ? 'mdi-chart-bar' : 'mdi-table',
    code: '',
    click: method.switchRenderMode
  }
])

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
