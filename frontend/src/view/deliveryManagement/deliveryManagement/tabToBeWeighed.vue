<template>
  <div class="operateArea">
    <v-row no-gutters>
      <!-- Operate Btn -->
      <v-col cols="3" class="col">
        <!-- <tooltip-btn icon="mdi-refresh" :tooltip-text="$t('system.page.refresh')" @click="method.refresh"></tooltip-btn>
        <tooltip-btn icon="mdi-export-variant" :tooltip-text="$t('system.page.export')" @click="method.exportTable"> </tooltip-btn>
        <tooltip-btn icon="mdi-weight" :tooltip-text="$t('wms.deliveryManagement.weigh')" @click="method.handleWeigh"> </tooltip-btn> -->

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
          <v-col cols="4">
            <v-text-field
              v-model="data.searchForm.spu_name"
              clearable
              hide-details
              density="comfortable"
              class="searchInput ml-5 mt-1"
              :label="$t('wms.deliveryManagement.spu_name')"
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
      <!-- <vxe-column field="weighing_no" :title="$t('wms.deliveryManagement.weighing_no')"></vxe-column> -->
      <vxe-column field="dispatch_no" :title="$t('wms.deliveryManagement.dispatch_no')"></vxe-column>
      <vxe-column field="spu_code" :title="$t('wms.deliveryManagement.spu_code')"></vxe-column>
      <vxe-column field="spu_description" width="200px" :title="$t('wms.deliveryManagement.spu_description')"></vxe-column>
      <vxe-column field="spu_name" :title="$t('wms.deliveryManagement.spu_name')"></vxe-column>
      <vxe-column field="sku_code" :title="$t('wms.deliveryManagement.sku_code')"></vxe-column>
      <vxe-column field="bar_code" :title="$t('wms.deliveryManagement.bar_code')"></vxe-column>
      <vxe-column field="qty" :title="$t('wms.deliveryManagement.order_qty')"></vxe-column>
      <vxe-column field="weight" :title="$t('wms.deliveryManagement.detailWeight')">
        <template #default="{ row }">
          <span>{{ `${row.weight} ${GetUnit('weight', row.weight_unit)}` }}</span>
        </template>
      </vxe-column>
      <vxe-column field="volume" :title="$t('wms.deliveryManagement.detailVolume')">
        <template #default="{ row }">
          <span>{{ `${row.volume} ${GetUnit('volume', row.volume_unit)}` }}</span>
        </template>
      </vxe-column>
      <vxe-column field="package_person" :title="$t('wms.deliveryManagement.package_person')"></vxe-column>
      <vxe-column field="package_no" :title="$t('wms.deliveryManagement.package_no')"></vxe-column>
      <!-- <vxe-column field="weighing_person" :title="$t('wms.deliveryManagement.weighing_person')"></vxe-column>
      <vxe-column field="weighing_weight" :title="$t('wms.deliveryManagement.weighing_weight')">
        <template #default="{ row }">
          <span>{{ `${row.weighing_weight} ${GetUnit('weight', row.weight_unit)}` }}</span>
        </template>
      </vxe-column> -->
      <vxe-column field="customer_name" :title="$t('wms.deliveryManagement.customer_name')"></vxe-column>
      <vxe-column field="creator" :title="$t('wms.deliveryManagement.creator')"></vxe-column>
      <vxe-column field="create_time" width="170px" :title="$t('wms.deliveryManagement.create_time')"></vxe-column>
      <vxe-column field="operate" :title="$t('system.page.operate')" width="140" :resizable="false" show-overflow>
        <template #default="{ row }">
          <div style="width: 100%; display: flex; justify-content: center">
            <tooltip-btn :flat="true" icon="mdi-eye-outline" :tooltip-text="$t('system.page.view')" @click="method.viewRow(row)"></tooltip-btn>
            <!-- <tooltip-btn
              :flat="true"
              icon="mdi-weight"
              :tooltip-text="$t('wms.deliveryManagement.weigh')"
              @click="method.handleWeigh(row)"
            ></tooltip-btn> -->
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
    <!-- <ToBeWeighedConfirm
      :show-dialog="data.showDialog"
      :default-weight="data.defaultWeight"
      :max-qty="data.dialogMaxQty"
      :weight-unit="data.dialogWeightUnit"
      @close="method.dialogClose"
      @submit="method.dialogSubmit"
    /> -->
    <SearchDeliveredDetail :id="data.showDeliveredDetailID" :show-dialog="data.showDeliveredDetail" @close="method.closeDeliveredDetail" />
    <WeightConfirm
      :show-dialog="data.showDialog"
      :dialog-title="$t('wms.deliveryManagement.weigh')"
      :is-weight="true"
      :data-list="data.confirmList"
      @close="method.dialogClose"
      @submit="method.dialogSubmit"
    />
  </div>
