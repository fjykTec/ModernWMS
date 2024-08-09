<template>
  <div class="operateArea">
    <v-row no-gutters>
      <!-- Operate Btn -->
      <v-col cols="3" class="col">
        <!-- <tooltip-btn icon="mdi-refresh" :tooltip-text="$t('system.page.refresh')" @click="method.refresh"></tooltip-btn>
        <tooltip-btn icon="mdi-export-variant" :tooltip-text="$t('system.page.export')" @click="method.exportTable"> </tooltip-btn>
        <tooltip-btn icon="mdi-car-cog" :tooltip-text="$t('wms.deliveryManagement.setFreight')" @click="method.setFreight"> </tooltip-btn>
        <tooltip-btn icon="mdi-email-check" :tooltip-text="$t('wms.deliveryManagement.signIn')" @click="method.handleSignIn"></tooltip-btn> -->

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
      <vxe-column :title="$t('wms.deliveryManagement.state')">
        <template #default="{ row }">
          <span>{{ `${row.is_todo ? $t('wms.deliveryManagement.deliveryTodo') : $t('wms.deliveryManagement.deliveryReady')}` }}</span>
        </template>
      </vxe-column>
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
      <vxe-column field="package_no" :title="$t('wms.deliveryManagement.package_no')"></vxe-column>
      <vxe-column field="weighing_weight" :title="$t('wms.deliveryManagement.weighing_weight')">
        <template #default="{ row }">
          <span>{{ `${row.weighing_weight} ${GetUnit('weight', row.weight_unit)}` }}</span>
        </template>
      </vxe-column>
      <vxe-column field="weighing_no" :title="$t('wms.deliveryManagement.weighing_no')"></vxe-column>
      <vxe-column field="customer_name" :title="$t('wms.deliveryManagement.customer_name')"></vxe-column>
      <vxe-column field="pick_checker" :title="$t('wms.deliveryManagement.pick_checker')"></vxe-column>
      <vxe-column field="creator" :title="$t('wms.deliveryManagement.creator')"></vxe-column>
      <vxe-date-column
        field="create_time"
        width="170px"
        format="yyyy-MM-dd HH:mm"
        :title="$t('wms.deliveryManagement.create_time')"
      ></vxe-date-column>
      <vxe-column field="operate" :title="$t('system.page.operate')" width="140" :resizable="false" show-overflow>
        <template #default="{ row }">
          <div style="width: 100%; display: flex; justify-content: center">
            <tooltip-btn :flat="true" icon="mdi-eye-outline" :tooltip-text="$t('system.page.view')" @click="method.viewRow(row)"></tooltip-btn>
            <!-- <tooltip-btn
              :flat="true"
              icon="mdi-email-check"
              :tooltip-text="$t('wms.deliveryManagement.signIn')"
              @click="method.handleSignIn(row)"
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
    <!-- <ToBeSignInConfirm
      :show-dialog="data.showDialog"
      :dialog-default-qty="data.dialogDefaultQty"
      @close="method.dialogClose"
      @submit="method.dialogSubmit"
    /> -->
    <SignInConfirm
      ref="SignInConfirmRef"
      :dialog-title="$t('wms.deliveryManagement.signIn')"
      :is-weight="false"
      :is-sign-in="true"
      @submit="method.dialogSubmit"
    />
    <SearchDeliveredDetail :id="data.showDeliveredDetailID" :show-dialog="data.showDeliveredDetail" @close="method.closeDeliveredDetail" />
    <ToBeFreightfee :show-dialog="data.showSetFreight" @close="method.freightfeeClose" @submit="method.freightfeeSubmit" />
  </div>
</template>

<script lang="ts" setup>
import { computed, ref, reactive, watch, onMounted } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight } from '@/constant/style'
import { DeliveryManagementDetailVO, SetCarrierVO, ConfirmItem } from '@/types/DeliveryManagement/DeliveryManagement'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { hookComponent } from '@/components/system'
import { getDelivery, handleSignIn, setCarrier, handleDelivery } from '@/api/wms/deliveryManagement'
import tooltipBtn from '@/components/tooltip-btn.vue'
import i18n from '@/languages/i18n'
// import ToBeSignInConfirm from './to-be-sign-in-confirm.vue'
import SignInConfirm from './package-confirm.vue'
import { GetUnit } from '@/constant/commodityManagement'
import ToBeFreightfee from './to-be-freightfee.vue'
import customPager from '@/components/custom-pager.vue'
import { setSearchObject, getMenuAuthorityList } from '@/utils/common'
import { TablePage, btnGroupItem } from '@/types/System/Form'
import SearchDeliveredDetail from './search-delivered-detail.vue'
import { exportData } from '@/utils/exportTable'
import { DEBOUNCE_TIME } from '@/constant/system'
import BtnGroup from '@/components/system/btnGroup.vue'
import { httpCodeJudge } from '@/utils/http/httpCodeJudge'

const xTable = ref()
const SignInConfirmRef = ref()

