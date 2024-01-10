<template>
  <v-dialog v-model="data.showDialog" width="400px" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('system.page.setSearch')}`"></v-toolbar>
        <v-card-text>
          <vxe-table
            ref="xTable"
            :checkbox-config="{ checkRowKeys: data.actions }"
            :data="data.searchList"
            :radio-config="{ labelField: 'value' }"
            :row-config="{ isHover: true, keyField: 'value' }"
            align="center"
            row-id="value"
            max-height="500"
            @checkbox-all="method.selectAllEvent"
            @checkbox-change="method.selectChangeEvent"
          >
            <vxe-column type="checkbox" width="60"></vxe-column>
            <vxe-column :title="$t('base.roleMenu.modalTitle.actionTitle')" field="label"></vxe-column>
          </vxe-table>
        </v-card-text>

        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="method.closeDialog()">{{ $t('system.page.close') }}</v-btn>
          <v-btn color="primary" variant="text" @click="method.submit">{{ $t('system.page.submit') }}</v-btn>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>
</template>

<script setup lang="tsx">
import { reactive, ref } from 'vue'
import i18n from '@/languages/i18n'
import searchSetting from '@/constant/searchSettingSet'
import { setStorage, getStorage } from '@/utils/common'
import { hookComponent } from '@/components/system'

const emit = defineEmits(['success'])

const xTable = ref()

const props = defineProps<{
  options: string[]
  i18nKey: string
}>()

const data = reactive({
  showDialog: false as boolean,
  actions: [] as string[],
  searchList: [] as {
    label: string
    value: string
  }[],
  menu_name: '' as string
})

const method = reactive({
  openDialog: (menu_name: string) => {
    data.menu_name = menu_name

    // 获取当前路由的所有搜索项
    data.searchList = searchSetting[data.menu_name].list.map((item: { type: string; name: string }) => ({
      label: i18n.global.t(`${ props.i18nKey }.${ item.name }`),
      value: item.name
    }))

    // 赋值初始值
    data.actions = [...props.options]

    data.showDialog = true
  },
  closeDialog: () => {
    data.showDialog = false
  },
  submit: () => {
    if (data.actions.length > 3 || data.actions.length === 0) {
      hookComponent.$message({
        type: 'error',
        content: `${ i18n.global.t('system.tips.setSearchError') }`
      })
      return
    }

    let storage = getStorage('menu_search_setting')

    if (storage) {
      storage[data.menu_name] = data.actions
    } else {
      storage = {
        [data.menu_name]: data.actions
      }
    }

    setStorage('menu_search_setting', storage)

    emit('success')

    method.closeDialog()
  },
  selectAllEvent: ({ checked }) => {
    checked ? (data.actions = data.searchList.map((item) => item.value)) : (data.actions = [])
  },
  selectChangeEvent: () => {
    const records = xTable.value.getCheckboxRecords()
    data.actions = records.map((item) => item.value)
  }
})

defineExpose({
  openDialog: method.openDialog,
  closeDialog: method.closeDialog
})
</script>

<style scoped lang="less"></style>
