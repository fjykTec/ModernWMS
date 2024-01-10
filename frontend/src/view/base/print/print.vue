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
                <!-- new version -->
                <BtnGroup :authority-list="data.authorityList" :btn-list="data.btnList" />
              </v-col>

              <!-- Search Input -->
              <v-col cols="8" sm="8">
                <v-row no-gutters @keyup.enter="method.sureSearch">
                  <v-col cols="4"></v-col>
                  <v-col cols="4">
                    <v-text-field
                      v-model="data.searchForm.vue_path"
                      clearable
                      hide-details
                      density="comfortable"
                      class="searchInput ml-5 mt-1"
                      :label="$t('base.printSolution.vue_path')"
                      variant="solo"
                    >
                    </v-text-field>
                  </v-col>
                  <!-- <v-col cols="4">
                    <v-text-field
                      v-model="data.searchForm.tab_page"
                      clearable
                      hide-details
                      density="comfortable"
                      class="searchInput ml-5 mt-1"
                      :label="$t('base.printSolution.tab_page')"
                      variant="solo"
                    >
                    </v-text-field>
                  </v-col> -->
                  <v-col cols="4">
                    <v-text-field
                      v-model="data.searchForm.solution_name"
                      clearable
                      hide-details
                      density="comfortable"
                      class="searchInput ml-5 mt-1"
                      :label="$t('base.printSolution.solution_name')"
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
              <vxe-column type="checkbox" width="50" fixed="left"></vxe-column>
              <vxe-column type="seq" width="60"></vxe-column>
              <vxe-column field="vue_path" :title="$t('base.printSolution.vue_path')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ row.sku_name }}</span>
                  <span v-else>{{ $t(`router.sideBar.${row.vue_path}`) }}</span>
                </template>
              </vxe-column>
              <!-- <vxe-column field="tab_page" :title="$t('base.printSolution.tab_page')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ row.unit }}</span>
                  <span v-else>{{ row.tab_page }}</span>
                </template>
              </vxe-column> -->
              <vxe-column field="solution_name" width="150px" :title="$t('base.printSolution.solution_name')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ row.sku_code }}</span>
                  <span v-else>{{ row.solution_name }}</span>
                </template>
              </vxe-column>
              <vxe-column
                field="operate"
                :title="$t('system.page.operate')"
                width="160px"
                :resizable="false"
                show-overflow
              >
                <template #default="{ row }">
                  <div>
                    <tooltip-btn
                      :flat="true"
                      icon="mdi-pencil-outline"
                      :tooltip-text="$t('system.page.edit')"
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
    <addOrUpdateDialog
      :show-dialog="data.showDialog"
      :form="data.dialogForm"
      @close="method.closeDialog"
      @saveSuccess="method.saveSuccess"
    />
  </div>
</template>

<script lang="ts" setup>
import { computed, onMounted, reactive, ref, watch } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight, errorColor } from '@/constant/style'
import tooltipBtn from '@/components/tooltip-btn.vue'
import { DataProps, PrintSolutionVO } from '@/types/Base/PrintSolution'
import { getPrintSolutionList, deletePrintSolution } from '@/api/base/printSolution'
import { hookComponent } from '@/components/system'
import addOrUpdateDialog from './add-or-update-print.vue'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import i18n from '@/languages/i18n'
import customPager from '@/components/custom-pager.vue'
import { setSearchObject, getMenuAuthorityList } from '@/utils/common'
import { exportData } from '@/utils/exportTable'
import { DEBOUNCE_TIME } from '@/constant/system'
import BtnGroup from '@/components/system/btnGroup.vue'

const xTable = ref()

const data: DataProps = reactive({
  searchForm: {
    vue_path: '',
    tab_page: '',
    solution_name: '',
  },
  timer: null,
  tableData: [],
  tablePage: {
    total: 0,
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE
  },
  // Dialog info
  showDialog: false,
  dialogForm: {
    id: 0,
    vue_path: '',
    tab_page: '',
    solution_name: '',
    config_json: '',
    report_length: 0,
    report_width: 0,
    report_direction: 'st',
    tenant_id: 0
  },
  btnList: [],
  authorityList: getMenuAuthorityList(),
  selectRowData: []
})

const method = reactive({
  // Check if the checkbox can be checked
  getCheckBoxDisableState: ({ row }: { row: any }): boolean => row.parent_id,
  selectAllEvent({ checked }) {
    const records = xTable.value.getCheckboxRecords()
    checked ? (data.selectRowData = records) : (data.selectRowData = [])
  },
  selectChangeEvent() {
    data.selectRowData = xTable.value.getCheckboxRecords()
  },
  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.getCompanyList()
  },
  // Find Data by Pagination
  getCompanyList: async () => {
    const { data: res } = await getPrintSolutionList(data.tablePage)
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
  // Add user
  add: () => {
    data.dialogForm = {
      id: 0,
      vue_path: '',
      tab_page: '',
      solution_name: '',
      config_json: '',
      report_length: 0,
      report_width: 0,
      report_direction: ''
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
  editRow(row: PrintSolutionVO) {
    data.dialogForm = JSON.parse(JSON.stringify(row))
    data.showDialog = true
  },
  deleteRow(row: PrintSolutionVO) {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteMessage'),
      handleConfirm: async () => {
        if (row.id) {
          const { data: res } = await deletePrintSolution(row.id)
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
      filename: i18n.global.t('router.sideBar.print'),
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
    }
  ]
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
