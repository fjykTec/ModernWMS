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
                    <!-- <tooltip-btn
                      icon="mdi-lock-open-outline"
                      :tooltip-text="$t('wms.warehouseWorking.warehouseFreeze.freeze')"
                      @click="method.add(FREEZE_JOB_FREEZE)"
                    ></tooltip-btn>
                    <tooltip-btn
                      icon="mdi-lock-open-variant-outline"
                      :tooltip-text="$t('wms.warehouseWorking.warehouseFreeze.unfreeze')"
                      @click="method.add(FREEZE_JOB_UNFREEZE)"
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
                          :label="$t('wms.warehouseWorking.warehouseFreeze.job_code')"
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
                  <vxe-column field="job_code" width="150px" :title="$t('wms.warehouseWorking.warehouseFreeze.job_code')"></vxe-column>
                  <vxe-column field="job_type" width="150px" :title="$t('wms.warehouseWorking.warehouseFreeze.job_type')">
                    <template #default="{ row, column }">
                      <span>{{ formatFreezeJobType(row[column.property]) }}</span>
                    </template>
                  </vxe-column>
                  <vxe-column field="warehouse_name" width="150px" :title="$t('wms.warehouseWorking.warehouseFreeze.warehouse')"></vxe-column>
                  <vxe-column field="location_name" width="150px" :title="$t('wms.warehouseWorking.warehouseFreeze.location_name')"></vxe-column>
                  <vxe-column field="spu_code" width="150px" :title="$t('base.commodityManagement.spu_code')"></vxe-column>
                  <vxe-column field="spu_name" width="150px" :title="$t('base.commodityManagement.spu_name')"></vxe-column>
                  <vxe-column field="sku_code" width="150px" :title="$t('base.commodityManagement.sku_code')"></vxe-column>
                  <vxe-column field="series_number" width="150px" :title="$t('wms.stockLocation.series_number')"></vxe-column>
                  <vxe-column field="handler" width="150px" :title="$t('wms.warehouseWorking.warehouseFreeze.handler')"></vxe-column>
                  <vxe-date-column
                    field="handle_time"
                    width="170px"
                    format="yyyy-MM-dd HH:mm"
                    :title="$t('wms.warehouseWorking.warehouseFreeze.handle_time')"
                  ></vxe-date-column>
                  <!-- <vxe-column field="creator" :title="$t('wms.warehouseWorking.warehouseFreeze.creator')"></vxe-column>
                  <vxe-column
                    field="create_time"
                    width="170px"
                    :title="$t('wms.warehouseWorking.warehouseFreeze.create_time')"
                  ></vxe-column> -->
                  <vxe-column field="operate" :title="$t('system.page.operate')" width="100" :resizable="false" show-overflow>
                    <template #default="{ row }">
                      <tooltip-btn
                        :flat="true"
                        icon="mdi-eye-outline"
                        :tooltip-text="$t('system.page.view')"
                        @click="method.viewRow(row)"
                      ></tooltip-btn>
                      <!-- <tooltip-btn
                        :flat="true"
                        icon="mdi-book-open-outline"
                        :tooltip-text="$t('wms.warehouseWorking.warehouseMove.confirmMove')"
                        @click="method.confirmMove(row)"
                      ></tooltip-btn> -->
                      <!-- <tooltip-btn
                        :flat="true"
                        icon="mdi-delete-outline"
                        :tooltip-text="$t('system.page.delete')"
                        :icon-color="errorColor"
                        @click="method.deleteRow(row)"
                      ></tooltip-btn> -->
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
        :freeze-type="data.freezeType"
        @close="method.closeDialog"
        @saveSuccess="method.saveSuccess"
      />
    </div>
  </div>
</template>

<script lang="ts" setup>
import { computed, ref, reactive, onActivated, watch, nextTick, onMounted } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight } from '@/constant/style'
import { WarehouseFreezeVO } from '@/types/WarehouseWorking/WarehouseFreeze'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { FREEZE_JOB_FREEZE, FREEZE_JOB_UNFREEZE } from '@/constant/warehouseWorking'
import { hookComponent } from '@/components/system'
import { formatFreezeJobType } from '@/utils/format/formatWarehouseWorking'
import { getStockFreezeList, getStockFreezeOne } from '@/api/wms/warehouseFreeze'
import { DEBOUNCE_TIME } from '@/constant/system'
import { setSearchObject, getMenuAuthorityList } from '@/utils/common'
import { SearchObject, btnGroupItem } from '@/types/System/Form'
import tooltipBtn from '@/components/tooltip-btn.vue'
import addOrUpdateDialog from './add-or-update-freeze.vue'
import i18n from '@/languages/i18n'
import customPager from '@/components/custom-pager.vue'
import { exportData } from '@/utils/exportTable'
import BtnGroup from '@/components/system/btnGroup.vue'

