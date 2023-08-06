<template>
  <div class="container">
    <div>
      <!-- Main Content -->
      <v-card class="mt-5">
        <v-card-text>
          <div class="operateArea">
            <v-row no-gutters>
              <!-- Operate Btn -->
              <v-col cols="12" sm="3" class="col">
                <tooltip-btn icon="mdi-plus" :tooltip-text="$t('system.page.add')" @click="method.add()"></tooltip-btn>
                <tooltip-btn icon="mdi-refresh" :tooltip-text="$t('system.page.refresh')" @click="method.refresh()"></tooltip-btn>
                <tooltip-btn icon="mdi-export-variant" :tooltip-text="$t('system.page.export')" @click="method.exportTable"></tooltip-btn>
              </v-col>

              <!-- Search Input -->
              <v-col cols="12" sm="9">
                <v-row no-gutters @keyup.enter="method.sureSearch">
                  <v-col cols="4">
                    <v-text-field
                      v-model="data.searchForm.spu_code"
                      clearable
                      hide-details
                      density="comfortable"
                      class="searchInput ml-5 mt-1"
                      :label="$t('base.commodityManagement.spu_code')"
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
                      :label="$t('base.commodityManagement.spu_name')"
                      variant="solo"
                    >
                    </v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-text-field
                      v-model="data.searchForm.category_name"
                      clearable
                      hide-details
                      density="comfortable"
                      class="searchInput ml-5 mt-1"
                      :label="$t('base.commodityManagement.category_name')"
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
              :tree-config="data.tableTreeConfig"
            >
              <template #empty>
                {{ i18n.global.t('system.page.noData') }}
              </template>
              <vxe-column type="seq" width="60"></vxe-column>
              <vxe-column field="spu_code" width="150px" :title="$t('base.commodityManagement.spu_code')" tree-node>
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ row.sku_code }}</span>
                  <span v-else>{{ row.spu_code }}</span>
                </template>
              </vxe-column>
              <vxe-column field="spu_name" :title="$t('base.commodityManagement.spu_name')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ row.sku_name }}</span>
                  <span v-else>{{ row.spu_name }}</span>
                </template>
              </vxe-column>
              <vxe-column field="category_name" :title="$t('base.commodityManagement.category_name')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ row.unit }}</span>
                  <span v-else>{{ row.category_name }}</span>
                </template>
              </vxe-column>
              <vxe-column field="spu_description" :title="$t('base.commodityManagement.spu_description')"> </vxe-column>
              <vxe-column field="bar_code" :title="$t('base.commodityManagement.bar_code')"> </vxe-column>
              <vxe-column field="supplier_name" :title="$t('base.commodityManagement.supplier_name')"> </vxe-column>
              <vxe-column field="brand" :title="$t('base.commodityManagement.brand')"> </vxe-column>

              <vxe-column field="weight" :title="$t('base.commodityManagement.weight')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ `${row.weight} ${GetUnit('weight', row.weight_unit)}` }}</span>
                </template>
              </vxe-column>
              <vxe-column field="lenght" :title="$t('base.commodityManagement.lenght')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ `${row.lenght} ${GetUnit('length', row.length_unit)}` }}</span>
                </template>
              </vxe-column>
              <vxe-column field="width" :title="$t('base.commodityManagement.width')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ `${row.width} ${GetUnit('length', row.length_unit)}` }}</span>
                </template>
              </vxe-column>
              <vxe-column field="height" :title="$t('base.commodityManagement.height')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ `${row.height} ${GetUnit('length', row.length_unit)}` }}</span>
                </template>
              </vxe-column>
              <vxe-column field="volume" :title="$t('base.commodityManagement.volume')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ `${row.volume} ${GetUnit('volume', row.volume_unit)}` }}</span>
                </template>
              </vxe-column>

              <vxe-column field="cost" :title="$t('base.commodityManagement.cost')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ row.cost }}</span>
                </template>
              </vxe-column>
              <vxe-column field="price" :title="$t('base.commodityManagement.price')">
                <template #default="{ row }">
                  <span v-if="row.parent_id > 0">{{ row.price }}</span>
                </template>
              </vxe-column>
              <!-- <vxe-column field="sku_name" :title="$t('base.commodityManagement.sku_name')"></vxe-column> -->
              <!-- <vxe-column field="supplier_name" :title="$t('base.commodityManagement.supplier_name')"></vxe-column>
              <vxe-column field="brand" :title="$t('base.commodityManagement.brand')"></vxe-column>
              <vxe-column field="unit" :title="$t('base.commodityManagement.unit')"></vxe-column>
              <vxe-column field="cost" :title="$t('base.commodityManagement.cost')"></vxe-column> -->
              <vxe-column field="operate" :title="$t('system.page.operate')" width="160" :resizable="false" show-overflow>
                <template #default="{ row }">
                  <div v-if="!row.parent_id || row.parent_id <= 0">
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
                      :icon-color="errorColor"
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
    <addOrUpdateDialog :show-dialog="data.showDialog" :form="data.dialogForm" @close="method.closeDialog" @saveSuccess="method.saveSuccess" />
  </div>
