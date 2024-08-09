<template>
  <div class="container">
    <div>
      <!-- Main Content -->
      <v-card class="mt-5">
        <v-card-text>
          <div class="operateArea">
            <v-row no-gutters>
              <!-- Operate Btn -->
              <v-col cols="12" sm="4" class="col">
                <!-- <tooltip-btn icon="mdi-plus" :tooltip-text="$t('system.page.add')" @click="method.add()"></tooltip-btn>
                <tooltip-btn icon="mdi-refresh" :tooltip-text="$t('system.page.refresh')" @click="method.refresh()"></tooltip-btn>
                <tooltip-btn icon="mdi-export-variant" :tooltip-text="$t('system.page.export')" @click="method.exportTable"></tooltip-btn> -->

                <!-- new version -->
                <BtnGroup :authority-list="data.authorityList" :btn-list="data.btnList" />
              </v-col>

              <!-- Search Input -->
              <v-col cols="12" sm="8">
                <v-row no-gutters @keyup.enter="method.sureSearch">
                  <v-col cols="4">
                    <v-text-field
                      v-model="data.searchForm.spu_code"
                      clearable
                      hide-details
                      density="comfortable"
                      class="searchInput ml-5 mt-1"
                      :label="$t('base.commodityManagement.spu_code')"
                      variant="solo"
                    >
                    </v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-text-field
                      v-model="data.searchForm.spu_name"
                      clearable
                      hide-details
                      density="comfortable"
                      class="searchInput ml-5 mt-1"
                      :label="$t('base.commodityManagement.spu_name')"
                      variant="solo"
                    >
                    </v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-text-field
                      v-model="data.searchForm.category_name"
                      clearable
                      hide-details
                      density="comfortable"
                      class="searchInput ml-5 mt-1"
                      :label="$t('base.commodityManagement.category_name')"
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
              ref="xTable"
              :data="data.tableData"
              :column-config="{
                minWidth: '100px'
              }"
              :height="tableHeight"
              align="center"
              :tree-config="data.tableTreeConfig"
            >
              <template #empty>
                {{ i18n.global.t('system.page.noData') }}
              </template>
              <vxe-column type="checkbox" width="50" fixed="left"></vxe-column>
              <vxe-column type="seq" width="60"></vxe-column>
              <vxe-column tree-node width="60">
                <template #header>
                  <div
                    style="height: 100%; display: flex; align-items: center; justify-content: flex-start; cursor: pointer"
                    @click="method.expandAllRows()"
                  >
                    <v-tooltip location="bottom">
                      <template #activator="{ props }">
                        <div v-bind="props">
                          <v-icon v-if="!isExpandAll" size="large">mdi-menu-right</v-icon>
                          <v-icon v-else size="large">mdi-menu-down</v-icon>
                        </div>
                      </template>
                      <span>{{ $t('base.roleMenu.expandRow') }}</span>
                    </v-tooltip>
                  </div>
                </template>
              </vxe-column>
              <vxe-column field="spu_code" width="150px" :title="$t('base.commodityManagement.spu_code')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ row.sku_code }}</span>
                  <span v-else>{{ row.spu_code }}</span>
                </template>
              </vxe-column>
              <vxe-column field="spu_name" :title="$t('base.commodityManagement.spu_name')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ row.sku_name }}</span>
                  <span v-else>{{ row.spu_name }}</span>
                </template>
              </vxe-column>
              <vxe-column field="category_name" :title="$t('base.commodityManagement.category_name')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ row.unit }}</span>
                  <span v-else>{{ row.category_name }}</span>
                </template>
              </vxe-column>
              <vxe-column field="spu_description" width="200" :title="$t('base.commodityManagement.spu_description')"> </vxe-column>
              <vxe-column field="supplier_name" :title="$t('base.commodityManagement.supplier_name')"> </vxe-column>
              <vxe-column field="brand" :title="$t('base.commodityManagement.brand')"> </vxe-column>

              <vxe-column field="bar_code" :title="$t('base.commodityManagement.bar_code')"> </vxe-column>
              <vxe-column field="weight" :title="$t('base.commodityManagement.weight')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ `${row.weight} ${GetUnit('weight', row.weight_unit)}` }}</span>
                </template>
              </vxe-column>
              <vxe-column field="lenght" :title="$t('base.commodityManagement.lenght')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ `${row.lenght} ${GetUnit('length', row.length_unit)}` }}</span>
                </template>
              </vxe-column>
              <vxe-column field="width" :title="$t('base.commodityManagement.width')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ `${row.width} ${GetUnit('length', row.length_unit)}` }}</span>
                </template>
              </vxe-column>
              <vxe-column field="height" :title="$t('base.commodityManagement.height')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ `${row.height} ${GetUnit('length', row.length_unit)}` }}</span>
                </template>
              </vxe-column>
              <vxe-column field="volume" :title="$t('base.commodityManagement.volume')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ `${row.volume} ${GetUnit('volume', row.volume_unit)}` }}</span>
                </template>
              </vxe-column>

              <vxe-column field="cost" :title="$t('base.commodityManagement.cost')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ row.cost }}</span>
                </template>
              </vxe-column>
              <vxe-column field="price" :title="$t('base.commodityManagement.price')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ row.price }}</span>
                </template>
              </vxe-column>
              <!-- <vxe-column field="sku_name" :title="$t('base.commodityManagement.sku_name')"></vxe-column> -->
              <!-- <vxe-column field="supplier_name" :title="$t('base.commodityManagement.supplier_name')"></vxe-column>
              <vxe-column field="brand" :title="$t('base.commodityManagement.brand')"></vxe-column>
              <vxe-column field="unit" :title="$t('base.commodityManagement.unit')"></vxe-column>
              <vxe-column field="cost" :title="$t('base.commodityManagement.cost')"></vxe-column> -->
              <vxe-column field="operate" :title="$t('system.page.operate')" width="160px" :resizable="false" show-overflow>
                <template #default="{ row }">
                  <div v-if="!row.parent_id || row.parent_id <= 0">
                    <tooltip-btn
                      :flat="true"
                      icon="mdi-pencil-outline"
                      :tooltip-text="$t('system.page.edit')"
                      :disabled="!data.authorityList.includes('save')"
                      @click="method.editRow(row)"
                    ></tooltip-btn>
                    <tooltip-btn
                      :flat="true"
                      icon="mdi-delete-outline"
                      :tooltip-text="$t('system.page.delete')"
                      :icon-color="!data.authorityList.includes('delete') ? '' : errorColor"
                      :disabled="!data.authorityList.includes('delete')"
                      @click="method.deleteRow(row)"
                    ></tooltip-btn>
                  </div>
                  <div v-else>
                    <!-- <tooltip-btn
                      :flat="true"
                      icon="mdi-qrcode"
                      :tooltip-text="$t('base.commodityManagement.printQrCode')"
                      :disabled="!data.authorityList.includes('printQrCode')"
                      @click="method.printQrCode(row)"
                    ></tooltip-btn> -->
                    <!-- <tooltip-btn
                      :flat="true"
                      icon="mdi-barcode"
                      :tooltip-text="$t('base.commodityManagement.printBarCode')"
                      :disabled="!data.authorityList.includes('printBarCode')"
                      @click="method.printBarCode(row)"
                    ></tooltip-btn> -->
                    <tooltip-btn
                      :flat="true"
                      icon="mdi-alarm-light"
                      :tooltip-text="$t('base.commodityManagement.saftyStock')"
                      :disabled="!data.authorityList.includes('saftyStock')"
                      @click="method.openUpdateSaftyStockDialog(row)"
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
          </div>
        </v-card-text>
      </v-card>
    </div>
    <!-- Add or modify data mode window -->
    <addOrUpdateDialog :show-dialog="data.showDialog" :form="data.dialogForm" @close="method.closeDialog" @saveSuccess="method.saveSuccess" />

    <update-sku-safety-stock ref="updateSkuSaftyStockRef" @sure="method.updateSkuSaftyStockByRow" />

    <!-- Print QR code -->
    <qr-code-dialog ref="qrCodeDialogRef" :menu="'commodityManagement'">
      <template #left="{ slotData }">
        <p>{{ $t('base.commodityManagement.spu_code') }}:{{ slotData.spu_code }}</p>
        <p>{{ $t('base.commodityManagement.spu_name') }}:{{ slotData.spu_name }}</p>
        <p>{{ $t('base.commodityManagement.sku_code') }}:{{ slotData.sku_code }}</p>
        <p>{{ $t('base.commodityManagement.sku_name') }}:{{ slotData.sku_name }}</p>
      </template>
    </qr-code-dialog>

    <!-- Print barcode -->
    <bar-code-dialog ref="barCodeDialogRef" :menu="'commodityManagement'" />
    <hprintDialog ref="hprintDialogRef" :form="printDate.printForm" :tab-page="'print_page_main'" />
  </div>
