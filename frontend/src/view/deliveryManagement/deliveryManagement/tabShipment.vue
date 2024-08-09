<template>
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
          <v-col cols="4">
            <v-text-field
              v-model="data.searchForm.dispatch_no"
              clearable
              hide-details
              density="comfortable"
              class="searchInput ml-5 mt-1"
              :label="$t('wms.deliveryManagement.dispatch_no')"
              variant="solo"
            >
            </v-text-field>
          </v-col>
          <v-col cols="4">
            <v-select
              v-model="data.searchForm.dispatch_status"
              :items="data.combobox.dispatch_status"
              item-title="label"
              item-value="value"
              :label="$t('wms.deliveryManagement.dispatch_status')"
              variant="solo"
              density="comfortable"
              class="searchInput ml-5 mt-1"
              clearable
              hide-details
            ></v-select>
          </v-col>
          <v-col cols="4">
            <v-text-field
              v-model="data.searchForm.customer_name"
              clearable
              hide-details
              density="comfortable"
              class="searchInput ml-5 mt-1"
              :label="$t('wms.deliveryManagement.customer_name')"
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
      <vxe-column type="checkbox" width="50" fixed="left"></vxe-column>
      <vxe-column type="seq" width="60"></vxe-column>
      <vxe-column field="dispatch_no" width="120" :title="$t('wms.deliveryManagement.dispatch_no')"></vxe-column>
      <vxe-column field="dispatch_status" :title="$t('wms.deliveryManagement.dispatch_status')">
        <template #default="{ row }">
          <span>{{ getShipmentState(row.dispatch_status) }}</span>
        </template>
      </vxe-column>
      <vxe-column field="qty" :title="$t('wms.deliveryManagement.qty')"></vxe-column>
      <vxe-column field="weight" :title="$t('wms.deliveryManagement.weight')">
        <template #default="{ row }">
          <span>{{ `${row.weight} ${GetUnit('weight', 2)}` }}</span>
        </template>
      </vxe-column>
      <vxe-column field="volume" :title="$t('wms.deliveryManagement.volume')">
        <template #default="{ row }">
          <span>{{ `${row.volume} ${GetUnit('volume', 1)}` }}</span>
        </template>
      </vxe-column>
      <vxe-column field="customer_name" :title="$t('wms.deliveryManagement.customer_name')"></vxe-column>
      <vxe-column field="creator" :title="$t('wms.deliveryManagement.creator')"></vxe-column>
      <vxe-column field="operate" :title="$t('system.page.operate')" width="300px" :resizable="false" show-overflow>
        <template #default="{ row }">
          <div style="width: 100%; display: flex; justify-content: center">
            <tooltip-btn :flat="true" icon="mdi-eye-outline" :tooltip-text="$t('system.page.view')" @click="method.viewRow(row)"></tooltip-btn>
            <tooltip-btn
              :disabled="!data.authorityList.includes('invoice-confirm') || (row.dispatch_status !== 0 && row.dispatch_status !== 1)"
              :flat="true"
              icon="mdi-clipboard-check-outline"
              :tooltip-text="$t('wms.deliveryManagement.confirmOrder')"
              @click="method.confirmOrder(row)"
            ></tooltip-btn>
            <tooltip-btn
              :disabled="!data.authorityList.includes('picked-confirm') || row.dispatch_status !== 2"
              :flat="true"
              icon="mdi-cart-arrow-down"
              :tooltip-text="$t('wms.deliveryManagement.confirmPicking')"
              @click="method.confirmPicking(row)"
            ></tooltip-btn>
            <tooltip-btn
              :disabled="
                (row.dispatch_status === 2 && !data.authorityList.includes('invoice-revoke')) ||
                  (row.dispatch_status === 3 && !data.authorityList.includes('picked-revoke')) ||
                  (row.dispatch_status !== 2 && row.dispatch_status !== 3)
              "
              :flat="true"
              icon="mdi-arrow-u-left-top"
              :tooltip-text="$t('wms.deliveryManagement.backToThePreviousStep')"
              @click="method.backToThePreviousStep(row)"
            ></tooltip-btn>
            <tooltip-btn
              :disabled="!data.authorityList.includes('invoice-delete') || (row.dispatch_status !== 0 && row.dispatch_status !== 1)"
              :flat="true"
              icon="mdi-delete-outline"
              :tooltip-text="$t('system.page.delete')"
              :icon-color="
                !data.authorityList.includes('invoice-delete') || (row.dispatch_status !== 0 && row.dispatch_status !== 1) ? '' : errorColor
              "
              @click="method.deleteRow(row)"
            ></tooltip-btn>
          </div>
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
    <!-- Add or modify data mode window -->
    <addOrUpdateShipment :show-dialog="data.showDialog" :form="data.dialogForm" @close="method.closeDialog" @saveSuccess="method.saveSuccess" />
    <!-- Confirm Order -->
    <ConfirmOrder
      :confirm-order-no="data.confirmOrderNo"
      :show-dialog="data.showConfirmOrder"
      @close="method.closeConfirmOrder"
      @save-success="method.confirmSuccess"
    />
    <!-- View order details -->
    <SearchDeliveredMainDetail
      :dispatch-no="data.showDeliveredMainDetailNo"
      :show-dialog="data.showDeliveredMainDetail"
      @close="method.closeDeliveredDetail"
    />

    <!-- Print QR code -->

    <qr-code-dialog ref="qrCodeDialogRef" :menu="'deliveryManagement-shipment'">
      <template #left="{ slotData }">
        <p>{{ $t('wms.deliveryManagement.dispatch_no') }}:{{ slotData.dispatch_no }}</p> &nbsp;
      </template>
    </qr-code-dialog>
  </div>