const data = reactive({
  showDeliveredDetailID: 0,
  showDeliveredDetail: false,
  showSetFreight: false,
  dialogDefaultQty: 0,
  packageRow: ref<DeliveryManagementDetailVO>(),
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
  confirmList: ref<ConfirmItem[]>([]),
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
  setFreight: () => {
    const $table = xTable.value
    if ($table.getCheckboxRecords().length > 0) {
      data.showSetFreight = true
    } else {
      hookComponent.$message({
        type: 'error',
        content: `${ i18n.global.t('base.userManagement.checkboxIsNull') }`
      })
    }
  },
  freightfeeClose: () => {
    data.showSetFreight = false
  },
  freightfeeSubmit: async (form: { carrier: string; freightfee_id: number; waybill_no: string }) => {
    const reqList: SetCarrierVO[] = []
    const $table = xTable.value
    for (const item of $table.getCheckboxRecords()) {
      reqList.push({
        id: item.id,
        dispatch_no: item.dispatch_no,
        dispatch_status: item.dispatch_status,
        freightfee_id: form.freightfee_id,
        carrier: form.carrier,
        waybill_no: form.waybill_no
      })
    }
    const { data: res } = await setCarrier(reqList)
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
    method.freightfeeClose()
    method.refresh()
  },
  // Callback after entering packaging value
  dialogSubmit: async (list: ConfirmItem[]) => {
    const SingInList = list.map((item) => ({
      id: item.id,
      dispatch_no: item.dispatch_no,
      dispatch_status: item.dispatch_status,
      damage_qty: item.qty
    }))

    const { data: res } = await handleSignIn(SingInList)
    if (!res.isSuccess) {
      // 2023-12-06 Add automatic refresh of expired data
      if (httpCodeJudge(res.errorMessage)) {
        method.refresh()

        SignInConfirmRef.value.closeDialog()

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
    SignInConfirmRef.value.closeDialog()
    method.refresh()
    // if (data.packageRow) {
    //   const { data: res } = await handleSignIn([
    //     {
    //       id: data.packageRow.id,
    //       dispatch_no: data.packageRow.dispatch_no,
    //       dispatch_status: data.packageRow.dispatch_status,
    //       damage_qty: qty
    //     }
    //   ])
    //   if (!res.isSuccess) {
    //     hookComponent.$message({
    //       type: 'error',
    //       content: res.errorMessage
    //     })
    //     return
    //   }
    //   hookComponent.$message({
    //     type: 'success',
    //     content: res.data
    //   })
    //   method.dialogClose()
    //   method.refresh()
    // }
  },
  handleSignIn: async () => {
    const $table = xTable.value
    let checkTableList = $table.getCheckboxRecords()

    checkTableList = checkTableList.filter((item: DeliveryManagementDetailVO) => !item.is_todo)
    const confirmList: ConfirmItem[] = []
    if (checkTableList.length > 0) {
      // Processing the data required by the window
      for (const item of checkTableList) {
        confirmList.push({
          id: item.id,
          spu_name: item.spu_name,
          spu_code: item.spu_code,
          sku_code: item.sku_code,
          maxQty: item.picked_qty,
          qty: 0,
          dispatch_no: item.dispatch_no,
          dispatch_status: item.dispatch_status
        })
      }
      SignInConfirmRef.value.openDialog(confirmList)
    } else {
      hookComponent.$message({
        type: 'error',
        content: `${ i18n.global.t('wms.deliveryManagement.opeartionCheckboxIsNull') }`
      })
    }
    // data.packageRow = row
    // data.dialogDefaultQty = row.picked_qty ? row.picked_qty : 0
    // data.showDialog = true
  },
  // Refresh data
  refresh: () => {
    method.getDelivery()
  },
  getDelivery: async () => {
    const { data: res } = await getDelivery(data.tablePage)
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

    method.getDelivery()
  }),
  exportTable: () => {
    const $table = xTable.value
    exportData({
      table: $table,
      filename: i18n.global.t('wms.deliveryManagement.outOfWarehouse'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },
  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.getDelivery()
  },
  handleDeliver: async () => {
    const $table = xTable.value
    let checkTableList = $table.getCheckboxRecords()

    checkTableList = checkTableList.filter((item: DeliveryManagementDetailVO) => item.is_todo)

    if (checkTableList.length > 0) {
      const deliveredList = checkTableList.map((row: DeliveryManagementDetailVO) => ({
        id: row.id,
        dispatch_no: row.dispatch_no,
        dispatch_status: row.dispatch_status,
        picked_qty: row.picked_qty
      }))
      hookComponent.$dialog({
        content: `${ i18n.global.t('wms.deliveryManagement.irreversible') }, ${ i18n.global.t('wms.deliveryManagement.confirmDelivery') }?`,
        handleConfirm: async () => {
          const { data: res } = await handleDelivery(deliveredList)
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
    } else {
      hookComponent.$message({
        type: 'error',
        content: `${ i18n.global.t('wms.deliveryManagement.opeartionCheckboxIsNull') }`
      })
    }
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
      name: i18n.global.t('wms.deliveryManagement.delivery'),
      icon: 'mdi-cube-send',
      code: 'delivered-delivery',
      click: method.handleDeliver
    },
    {
      name: i18n.global.t('wms.deliveryManagement.setFreight'),
      icon: 'mdi-car-cog',
      code: 'delivered-setCarrier',
      click: method.setFreight
    },
    {
      name: i18n.global.t('wms.deliveryManagement.signIn'),
      icon: 'mdi-email-check',
      code: 'delivered-signIn',
      click: method.handleSignIn
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
  getDelivery: method.getDelivery
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
