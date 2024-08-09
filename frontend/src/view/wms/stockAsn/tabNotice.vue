<template>
  <div class="operateArea">
    <v-row no-gutters>
      <!-- Operate Btn -->
      <v-col cols="3" class="col">
        <!-- <tooltip-btn icon="mdi-plus" :tooltip-text="$t('system.page.add')" @click="method.add"></tooltip-btn>
        <tooltip-btn icon="mdi-refresh" :tooltip-text="$t('system.page.refresh')" @click="method.refresh"></tooltip-btn>
        <tooltip-btn icon="mdi-export-variant" :tooltip-text="$t('system.page.export')" @click="method.exportTable"> </tooltip-btn> -->

        <!-- new version -->
        <BtnGroup :authority-list="data.authorityList" :btn-list="data.btnList" />
      </v-col>

      <!-- Search Input -->
      <v-col cols="9">
        <v-row no-gutters @keyup.enter="method.sureSearch">
          <v-col cols="4"></v-col>
          <v-col cols="4">
            <v-text-field
              v-model="data.searchForm.supplier_name"
              clearable
              hide-details
              density="comfortable"
              class="searchInput ml-5 mt-1"
              :label="$t('wms.stockAsnInfo.supplier_name')"
              variant="solo"
            >
            </v-text-field>
          </v-col>
          <v-col cols="4">
            <v-text-field
              v-model="data.searchForm.sku_name"
              clearable
              hide-details
              density="comfortable"
              class="searchInput ml-5 mt-1"
              :label="$t('wms.stockAsnInfo.sku_name')"
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
      <vxe-column field="asn_no" :title="$t('wms.stockAsnInfo.asn_no')"></vxe-column>
      <vxe-column field="asn_batch" :title="$t('wms.stockAsnInfo.asn_batch')"></vxe-column>
      <vxe-date-column field="estimated_arrival_time" :title="$t('wms.stockAsnInfo.estimated_arrival_time')"></vxe-date-column>
      <vxe-column field="goods_owner_name" :title="$t('wms.stockAsnInfo.goods_owner_name')"></vxe-column>
      <vxe-column field="operate" :title="$t('system.page.operate')" width="140px" :resizable="false" show-overflow>
        <template #default="{ row }">
          <!-- <tooltip-btn
            :flat="true"
            icon="mdi-qrcode"
            :tooltip-text="$t('base.commodityManagement.printQrCode')"
            :disabled="!data.authorityList.includes('notice-printQrCode')"
            @click="method.printQrCode(row)"
          ></tooltip-btn> -->
          <tooltip-btn
            :flat="true"
            icon="mdi-pencil-outline"
            :tooltip-text="$t('system.page.edit')"
            :disabled="!data.authorityList.includes('notice-save')"
            @click="method.editRow(row)"
          ></tooltip-btn>
          <tooltip-btn
            :flat="true"
            icon="mdi-delete-outline"
            :tooltip-text="$t('system.page.delete')"
            :icon-color="!data.authorityList.includes('notice-delete') ? '' : errorColor"
            :disabled="!data.authorityList.includes('notice-delete')"
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
  <addOrUpdateNotice :show-dialog="data.showDialog" :form="data.dialogForm" @close="method.closeDialog" @saveSuccess="method.saveSuccess" />
  <skuInfo :show-dialog="data.showDialogShowInfo" :form="data.dialogForm" @close="method.closeDialogShowInfo" />

  <!-- Print QR code -->
  <qr-code-dialog ref="qrCodeDialogRef" :menu="'stockAsnInfo-notice'">
    <template #left="{ slotData }">
      <p>{{ $t('wms.stockAsnInfo.num') }}:{{ slotData.asn_no }}</p> &nbsp;
      <p>{{ $t('wms.stockAsnInfo.asn_batch') }}:{{ slotData.asn_batch }}</p> &nbsp;
    </template>
  </qr-code-dialog>
</template>

<script lang="ts" setup>
import { computed, ref, reactive, watch, onMounted } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight, errorColor } from '@/constant/style'
import { StockAsnVO } from '@/types/WMS/StockAsn'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { hookComponent } from '@/components/system'
import { DEBOUNCE_TIME } from '@/constant/system'
import { setSearchObject, getMenuAuthorityList } from '@/utils/common'
import { SearchObject, btnGroupItem } from '@/types/System/Form'
import { listNew as getStockAsnList, deleteAsnByID } from '@/api/wms/stockAsn'
import tooltipBtn from '@/components/tooltip-btn.vue'
import i18n from '@/languages/i18n'
import addOrUpdateNotice from './new-add-or-update-notice.vue'
import customPager from '@/components/custom-pager.vue'
import skuInfo from './sku-info.vue'
import { exportData } from '@/utils/exportTable'
import BtnGroup from '@/components/system/btnGroup.vue'
import QrCodeDialog from '@/components/codeDialog/qrCodeDialog.vue'
import { httpCodeJudge } from '@/utils/http/httpCodeJudge'

const xTable = ref()
const qrCodeDialogRef = ref()

const data = reactive({
  showDialog: false,
  showDialogShowInfo: false,
  searchForm: {
    supplier_name: '',
    sku_name: ''
  },
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
  activeTab: null,
  tableData: ref<StockAsnVO[]>([]),
  tablePage: reactive({
    total: 0,
    sqlTitle: 'asn_status:0',
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE,
    searchObjects: ref<Array<SearchObject>>([])
  }),
  timer: ref<any>(null),
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
        item.type = 'asn'
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
  closeDialogShowInfo: () => {
    data.showDialogShowInfo = false
  },
  showSkuInfo(row: StockAsnVO) {
    data.dialogForm = JSON.parse(JSON.stringify(row))
    data.showDialogShowInfo = true
  },
  // Open a dialog to add
  add: () => {
    data.dialogForm = {
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
    }
    data.showDialog = true
  },
  editRow(row: StockAsnVO) {
    data.dialogForm = JSON.parse(JSON.stringify(row))
    data.showDialog = true
  },
  deleteRow(row: StockAsnVO) {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteMessage'),
      handleConfirm: async () => {
        if (row.id) {
          const { data: res } = await deleteAsnByID(row.id)
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
  // Shut add or update dialog
  closeDialog: () => {
    data.showDialog = false
  },
  // After add or update success.
  saveSuccess: () => {
    method.refresh()
    method.closeDialog()
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
    const $table = xTable.value
    exportData({
      table: $table,
      filename: i18n.global.t('wms.stockAsn.tabNotice'),
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
  data.btnList = [
    {
      name: i18n.global.t('system.page.add'),
      icon: 'mdi-plus',
      code: 'notice-save',
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
      code: 'notice-export',
      click: method.exportTable
    }
    // {
    //   name: i18n.global.t('base.commodityManagement.printQrCode'),
    //   icon: 'mdi-qrcode',
    //   code: 'notice-printQrCode',
    //   click: method.printQrCode
    // }
  ]
})

const cardHeight = computed(() => computedCardHeight({}))
const tableHeight = computed(() => computedTableHeight({}))

defineExpose({
  getStockAsnList: method.getStockAsnList
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
