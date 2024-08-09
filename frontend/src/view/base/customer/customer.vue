<!-- customer Setting -->
<template>
  <div class="container">
    <div>
      <!-- Main Content -->
      <v-card class="mt-5">
        <v-card-text>
          <!-- <v-window v-model="data.activeTab">
            <v-window-item> -->
          <div class="operateArea">
            <v-row no-gutters>
              <!-- Operate Btn -->
              <v-col cols="12" sm="4" class="col">
                <!-- <tooltip-btn icon="mdi-plus" :tooltip-text="$t('system.page.add')" @click="method.add()"></tooltip-btn>
                <tooltip-btn icon="mdi-refresh" :tooltip-text="$t('system.page.refresh')" @click="method.refresh()"></tooltip-btn>
                <tooltip-btn icon="mdi-database-import-outline" :tooltip-text="$t('system.page.import')" @click="method.openDialogImport">
                </tooltip-btn>
                <tooltip-btn icon="mdi-export-variant" :tooltip-text="$t('system.page.export')" @click="method.exportTable"> </tooltip-btn> -->

                <!-- new version -->
                <BtnGroup :authority-list="data.authorityList" :btn-list="data.btnList" />
              </v-col>

              <!-- Search Input -->
              <v-col cols="12" sm="8">
                <v-row no-gutters @keyup.enter="method.sureSearch">
                  <v-col cols="12" sm="4"></v-col>
                  <v-col cols="12" sm="4"></v-col>
                  <v-col cols="12" sm="4">
                    <v-text-field
                      v-model="data.searchForm.customer_name"
                      clearable
                      hide-details
                      density="comfortable"
                      class="searchInput ml-5 mt-1"
                      :label="$t('base.customer.customer_name')"
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
            >
              <template #empty>
                {{ i18n.global.t('system.page.noData') }}
              </template>
              <vxe-column type="seq" width="60"></vxe-column>
              <vxe-column type="checkbox" width="50"></vxe-column>
              <vxe-column field="customer_name" :title="$t('base.customer.customer_name')"></vxe-column>
              <vxe-column field="city" :title="$t('base.customer.city')"></vxe-column>
              <vxe-column field="address" :title="$t('base.customer.address')"></vxe-column>
              <vxe-column field="manager" :title="$t('base.customer.manager')"></vxe-column>
              <vxe-column field="email" :title="$t('base.customer.email')"></vxe-column>
              <vxe-column field="contact_tel" :title="$t('base.customer.contact_tel')"></vxe-column>
              <vxe-column field="creator" :title="$t('base.customer.creator')"></vxe-column>
              <vxe-date-column field="create_time" width="170px" format="yyyy-MM-dd HH:mm" :title="$t('base.customer.create_time')">
              </vxe-date-column>
              <vxe-date-column field="last_update_time" width="170px" format="yyyy-MM-dd HH:mm" :title="$t('base.customer.last_update_time')">
              </vxe-date-column>
              <vxe-column field="operate" :title="$t('system.page.operate')" width="160" :resizable="false" show-overflow>
                <template #default="{ row }">
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
                    :icon-color="!data.authorityList.includes('delete')?'':errorColor"
                    :disabled="!data.authorityList.includes('delete')"
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
          <!-- </v-window-item>
          </v-window> -->
        </v-card-text>
      </v-card>
    </div>
  </div>
  <addOrUpdateDialog :show-dialog="data.showDialog" :form="data.dialogForm" @close="method.closeDialog" @saveSuccess="method.saveSuccess" />
  <importCustomerTable :show-dialog="data.showDialogImport" @close="method.closeDialogImport" @saveSuccess="method.saveSuccessImport" />
  <hprintDialog ref="hprintDialogRef" :form="data.printForm" :tab-page="'print_page_main'" />
</template>

<script lang="tsx" setup>
import { computed, ref, reactive, onMounted, watch } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight, errorColor } from '@/constant/style'
import { CustomerVO } from '@/types/Base/Customer'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import tooltipBtn from '@/components/tooltip-btn.vue'
import addOrUpdateDialog from './add-or-update-customer.vue'
import { hookComponent } from '@/components/system'
import { DEBOUNCE_TIME } from '@/constant/system'
import { setSearchObject, getMenuAuthorityList } from '@/utils/common'
import { SearchObject, btnGroupItem } from '@/types/System/Form'
import i18n from '@/languages/i18n'
import { getCustomerList, deleteCustomer } from '@/api/base/customer'
import importCustomerTable from './import-customer-table.vue'
import customPager from '@/components/custom-pager.vue'
import { exportData } from '@/utils/exportTable'
import BtnGroup from '@/components/system/btnGroup.vue'
import hprintDialog from '@/components/hiprint/hiprintFast.vue'

const xTable = ref()
const hprintDialogRef = ref()

const data = reactive({
  searchForm: {
    customer_name: ''
  },
  tableData: [],
  // activeTab: null,
  showDialog: false,
  showDialogImport: false,
  dialogForm: {
    id: 0,
    customer_name: '',
    city: '',
    address: '',
    manager: '',
    email: '',
    contact_tel: '',
    is_valid: true
  },
  tablePage: {
    total: 0,
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE,
    searchObjects: ref<Array<SearchObject>>([])
  },
   printForm: {} as any,
  timer: ref<any>(null),
  btnList: [] as btnGroupItem[],
  // Menu operation permissions
  authorityList: getMenuAuthorityList() as string[]
})

const method = reactive({
  // Import Dialog
  openDialogImport: () => {
    data.showDialogImport = true
  },
  closeDialogImport: () => {
    data.showDialogImport = false
  },
  saveSuccessImport: () => {
    method.refresh()
    method.closeDialog()
  },
  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.getData()
  },
  // Add user
  add: () => {
    data.dialogForm = {
      id: 0,
      customer_name: '',
      city: '',
      address: '',
      manager: '',
      email: '',
      contact_tel: '',
      is_valid: true
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
    method.getData()
  },
  editRow(row: CustomerVO) {
    data.dialogForm = JSON.parse(JSON.stringify(row))
    data.showDialog = true
  },
  deleteRow(row: CustomerVO) {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteMessage'),
      handleConfirm: async () => {
        if (row.id) {
          const { data: res } = await deleteCustomer(row.id)
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
    method.getData()
  }),
  exportTable: () => {
    const $table = xTable.value
    exportData({
      table: $table,
      filename: i18n.global.t('router.sideBar.customer'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },
  getData: async () => {
    const { data: res } = await getCustomerList(data.tablePage)
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
    print: () => {
    const records = xTable.value.getCheckboxRecords()
    if (records.length > 0) {
      data.printForm = { detailList: records }
      const ref = hprintDialogRef.value
      ref.method.handleOpen()
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('base.userManagement.checkboxIsNull')
      })
    }
  },
})
onMounted(() => {
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
      name: i18n.global.t('system.page.import'),
      icon: 'mdi-database-import-outline',
      code: 'import',
      click: method.openDialogImport
    },
    {
      name: i18n.global.t('system.page.export'),
      icon: 'mdi-export-variant',
      code: 'export',
      click: method.exportTable
    },
    {
      name: i18n.global.t('system.page.print'),
      icon: 'mdi-printer',
      code: '',
      click: method.print
    }
  ]

  method.getData()
})

const cardHeight = computed(() => computedCardHeight({ hasTab: false }))
const tableHeight = computed(() => computedTableHeight({ hasTab: false }))

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
