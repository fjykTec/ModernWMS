<!-- Select Modal -->
<template>
  <v-dialog v-model="isShow" width="70%" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('base.freightSetting.freightSelectModal')}`"></v-toolbar>
        <v-card-text>
          <v-row>
            <v-col cols="3">
              <div class="searchForm" :style="{ height: searchFormHeight }">
                <tooltip-btn icon="mdi-refresh" size="x-small" :tooltip-text="$t('system.page.reset')" @click="method.resetForm"></tooltip-btn>
                <tooltip-btn icon="mdi-magnify" size="x-small" :tooltip-text="$t('system.page.confirm')" @click="method.sureSearch"></tooltip-btn>
                <v-form ref="formRef" class="mt-4">
                  <v-text-field
                    v-model="data.searchForm.carrier"
                    :label="$t('base.freightSetting.carrier')"
                    variant="outlined"
                    density="compact"
                    hide-details
                    clearable
                    class="mb-4"
                  ></v-text-field>
                  <v-text-field
                    v-model="data.searchForm.departure_city"
                    :label="$t('base.freightSetting.departure_city')"
                    variant="outlined"
                    density="compact"
                    hide-details
                    clearable
                    class="mb-4"
                  ></v-text-field>
                  <v-text-field
                    v-model="data.searchForm.arrival_city"
                    :label="$t('base.freightSetting.arrival_city')"
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
                  <vxe-column field="carrier" :title="$t('base.freightSetting.carrier')"></vxe-column>
                  <vxe-column field="departure_city" :title="$t('base.freightSetting.departure_city')"></vxe-column>
                  <vxe-column field="arrival_city" :title="$t('base.freightSetting.arrival_city')"></vxe-column>
                  <vxe-column field="price_per_weight" :title="$t('base.freightSetting.price_per_weight')"></vxe-column>
                  <vxe-column field="price_per_volume" :title="$t('base.freightSetting.price_per_volume')"></vxe-column>
                  <vxe-column field="min_payment" :title="$t('base.freightSetting.min_payment')"></vxe-column>
                  <vxe-column field="creator" :title="$t('base.freightSetting.creator')"></vxe-column>
                  <vxe-column field="create_time" width="170px" :title="$t('base.freightSetting.create_time')"></vxe-column>
                  <vxe-column field="is_valid" :title="$t('base.warehouseSetting.is_valid')">
                    <template #default="{ row, column }">
                      <span>{{ formatIsValid(row[column.property]) }}</span>
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
import { getFreightList } from '@/api/base/freightSetting'
import tooltipBtn from '@/components/tooltip-btn.vue'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { formatIsValid } from '@/utils/format/formatSystem'
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
    carrier: '',
    departure_city: '',
    arrival_city: ''
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
