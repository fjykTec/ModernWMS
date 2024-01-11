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
                <!-- <v-row no-gutters @keyup.enter="method.sureSearch">
                  <v-col cols="12" sm="4">
                    <v-text-field
                      v-model="data.searchForm.userName"
                      clearable
                      hide-details
                      density="comfortable"
                      class="searchInput ml-5 mt-1"
                      :label="$t('login.userName')"
                      variant="solo"
                    >
                    </v-text-field>
                  </v-col>
                  <v-col cols="12" sm="4">
                    <v-text-field
                      v-model="data.searchForm.userName1"
                      clearable
                      hide-details
                      density="comfortable"
                      class="searchInput ml-5 mt-1"
                      :label="$t('login.userName')"
                      variant="solo"
                    >
                    </v-text-field>
                  </v-col>
                  <v-col cols="12" sm="4">
                    <v-text-field
                      v-model="data.searchForm.userName2"
                      clearable
                      hide-details
                      density="comfortable"
                      class="searchInput ml-5 mt-1"
                      :label="$t('login.userName')"
                      variant="solo"
                    >
                    </v-text-field>
                  </v-col>
                </v-row> -->
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
              <vxe-column field="role_name" :title="$t('base.userRoleSetting.role_name')"></vxe-column>
              <vxe-column field="is_valid" :title="$t('base.userRoleSetting.is_valid')">
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
          </div>
        </v-card-text>
      </v-card>
    </div>
    <!-- Add or modify data mode window -->
    <addOrUpdateDialog :show-dialog="data.showDialog" :form="data.dialogForm" @close="method.closeDialog" @saveSuccess="method.saveSuccess" />
  </div>
</template>

<script lang="ts" setup>
import { computed, reactive, onMounted, ref } from 'vue'
import { computedCardHeight, computedTableHeight, errorColor } from '@/constant/style'
import tooltipBtn from '@/components/tooltip-btn.vue'
import { DataProps, UserRoleVO } from '@/types/Base/UserRoleSetting'
import { getUserRoleAll, deleteUserRole } from '@/api/base/userRoleSetting'
import { hookComponent } from '@/components/system'
import addOrUpdateDialog from './add-or-update-user-role.vue'
import i18n from '@/languages/i18n'
import { exportData } from '@/utils/exportTable'
import BtnGroup from '@/components/system/btnGroup.vue'
import { getMenuAuthorityList } from '@/utils/common'

const xTable = ref()

const data: DataProps = reactive({
  // searchForm: {
  //   userName: '',
  //   userName1: '',
  //   userName2: ''
  // },
  tableData: [],
  // Dialog info
  showDialog: false,
  dialogForm: {
    id: 0,
    role_name: '',
    is_valid: true
  },
  btnList: [],
  // Menu operation permissions
  authorityList: getMenuAuthorityList()
})

const method = reactive({
  sureSearch: () => {
    // console.log(data.searchForm)
  },
  // Find Data by Pagination
  getUserRoleList: async () => {
    const { data: res } = await getUserRoleAll()
    if (!res.isSuccess) {
      hookComponent.$message({
        type: 'error',
        content: res.errorMessage
      })
      return
    }
    data.tableData = res.data
  },
  // Add user
  add: () => {
    data.dialogForm = {
      id: 0,
      role_name: '',
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
    method.getUserRoleList()
  },
  editRow(row: UserRoleVO) {
    data.dialogForm = JSON.parse(JSON.stringify(row))
    data.showDialog = true
  },
  deleteRow(row: UserRoleVO) {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteMessage'),
      handleConfirm: async () => {
        if (row.id) {
          const { data: res } = await deleteUserRole(row.id)
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
      filename: i18n.global.t('router.sideBar.userRoleSetting'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  }
})

onMounted(async () => {
  await method.getUserRoleList()

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

const tableHeight = computed(() => computedTableHeight({ hasTab: false, hasPager: false }))
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
