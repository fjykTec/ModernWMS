<template>
  <v-dialog v-model="data.showDialog" :persistent="true" :width="'30%'" transition="dialog-top-transition">
    <template #default>
      <v-card class="formCard">
        <v-toolbar :title="`${$t('base.roleMenu.modalTitle.editMenuAction')}`" color="white"></v-toolbar>
        <v-card-text>
          <!--          <v-form ref="formRef">-->
          <!--            <v-select-->
          <!--              v-model="data.actions"-->
          <!--              :items="currentActionList"-->
          <!--              :menu-props="{ maxHeight: 400 }"-->
          <!--              item-title="label"-->
          <!--              item-value="value"-->
          <!--              :label="$t('base.roleMenu.modalTitle.actionTitle')"-->
          <!--              variant="outlined"-->
          <!--              chips-->
          <!--              multiple-->
          <!--              clearable-->
          <!--            >-->
          <!--            </v-select>-->
          <!--          </v-form>-->
          <vxe-table
            ref="xTable"
            :checkbox-config="{checkRowKeys: data.actions}"
            :data="currentActionList"
            :radio-config="{labelField: 'value'}"
            :row-config="{isHover: true, keyField:'value'}"
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
          <v-btn variant="text" @click="method.closeDialog">{{ $t('system.page.close') }}</v-btn>
          <v-btn color="primary" variant="text" @click="method.saveSuccess">{{ $t('system.page.submit') }}</v-btn>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>
</template>

<script lang="tsx" setup>
import { computed, reactive, ref } from 'vue'
import { actionDict, getActionName } from './actionList'
import { RoleMenuDetailVo } from '@/types/Base/RoleMenu'

const emit = defineEmits(['close', 'saveSuccess'])

const props = defineProps<{
  row: RoleMenuDetailVo
}>()

const currentActionList = computed(() => {
  if (props.row.menu_name && Object.hasOwn(actionDict, props.row.menu_name)) {
    return actionDict[props.row.menu_name].map((item: string) => ({
      label: getActionName(item, props.row.menu_name),
      value: item
    }))
  }
  return []
})

const xTable = ref()

const data = reactive({
  actions: [] as string[],
  showDialog: false as boolean
})

const method = reactive({
  openDialog: (cache: string[]) => {
    data.actions = []
    data.actions = cache
    data.showDialog = true
  },
  closeDialog: () => {
    data.showDialog = false
  },
  saveSuccess: () => {
    emit('saveSuccess', data.actions)
  },
  selectAllEvent: ({ checked }) => {
    checked
      ? data.actions = currentActionList.value.map(item => item.value)
      : data.actions = []
  },
  selectChangeEvent: () => {
    const records = xTable.value.getCheckboxRecords()
    data.actions = records.map(item => item.value)
  }
})

defineExpose({
  openDialog: method.openDialog,
  closeDialog: method.closeDialog
})
</script>

<style lang="less" scoped></style>
