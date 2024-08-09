<!-- Warehouse Processing -->
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
                    <!-- <tooltip-btn
                      icon="mdi-arrow-split-vertical"
                      :tooltip-text="$t('wms.warehouseWorking.warehouseProcessing.process_split')"
                      @click="method.add(PROCESS_JOB_SPLIT)"
                    ></tooltip-btn>
                    <tooltip-btn
                      icon="mdi-group"
                      :tooltip-text="$t('wms.warehouseWorking.warehouseProcessing.process_combine')"
                      @click="method.add(PROCESS_JOB_COMBINE)"
                    ></tooltip-btn>
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
                          :label="$t('wms.warehouseWorking.warehouseProcessing.job_code')"
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
                  <vxe-column field="job_code" width="150px" :title="$t('wms.warehouseWorking.warehouseProcessing.job_code')"></vxe-column>
                  <vxe-column field="job_type" :title="$t('wms.warehouseWorking.warehouseProcessing.job_type')">
                    <template #default="{ row, column }">
                      <span>{{ formatProcessJobType(row[column.property]) }}</span>
                    </template>
                  </vxe-column>
                  <vxe-column field="adjust_status" :title="$t('wms.warehouseWorking.warehouseProcessing.adjust_status')">
                    <template #default="{ row, column }">
                      <span>{{ formatIsValid(row[column.property]) }}</span>
                    </template>
                  </vxe-column>
                  <vxe-column field="processor" :title="$t('wms.warehouseWorking.warehouseProcessing.processor')"></vxe-column>
                  <vxe-date-column
                    field="process_time"
                    width="170px"
                    format="yyyy-MM-dd HH:mm"
                    :title="$t('wms.warehouseWorking.warehouseProcessing.process_time')"
                  ></vxe-date-column>
                  <vxe-column field="creator" :title="$t('wms.warehouseWorking.warehouseProcessing.creator')"></vxe-column>
                  <vxe-date-column
                    field="create_time"
                    width="170px"
                    format="yyyy-MM-dd HH:mm"
                    :title="$t('wms.warehouseWorking.warehouseProcessing.create_time')"
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
                        :tooltip-text="$t('wms.warehouseWorking.warehouseProcessing.confirmProcess')"
                        :disabled="!data.authorityList.includes('confirmOpeartion') || method.confirmProcessBtnDisabled(row)"
                        @click="method.confirmProcess(row)"
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
                        :icon-color="!data.authorityList.includes('delete') || method.confirmProcessBtnDisabled(row) ? '' : errorColor"
                        :disabled="!data.authorityList.includes('delete') || method.confirmProcessBtnDisabled(row)"
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
      <addOrUpdateDialog
        :show-dialog="data.showDialog"
        :form="data.dialogForm"
        :process-type="data.processType"
        @close="method.closeDialog"
        @saveSuccess="method.saveSuccess"
      />
      <!-- Print QR code -->
      <qr-code-dialog ref="qrCodeDialogRef" :menu="'stockAsnInfo-notice'">
        <template #left="{ slotData }">
          <p>{{ $t('wms.warehouseWorking.warehouseProcessing.job_code') }}:{{ slotData.job_code }}</p> &nbsp;
          <p>{{ $t('wms.warehouseWorking.warehouseProcessing.job_type') }}:{{ formatProcessJobType(slotData.job_type) }}</p> &nbsp;
        </template>
      </qr-code-dialog>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { computed, ref, reactive, onActivated, watch, nextTick, onMounted } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight, errorColor } from '@/constant/style'
import { WarehouseProcessingVO, WarehouseProcessingDetailVO } from '@/types/WarehouseWorking/WarehouseProcessing'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { hookComponent } from '@/components/system'
import { deleteStockProcess, getStockProcessList, getStockProcessOne, confirmAdjustment, confirmProcess } from '@/api/wms/warehouseProcessing'
import { PROCESS_JOB_COMBINE, PROCESS_JOB_SPLIT } from '@/constant/warehouseWorking'
import { DEBOUNCE_TIME } from '@/constant/system'
import { setSearchObject, getMenuAuthorityList } from '@/utils/common'
import { SearchObject, btnGroupItem } from '@/types/System/Form'
import { formatIsValid, formatDate } from '@/utils/format/formatSystem'
import { formatProcessJobType } from '@/utils/format/formatWarehouseWorking'
import tooltipBtn from '@/components/tooltip-btn.vue'
import addOrUpdateDialog from './add-or-update-process.vue'
import i18n from '@/languages/i18n'
import customPager from '@/components/custom-pager.vue'
import { exportData } from '@/utils/exportTable'
import BtnGroup from '@/components/system/btnGroup.vue'
import QrCodeDialog from '@/components/codeDialog/qrCodeDialog.vue'

const xTable = ref()
const qrCodeDialogRef = ref()