</template>

<script lang="ts" setup>
import { computed, ref, reactive, watch, onMounted } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight } from '@/constant/style'
import { DeliveryManagementDetailVO, ConfirmItem } from '@/types/DeliveryManagement/DeliveryManagement'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { hookComponent } from '@/components/system'
import { getToBeWeighed, handleWeigh } from '@/api/wms/deliveryManagement'
import tooltipBtn from '@/components/tooltip-btn.vue'
import i18n from '@/languages/i18n'
import { GetUnit } from '@/constant/commodityManagement'
import customPager from '@/components/custom-pager.vue'
import { setSearchObject, getMenuAuthorityList } from '@/utils/common'
import { TablePage, btnGroupItem } from '@/types/System/Form'
import SearchDeliveredDetail from './search-delivered-detail.vue'
import { exportData } from '@/utils/exportTable'
import { DEBOUNCE_TIME } from '@/constant/system'
import WeightConfirm from './package-confirm.vue'
import BtnGroup from '@/components/system/btnGroup.vue'

const xTable = ref()

const data = reactive({
  showDeliveredDetailID: 0,
  showDeliveredDetail: false,
  showDialog: false,
  confirmList: ref<ConfirmItem[]>([]),
  dialogMaxQty: 0,
  dialogWeightUnit: '',
  defaultWeight: 0,
  weighedRow: ref<DeliveryManagementDetailVO>(),
  searchForm: {
    dispatch_no: '',
    customer_name: '',
    spu_name: ''
  },
  timer: ref<any>(null),
  activeTab: null,
  tableData: ref<DeliveryManagementDetailVO[]>([]),
  tablePage: ref<TablePage>({
    total: 0,
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE,
    searchObjects: []
  }),
  btnList: [] as btnGroupItem[],
  // Menu operation permissions
  authorityList: getMenuAuthorityList()
})

const method = reactive({
  closeDeliveredDetail: () => {
    data.showDeliveredDetail = false
  },
  viewRow: (row: DeliveryManagementDetailVO) => {
    data.showDeliveredDetailID = row.id
    data.showDeliveredDetail = true
  },
  dialogClose: () => {
    data.showDialog = false
  },
  // Callback after entering packaging value
  dialogSubmit: async (list: ConfirmItem[]) => {
    const packList = list.map((item) => ({
      id: item.id,
      dispatch_no: item.dispatch_no,
      dispatch_status: item.dispatch_status,
      weighing_qty: item.qty,
      weighing_weight: item.weight,
      picked_qty: item.picked_qty
    }))
    // if (data.weighedRow) {
    const { data: res } = await handleWeigh(packList)
    if (!res.isSuccess) {
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
    method.dialogClose()
    method.refresh()
    // }
  },
  handleWeigh: async () => {
    const $table = xTable.value
    const checkTableList = $table.getCheckboxRecords()
    const confirmList: ConfirmItem[] = []
    if (checkTableList.length > 0) {
      // Processing the data required by the window
      for (const item of checkTableList) {
        confirmList.push({
          id: item.id,
          spu_name: item.spu_name,
          spu_code: item.spu_code,
          sku_code: item.sku_code,
          maxQty: item.unweighing_qty,
          qty: item.unweighing_qty,
          weight: item.weight,
          weight_unit: GetUnit('weight', item.weight_unit),
          dispatch_no: item.dispatch_no,
          dispatch_status: item.dispatch_status,
          picked_qty: item.picked_qty
        })
      }
      data.confirmList = confirmList
      data.showDialog = true
    } else {
      hookComponent.$message({
        type: 'error',
        content: `${ i18n.global.t('base.userManagement.checkboxIsNull') }`
      })
    }
    // data.weighedRow = row
    // data.dialogWeightUnit = row.weight_unit !== undefined ? GetUnit('weight', row.weight_unit) : ''
    // data.dialogMaxQty = row.unweighing_qty ? row.unweighing_qty : 0
    // data.defaultWeight = row.weight ? row.weight : 0
    // data.showDialog = true
  },
  // Refresh data
  refresh: () => {
    method.getToBeWeighed()
  },
  getToBeWeighed: async () => {
    const { data: res } = await getToBeWeighed(data.tablePage)
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

    method.getToBeWeighed()
  }),
  exportTable: () => {
    const $table = xTable.value
    exportData({
      table: $table,
      filename: i18n.global.t('wms.deliveryManagement.toBeWeighed'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },
  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.getToBeWeighed()
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
      code: 'weighed-export',
      click: method.exportTable
    },
    {
      name: i18n.global.t('wms.deliveryManagement.weigh'),
      icon: 'mdi-weight',
      code: 'weighed-weigh',
      click: method.handleWeigh
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
  getToBeWeighed: method.getToBeWeighed
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
