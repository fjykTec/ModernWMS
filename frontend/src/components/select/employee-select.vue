<!-- Select Modal -->
<template>
  <v-dialog v-model="data.showDialog" width="70%" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('base.userManagement.userModalTitle')}`"></v-toolbar>
        <v-card-text>
          <v-row>
            <v-col cols="3">
              <div class="searchForm" :style="{ height: searchFormHeight }">
                <tooltip-btn icon="mdi-refresh" size="x-small" :tooltip-text="$t('system.page.reset')" @click="method.resetForm"></tooltip-btn>
                <tooltip-btn icon="mdi-magnify" size="x-small" :tooltip-text="$t('system.page.confirm')" @click="method.sureSearch"></tooltip-btn>
                <v-form ref="formRef" class="mt-4">
                  <v-text-field
                    v-model="data.searchForm.user_num"
                    :label="$t('base.userManagement.user_num')"
                    variant="outlined"
                    density="compact"
                    hide-details
                    clearable
                    class="mb-4"
                  >
                  </v-text-field>
                  <v-text-field
                    v-model="data.searchForm.user_name"
                    :label="$t('base.userManagement.user_name')"
                    variant="outlined"
                    density="compact"
                    hide-details
                    clearable
                    class="mb-4"
                  >
                  </v-text-field>
                  <v-text-field
                    v-model="data.searchForm.user_role"
                    :label="$t('base.userManagement.user_role')"
                    variant="outlined"
                    density="compact"
                    hide-details
                    clearable
                    class="mb-4"
                  >
                  </v-text-field>
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
import tooltipBtn from '@/components/tooltip-btn.vue'
import { PAGE_SIZE, PAGE_LAYOUT, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { SearchObject } from '@/types/System/Form'
import { computedSelectTableSearchHeight, SYSTEM_HEIGHT } from '@/constant/style'
import { DEBOUNCE_TIME } from '@/constant/system'
import { setSearchObject } from '@/utils/common'
import customPager from '@/components/custom-pager.vue'
import i18n from '@/languages/i18n'
import { getUserList } from '@/api/base/userManagement'

const emit = defineEmits(['sureSelect'])
const xTable = ref()

const data = reactive({
  showDialog: false as boolean,
  tableData: [],
  tablePage: reactive({
    total: 0,
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE,
    searchObjects: ref<Array<SearchObject>>([])
  }),
  searchForm: {
    user_num: '',
    user_name: '',
    user_role: ''
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

  openDialog: () => {
    method.getList()

    data.showDialog = true
  },
  closeDialog: () => {
    data.showDialog = false
  }
})

defineExpose({
  openDialog: method.openDialog,
  closeDialog: method.closeDialog
})

const searchFormHeight = computed(() => computedSelectTableSearchHeight({}))

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