const data = reactive({
  showDialog: false,
  processType: PROCESS_JOB_COMBINE,
  timer: ref<any>(null),
  activeTab: null,
  searchForm: {
    job_code: ''
  },
  tableData: ref<WarehouseProcessingVO[]>([]),
  dialogForm: {
    id: 0,
    job_code: '',
    job_type: PROCESS_JOB_COMBINE,
    process_status: false,
    processor: '',
    process_time: '',
    source_detail_list: ref<any[]>([]),
    target_detail_list: ref<any[]>([]),
    creator: '',
    create_time: '',
    adjust_status: false
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
  // Print QR code
  printQrCode: () => {
    const records = xTable.value.getCheckboxRecords()

    // data.selectRowData.length === 0 ? data.selectRowData = [row] : ''
    // const records:any[] = data.selectRowData
    if (records.length > 0) {
      for (const item of records) {
        item.type = 'warehouseProcessing'
      }
      qrCodeDialogRef.value.openDialog(records)
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('base.userManagement.checkboxIsNull')
      })
    }
  },
  // Open a dialog to add
  add: (jobType: boolean) => {
    data.processType = jobType
    data.dialogForm = {
      id: 0,
      job_code: '',
      job_type: jobType,
      process_status: false,
      processor: '',
      process_time: '',
      source_detail_list: [],
      target_detail_list: [],
      creator: '',
      create_time: '',
      adjust_status: false
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

  // Refresh data
  refresh: () => {
    method.getStockProcessList()
  },

  // Shut add or update dialog
  closeDialog: () => {
    data.showDialog = false
  },

  getStockProcessList: async () => {
    const { data: res } = await getStockProcessList(data.tablePage)
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

  viewRow: async (row: WarehouseProcessingVO) => {
    await method.getOne(row.id)
    nextTick(() => {
      data.showDialog = true
    })
  },

  getOne: async (id: number) => {
    const { data: res } = await getStockProcessOne(id)
    if (!res.isSuccess) {
      hookComponent.$message({
        type: 'error',
        content: res.errorMessage
      })
      return
    }
    res.data.target_detail_list.forEach((element) => {
      element.expiry_date = formatDate(element.expiry_date, 'yyyy-MM-dd')
      element.putaway_date = formatDate(element.putaway_date, 'yyyy-MM-dd')
    })
    data.dialogForm = res.data
    data.processType = res.data.job_type
  },

  deleteRow(row: WarehouseProcessingVO) {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteMessage'),
      handleConfirm: async () => {
        if (row.id) {
          const { data: res } = await deleteStockProcess(row.id)
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
      filename: i18n.global.t('router.sideBar.warehouseProcessing'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },

  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.refresh()
  },

  // The btn will become disabled when the 'process_status' is false
  confirmProcessBtnDisabled: (row: WarehouseProcessingVO) => !!row.process_status,

  // The btn will become disabled when the 'process_status' is false
  // or the 'adjust_status' is true
  confirmAdjustBtnDisabled: (row: WarehouseProcessingVO) => !row.process_status || !!row.adjust_status,

  confirmProcess: async (row: WarehouseProcessingDetailVO) => {
    hookComponent.$dialog({
      content: i18n.global.t('wms.warehouseWorking.warehouseProcessing.beforeConfirmProcess'),
      handleConfirm: async () => {
        if (row.id) {
          const { data: res } = await confirmProcess(row.id)
          if (!res.isSuccess) {
            hookComponent.$message({
              type: 'error',
              content: res.errorMessage
            })
            return
          }
          hookComponent.$message({
            type: 'success',
            content: `${ i18n.global.t('wms.warehouseWorking.warehouseProcessing.confirmProcess') }${ i18n.global.t('system.tips.success') }`
          })
          method.refresh()
        }
      }
    })
  },

  confirmAdjust: async (row: WarehouseProcessingDetailVO) => {
    hookComponent.$dialog({
      content: i18n.global.t('wms.warehouseWorking.warehouseProcessing.beforeConfirmAdjust'),
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
            content: `${ i18n.global.t('wms.warehouseWorking.warehouseProcessing.confirmAdjust') }${ i18n.global.t('system.tips.success') }`
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
      name: i18n.global.t('wms.warehouseWorking.warehouseProcessing.process_split'),
      icon: 'mdi-arrow-split-vertical',
      code: 'split',
      click: () => {
        method.add(PROCESS_JOB_SPLIT)
      }
    },
    {
      name: i18n.global.t('wms.warehouseWorking.warehouseProcessing.process_combine'),
      icon: 'mdi-group',
      code: 'group',
      click: () => {
        method.add(PROCESS_JOB_COMBINE)
      }
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
    },
    {
      name: i18n.global.t('base.commodityManagement.printQrCode'),
      icon: 'mdi-qrcode',
      code: '',
      click: method.printQrCode
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
