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
      <!-- <vxe-column type="checkbox" width="50"></vxe-column> -->
      <vxe-column field="dispatch_no" :title="$t('wms.deliveryManagement.dispatch_no')"></vxe-column>
      <vxe-column field="spu_code" :title="$t('wms.deliveryManagement.spu_code')"></vxe-column>
      <vxe-column field="spu_description" width="200px" :title="$t('wms.deliveryManagement.spu_description')"></vxe-column>
      <vxe-column field="spu_name" :title="$t('wms.deliveryManagement.spu_name')"></vxe-column>
      <vxe-column field="sku_code" :title="$t('wms.deliveryManagement.sku_code')"></vxe-column>
      <vxe-column field="bar_code" :title="$t('wms.deliveryManagement.bar_code')"></vxe-column>
      <vxe-column field="qty" :title="$t('wms.deliveryManagement.order_qty')"></vxe-column>
      <vxe-column field="unpicked_qty" :title="$t('wms.deliveryManagement.unpicked_qty')"></vxe-column>
      <vxe-column field="picked_qty" :title="$t('wms.deliveryManagement.picked_qty')"></vxe-column>
      <vxe-column field="weight" :title="$t('wms.deliveryManagement.detailWeight')">
        <template #default="{ row }">
          <span>{{ `${row.volume} ${GetUnit('weight', row.volume_unit)}` }}</span>
        </template>
      </vxe-column>
      <vxe-column field="volume" :title="$t('wms.deliveryManagement.detailVolume')">
        <template #default="{ row }">
          <span>{{ `${row.volume} ${GetUnit('volume', row.volume_unit)}` }}</span>
        </template>
      </vxe-column>
      <vxe-column field="customer_name" :title="$t('wms.deliveryManagement.customer_name')"></vxe-column>
      <vxe-column field="pick_checker" :title="$t('wms.deliveryManagement.pick_checker')"></vxe-column>
      <vxe-column field="creator" :title="$t('wms.deliveryManagement.creator')"></vxe-column>
      <vxe-date-column
        field="create_time"
        width="170px"
        format="yyyy-MM-dd HH:mm"
        :title="$t('wms.deliveryManagement.create_time')"
      ></vxe-date-column>
      <vxe-column field="operate" :title="$t('system.page.operate')" width="120" :resizable="false" show-overflow>
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
    <SearchDeliveredDetail :id="data.showDeliveredDetailID" :show-dialog="data.showDeliveredDetail" @close="method.closeDeliveredDetail" />
  </div>
</template>

<script lang="ts" setup>
import { computed, ref, reactive, watch, onMounted } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight } from '@/constant/style'
import { DeliveryManagementDetailVO } from '@/types/DeliveryManagement/DeliveryManagement'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { hookComponent } from '@/components/system'
import { getPicked } from '@/api/wms/deliveryManagement'
import tooltipBtn from '@/components/tooltip-btn.vue'
import i18n from '@/languages/i18n'
import { GetUnit } from '@/constant/commodityManagement'
import customPager from '@/components/custom-pager.vue'
import { setSearchObject, getMenuAuthorityList } from '@/utils/common'
import { TablePage, btnGroupItem } from '@/types/System/Form'
import SearchDeliveredDetail from './search-delivered-detail.vue'
import { exportData } from '@/utils/exportTable'
import { DEBOUNCE_TIME } from '@/constant/system'
import BtnGroup from '@/components/system/btnGroup.vue'

const xTable = ref()

const data = reactive({
  showDeliveredDetailID: 0,
  showDeliveredDetail: false,
  dialogForm: {
    id: 0
  },
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
  // Refresh data
  refresh: () => {
    method.getPicked()
  },
  getPicked: async () => {
    const { data: res } = await getPicked(data.tablePage)
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

    method.getPicked()
  }),
  exportTable: () => {
    const $table = xTable.value
    exportData({
      table: $table,
      filename: i18n.global.t('wms.deliveryManagement.picked'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },
  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.getPicked()
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
      code: 'picked-export',
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
  getPicked: method.getPicked
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
