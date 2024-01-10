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
                <vxe-table ref="xTable" :column-config="{ minWidth: '120px' }" :data="data.tableData" :height="tableHeight" align="center">
                  <template #empty>
                    {{ i18n.global.t('system.page.noData') }}
                  </template>
                  <vxe-column type="seq" width="60"></vxe-column>
                  <vxe-column field="warehouse_name" :title="$t('wms.saftyStock.warehouse_name')"></vxe-column>
                  <vxe-column field="spu_code" :title="$t('wms.saftyStock.spu_code')"></vxe-column>
                  <vxe-column field="spu_name" :title="$t('wms.saftyStock.spu_name')"></vxe-column>
                  <vxe-column field="sku_id" :title="$t('wms.saftyStock.sku_id')"></vxe-column>
                  <vxe-column field="sku_code" :title="$t('wms.saftyStock.sku_code')"></vxe-column>
                  <vxe-column field="sku_name" :title="$t('wms.saftyStock.sku_name')"></vxe-column>
                  <vxe-column field="qty" :title="$t('wms.saftyStock.qty')"></vxe-column>
                  <vxe-column field="qty_available" :title="$t('wms.saftyStock.qty_available')"></vxe-column>
                  <vxe-column field="qty_locked" :title="$t('wms.saftyStock.qty_locked')"></vxe-column>
                  <vxe-column field="qty_frozen" :title="$t('wms.saftyStock.qty_frozen')"></vxe-column>
                  <vxe-column field="safety_stock_qty" :title="$t('wms.saftyStock.safety_stock_qty')"></vxe-column>
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
import { FREEZE_JOB_FREEZE } from '@/constant/warehouseWorking'
import { DEBOUNCE_TIME } from '@/constant/system'
import { setSearchObject, getMenuAuthorityList } from '@/utils/common'
import { SearchObject, btnGroupItem } from '@/types/System/Form'
import i18n from '@/languages/i18n'
import customPager from '@/components/custom-pager.vue'
import { exportData } from '@/utils/exportTable'
import BtnGroup from '@/components/system/btnGroup.vue'
import { list as getSafetyStockList } from '@/api/wms/saftyStock'
import { hookComponent } from '@/components/system'
import { SafetyStockVo } from '@/types/WMS/SafetyStock'

const xTable = ref()

const data = reactive({
  showDialog: false,
  freezeType: FREEZE_JOB_FREEZE,
  timer: ref<any>(null),
  activeTab: null,
  searchForm: {
    spu_name: ''
  },
  tableData: ref<SafetyStockVo[]>([]),
  tablePage: reactive({
    total: 0,
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE,
    searchObjects: ref<Array<SearchObject>>([])
  }),
  btnList: [] as btnGroupItem[],
  // Menu operation permissions
  authorityList: getMenuAuthorityList()
})

const method = reactive({
  // get data
  list: async () => {
    const { data: res } = await getSafetyStockList(data.tablePage)
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
      filename: i18n.global.t('router.sideBar.saftyStock'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },

  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.refresh()
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
      code: 'export',
      click: method.exportTable
    }
  ]

  method.refresh()
})

const cardHeight = computed(() => computedCardHeight({ hasTab: false }))
const tableHeight = computed(() => computedTableHeight({ hasTab: false }))

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