</template>

<script lang="ts" setup>
import { computed, reactive, onMounted, ref, watch } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { computedCardHeight, computedTableHeight, errorColor } from '@/constant/style'
import tooltipBtn from '@/components/tooltip-btn.vue'
import { CommodityVO, DataProps } from '@/types/Base/CommodityManagement'
import { getSpuList, deleteSpu } from '@/api/base/commodityManagementSetting'
import { hookComponent } from '@/components/system'
import addOrUpdateDialog from './add-or-update-commodity.vue'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import i18n from '@/languages/i18n'
import { GetUnit } from '@/constant/commodityManagement'
import customPager from '@/components/custom-pager.vue'
import { setSearchObject } from '@/utils/common'
import { exportData } from '@/utils/exportTable'
import { DEBOUNCE_TIME } from '@/constant/system'

const xTable = ref()

const data: DataProps = reactive({
  searchForm: {
    spu_code: '',
    spu_name: '',
    category_name: ''
  },
  timer: null,
  tableData: [],
  tablePage: {
    total: 0,
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE
  },
  tableTreeConfig: {
    transform: true,
    rowField: 'tree_id',
    parentField: 'parent_id'
  },
  // Dialog info
  showDialog: false,
  dialogForm: {
    id: 0,
    spu_code: '',
    spu_name: '',
    category_id: 0,
    category_name: '',
    spu_description: '',
    bar_code: '',
    supplier_id: 0,
    supplier_name: '',
    brand: '',
    origin: '',
    length_unit: 1,
    volume_unit: 0,
    weight_unit: 1,
    detailList: []
  }
})

const method = reactive({
  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.getCompanyList()
  },
  // Find Data by Pagination
  getCompanyList: async () => {
    const { data: res } = await getSpuList(data.tablePage)
    if (!res.isSuccess) {
      hookComponent.$message({
        type: 'error',
        content: res.errorMessage
      })
      return
    }
    data.tableData = []
    for (const item of res.data.rows) {
      item.tree_id = item.id
      data.tableData.push(item)
      for (const ditem of item.detailList) {
        ditem.tree_id = 0
        ditem.parent_id = item.id
        ditem.length_unit = item.length_unit
        ditem.volume_unit = item.volume_unit
        ditem.weight_unit = item.weight_unit
        data.tableData.push(ditem)
      }
    }
    data.tablePage.total = res.data.totals
  },
  // Add user
  add: () => {
    data.dialogForm = {
      id: 0,
      spu_code: '',
      spu_name: '',
      spu_description: '',
      bar_code: '',
      brand: '',
      length_unit: 1,
      volume_unit: 0,
      weight_unit: 1,
      detailList: []
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
  editRow(row: CommodityVO) {
    data.dialogForm = JSON.parse(JSON.stringify(row))
    data.showDialog = true
  },
  deleteRow(row: CommodityVO) {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteMessage'),
      handleConfirm: async () => {
        if (row.id) {
          const { data: res } = await deleteSpu(row.id)
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
      filename: i18n.global.t('router.sideBar.commodityManagement'),
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