</template>

<script lang="ts" setup>
import { computed, ref, reactive, watch, onMounted } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight, errorColor } from '@/constant/style'
import { DeliveryManagementVO } from '@/types/DeliveryManagement/DeliveryManagement'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { hookComponent } from '@/components/system'
import { getShipment, delShipment, cancelOrderByDispatch, confirmPicking } from '@/api/wms/deliveryManagement'
import tooltipBtn from '@/components/tooltip-btn.vue'
import i18n from '@/languages/i18n'
import addOrUpdateShipment from './add-or-update-shipment.vue'
import { getShipmentState } from './shipmentFun'
import ConfirmOrder from './confirm-order.vue'
import { GetUnit } from '@/constant/commodityManagement'
import customPager from '@/components/custom-pager.vue'
import { setSearchObject, getMenuAuthorityList } from '@/utils/common'
import { TablePage, btnGroupItem } from '@/types/System/Form'
import { exportData } from '@/utils/exportTable'
import { DEBOUNCE_TIME } from '@/constant/system'
import SearchDeliveredMainDetail from './search-delivered-main-detail.vue'
import BtnGroup from '@/components/system/btnGroup.vue'
import QrCodeDialog from '@/components/codeDialog/qrCodeDialog.vue'
import { httpCodeJudge } from '@/utils/http/httpCodeJudge'

const xTable = ref()
const qrCodeDialogRef = ref()

const data = reactive({
  showDeliveredMainDetailNo: '',
  showDeliveredMainDetail: false,
  showDialog: false,
  dialogForm: {
    id: 0,
    detailList: []
  },
  searchForm: ref<{
    dispatch_no: string
    customer_name: string
    dispatch_status?: string
  }>({
    dispatch_no: '',
    customer_name: ''
  }),
  timer: ref<any>(null),
  activeTab: null,
  tableData: ref<DeliveryManagementVO[]>([]),
  tablePage: ref<TablePage>({
    total: 0,
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE,
    searchObjects: []
  }),
  showConfirmOrder: false,
  confirmOrderNo: '',
  combobox: {
    dispatch_status: [
      {
        label: i18n.global.t('wms.deliveryManagement.preShipment'),
        value: '0'
      },
      {
        label: i18n.global.t('wms.deliveryManagement.newShipment'),
        value: '1'
      },
      {
        label: i18n.global.t('wms.deliveryManagement.goodsToBePicked'),
        value: '2'
      },
      {
        label: i18n.global.t('wms.deliveryManagement.picked'),
        value: '3'
      },
      {
        label: i18n.global.t('wms.deliveryManagement.packaged'),
        value: '4'
      },
      {
        label: i18n.global.t('wms.deliveryManagement.weighed'),
        value: '5'
      },
      {
        label: i18n.global.t('wms.deliveryManagement.outOfWarehouse'),
        value: '6'
      },
      {
        label: i18n.global.t('wms.deliveryManagement.signedIn'),
        value: '7'
      }
    ]
  },
  btnList: [] as btnGroupItem[],
  // Menu operation permissions
  authorityList: getMenuAuthorityList(),
  selectRowData: []
})

