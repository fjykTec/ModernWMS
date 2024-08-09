<template>
  <div class="operateArea">
    <v-row no-gutters>
      <!-- Operate Btn -->
      <v-col cols="3" class="col">
        <!-- <tooltip-btn icon="mdi-refresh" :tooltip-text="$t('system.page.refresh')" @click="method.refresh"></tooltip-btn>
        <tooltip-btn icon="mdi-export-variant" :tooltip-text="$t('system.page.export')" @click="method.exportTable"> </tooltip-btn> -->

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
    <vxe-table ref="xTableStockLocation" :column-config="{ minWidth: '100px' }" :data="data.tableData" :height="tableHeight" align="center">
      <template #empty>
        {{ i18n.global.t('system.page.noData') }}
      </template>
      <vxe-column type="seq" width="60"></vxe-column>
      <vxe-column type="checkbox" width="50"></vxe-column>
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
      <vxe-column field="price" :title="$t('wms.stockAsnInfo.price')"></vxe-column>
      <vxe-column field="weight" :title="$t('wms.stockAsnInfo.weight')"></vxe-column>
      <vxe-column field="volume" :title="$t('wms.stockAsnInfo.volume')"></vxe-column>
      <!-- <vxe-column field="operate" :title="$t('system.page.operate')" width="160" :resizable="false" show-overflow>
        <template #default="{ row }">
          <tooltip-btn
            :flat="true"
            icon="mdi-check"
            :tooltip-text="$t('system.page.confirm')"
            :disabled="!data.authorityList.includes('unloaded-confirm')"
            @click="method.editRow(row)"
          ></tooltip-btn>
          <tooltip-btn
            :flat="true"
            icon="mdi-delete-outline"
            :tooltip-text="$t('system.page.delete')"
            :icon-color="errorColor"
            :disabled="!data.authorityList.includes('unloaded-delete')"
            @click="method.deleteRow(row)"
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
  <skuInfo :show-dialog="data.showDialogShowInfo" :form="data.dialogForm" @close="method.closeDialogShowInfo" />
  <!-- Pending unloading confirmation box -->
  <ConfirmUnloadModal ref="ConfirmUnloadModalRef" @sure="method.sureBackUnloadConfirm" />
</template>

<script lang="ts" setup>
import { computed, ref, reactive, watch, onMounted } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight } from '@/constant/style'
import { StockAsnVO } from '@/types/WMS/StockAsn'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { hookComponent } from '@/components/system'
import { DEBOUNCE_TIME } from '@/constant/system'
import { setSearchObject, getMenuAuthorityList } from '@/utils/common'
import { SearchObject, btnGroupItem } from '@/types/System/Form'
import { getStockAsnList, unloadAsn, confirmAsnCancel, confirmUnload, unconfirmArrival } from '@/api/wms/stockAsn'
import i18n from '@/languages/i18n'
import customPager from '@/components/custom-pager.vue'
import skuInfo from './sku-info.vue'
// import { exportData } from '@/utils/exportTable'
import BtnGroup from '@/components/system/btnGroup.vue'
import ConfirmUnloadModal from './confirm-unload.vue'
import { httpCodeJudge } from '@/utils/http/httpCodeJudge'

const xTableStockLocation = ref()
const ConfirmUnloadModalRef = ref()

const data = reactive({
  showDialog: false,
  showDialogShowInfo: false,
  searchForm: {
    supplier_name: '',
    sku_name: ''
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
    sqlTitle: 'asn_status:1',
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE,
    searchObjects: ref<Array<SearchObject>>([])
  }),
  timer: ref<any>(null),
  btnList: [] as btnGroupItem[],
  // Menu operation permissions
  authorityList: getMenuAuthorityList()
})

const method = reactive({
  // Withdrawal process
  handleRevoke: () => {
    const checkRecords = xTableStockLocation.value.getCheckboxRecords()
    if (checkRecords.length > 0) {
      const idList = checkRecords.map((item: StockAsnVO) => item.id)
      hookComponent.$dialog({
        content: i18n.global.t('system.tips.beforeOperation'),
        handleConfirm: async () => {
          const { data: res } = await unconfirmArrival(idList)
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
            content: `${ i18n.global.t('system.page.revoke') }${ i18n.global.t('system.tips.success') }`
          })
          method.refresh()
        }
      })
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('wms.stockAsnInfo.selectOne')
      })
    }
  },
  // Open the arrival confirmation box
  handleConfirm: () => {
    const checkRecords = xTableStockLocation.value.getCheckboxRecords()
    if (checkRecords.length > 0) {
      ConfirmUnloadModalRef.value.openDialog()
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('wms.stockAsnInfo.selectOne')
      })
    }
  },
  // After confirmation
  sureBackUnloadConfirm: async (form: { unloadTime: string; unloadPerson: string; unloadPersonID: number }) => {
    const checkRecords = xTableStockLocation.value.getCheckboxRecords()
    const reqBody = checkRecords.map((item: StockAsnVO) => ({ id: item.id, ...form }))

    const { data: res } = await confirmUnload(reqBody)
    if (!res.isSuccess) {
      // 2023-12-06 Add automatic refresh of expired data
      if (httpCodeJudge(res.errorMessage)) {
        method.refresh()
        ConfirmUnloadModalRef.value.closeDialog()

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
      content: `${ i18n.global.t('system.page.confirm') }${ i18n.global.t('system.tips.success') }`
    })
    ConfirmUnloadModalRef.value.closeDialog()
    method.refresh()
  },
  closeDialogShowInfo: () => {
    data.showDialogShowInfo = false
  },
  showSkuInfo(row: StockAsnVO) {
    data.dialogForm = JSON.parse(JSON.stringify(row))
    data.showDialogShowInfo = true
  },
  editRow(row: StockAsnVO) {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeAsnUnload'),
      handleConfirm: async () => {
        if (row.id) {
          const { data: res } = await unloadAsn(row.id)
          if (!res.isSuccess) {
            hookComponent.$message({
              type: 'error',
              content: res.errorMessage
            })
            return
          }
          hookComponent.$message({
            type: 'success',
            content: `${ i18n.global.t('system.page.confirm') }${ i18n.global.t('system.tips.success') }`
          })
          method.refresh()
        }
      }
    })
  },
  deleteRow(row: StockAsnVO) {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteMessage'),
      handleConfirm: async () => {
        if (row.id) {
          const { data: res } = await confirmAsnCancel(row.id)
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
    $table.exportData({
      table: $table,
      filename: i18n.global.t('wms.stockAsn.tabToDoUnload'),
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
      name: i18n.global.t('system.page.refresh'),
      icon: 'mdi-refresh',
      code: '',
      click: method.refresh
    },
    {
      name: i18n.global.t('system.page.export'),
      icon: 'mdi-export-variant',
      code: 'unloaded-export',
      click: method.exportTable
    },
    {
      name: i18n.global.t('wms.stockAsnInfo.confirmUnload'),
      icon: 'mdi-check',
      code: 'unloaded-confirm',
      click: method.handleConfirm
    },
    {
      name: i18n.global.t('wms.stockAsnInfo.revoke'),
      icon: 'mdi-arrow-left-top',
      code: 'unloaded-delete',
      click: method.handleRevoke
    }
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
