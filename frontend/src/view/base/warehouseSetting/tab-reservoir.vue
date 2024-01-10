<template>
  <div class="operateArea">
    <v-row no-gutters>
      <!-- Operate Btn -->
      <v-col cols="3" class="col">
        <!-- <tooltip-btn icon="mdi-plus" :tooltip-text="$t('system.page.add')" @click="method.add"></tooltip-btn>
        <tooltip-btn icon="mdi-refresh" :tooltip-text="$t('system.page.refresh')" @click="method.refresh"></tooltip-btn>
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
              v-model="data.searchForm.warehouse_name"
              clearable
              hide-details
              density="comfortable"
              class="searchInput ml-5 mt-1"
              :label="$t('base.warehouseSetting.warehouse_name')"
              variant="solo"
            >
            </v-text-field>
          </v-col>
          <v-col cols="4">
            <v-text-field
              v-model="data.searchForm.area_name"
              clearable
              hide-details
              density="comfortable"
              class="searchInput ml-5 mt-1"
              :label="$t('base.warehouseSetting.area_name')"
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
    <vxe-table ref="xTableWarehouseArea" :data="data.tableData" :height="tableHeight" align="center">
      <vxe-column type="seq" width="60"></vxe-column>
      <vxe-column type="checkbox" width="50"></vxe-column>
      <vxe-column field="warehouse_name" :title="$t('base.warehouseSetting.warehouse_name')"></vxe-column>
      <vxe-column field="area_name" :title="$t('base.warehouseSetting.area_name')"></vxe-column>
      <vxe-column field="area_property" :title="$t('base.warehouseSetting.area_property')">
        <template #default="{ row, column }">
          <span>{{ formatAreaProperty(row[column.property]) }}</span>
        </template>
      </vxe-column>
      <vxe-column field="is_valid" :title="$t('base.warehouseSetting.is_valid')">
        <template #default="{ row, column }">
          <span>{{ formatIsValid(row[column.property]) }}</span>
        </template>
      </vxe-column>
      <vxe-column field="operate" :title="$t('system.page.operate')" width="160" :resizable="false" show-overflow>
        <template #default="{ row }">
          <tooltip-btn
            :flat="true"
            icon="mdi-pencil-outline"
            :tooltip-text="$t('system.page.edit')"
            :disabled="!data.authorityList.includes('area-save')"
            @click="method.editRow(row)"
          ></tooltip-btn>
          <tooltip-btn
            :flat="true"
            icon="mdi-delete-outline"
            :tooltip-text="$t('system.page.delete')"
            :icon-color="!data.authorityList.includes('area-delete')?'':errorColor"
            :disabled="!data.authorityList.includes('area-delete')"
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
  <add-or-update-dialog :show-dialog="data.showDialog" :form="data.dialogForm" @close="method.closeDialog" @saveSuccess="method.saveSuccess" />
</template>

<script lang="ts" setup>
import { computed, ref, reactive, watch, onMounted } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight, errorColor } from '@/constant/style'
import { WarehouseAreaVO, AreaProperty } from '@/types/Base/Warehouse'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { hookComponent } from '@/components/system'
import { deleteWarehouseArea, getWarehouseAreaList } from '@/api/base/warehouseSetting'
import tooltipBtn from '@/components/tooltip-btn.vue'
import addOrUpdateDialog from './add-or-update-reservoir.vue'
import i18n from '@/languages/i18n'
import { formatAreaProperty } from '@/utils/format/formatWarehouse'
import { formatIsValid } from '@/utils/format/formatSystem'
import customPager from '@/components/custom-pager.vue'
import { DEBOUNCE_TIME } from '@/constant/system'
import { SearchObject, btnGroupItem } from '@/types/System/Form'
import { exportData } from '@/utils/exportTable'
import BtnGroup from '@/components/system/btnGroup.vue'
import { setSearchObject, getMenuAuthorityList } from '@/utils/common'

const xTableWarehouseArea = ref()

const data = reactive({
  showDialog: false,
  dialogForm: ref<WarehouseAreaVO>({
    id: 0,
    parent_id: 0,
    warehouse_name: '',
    area_name: '',
    area_property: AreaProperty.picking_area,
    is_valid: true
  }),
  searchForm: {
    warehouse_name: '',
    area_name: ''
  },
  activeTab: null,
  tableData: ref<WarehouseAreaVO[]>([]),
  tablePage: reactive({
    total: 0,
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE,
    searchObjects: ref<Array<SearchObject>>([])
  }),
  timer: ref<any>(null),
  btnList: [] as btnGroupItem[],
  // Menu operation permissions
  authorityList: getMenuAuthorityList() as string[]
})

const method = reactive({
  // Open a dialog to add
  add: () => {
    data.dialogForm = {
      id: 0,
      parent_id: 0,
      warehouse_name: '',
      area_name: '',
      area_property: AreaProperty.picking_area,
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
  // Refresh data
  refresh: () => {
    method.getWarehouseAreaList()
  },
  getWarehouseAreaList: async () => {
    const { data: res } = await getWarehouseAreaList(data.tablePage)
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

  editRow(row: WarehouseAreaVO) {
    data.dialogForm = JSON.parse(JSON.stringify(row))
    data.showDialog = true
  },
  deleteRow(row: WarehouseAreaVO) {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteMessage'),
      handleConfirm: async () => {
        if (row.id) {
          const { data: res } = await deleteWarehouseArea(row.id)
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

    method.getWarehouseAreaList()
  }),
  exportTable: () => {
    const $table = xTableWarehouseArea.value
    exportData({
      table: $table,
      filename: i18n.global.t('base.warehouseSetting.reservoirSetting'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },
  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.getWarehouseAreaList()
  }
})

onMounted(() => {
  data.btnList = [
    {
      name: i18n.global.t('system.page.add'),
      icon: 'mdi-plus',
      code: 'area-save',
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
      code: 'area-export',
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
  getWarehouseAreaList: method.getWarehouseAreaList
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
