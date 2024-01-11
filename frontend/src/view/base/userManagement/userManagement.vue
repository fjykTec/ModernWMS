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
              <v-col cols="12" sm="8">
                <v-row no-gutters @keyup.enter="method.sureSearch">
                  <v-col cols="4">
                    <v-text-field
                      v-model="data.searchForm.user_num"
                      clearable
                      hide-details
                      density="comfortable"
                      class="searchInput ml-5 mt-1"
                      :label="$t('base.userManagement.user_num')"
                      variant="solo"
                    >
                    </v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-text-field
                      v-model="data.searchForm.user_name"
                      clearable
                      hide-details
                      density="comfortable"
                      class="searchInput ml-5 mt-1"
                      :label="$t('base.userManagement.user_name')"
                      variant="solo"
                    >
                    </v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-text-field
                      v-model="data.searchForm.user_role"
                      clearable
                      hide-details
                      density="comfortable"
                      class="searchInput ml-5 mt-1"
                      :label="$t('base.userManagement.user_role')"
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
            <vxe-table ref="xTable" :data="data.tableData" :height="tableHeight" align="center">
              <template #empty>
                {{ i18n.global.t('system.page.noData') }}
              </template>
              <vxe-column type="seq" width="60"></vxe-column>
              <vxe-column type="checkbox" width="50"></vxe-column>
              <vxe-column field="user_num" :title="$t('base.userManagement.user_num')"></vxe-column>
              <vxe-column field="user_name" :title="$t('base.userManagement.user_name')"></vxe-column>
              <vxe-column field="user_role" :title="$t('base.userManagement.user_role')"></vxe-column>
              <vxe-column field="sex" :title="$t('base.userManagement.sex')">
                <template #default="{ row }">
                  <span>{{ $t(`system.combobox.sex.${row.sex}`) }}</span>
                </template>
              </vxe-column>
              <vxe-column field="contact_tel" :title="$t('base.userManagement.contact_tel')"></vxe-column>
              <vxe-column field="is_valid" :title="$t('base.userManagement.is_valid')">
                <template #default="{ row, column }">
                  <span>{{ row[column.property] ? $t('system.combobox.yesOrNo.yes') : $t('system.combobox.yesOrNo.no') }}</span>
                </template>
              </vxe-column>
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
            <!-- <vxe-grid v-bind="data.gridOptions">
              <template #pager>
                <custom-pagerr
                  v-model:current-page="data.tablePage.pageIndex"
                  v-model:page-size="data.tablePage.pageSize"
                  :layouts="['Sizes', 'PrevJump', 'PrevPage', 'Number', 'NextPage', 'NextJump', 'FullJump', 'Total']"
                  :total="data.tablePage.total"
                  @page-change="handlePageChange"
                >
                </custom-pagerr>
              </template>
            </vxe-grid> -->
          </div>
        </v-card-text>
      </v-card>
    </div>
    <!-- Add or modify data mode window -->
    <addOrUpdateDialog :show-dialog="data.showDialog" :form="data.dialogForm" @close="method.closeDialog" @saveSuccess="method.saveSuccess" />
    <import-table :show-dialog="data.showDialogImport" @close="method.closeDialogImport" @saveSuccess="method.saveSuccessImport" />
  </div>
</template>

<script lang="ts" setup>
import { computed, reactive, onMounted, ref, watch } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight, errorColor } from '@/constant/style'
import tooltipBtn from '@/components/tooltip-btn.vue'
import { DataProps, UserVO } from '@/types/Base/UserManagement'
import { getUserList, deleteUser, resetPassword } from '@/api/base/userManagement'
import { hookComponent } from '@/components/system'
import addOrUpdateDialog from './add-or-update-user.vue'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import i18n from '@/languages/i18n'
import customPager from '@/components/custom-pager.vue'
import { setSearchObject, getMenuAuthorityList } from '@/utils/common'
import importTable from './import-table.vue'
import { exportData } from '@/utils/exportTable'
import { DEBOUNCE_TIME } from '@/constant/system'
import BtnGroup from '@/components/system/btnGroup.vue'

const xTable = ref()

const data: DataProps = reactive({
  showDialogImport: false,
  searchForm: {
    user_num: '',
    user_name: '',
    user_role: ''
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
    user_num: '',
    user_name: '',
    contact_tel: '',
    auth_string: '',
    is_valid: true
  },
  btnList: [],
  authorityList: getMenuAuthorityList()
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
    method.closeDialogImport()
  },
  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.getUserList()
  },
  // Find Data by Pagination
  getUserList: async () => {
    const { data: res } = await getUserList(data.tablePage)
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
      user_num: '',
      user_name: '',
      contact_tel: '',
      auth_string: '',
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
    method.getUserList()
  },
  editRow(row: UserVO) {
    data.dialogForm = JSON.parse(JSON.stringify(row))
    data.showDialog = true
  },
  deleteRow(row: UserVO) {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteMessage'),
      handleConfirm: async () => {
        if (row.id) {
          const { data: res } = await deleteUser(row.id)
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
  // When change paging
  handlePageChange: ref<VxePagerEvents.PageChange>(({ currentPage, pageSize }) => {
    data.tablePage.pageIndex = currentPage
    data.tablePage.pageSize = pageSize
    method.refresh()
  }),
  // Export table
  exportTable: () => {
    const $table = xTable.value
    exportData({
      table: $table,
      filename: i18n.global.t('router.sideBar.userManagement'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },
  // Reset password
  restPwd: async () => {
    const checkRecord: UserVO[] = xTable.value.getCheckboxRecords(true)
    if (checkRecord.length > 0) {
      hookComponent.$dialog({
        content: i18n.global.t('base.userManagement.beforeResetPwd'),
        handleConfirm: async () => {
          const idList = checkRecord.map((item) => item.id)
          const { data: res } = await resetPassword(idList)
          if (!res.isSuccess) {
            hookComponent.$message({
              type: 'error',
              content: res.errorMessage
            })
            return
          }
          hookComponent.$dialog({
            content: `${ i18n.global.t('base.userManagement.afterResetPwd') } ${ res.data }`
          })
        }
      })
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('base.userManagement.checkboxIsNull')
      })
    }
  }
})

onMounted(async () => {
  await method.getUserList()

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
      name: i18n.global.t('base.userManagement.restPwd'),
      icon: 'mdi-lock-reset',
      code: 'resetPwd',
      click: method.restPwd
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
