<!-- Warehouse Taking -->
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
                    <!-- <tooltip-btn icon="mdi-plus" :tooltip-text="$t('system.page.add')" @click="method.add()"></tooltip-btn>
                    <tooltip-btn icon="mdi-refresh" :tooltip-text="$t('system.page.refresh')" @click="method.refresh"></tooltip-btn>
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
                          v-model="data.searchForm.job_code"
                          clearable
                          hide-details
                          density="comfortable"
                          class="searchInput ml-5 mt-1"
                          :label="$t('wms.warehouseWorking.warehouseTaking.job_code')"
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
                <vxe-table ref="xTable" :column-config="{ minWidth: '100px' }" :data="data.tableData" :height="tableHeight" align="center">
                  <template #empty>
                    {{ i18n.global.t('system.page.noData') }}
                  </template>
                  <vxe-column type="seq" width="60"></vxe-column>
                  <vxe-column type="checkbox" width="50"></vxe-column>
                  <vxe-column field="job_code" width="150px" :title="$t('wms.warehouseWorking.warehouseTaking.job_code')"></vxe-column>
                  <vxe-column field="job_status" width="150px" :title="$t('wms.warehouseWorking.warehouseTaking.job_status')">
                    <template #default="{ row, column }">
                      <span>{{ formatTakingJobStatus(row[column.property]) }}</span>
                    </template>
                  </vxe-column>
                  <vxe-column field="spu_code" width="150px" :title="$t('base.commodityManagement.spu_code')"></vxe-column>
                  <vxe-column field="spu_name" width="150px" :title="$t('base.commodityManagement.spu_name')"></vxe-column>
                  <vxe-column field="sku_code" width="150px" :title="$t('base.commodityManagement.sku_code')"></vxe-column>
                  <vxe-column field="series_number" width="150px" :title="$t('wms.stockLocation.series_number')"></vxe-column>
                  <vxe-column field="warehouse_name" width="150px" :title="$t('wms.warehouseWorking.warehouseTaking.warehouse')"></vxe-column>
                  <vxe-column field="location_name" width="150px" :title="$t('wms.warehouseWorking.warehouseTaking.location_name')"></vxe-column>
                  <vxe-column field="book_qty" width="150px" :title="$t('wms.warehouseWorking.warehouseTaking.book_qty')"></vxe-column>
                  <vxe-column field="counted_qty" width="150px" :title="$t('wms.warehouseWorking.warehouseTaking.counted_qty')"></vxe-column>
                  <vxe-column field="difference_qty" width="150px" :title="$t('wms.warehouseWorking.warehouseTaking.difference_qty')"></vxe-column>
                  <vxe-column field="handler" width="150px" :title="$t('wms.warehouseWorking.warehouseTaking.handler')"></vxe-column>
                  <vxe-date-column
                    field="handle_time"
                    width="170px"
                    format="yyyy-MM-dd HH:mm"
                    :title="$t('wms.warehouseWorking.warehouseTaking.handle_time')"
                  ></vxe-date-column>
                  <vxe-column field="creator" :title="$t('wms.warehouseWorking.warehouseTaking.creator')"></vxe-column>
                  <vxe-date-column
                    field="create_time"
                    width="170px"
                    format="yyyy-MM-dd HH:mm"
                    :title="$t('wms.warehouseWorking.warehouseTaking.create_time')"
                  ></vxe-date-column>
                  <vxe-column field="operate" :title="$t('system.page.operate')" width="250" :resizable="false" show-overflow>
                    <template #default="{ row }">
                      <tooltip-btn
                        :flat="true"
                        icon="mdi-eye-outline"
                        :tooltip-text="$t('system.page.view')"
                        @click="method.viewRow(row)"
                      ></tooltip-btn>
                      <tooltip-btn
                        :flat="true"
                        icon="mdi-book-check-outline"
                        :tooltip-text="$t('wms.warehouseWorking.warehouseTaking.confirmTaking')"
                        :disabled="!data.authorityList.includes('confirmOpeartion') || method.isConfirmTaking(row)"
                        @click="method.confirmTaking(row)"
                      ></tooltip-btn>
                      <tooltip-btn
                        :flat="true"
                        icon="mdi-book-open-outline"
                        :tooltip-text="$t('wms.warehouseWorking.warehouseProcessing.confirmAdjust')"
                        :disabled="!data.authorityList.includes('confirmAdjust') || method.confirmAdjustBtnDisabled(row)"
                        @click="method.confirmAdjust(row)"
                      ></tooltip-btn>
                      <tooltip-btn
                        :flat="true"
                        icon="mdi-delete-outline"
                        :tooltip-text="$t('system.page.delete')"
                        :icon-color="!data.authorityList.includes('delete') || method.isConfirmTaking(row) ? '' : errorColor"
                        :disabled="!data.authorityList.includes('delete') || method.isConfirmTaking(row)"
                        @click="method.deleteRow(row)"
                      ></tooltip-btn>
                    </template>
                  </vxe-column>
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
      <addOrUpdateDialog :show-dialog="data.showDialog" :form="data.dialogForm" @close="method.closeDialog" @saveSuccess="method.saveSuccess" />
      <number-input
        :show-dialog="data.showDialogNumberInput"
        :form="data.dialogForm"
        @close="method.closeDialogNumberInput"
        @saveSuccess="method.saveSuccessNumberInput"
      />
    </div>
  </div>
