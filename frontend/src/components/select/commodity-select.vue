<!-- Select Modal -->
<template>
  <v-dialog v-model="isShow" width="70%" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('wms.stock.stockSelectModal')}`"></v-toolbar>
        <v-card-text>
          <v-row>
            <v-col cols="3">
              <div class="searchForm" :style="{ height: searchFormHeight }">
                <tooltip-btn icon="mdi-refresh" size="x-small" :tooltip-text="$t('system.page.reset')" @click="method.resetForm"></tooltip-btn>
                <tooltip-btn icon="mdi-magnify" size="x-small" :tooltip-text="$t('system.page.confirm')" @click="method.sureSearch"></tooltip-btn>
                <v-form ref="formRef" class="mt-4">
                  <v-text-field
                    v-model="data.searchForm.warehouse_name"
                    :label="$t('wms.stock.warehouse')"
                    variant="outlined"
                    density="compact"
                    hide-details
                    clearable
                    class="mb-4"
                  ></v-text-field>
                  <v-text-field
                    v-model="data.searchForm.location_name"
                    :label="$t('wms.stock.location_name')"
                    variant="outlined"
                    density="compact"
                    hide-details
                    clearable
                    class="mb-4"
                  ></v-text-field>
                  <v-text-field
                    v-model="data.searchForm.spu_name"
                    :label="$t('base.commodityManagement.spu_name')"
                    variant="outlined"
                    density="compact"
                    hide-details
                    clearable
                    class="mb-4"
                  ></v-text-field>
                  <v-text-field
                    v-model="data.searchForm.sku_code"
                    :label="$t('base.commodityManagement.sku_code')"
                    variant="outlined"
                    density="compact"
                    hide-details
                    clearable
                    class="mb-4"
                  ></v-text-field>
                </v-form>
              </div>
            </v-col>
            <v-col cols="9">
              <div class="dataTable">
                <vxe-table
                  ref="xTable"
                  :column-config="{ minWidth: '100px' }"
                  :data="data.tableData"
                  :height="SYSTEM_HEIGHT.SELECT_TABLE"
                  align="center"
                >
                  <template #empty>
                    {{ i18n.global.t('system.page.noData') }}
                  </template>
                  <vxe-column type="seq" width="60"></vxe-column>
                  <vxe-column type="checkbox" width="50"></vxe-column>
                  <vxe-column field="warehouse_name" :title="$t('wms.stock.warehouse')"></vxe-column>
                  <vxe-column field="location_name" :title="$t('wms.stock.location_name')"></vxe-column>
                  <vxe-column field="goods_owner_name" :title="$t('base.ownerOfCargo.goods_owner_name')"></vxe-column>
                  <vxe-column field="spu_code" :title="$t('base.commodityManagement.spu_code')"></vxe-column>
                  <vxe-column field="spu_name" :title="$t('base.commodityManagement.spu_name')"></vxe-column>
                  <vxe-column field="sku_code" :title="$t('base.commodityManagement.sku_code')"></vxe-column>
                  <vxe-column field="sku_name" :title="$t('base.commodityManagement.sku_name')"></vxe-column>
                  <vxe-column field="series_number" :title="$t('wms.stockLocation.series_number')"></vxe-column>
                  <vxe-column field="qty_available" :title="$t('wms.stock.qty_available')"></vxe-column>
                  <vxe-column field="price" :title="$t('wms.stockAsnInfo.price')"></vxe-column>
                  <vxe-date-column field="expiry_date" :title="$t('wms.stockAsnInfo.expiry_date')"> </vxe-date-column>
                  <vxe-date-column field="putaway_date" :title="$t('wms.stockAsnInfo.putaway_date')"> </vxe-date-column>
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
            </v-col>
          </v-row>
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="method.closeDialog">{{ $t('system.page.close') }}</v-btn>
          <v-btn color="primary" variant="text" @click="method.submit">{{ $t('system.page.confirm') }}</v-btn>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>
</template>

<script lang="ts" setup>
/**
 * @description Select Modal
 * @example 
 * <xxx-select :show-dialog="data.showDialogSelect" @close="method.closeDialogSelect" @sureSelect="method.sureSelect" />
 *
 * openSelect: () => {
    data.showDialogSelect = true
  },
  closeDialogSelect: () => {
    data.showDialogSelect = false
  },
 */
import { reactive, computed, ref, watch } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import { hookComponent } from '@/components/system/index'
import { getStockSelectList } from '@/api/wms/stockManagement'
import tooltipBtn from '@/components/tooltip-btn.vue'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { SearchObject } from '@/types/System/Form'
import { computedSelectTableSearchHeight, SYSTEM_HEIGHT } from '@/constant/style'
import { DEBOUNCE_TIME } from '@/constant/system'
import { setSearchObject } from '@/utils/common'
import customPager from '@/components/custom-pager.vue'
import i18n from '@/languages/i18n'

const emit = defineEmits(['close', 'sureSelect'])
const xTable = ref()

const props = defineProps<{
  showDialog: boolean
  sqlTitle?: string
}>()

const isShow = computed(() => props.showDialog)

const data = reactive({
  tableData: [],
  tablePage: reactive({
    total: 0,
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE,
    searchObjects: ref<Array<SearchObject>>([])
  }),
  searchForm: {
    location_name: '',
    warehouse_name: '',
    spu_name: '',
    sku_code: ''
  },
  timer: ref<any>(null)
})

const method = reactive({
  handlePageChange: ref<VxePagerEvents.PageChange>(({ currentPage, pageSize }) => {
    data.tablePage.pageIndex = currentPage
    data.tablePage.pageSize = pageSize

    method.getList()
  }),

  sureSearch: () => {
    data.tablePage.searchObjects = setSearchObject(data.searchForm)
    method.getList()
  },

  getList: async () => {
    const { data: res } = await getStockSelectList({
      ...data.tablePage,
      sqlTitle: props.sqlTitle
    })
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

  resetForm: () => {
    // Clear search input
    for (const key in data.searchForm) {
      const str = key as string
      data.searchForm[str as keyof typeof data.searchForm] = ''
    }

    // Clear search params
    data.tablePage.searchObjects = []
  },

  submit: () => {
    const checkRecords = xTable.value.getCheckboxRecords()
    emit('sureSelect', checkRecords)
    method.closeDialog()
  },

  closeDialog: () => {
    emit('close')
  }
})

const searchFormHeight = computed(() => computedSelectTableSearchHeight({}))

watch(
  () => isShow.value,
  (newVal) => {
    newVal && method.getList()
  }
)

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
.searchForm {
  border: 1px solid #eee;
  box-sizing: border-box;
  padding: 20px;
  overflow: auto;
}
</style>