</template>

<script lang="ts" setup>
import { computed, onMounted, reactive, ref, watch } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight, errorColor } from '@/constant/style'
import tooltipBtn from '@/components/tooltip-btn.vue'
import { CommodityVO, DataProps, CommodityDetailVO, UpdateSaftyStockReqBodyVO } from '@/types/Base/CommodityManagement'
import { getSpuList, deleteSpu, updateSaftyStock } from '@/api/base/commodityManagementSetting'
import { hookComponent } from '@/components/system'
import addOrUpdateDialog from './add-or-update-commodity.vue'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import i18n from '@/languages/i18n'
import { GetUnit } from '@/constant/commodityManagement'
import customPager from '@/components/custom-pager.vue'
import { setSearchObject, getMenuAuthorityList } from '@/utils/common'
import { exportData } from '@/utils/exportTable'
import { DEBOUNCE_TIME } from '@/constant/system'
import BtnGroup from '@/components/system/btnGroup.vue'
import updateSkuSafetyStock from './update-sku-safety-stock.vue'
// import qrCodeDialogDialog from './qrCodeDialog.vue'
import BarCodeDialog from '@/components/codeDialog/barCodeDialog.vue'
import QrCodeDialog from '@/components/codeDialog/qrCodeDialog.vue'
import hprintDialog from '@/components/hiprint/hiprintFast.vue'

