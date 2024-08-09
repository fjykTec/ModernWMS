<template>
  <v-dialog v-model="data.showDialog" width="80%" height="80%" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('system.homeHeader.log')}`"></v-toolbar>
        <v-card-text>
          <v-row>
            <v-col :cols="3">
              <v-card :height="searchFormHeight">
                <NavListVue
                  :list-data="data.menuList"
                  :title="data.navListOptions.title"
                  :label-key="data.navListOptions.labelKey"
                  :index-key="data.navListOptions.indexKey"
                  :index-value="data.navListOptions.indexValue"
                  @item-click="method.navListClick"
                />
              </v-card>
            </v-col>
            <v-col :cols="9">
              <v-row justify="end" @keyup.enter="method.sureSearch">
                <v-col :cols="4">
                  <v-text-field
                    v-model="data.searchForm.user_name"
                    :label="$t('system.viewLog.user_name')"
                    variant="outlined"
                    density="compact"
                    hide-details
                    clearable
                    class="mb-4"
                  >
                  </v-text-field>
                </v-col>
              </v-row>

              <vxe-table ref="xTable" :column-config="{ minWidth: '100px' }" align="center" :height="tableHeight" :data="data.tableData">
                <template #empty>
                  {{ i18n.global.t('system.page.noData') }}
                </template>
                <vxe-column type="seq" width="60"></vxe-column>
                <vxe-column field="user_name" width="140" :title="$t('system.viewLog.user_name')"></vxe-column>
                <vxe-column field="action_content" :title="$t('system.viewLog.action_content')"></vxe-column>
                <vxe-date-column field="action_time" :title="$t('system.viewLog.action_time')" format="yyyy-MM-dd HH:mm" width="170px">
                </vxe-date-column>
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
            </v-col>
          </v-row>
        </v-card-text>

        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="method.closeDialog()">{{ $t('system.page.close') }}</v-btn>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>
</template>

<script setup lang="tsx">
import { reactive, computed, ref, onMounted, watch } from 'vue'
import { VxePagerEvents } from 'vxe-table'
import i18n from '@/languages/i18n'
import { computedSelectTableSearchHeight } from '@/constant/style'
import { getActionLog } from '@/api/sys/system'
import { DEFAULT_PAGE_SIZE, PAGE_SIZE, PAGE_LAYOUT } from '@/constant/vxeTable'
import { hookComponent } from '.'
import { PageConfigProps } from '@/types/System/Form'
import customPager from '@/components/custom-pager.vue'
import { actionDict } from '@/view/base/roleMenu/actionList'
import NavListVue from '@/components/page/nav-list.vue'
import { setSearchObject } from '@/utils/common'
import { DEBOUNCE_TIME } from '@/constant/system'

const menuList = [] as any[]

Object.keys(actionDict).forEach((key) => {
  // if (key === 'companySetting') {
  //   menuList.push({ type: 'subheader', title: i18n.global.t('router.sideBar.baseModule') })
  // } else if (key === 'stockAsn') {
  //   menuList.push({ type: 'subheader', title: i18n.global.t('router.sideBar.stockAsn') })
  // } else if (key === 'stockManagement') {
  //   menuList.push({ type: 'subheader', title: i18n.global.t('router.sideBar.statisticAnalysis') })
  // } else if (key === 'warehouseProcessing') {
  //   menuList.push({ type: 'subheader', title: i18n.global.t('router.sideBar.warehouseWorkingModule') })
  // } else if (key === 'deliveryManagement') {
  //   menuList.push({ type: 'subheader', title: i18n.global.t('router.sideBar.deliveryManagement') })
  // }

  const obj = {
    title: i18n.global.t(`router.sideBar.${ key }`),
    value: key
  }

  menuList.push(obj)
})

const data = reactive({
  showDialog: false as boolean,
  menuList: menuList as any[],
  tablePage: {
    total: 0,
    pageIndex: 1,
    pageSize: DEFAULT_PAGE_SIZE,
    searchObjects: []
  } as PageConfigProps,
  tableData: [] as {
    id: number
    vue_path: string
    user_name: string
    action_content: string
    action_time: string
  }[],
  // Menu Search Object
  menuSearchObject: {} as any,
  // Fuzzy search object
  likeSearchObject: [] as any[],
  // Navigation List Configuration
  navListOptions: {
    title: i18n.global.t('base.roleMenu.menu_name'),
    labelKey: 'title',
    indexKey: 'value',
    indexValue: ''
  },
  // query form
  searchForm: {
    user_name: ''
  },
  // anti-shake
  timer: ref<any>(null)
})

const searchFormHeight = computed(() => computedSelectTableSearchHeight({}))
const tableHeight = computed(() => {
  let height: any = computedSelectTableSearchHeight({})

  height = height.split('px')[0]

  return `${ height - 74 }px`
})

onMounted(() => {
  const menu = data.menuList.find((item) => !item.type)

  // The first menu is selected by default
  if (menu) {
    method.navListClick(menu)
  }
})

const method = reactive({
  // Click navList
  navListClick: (item: { title: string; value: string }) => {
    data.navListOptions.indexValue = String(item.value)

    data.menuSearchObject = {
      name: 'vue_path',
      operator: 1,
      text: item.value,
      value: item.value
    }

    method.list()
  },
  list: async () => {
    const { data: res } = await getActionLog({ ...data.tablePage, searchObjects: [data.menuSearchObject, ...data.likeSearchObject] })
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
  openDialog: () => {
    data.showDialog = true

    method.list()
  },
  closeDialog: () => {
    data.showDialog = false
  },
  handlePageChange: ref<VxePagerEvents.PageChange>(({ currentPage, pageSize }) => {
    data.tablePage.pageIndex = currentPage
    data.tablePage.pageSize = pageSize
    method.list()
  }),
  sureSearch: () => {
    data.likeSearchObject = setSearchObject(data.searchForm)

    method.list()
  }
})

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
  openDialog: method.openDialog,
  closeDialog: method.closeDialog
})
</script>

<style scoped lang="less">
.searchForm {
  // box-sizing: border-box;
  box-shadow: rgb(153, 153, 153) 0px 0px 10px;
  border-radius: 5px;
  overflow: auto;
  &::-webkit-scrollbar {
    width: 6px;
  }

  &::-webkit-scrollbar-thumb {
    background-color: #c6c6c6;
    border-radius: 10px;
  }

  &::-webkit-scrollbar-thumb:hover {
    background-color: #a8a8a8;
  }

  &::-webkit-scrollbar-button {
    display: none;
  }
}
</style>