const method = reactive({
  // Print QR code
  printQrCode: () => {
    const records = xTable.value.getCheckboxRecords()

    // data.selectRowData.length === 0 ? data.selectRowData = [row] : ''
    // const records:any[] = data.selectRowData
    if (records.length > 0) {
      for (const item of records) {
        item.id = item.dispatch_no
        item.type = 'delivery'
      }
      qrCodeDialogRef.value.openDialog(records)
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('base.userManagement.checkboxIsNull')
      })
    }
  },
  selectAllEvent({ checked }) {
    const records = xTable.value.getCheckboxRecords()
    checked ? (data.selectRowData = records) : (data.selectRowData = [])
  },
  selectChangeEvent() {
    data.selectRowData = xTable.value.getCheckboxRecords()
  },
  viewRow: (row: DeliveryManagementVO) => {
    if (row.dispatch_no) {
      data.showDeliveredMainDetailNo = row.dispatch_no
      data.showDeliveredMainDetail = true
    }
  },
  closeDeliveredDetail: () => {
    data.showDeliveredMainDetail = false
  },
  // Confirm picking
  confirmPicking: async (row: DeliveryManagementVO) => {
    hookComponent.$dialog({
      content: `${ i18n.global.t('wms.deliveryManagement.confirmPicking') }?`,
      handleConfirm: async () => {
        if (row.dispatch_no) {
          const { data: res } = await confirmPicking(row.dispatch_no)
          if (!res.isSuccess) {
            // 2023-12-06 Add automatic refresh of expired data
            if (httpCodeJudge(res.errorMessage)) {
              method.refresh()

              return
            }

            hookComponent.$message({
              type: 'error',
              content: res.errorMessage
            })
            return
          }
          hookComponent.$message({
            type: 'success',
            content: res.data
          })
          method.refresh()
        }
      }
    })
  },
  // Back to the previous step
  backToThePreviousStep(row: DeliveryManagementVO) {
    hookComponent.$dialog({
      content: `${ i18n.global.t('wms.deliveryManagement.confirmBack') }?`,
      handleConfirm: async () => {
        const { data: res } = await cancelOrderByDispatch({
          dispatch_no: row.dispatch_no,
          dispatch_status: row.dispatch_status
        })
        if (!res.isSuccess) {
          // 2023-12-06 Add automatic refresh of expired data
          if (httpCodeJudge(res.errorMessage)) {
            method.refresh()

            return
          }

          hookComponent.$message({
            type: 'error',
            content: res.errorMessage
          })
          return
        }
        hookComponent.$message({
          type: 'success',
          content: res.data
        })
        method.refresh()
      }
    })
  },
  // Close the order confirmation window
  closeConfirmOrder: () => {
    data.showConfirmOrder = false
  },
  // After confirming the shipment document
  confirmSuccess: () => {
    method.refresh()
    method.closeConfirmOrder()
  },
  // after Add or update success.
  saveSuccess: () => {
    method.refresh()
    method.closeDialog()
  },
  // Shut add or update dialog
  closeDialog: () => {
    data.showDialog = false
  },
  // Refresh dialog data
  clearDialogForm: () => {
    data.dialogForm = {
      id: 0,
      detailList: []
    }
  },
  add: () => {
    method.clearDialogForm()
    data.showDialog = true
  },
  confirmOrder: (row: DeliveryManagementVO) => {
    if (row.dispatch_status === undefined || ![0, 1].includes(row.dispatch_status)) {
      hookComponent.$message({
        type: 'error',
        content: `${ i18n.global.t('wms.deliveryManagement.incorrectStatusMsg') }${ getShipmentState(0) }${ i18n.global.t(
          'wms.deliveryManagement.or'
        ) }${ getShipmentState(1) }`
      })
      return
    }
    if (row.dispatch_no) {
      data.confirmOrderNo = row.dispatch_no
      data.showConfirmOrder = true
    }
  },
  deleteRow: (row: DeliveryManagementVO) => {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteMessage'),
      handleConfirm: async () => {
        if (row.dispatch_no) {
          const { data: res } = await delShipment(row.dispatch_no)
          if (!res.isSuccess) {
            // 2023-12-06 Add automatic refresh of expired data
            if (httpCodeJudge(res.errorMessage)) {
              method.refresh()

              return
            }

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
  // Refresh data
  refresh: () => {
    method.getShipment()
  },
  getShipment: async () => {
    const { data: res } = await getShipment(data.tablePage)
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

    method.getShipment()
  }),
  exportTable: () => {
    const $table = xTable.value
    exportData({
      table: $table,
      filename: i18n.global.t('wms.deliveryManagement.shipment'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },
  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm, ['dispatch_status'])
    method.getShipment()
  }
})

onMounted(() => {
  data.btnList = [
    {
      name: i18n.global.t('system.page.add'),
      icon: 'mdi-plus',
      code: 'invoice-save',
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
      code: 'invoice-export',
      click: method.exportTable
    },
    {
      name: i18n.global.t('base.commodityManagement.printQrCode'),
      icon: 'mdi-qrcode',
      code: 'invoice-printQrCode',
      click: method.printQrCode
    }
  ]
})

const cardHeight = computed(() => computedCardHeight({}))
const tableHeight = computed(() => computedTableHeight({}))

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

defineExpose({
  getShipment: method.getShipment
})
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
