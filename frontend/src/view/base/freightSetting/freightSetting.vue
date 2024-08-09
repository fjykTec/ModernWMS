<!-- Freight Setting -->
<template>
  <div class="container">
    <div>
      <!-- Main Content -->
      <v-card class="mt-5">
        <v-card-text>
          <v-window v-model="data.activeTab">
            <v-window-item>
              <div class="operateArea">
                <v-row no-gutters>
                  <!-- Operate Btn -->
                  <v-col cols="3" class="col">
                    <!-- <tooltip-btn icon="mdi-plus" :tooltip-text="$t('system.page.add')" @click="method.add"></tooltip-btn>
                    <tooltip-btn icon="mdi-refresh" :tooltip-text="$t('system.page.refresh')" @click="method.refresh"></tooltip-btn>
                    <tooltip-btn icon="mdi-database-import-outline" :tooltip-text="$t('system.page.import')" @click="method.openDialogImport">
                    </tooltip-btn>
                    <tooltip-btn icon="mdi-export-variant" :tooltip-text="$t('system.page.export')" @click="method.exportTable"> </tooltip-btn> -->

                    <!-- new version -->
                    <BtnGroup :authority-list="data.authorityList" :btn-list="data.btnList" />
                  </v-col>

                  <!-- Search Input -->
                  <v-col cols="9">
                    <v-row no-gutters @keyup.enter="method.sureSearch">
                      <v-col cols="4">
                        <v-text-field
                          v-model="data.searchForm.carrier"
                          clearable
                          hide-details
                          density="comfortable"
                          class="searchInput ml-5 mt-1"
                          :label="$t('base.freightSetting.carrier')"
                          variant="solo"
                        >
                        </v-text-field>
                      </v-col>
                      <v-col cols="4">
                        <v-text-field
                          v-model="data.searchForm.departure_city"
                          clearable
                          hide-details
                          density="comfortable"
                          class="searchInput ml-5 mt-1"
                          :label="$t('base.freightSetting.departure_city')"
                          variant="solo"
                        >
                        </v-text-field>
                      </v-col>
                      <v-col cols="4">
                        <v-text-field
                          v-model="data.searchForm.arrival_city"
                          clearable
                          hide-details
                          density="comfortable"
                          class="searchInput ml-5 mt-1"
                          :label="$t('base.freightSetting.arrival_city')"
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
                  <vxe-column field="carrier" :title="$t('base.freightSetting.carrier')"></vxe-column>
                  <vxe-column field="departure_city" :title="$t('base.freightSetting.departure_city')"></vxe-column>
                  <vxe-column field="arrival_city" :title="$t('base.freightSetting.arrival_city')"></vxe-column>
                  <vxe-column field="price_per_weight" :title="$t('base.freightSetting.price_per_weight')"></vxe-column>
                  <vxe-column field="price_per_volume" :title="$t('base.freightSetting.price_per_volume')"></vxe-column>
                  <vxe-column field="min_payment" :title="$t('base.freightSetting.min_payment')"></vxe-column>
                  <vxe-column field="creator" :title="$t('base.freightSetting.creator')"></vxe-column>
                  <vxe-date-column
                    field="create_time"
                    width="170px"
                    format="yyyy-MM-dd HH:mm" 
                    :title="$t('base.freightSetting.create_time')"
                  ></vxe-date-column>
                  <vxe-column field="is_valid" :title="$t('base.freightSetting.is_valid')">
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
                ></custom-pager>
              </div>
            </v-window-item>
          </v-window>
        </v-card-text>
      </v-card>
      <addOrUpdateDialog :show-dialog="data.showDialog" :form="data.dialogForm" @close="method.closeDialog" @saveSuccess="method.saveSuccess" />
      <import-table :show-dialog="data.showDialogImport" @close="method.closeDialogImport" @saveSuccess="method.saveSuccessImport" />
    </div>
  </div>
</template>

<script lang="ts" setup>
import { computed, ref, reactive, onMounted, watch } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight, errorColor } from '@/constant/style'
import { FreightVO } from '@/types/Base/Freight'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { hookComponent } from '@/components/system'
import { deleteFreight, getFreightList } from '@/api/base/freightSetting'
import { DEBOUNCE_TIME } from '@/constant/system'
import { setSearchObject, getMenuAuthorityList } from '@/utils/common'
import { SearchObject, btnGroupItem } from '@/types/System/Form'
import tooltipBtn from '@/components/tooltip-btn.vue'
import customPager from '@/components/custom-pager.vue'
import addOrUpdateDialog from './add-or-update-freight.vue'
import importTable from './import-table.vue'
import i18n from '@/languages/i18n'
import { exportData } from '@/utils/exportTable'
import BtnGroup from '@/components/system/btnGroup.vue'

const xTable = ref()

const data = reactive({
  showDialog: false,
  showDialogImport: false,

  dialogForm: {
    id: 0,
    carrier: '',
    departure_city: '',
    arrival_city: '',
    price_per_weight: 0,
    price_per_volume: 0,
    min_payment: 0,
    is_valid: true
  },
  searchForm: {
    carrier: '',
    departure_city: '',
    arrival_city: ''
  },
  activeTab: null,
  tableData: ref<FreightVO[]>([]),
  tablePage: reactive({
    total: 0,
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE,
    searchObjects: ref<Array<SearchObject>>([])
  }),
  timer: ref<any>(null),
  btnList: [] as btnGroupItem[],
  authorityList: getMenuAuthorityList() as string[]
})

const method = reactive({
  // Open a dialog to add
  add: () => {
    data.dialogForm = {
      id: 0,
      carrier: '',
      departure_city: '',
      arrival_city: '',
      price_per_weight: 0,
      price_per_volume: 0,
      min_payment: 0,
      is_valid: true
    }
    data.showDialog = true
  },
  // Shut add or update dialog
  closeDialog: () => {
    data.showDialog = false
  },
  // After add or update success.
  saveSuccess: () => {
    method.refresh()
    method.closeDialog()
  },
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
  // Refresh data
  refresh: () => {
    method.getFreightList()
  },
  getFreightList: async () => {
    const { data: res } = await getFreightList(data.tablePage)
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
  editRow(row: FreightVO) {
    data.dialogForm = JSON.parse(JSON.stringify(row))
    data.showDialog = true
  },
  deleteRow(row: FreightVO) {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteMessage'),
      handleConfirm: async () => {
        if (row.id) {
          const { data: res } = await deleteFreight(row.id)
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

    method.getFreightList()
  }),
  exportTable: () => {
    const $table = xTable.value
    exportData({
      table: $table,
      filename: i18n.global.t('router.sideBar.freightSetting'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },
  importTable: () => {
    const $table = xTable.value
    $table.importData()
  },
  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.getFreightList()
  }
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
    }
  ]

  method.getFreightList()
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
