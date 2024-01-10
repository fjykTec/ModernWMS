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
              v-model="data.searchForm.warehouse_name"
              clearable
              hide-details
              density="comfortable"
              class="searchInput ml-5 mt-1"
              :label="$t('base.warehouseSetting.warehouse_name')"
              variant="solo"
            >
            </v-text-field>
          </v-col>
          <v-col cols="4">
            <v-text-field
              v-model="data.searchForm.warehouse_area_name"
              clearable
              hide-details
              density="comfortable"
              class="searchInput ml-5 mt-1"
              :label="$t('base.warehouseSetting.area_name')"
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
    <vxe-table
      ref="xTableGoodsLocation"
      :column-config="{ minWidth: '100px' }"
      :data="data.tableData"
      :height="tableHeight"
      align="center"
    >
      <vxe-column type="checkbox" width="50" fixed="left"></vxe-column>
      <vxe-column type="seq" width="60"></vxe-column>
      <vxe-column field="warehouse_name" :title="$t('base.warehouseSetting.warehouse_name')"></vxe-column>
      <vxe-column field="warehouse_area_name" :title="$t('base.warehouseSetting.area_name')"></vxe-column>
      <vxe-column field="warehouse_area_property" :title="$t('base.warehouseSetting.area_property')">
        <template #default="{ row, column }">
          <span>{{ formatAreaProperty(row[column.property]) }}</span>
        </template>
      </vxe-column>
      <vxe-column field="location_name" :title="$t('base.warehouseSetting.location_name')"></vxe-column>
      <vxe-column width="150px" field="location_length" :title="$t('base.warehouseSetting.location_length')"></vxe-column>
      <vxe-column width="150px" field="location_width" :title="$t('base.warehouseSetting.location_width')"></vxe-column>
      <vxe-column width="150px" field="location_heigth" :title="$t('base.warehouseSetting.location_heigth')"></vxe-column>
      <vxe-column width="150px" field="location_volume" :title="$t('base.warehouseSetting.location_volume')"></vxe-column>
      <vxe-column width="150px" field="location_load" :title="$t('base.warehouseSetting.location_load')"></vxe-column>
      <vxe-column field="roadway_number" :title="$t('base.warehouseSetting.roadway_number')"></vxe-column>
      <vxe-column field="shelf_number" :title="$t('base.warehouseSetting.shelf_number')"></vxe-column>
      <vxe-column field="layer_number" :title="$t('base.warehouseSetting.layer_number')"></vxe-column>
      <vxe-column field="tag_number" :title="$t('base.warehouseSetting.tag_number')"></vxe-column>
      <vxe-column field="is_valid" :title="$t('base.warehouseSetting.is_valid')">
        <template #default="{ row, column }">
          <span>{{ formatIsValid(row[column.property]) }}</span>
        </template>
      </vxe-column>
      <vxe-column field="operate" :title="$t('system.page.operate')" width="140px" :resizable="false" show-overflow>
        <template #default="{ row }">
          <!-- <tooltip-btn
            :flat="true"
            icon="mdi-qrcode"
            :tooltip-text="$t('base.commodityManagement.printQrCode')"
            :disabled="!data.authorityList.includes('location-printQrCode')"
            @click="method.printQrCode(row)"
          ></tooltip-btn> -->
          <!-- <tooltip-btn
            :flat="true"
            icon="mdi-barcode"
            :tooltip-text="$t('base.commodityManagement.printBarCode')"
            :disabled="!data.authorityList.includes('location-printBarCode')"
            @click="method.printBarCode(row)"
          ></tooltip-btn> -->
          <tooltip-btn
            :flat="true"
            icon="mdi-pencil-outline"
            :tooltip-text="$t('system.page.edit')"
            :disabled="!data.authorityList.includes('location-save')"
            @click="method.editRow(row)"
          ></tooltip-btn>
          <tooltip-btn
            :flat="true"
            icon="mdi-delete-outline"
            :tooltip-text="$t('system.page.delete')"
            :icon-color="errorColor"
            :disabled="!data.authorityList.includes('location-delete')"
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
  <add-or-update-dialog :show-dialog="data.showDialog" :form="data.dialogForm" @close="method.closeDialog" @saveSuccess="method.saveSuccess" />

  <!-- Print QR code -->
  <qr-code-dialog ref="qrCodeDialogRef" :menu="'warehouseSetting-location'">
    <template #left="{ slotData }">
      <p>{{ $t('base.warehouseSetting.warehouse_name') }}:{{ slotData.warehouse_name }}</p> &nbsp;
      <p>{{ $t('base.warehouseSetting.area_name') }}:{{ slotData.warehouse_area_name }}</p> &nbsp;
      <p>{{ $t('base.warehouseSetting.location_name') }}:{{ slotData.location_name }}</p> &nbsp;
    </template>
  </qr-code-dialog>
  <!-- Print barcode -->
  <bar-code-dialog ref="barCodeDialogRef" :menu="'warehouseSetting-location'" />
</template>