</template>

<script lang="ts" setup>
import { computed, ref, reactive, onActivated, watch, nextTick, onMounted } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight, errorColor } from '@/constant/style'
import { WarehouseTakingVO } from '@/types/WarehouseWorking/WarehouseTaking'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { hookComponent } from '@/components/system'
import { TAKING_JOB_FINISH, TAKING_JOB_UNFINISH } from '@/constant/warehouseWorking'
import { deleteStockTaking, getStockTakingList, getStockTakingOne, confirmAdjustment } from '@/api/wms/warehouseTaking'
import { DEBOUNCE_TIME } from '@/constant/system'
import { setSearchObject, getMenuAuthorityList } from '@/utils/common'
import { SearchObject, btnGroupItem } from '@/types/System/Form'
import { formatTakingJobStatus } from '@/utils/format/formatWarehouseWorking'
import tooltipBtn from '@/components/tooltip-btn.vue'
import addOrUpdateDialog from './add-or-update-taking.vue'
import numberInput from './number-input.vue'
import i18n from '@/languages/i18n'
import customPager from '@/components/custom-pager.vue'
import { exportData } from '@/utils/exportTable'
import BtnGroup from '@/components/system/btnGroup.vue'

const xTable = ref()

const data = reactive({
  showDialog: false,
  showDialogNumberInput: false,
  timer: ref<any>(null),
  activeTab: null,
  tableData: ref<WarehouseTakingVO[]>([]),
  dialogForm: ref<WarehouseTakingVO>({
    id: 0,
    job_code: '',
    job_status: TAKING_JOB_FINISH,
    sku_id: 0,
    goods_owner_id: 0,
    goods_location_id: 0,
    book_qty: 0,
    counted_qty: 0,
    difference_qty: 0,
    spu_code: '',
    spu_name: '',
    sku_code: '',
    series_number: '',
    warehouse_name: '',
    location_name: '',
    handler: '',
    handle_time: '',
    price: 0,
    expiry_date: '',
    putaway_date: '',
    adjust_status: false,
    creator: '',
    create_time: ''
  }),
  searchForm: {
    job_code: ''
  },
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
  // Open a dialog to add
  add: () => {
    data.dialogForm = {
      id: 0,
      job_code: '',
      job_status: TAKING_JOB_FINISH,
      sku_id: 0,
      goods_owner_id: 0,
      goods_location_id: 0,
      book_qty: 0,
      counted_qty: 0,
      difference_qty: 0,
      spu_code: '',
      spu_name: '',
      sku_code: '',
      series_number: '',
      warehouse_name: '',
      location_name: '',
      handler: '',
      handle_time: '',
      adjust_status: false,
      price: 0,
      expiry_date: '',
      putaway_date: '',
      creator: '',
      create_time: ''
    }
    nextTick(() => {
      data.showDialog = true
    })
  },

  // After add or update success.
  saveSuccess: () => {
    method.refresh()
    method.closeDialog()
  },

  // After confirm taking.
  saveSuccessNumberInput: () => {
    method.refresh()
    method.closeDialogNumberInput()
  },

  // Refresh data
  refresh: () => {
    method.getStockProcessList()
  },

  // Shut add or update dialog
  closeDialog: () => {
    data.showDialog = false
  },

  // Shut number input dialog
  closeDialogNumberInput: () => {
    data.showDialogNumberInput = false
  },

  getStockProcessList: async () => {
    const { data: res } = await getStockTakingList(data.tablePage)
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

  viewRow: async (row: WarehouseTakingVO) => {
    await method.getOne(row.id)
    nextTick(() => {
      data.showDialog = true
    })
  },

  getOne: async (id: number) => {
    const { data: res } = await getStockTakingOne(id)
    if (!res.isSuccess) {
      hookComponent.$message({
        type: 'error',
        content: res.errorMessage
      })
      return
    }

    data.dialogForm = res.data
  },

  deleteRow(row: WarehouseTakingVO) {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteMessage'),
      handleConfirm: async () => {
        if (row.id) {
          const { data: res } = await deleteStockTaking(row.id)
          if (!res.isSuccess) {
            hookComponent.$message({
              type: 'error',
              content: res.errorMessage
            })
            return
          }

          hookComponent.$message({
            type: 'success',
            content: `${ i18n.global.t('system.page.delete') }${ i18n.global.t('system.tips.success') }`
          })
          method.refresh()
        }
      }
    })
  },

  handlePageChange: ref<VxePagerEvents.PageChange>(({ currentPage, pageSize }) => {
    data.tablePage.pageIndex = currentPage
    data.tablePage.pageSize = pageSize
    method.refresh()
  }),

  exportTable: () => {
    const $table = xTable.value
    exportData({
      table: $table,
      filename: i18n.global.t('router.sideBar.warehouseTaking'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },

  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.refresh()
  },

  // The btn will become disabled when the 'job_status' is false
  confirmAdjustBtnDisabled: (row: WarehouseTakingVO) => row.job_status === TAKING_JOB_UNFINISH || !!row.adjust_status,

  // The btn will become disabled when the 'job_status' is true
  isConfirmTaking: (row: WarehouseTakingVO) => row.job_status === TAKING_JOB_FINISH,

  confirmTaking: async (row: WarehouseTakingVO) => {
    data.dialogForm = row
    nextTick(() => {
      data.showDialogNumberInput = true
    })
  },

  confirmAdjust: async (row: WarehouseTakingVO) => {
    hookComponent.$dialog({
      content: i18n.global.t('wms.warehouseWorking.warehouseTaking.beforeConfirmAdjust'),
      handleConfirm: async () => {
        if (row.id) {
          const { data: res } = await confirmAdjustment(row.id)
          if (!res.isSuccess) {
            hookComponent.$message({
              type: 'error',
              content: res.errorMessage
            })
            return
          }
          hookComponent.$message({
            type: 'success',
            content: `${ i18n.global.t('wms.warehouseWorking.warehouseTaking.confirmAdjust') }${ i18n.global.t('system.tips.success') }`
          })
          method.refresh()
        }
      }
    })
  }
})

onActivated(() => {
  method.refresh()
})

onMounted(() => {
  data.btnList = [
    {
      name: i18n.global.t('system.page.add'),
      icon: 'mdi-plus',
      code: 'save',
      click: method.add
    },
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