const xTable = ref()

const data = reactive({
  showDialog: false,
  freezeType: FREEZE_JOB_FREEZE,
  timer: ref<any>(null),
  activeTab: null,
  searchForm: {
    job_code: ''
  },
  tableData: ref<WarehouseFreezeVO[]>([]),
  dialogForm: {
    id: 0,
    job_code: '',
    job_type: FREEZE_JOB_FREEZE,
    sku_id: 0,
    goods_owner_id: 0,
    goods_location_id: 0,
    handler: '',
    handle_time: '',
    last_update_time: '',
    tenant_id: 0,
    warehouse_name: '',
    location_name: '',
    spu_code: '',
    spu_name: '',
    sku_code: '',
    series_number: '',
    creator: '',
    create_time: ''
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
  add: (jobType: boolean) => {
    data.freezeType = jobType
    data.dialogForm = {
      id: 0,
      job_code: '',
      job_type: FREEZE_JOB_FREEZE,
      sku_id: 0,
      goods_owner_id: 0,
      goods_location_id: 0,
      handler: '',
      handle_time: '',
      last_update_time: '',
      tenant_id: 0,
      warehouse_name: '',
      location_name: '',
      spu_code: '',
      spu_name: '',
      sku_code: '',
      series_number: '',
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

  // Refresh data
  refresh: () => {
    method.getStockProcessList()
  },

  // Shut add or update dialog
  closeDialog: () => {
    data.showDialog = false
  },

  getStockProcessList: async () => {
    const { data: res } = await getStockFreezeList(data.tablePage)
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

  viewRow: async (row: WarehouseFreezeVO) => {
    await method.getOne(row.id)
    nextTick(() => {
      data.showDialog = true
    })
  },

  getOne: async (id: number) => {
    const { data: res } = await getStockFreezeOne(id)
    if (!res.isSuccess) {
      hookComponent.$message({
        type: 'error',
        content: res.errorMessage
      })
      return
    }

    data.dialogForm = res.data
  },

  // deleteRow(row: WarehouseFreezeVO) {
  //   hookComponent.$dialog({
  //     content: i18n.global.t('system.tips.beforeDeleteMessage'),
  //     handleConfirm: async () => {
  //       if (row.id) {
  //         const { data: res } = await deleteStockFreeze(row.id)
  //         if (!res.isSuccess) {
  //           hookComponent.$message({
  //             type: 'error',
  //             content: res.errorMessage
  //           })
  //           return
  //         }

  //         hookComponent.$message({
  //           type: 'success',
  //           content: `${ i18n.global.t('system.page.delete') }${ i18n.global.t('system.tips.success') }`
  //         })
  //         method.refresh()
  //       }
  //     }
  //   })
  // },

  handlePageChange: ref<VxePagerEvents.PageChange>(({ currentPage, pageSize }) => {
    data.tablePage.pageIndex = currentPage
    data.tablePage.pageSize = pageSize
    method.refresh()
  }),

  exportTable: () => {
    const $table = xTable.value
    exportData({
      table: $table,
      filename: i18n.global.t('router.sideBar.warehouseFreeze'),
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

onActivated(() => {
  method.refresh()
})

onMounted(() => {
  data.btnList = [
    {
      name: i18n.global.t('wms.warehouseWorking.warehouseFreeze.freeze'),
      icon: 'mdi-lock-open-outline',
      code: 'freeze',
      click: () => {
        method.add(FREEZE_JOB_FREEZE)
      }
    },
    {
      name: i18n.global.t('wms.warehouseWorking.warehouseFreeze.unfreeze'),
      icon: 'mdi-lock-open-variant-outline',
      code: 'unfreeze',
      click: () => {
        method.add(FREEZE_JOB_UNFREEZE)
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