const xTable = ref()
const updateSkuSaftyStockRef = ref()
const qrCodeDialogRef = ref()
const barCodeDialogRef = ref()
const hprintDialogRef = ref()

const data: DataProps = reactive({
  searchForm: {
    spu_code: '',
    spu_name: '',
    category_name: ''
  },
  timer: null,
  tableData: [],
  tablePage: {
    total: 0,
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE
  },
  tableTreeConfig: {
    transform: true,
    rowField: 'tree_id',
    parentField: 'parent_id'
  },
  // Dialog info
  showDialog: false,
  dialogForm: {
    id: 0,
    spu_code: '',
    spu_name: '',
    category_id: 0,
    category_name: '',
    spu_description: '',
    bar_code: '',
    supplier_id: 0,
    supplier_name: '',
    brand: '',
    origin: '',
    length_unit: 1,
    volume_unit: 0,
    weight_unit: 1,
    detailList: []
  },
  btnList: [],
  authorityList: getMenuAuthorityList(),
  selectRowData: []
})
const printDate = reactive({
  printForm: {} as any
})
const method = reactive({
  // Check if the checkbox can be checked
  getCheckBoxDisableState: ({ row }: { row: any }): boolean => row.parent_id,
  // Print QR code
  printQrCode: () => {
    let records = xTable.value.getCheckboxRecords()
    const parentRecords = records.filter((item) => !item.parent_id)
    records = records.filter((item) => item.parent_id)

    // data.selectRowData.length === 0 ? (data.selectRowData = [row]) : ''
    // let records: any[] = data.selectRowData
    if (records.length > 0) {
      for (const parent of parentRecords) {
        for (const child of records) {
          if (parent.id === child.parent_id) {
            child.spu_code = parent.spu_code
            child.spu_name = parent.spu_name
            child.type = 'commodity'
          }
        }
      }
      records.forEach((item) => {
        item.sku_id = item.id
      })

      qrCodeDialogRef.value.openDialog(records)
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('base.userManagement.checkboxIsNull')
      })
    }
  },
  printBarCode: () => {
    let records = xTable.value.getCheckboxRecords()
    records = records.filter((item: any) => item.parent_id)
    records = records.filter((item: any) => item.bar_code)

    // data.selectRowData.length === 0 ? (data.selectRowData = [row]) : ''
    // let records = data.selectRowData
    if (records.length > 0) {
      barCodeDialogRef.value.openDialog(records)
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('base.commodityManagement.checkboxIsNullByPrint')
      })
    }
  },
  print: () => {
    const records = xTable.value.getCheckboxRecords()
    if (records.length > 0) {
      printDate.printForm = { detailList: records }
      const ref = hprintDialogRef.value
      ref.method.handleOpen()
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
  expandAllRows: () => {
    const expandRows = xTable.value.getTreeExpandRecords()
    const parentData = data.tableData.filter((item: any) => !item.parent_id)

    if (expandRows.length === parentData.length) {
      xTable.value.setAllTreeExpand(false)
    } else {
      xTable.value.setAllTreeExpand(true)
    }
  },
  updateSkuSaftyStockByRow: async (body: { sku_id: number; detailList: UpdateSaftyStockReqBodyVO[] }) => {
    const { data: res } = await updateSaftyStock(body)
    if (!res.isSuccess) {
      hookComponent.$message({
        type: 'error',
        content: res.errorMessage
      })
      return
    }
    hookComponent.$message({
      type: 'success',
      content: `${ i18n.global.t('system.page.submit') }${ i18n.global.t('system.tips.success') }`
    })
    updateSkuSaftyStockRef.value.closeDialog()
    method.refresh()
  },
  openUpdateSaftyStockDialog: (row: CommodityDetailVO) => {
    updateSkuSaftyStockRef.value.openDialog(row)
  },
  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.getCompanyList()
  },
  // Find Data by Pagination
  getCompanyList: async () => {
    const { data: res } = await getSpuList(data.tablePage)
    if (!res.isSuccess) {
      hookComponent.$message({
        type: 'error',
        content: res.errorMessage
      })
      return
    }
    data.tableData = []
    for (const item of res.data.rows) {
      item.tree_id = item.id
      data.tableData.push(item)
      for (const ditem of item.detailList) {
        ditem.tree_id = 0
        ditem.parent_id = item.id
        ditem.length_unit = item.length_unit
        ditem.volume_unit = item.volume_unit
        ditem.weight_unit = item.weight_unit
        data.tableData.push(ditem)
      }
    }
    data.tablePage.total = res.data.totals
  },
  // Add user
  add: () => {
    data.dialogForm = {
      id: 0,
      spu_code: '',
      spu_name: '',
      spu_description: '',
      brand: '',
      length_unit: 1,
      volume_unit: 0,
      weight_unit: 1,
      detailList: []
    }
    data.showDialog = true
  },
  // Shut add or update dialog
  closeDialog: () => {
    data.showDialog = false
  },
  // after Add or update success.
  saveSuccess: () => {
    method.refresh()
    method.closeDialog()
  },
  // Refresh data
  refresh: () => {
    method.getCompanyList()
  },
  editRow(row: CommodityVO) {
    data.dialogForm = JSON.parse(JSON.stringify(row))
    data.showDialog = true
  },
  deleteRow(row: CommodityVO) {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteMessage'),
      handleConfirm: async () => {
        if (row.id) {
          const { data: res } = await deleteSpu(row.id)
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
  // Export table
  exportTable: () => {
    const $table = xTable.value
    exportData({
      table: $table,
      filename: i18n.global.t('router.sideBar.commodityManagement'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },
  // When change paging
  handlePageChange: ref<VxePagerEvents.PageChange>(({ currentPage, pageSize }) => {
    data.tablePage.pageIndex = currentPage
    data.tablePage.pageSize = pageSize
    method.refresh()
  })
})

onMounted(async () => {
  await method.getCompanyList()

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
    },
    {
      name: i18n.global.t('base.commodityManagement.printQrCode'),
      icon: 'mdi-qrcode',
      code: 'printQrCode',
      click: method.printQrCode
    },
    {
      name: i18n.global.t('base.commodityManagement.printBarCode'),
      icon: 'mdi-barcode',
      code: 'printBarCode',
      click: method.printBarCode
    },
    {
      name: i18n.global.t('system.page.print'),
      icon: 'mdi-printer',
      code: '',
      click: method.print
    }
    // {
    //   name: i18n.global.t('base.commodityManagement.printQrCode'),
    //   icon: 'mdi-qrcode',
    //   code: 'printQrCode',
    //   click: method.printQrCode
    // },
    // {
    //   name: i18n.global.t('base.commodityManagement.printBarCode'),
    //   icon: 'mdi-barcode',
    //   code: 'printBarCode',
    //   click: method.printBarCode
    // }
  ]
})

const cardHeight = computed(() => computedCardHeight({ hasTab: false }))

const tableHeight = computed(() => computedTableHeight({ hasTab: false }))

const isExpandAll = computed(() => {
  const expandRows = xTable.value.getTreeExpandRecords()
  const parentData = data.tableData.filter((item: any) => !item.parent_id)

  if (expandRows.length === parentData.length) {
    return true
  }
  return false
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
