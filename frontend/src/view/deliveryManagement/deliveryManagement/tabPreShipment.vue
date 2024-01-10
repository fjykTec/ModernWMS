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
      <!-- <vxe-column type="checkbox" width="50"></vxe-column> -->
      <vxe-column field="dispatch_no" :title="$t('wms.deliveryManagement.dispatch_no')"></vxe-column>
      <!-- <vxe-column field="dispatch_status" :title="$t('wms.deliveryManagement.dispatch_status')"></vxe-column> -->
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
      <!-- <vxe-column
        field="create_time"
        width="170px"
        :title="$t('wms.deliveryManagement.create_time')"
      ></vxe-column> -->
      <vxe-column field="operate" :title="$t('system.page.operate')" width="70" :resizable="false" show-overflow>
        <template #default="{ row }">
          <div style="width: 100%; display: flex; justify-content: center">
            <tooltip-btn :flat="true" icon="mdi-eye-outline" :tooltip-text="$t('system.page.view')" @click="method.viewRow(row)"></tooltip-btn>
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
    <!-- View order details -->
    <SearchDeliveredMainDetail
      :dispatch-no="data.showDeliveredMainDetailNo"
      :show-dialog="data.showDeliveredMainDetail"
      @close="method.closeDeliveredDetail"
    />
  </div>
</template>

<script lang="ts" setup>
import { computed, ref, reactive, watch, onMounted } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight } from '@/constant/style'
import { DeliveryManagementVO } from '@/types/DeliveryManagement/DeliveryManagement'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { hookComponent } from '@/components/system'
import { getPreShipment } from '@/api/wms/deliveryManagement'
import tooltipBtn from '@/components/tooltip-btn.vue'
import i18n from '@/languages/i18n'
import { GetUnit } from '@/constant/commodityManagement'
import customPager from '@/components/custom-pager.vue'
import { setSearchObject, getMenuAuthorityList } from '@/utils/common'
import { TablePage, btnGroupItem } from '@/types/System/Form'
import { exportData } from '@/utils/exportTable'
import { DEBOUNCE_TIME } from '@/constant/system'
import SearchDeliveredMainDetail from './search-delivered-main-detail.vue'
import BtnGroup from '@/components/system/btnGroup.vue'

const xTable = ref()

const data = reactive({
  showDeliveredMainDetailNo: '',
  showDeliveredMainDetail: false,
  showDialog: false,
  dialogForm: {
    id: 0
  },
  searchForm: {
    dispatch_no: '',
    customer_name: ''
  },
  timer: ref<any>(null),
  activeTab: null,
  tableData: ref<DeliveryManagementVO[]>([]),
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
  viewRow: (row: DeliveryManagementVO) => {
    if (row.dispatch_no) {
      data.showDeliveredMainDetailNo = row.dispatch_no
      data.showDeliveredMainDetail = true
    }
  },
  closeDeliveredDetail: () => {
    data.showDeliveredMainDetail = false
  },
  // Refresh data
  refresh: () => {
    method.getPreShipment()
  },
  getPreShipment: async () => {
    const { data: res } = await getPreShipment(data.tablePage)
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

    method.getPreShipment()
  }),
  exportTable: () => {
    const $table = xTable.value
    exportData({
      table: $table,
      filename: i18n.global.t('wms.deliveryManagement.preShipment'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },
  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.getPreShipment()
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
      code: 'invoice-export',
      click: method.exportTable
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
  getPreShipment: method.getPreShipment
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
