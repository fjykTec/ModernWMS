<template>
  <div class="operateArea">
    <v-row no-gutters>
      <!-- Operate Btn -->
      <v-col cols="3" class="col">
        <tooltip-btn icon="mdi-plus" :tooltip-text="$t('system.page.add')" @click="method.add"></tooltip-btn>
        <tooltip-btn icon="mdi-refresh" :tooltip-text="$t('system.page.refresh')" @click="method.refresh"></tooltip-btn>
        <tooltip-btn icon="mdi-export-variant" :tooltip-text="$t('system.page.export')" @click="method.exportTable"> </tooltip-btn>
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
              v-model="data.searchForm.warehouse_area_name"
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
    <vxe-table ref="xTableGoodsLocation" :column-config="{ minWidth: '100px' }" :data="data.tableData" :height="tableHeight" align="center">
      <vxe-column type="seq" width="60"></vxe-column>
      <vxe-column type="checkbox" width="50"></vxe-column>
      <vxe-column field="warehouse_name" :title="$t('base.warehouseSetting.warehouse_name')"></vxe-column>
      <vxe-column field="warehouse_area_name" :title="$t('base.warehouseSetting.area_name')"></vxe-column>
      <vxe-column field="warehouse_area_property" :title="$t('base.warehouseSetting.area_property')">
        <template #default="{ row, column }">
          <span>{{ formatAreaProperty(row[column.property]) }}</span>
        </template>
      </vxe-column>
      <vxe-column field="location_name" :title="$t('base.warehouseSetting.location_name')"></vxe-column>
      <vxe-column width="150px" field="location_length" :title="$t('base.warehouseSetting.location_length')"></vxe-column>
      <vxe-column width="150px" field="location_width" :title="$t('base.warehouseSetting.location_width')"></vxe-column>
      <vxe-column width="150px" field="location_heigth" :title="$t('base.warehouseSetting.location_heigth')"></vxe-column>
      <vxe-column width="150px" field="location_volume" :title="$t('base.warehouseSetting.location_volume')"></vxe-column>
      <vxe-column width="150px" field="location_load" :title="$t('base.warehouseSetting.location_load')"></vxe-column>
      <vxe-column field="roadway_number" :title="$t('base.warehouseSetting.roadway_number')"></vxe-column>
      <vxe-column field="shelf_number" :title="$t('base.warehouseSetting.shelf_number')"></vxe-column>
      <vxe-column field="layer_number" :title="$t('base.warehouseSetting.layer_number')"></vxe-column>
      <vxe-column field="tag_number" :title="$t('base.warehouseSetting.tag_number')"></vxe-column>
      <vxe-column field="is_valid" :title="$t('base.warehouseSetting.is_valid')">
        <template #default="{ row, column }">
          <span>{{ formatIsValid(row[column.property]) }}</span>
        </template>
      </vxe-column>
      <vxe-column field="operate" :title="$t('system.page.operate')" width="160" :resizable="false" show-overflow>
        <template #default="{ row }">
          <tooltip-btn :flat="true" icon="mdi-pencil-outline" :tooltip-text="$t('system.page.edit')" @click="method.editRow(row)"></tooltip-btn>
          <tooltip-btn
            :flat="true"
            icon="mdi-delete-outline"
            :tooltip-text="$t('system.page.delete')"
            :icon-color="errorColor"
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
import { computed, ref, reactive, watch } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight, errorColor } from '@/constant/style'
import { GoodsLocationVO } from '@/types/Base/Warehouse'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { hookComponent } from '@/components/system'
import { deleteGoodsLocation, getGoodsLocationList } from '@/api/base/warehouseSetting'
import tooltipBtn from '@/components/tooltip-btn.vue'
import addOrUpdateDialog from './add-or-update-location.vue'
import i18n from '@/languages/i18n'
import { formatIsValid } from '@/utils/format/formatSystem'
import { formatAreaProperty } from '@/utils/format/formatWarehouse'
import customPager from '@/components/custom-pager.vue'
import { setSearchObject } from '@/utils/common'
import { DEBOUNCE_TIME } from '@/constant/system'
import { SearchObject } from '@/types/System/Form'
import { exportData } from '@/utils/exportTable'

const xTableGoodsLocation = ref()

const data = reactive({
  showDialog: false,
  dialogForm: ref<GoodsLocationVO>({
    id: 0,
    warehouse_name: '',
    warehouse_area_name: '',
    location_name: '',
    location_length: 0,
    location_width: 0,
    location_heigth: 0,
    location_volume: 0,
    location_load: 0,
    roadway_number: '',
    shelf_number: '',
    layer_number: '',
    tag_number: '',
    is_valid: true
  }),
  searchForm: {
    warehouse_name: '',
    warehouse_area_name: ''
  },
  activeTab: null,
  tableData: ref<GoodsLocationVO[]>([]),
  tablePage: reactive({
    total: 0,
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE,
    searchObjects: ref<Array<SearchObject>>([])
  }),
  timer: ref<any>(null)
})

const method = reactive({
  // Open a dialog to add
  add: () => {
    data.dialogForm = {
      id: 0,
      warehouse_name: '',
      warehouse_area_name: '',
      location_name: '',
      location_length: 0,
      location_width: 0,
      location_heigth: 0,
      location_volume: 0,
      location_load: 0,
      roadway_number: '',
      shelf_number: '',
      layer_number: '',
      tag_number: '',
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
    method.getGoodsLocationList()
  },
  getGoodsLocationList: async () => {
    const { data: res } = await getGoodsLocationList(data.tablePage)
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
  editRow(row: GoodsLocationVO) {
    data.dialogForm = JSON.parse(JSON.stringify(row))
    data.showDialog = true
  },
  deleteRow(row: GoodsLocationVO) {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteMessage'),
      handleConfirm: async () => {
        if (row.id) {
          const { data: res } = await deleteGoodsLocation(row.id)
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

    method.getGoodsLocationList()
  }),
  exportTable: () => {
    const $table = xTableGoodsLocation.value
    exportData({
      table: $table,
      filename: i18n.global.t('base.warehouseSetting.locationSetting'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },
  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.getGoodsLocationList()
  }
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
  getGoodsLocationList: method.getGoodsLocationList
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