<script lang="ts" setup>
import { computed, onMounted, reactive, ref, watch } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight, errorColor } from '@/constant/style'
import { GoodsLocationVO } from '@/types/Base/Warehouse'
import { DEFAULT_PAGE_SIZE, PAGE_LAYOUT, PAGE_SIZE } from '@/constant/vxeTable'
import { hookComponent } from '@/components/system'
import { deleteGoodsLocation, getGoodsLocationList } from '@/api/base/warehouseSetting'
import i18n from '@/languages/i18n'
import { formatIsValid } from '@/utils/format/formatSystem'
import { formatAreaProperty } from '@/utils/format/formatWarehouse'
import { getMenuAuthorityList, setSearchObject } from '@/utils/common'
import { DEBOUNCE_TIME } from '@/constant/system'
import { btnGroupItem, SearchObject } from '@/types/System/Form'
import { exportData } from '@/utils/exportTable'
import tooltipBtn from '@/components/tooltip-btn.vue'
import addOrUpdateDialog from './add-or-update-location.vue'
import customPager from '@/components/custom-pager.vue'
import BtnGroup from '@/components/system/btnGroup.vue'
import BarCodeDialog from '@/components/codeDialog/barCodeDialog.vue'
import QrCodeDialog from '@/components/codeDialog/qrCodeDialog.vue'

const xTableGoodsLocation = ref()
const qrCodeDialogRef = ref()
const barCodeDialogRef = ref()

const data = reactive({
  showDialog: false,
  dialogForm: ref<GoodsLocationVO>({
    id: 0,
    warehouse_name: '',
    warehouse_area_name: '',
    location_name: '',
    location_length: 0,
    location_width: 0,
    location_heigth: 0,
    location_volume: 0,
    location_load: 0,
    roadway_number: '',
    shelf_number: '',
    layer_number: '',
    tag_number: '',
    is_valid: true
  }),
  searchForm: {
    warehouse_name: '',
    warehouse_area_name: ''
  },
  activeTab: null,
  tableData: ref<GoodsLocationVO[]>([]),
  tablePage: reactive({
    total: 0,
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE,
    searchObjects: ref<Array<SearchObject>>([])
  }),
  selectRowData: [],
  timer: ref<any>(null),
  btnList: [] as btnGroupItem[],
  // Menu operation permissions
  authorityList: getMenuAuthorityList() as string[]
})

const method = reactive({
  // Print QR code
  printQrCode: () => {
    const records = xTableGoodsLocation.value.getCheckboxRecords()

    // data.selectRowData.length === 0 ? (data.selectRowData = [row]) : ''
    // const records: any[] = data.selectRowData
    if (records.length > 0) {
      for (const item of records) {
        item.type = 'warehouse'
      }
      qrCodeDialogRef.value.openDialog(records)
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('base.userManagement.checkboxIsNull')
      })
    }
  },
  printBarCode: () => {
    let records = xTableGoodsLocation.value.getCheckboxRecords()
    records = records.filter((item) => item.id)
    // data.selectRowData.length === 0 ? (data.selectRowData = [row]) : ''
    // let records: any[] = data.selectRowData

    if (records.length > 0) {
      barCodeDialogRef.value.openDialog(records)
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('base.userManagement.checkboxIsNull')
      })
    }
  },
  selectAllEvent({ checked }) {
    const records = xTableGoodsLocation.value.getCheckboxRecords()
    checked ? (data.selectRowData = records) : (data.selectRowData = [])
  },
  selectChangeEvent() {
    data.selectRowData = xTableGoodsLocation.value.getCheckboxRecords()
  },
  // Open a dialog to add
  add: () => {
    data.dialogForm = {
      id: 0,
      warehouse_name: '',
      warehouse_area_name: '',
      location_name: '',
      location_length: 0,
      location_width: 0,
      location_heigth: 0,
      location_volume: 0,
      location_load: 0,
      roadway_number: '',
      shelf_number: '',
      layer_number: '',
      tag_number: '',
      is_valid: true
    }
    data.showDialog = true
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
    method.getGoodsLocationList()
  },
  getGoodsLocationList: async () => {
    const { data: res } = await getGoodsLocationList(data.tablePage)
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
  editRow(row: GoodsLocationVO) {
    data.dialogForm = JSON.parse(JSON.stringify(row))
    data.showDialog = true
  },
  deleteRow(row: GoodsLocationVO) {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteMessage'),
      handleConfirm: async () => {
        if (row.id) {
          const { data: res } = await deleteGoodsLocation(row.id)
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

    method.getGoodsLocationList()
  }),
  exportTable: () => {
    const $table = xTableGoodsLocation.value
    exportData({
      table: $table,
      filename: i18n.global.t('base.warehouseSetting.locationSetting'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },
  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.getGoodsLocationList()
  }
})

onMounted(() => {
  data.btnList = [
    {
      name: i18n.global.t('system.page.add'),
      icon: 'mdi-plus',
      code: 'location-save',
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
      code: 'location-export',
      click: method.exportTable
    },
    {
      name: i18n.global.t('base.commodityManagement.printQrCode'),
      icon: 'mdi-qrcode',
      code: 'location-printQrCode',
      click: method.printQrCode
    },
    {
      name: i18n.global.t('base.commodityManagement.printBarCode'),
      icon: 'mdi-barcode',
      code: 'location-printBarCode',
      click: method.printBarCode
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
  getGoodsLocationList: method.getGoodsLocationList
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
