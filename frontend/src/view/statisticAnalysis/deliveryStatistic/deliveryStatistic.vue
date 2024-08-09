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
                      i18n-prefix="wms.deliveryStatistic"
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
                <vxe-table ref="xTable" :column-config="{ minWidth: '120px' }" :data="data.tableData" :height="tableHeight" align="center">
                  <template #empty>
                    {{ i18n.global.t('system.page.noData') }}
                  </template>
                  <vxe-column type="seq" width="60"></vxe-column>
                  <vxe-column field="dispatch_no" :title="$t('wms.deliveryStatistic.dispatch_no')"></vxe-column>
                  <vxe-column field="warehouse_name" :title="$t('wms.deliveryStatistic.warehouse_name')"></vxe-column>
                  <vxe-column field="location_name" :title="$t('wms.deliveryStatistic.location_name')"></vxe-column>
                  <vxe-column field="spu_code" :title="$t('wms.deliveryStatistic.spu_code')"></vxe-column>
                  <vxe-column field="spu_name" :title="$t('wms.deliveryStatistic.spu_name')"></vxe-column>
                  <vxe-column field="sku_code" :title="$t('wms.deliveryStatistic.sku_code')"></vxe-column>
                  <vxe-column field="sku_name" :title="$t('wms.deliveryStatistic.sku_name')"></vxe-column>
                  <vxe-column field="customer_name" :title="$t('wms.deliveryStatistic.customer_name')"></vxe-column>
                  <vxe-column field="series_number" :title="$t('wms.deliveryStatistic.series_number')"></vxe-column>
                  <vxe-column field="delivery_qty" :title="$t('wms.deliveryStatistic.delivery_qty')"></vxe-column>
                  <vxe-date-column
                    field="delivery_date"
                    :title="$t('wms.deliveryStatistic.delivery_date')"
                    width="170px"
                    format="yyyy-MM-dd HH:mm"
                  ></vxe-date-column>
                  <vxe-column field="price" :title="$t('wms.stockAsnInfo.price')"></vxe-column>
                  <vxe-date-column field="expiry_date" :title="$t('wms.stockAsnInfo.expiry_date')"></vxe-date-column>
                  <vxe-date-column field="putaway_date" :title="$t('wms.stockAsnInfo.putaway_date')"></vxe-date-column>                  
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
                <Chart type="delivery" :chart-data="data.tableData" />
              </div>
            </v-window-item>
          </v-window>
        </v-card-text>
      </v-card>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { computed, ref, reactive, watch, onMounted } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight } from '@/constant/style'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { DEBOUNCE_TIME } from '@/constant/system'
import { getMenuAuthorityList } from '@/utils/common'
import { SearchObject, btnGroupItem } from '@/types/System/Form'
import i18n from '@/languages/i18n'
import customPager from '@/components/custom-pager.vue'
import { exportData } from '@/utils/exportTable'
import BtnGroup from '@/components/system/btnGroup.vue'
import { list as getDeliveryStatisticList } from '@/api/wms/deliveryStatistic'
import { hookComponent } from '@/components/system'
import { DeliveryStatisticVo } from '@/types/WMS/DeliveryStatistic'
import SearchGroup from '@/components/system/search-group.vue'
import Chart from '../components/Chart.vue'

const xTable = ref()
const searchGroupRef = ref()

const data = reactive({
  showDialog: false,
  timer: ref<any>(null),
  activeTab: null,
  searchForm: {
    spu_code: '',
    spu_name: '',
    sku_code: '',
    sku_name: '',
    warehouse_name: '',
    customer_name: '',
    delivery_date_from: '',
    delivery_date_to: ''
  },
  tableData: ref<DeliveryStatisticVo[]>([]),
  tablePage: reactive({
    total: 0,
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE,
    searchObjects: ref<Array<SearchObject>>([])
  }),
  btnList: [] as btnGroupItem[],
  // Menu operation permissions
  authorityList: getMenuAuthorityList(),
  // Local search criteria settings
  menu_name: 'deliveryStatistic',
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
  // get data
  list: async () => {
    const searchForm = {}

    for (const item of Object.keys(data.searchForm)) {
      if (data.searchForm[item]) {
        searchForm[item] = data.searchForm[item]
      }
    }

    const { data: res } = await getDeliveryStatisticList({ ...data.tablePage, ...searchForm })
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

  // Refresh data
  refresh: () => {
    method.list()
  },

  // Shut add or update dialog
  closeDialog: () => {
    data.showDialog = false
  },

  // Pagination function
  handlePageChange: ref<VxePagerEvents.PageChange>(({ currentPage, pageSize }) => {
    data.tablePage.pageIndex = currentPage
    data.tablePage.pageSize = pageSize
    method.refresh()
  }),

  // Export Table
  exportTable: () => {
    const $table = xTable.value
    exportData({
      table: $table,
      filename: i18n.global.t('router.sideBar.deliveryStatistic'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },

  sureSearch: () => {
    // data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.refresh()
  },

  // Set Search
  handleSetSearch: () => {
    searchGroupRef.value.openDialog()
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
