<template>
  <div class="operateArea">
    <v-row no-gutters>
      <!-- Operate Btn -->
      <v-col cols="3" class="col">
        <!-- <tooltip-btn icon="mdi-refresh" :tooltip-text="$t('system.page.refresh')" @click="method.refresh"></tooltip-btn>
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
            :disabled="!data.authorityList.includes('delivered-confirm')"
            @click="method.editRow(row)"
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
  <!-- Confirm Arrival Box -->
  <confirm-arrival-modal ref="ConfirmArrivalRef" @sure="method.sureBackArrival" />
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
import { getStockAsnList, confirmAsn, confirmArrival } from '@/api/wms/stockAsn'
import i18n from '@/languages/i18n'
import customPager from '@/components/custom-pager.vue'
import skuInfo from './sku-info.vue'
import { exportData } from '@/utils/exportTable'
import BtnGroup from '@/components/system/btnGroup.vue'
import ConfirmArrivalModal from './confirm-arrival.vue'
import { httpCodeJudge } from '@/utils/http/httpCodeJudge'

const xTableStockLocation = ref()
const ConfirmArrivalRef = ref()

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
    sqlTitle: 'asn_status:0',
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
  // Open the confirmation of arrival box
  handleArrival: () => {
    const checkRecords = xTableStockLocation.value.getCheckboxRecords()
    if (checkRecords.length > 0) {
      ConfirmArrivalRef.value.openDialog()
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('wms.stockAsnInfo.selectOne')
      })
    }
  },
  // Confirm arrival
  sureBackArrival: async (dateStr: string) => {
    const checkRecords = xTableStockLocation.value.getCheckboxRecords()
    const reqBody = checkRecords.map((item: StockAsnVO) => ({ id: item.id, arrival_time: dateStr }))

    const { data: res } = await confirmArrival(reqBody)
    if (!res.isSuccess) {
      // 2023-12-06 Add automatic refresh of expired data
      if (httpCodeJudge(res.errorMessage)) {
        method.refresh()
        ConfirmArrivalRef.value.closeDialog()

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
    ConfirmArrivalRef.value.closeDialog()
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
      content: i18n.global.t('system.tips.beforeAsnConfirm'),
      handleConfirm: async () => {
        if (row.id) {
          const { data: res } = await confirmAsn(row.id)
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
    exportData({
      table: $table,
      filename: i18n.global.t('wms.stockAsn.tabToDoArrival'),
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
      code: 'delivered-export',
      click: method.exportTable
    },
    {
      name: i18n.global.t('wms.stockAsnInfo.confirmArrival'),
      icon: 'mdi-check',
      code: 'delivered-confirm',
      click: method.handleArrival
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
