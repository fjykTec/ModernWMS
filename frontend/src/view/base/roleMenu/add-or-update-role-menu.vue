<template>
  <v-dialog v-model="isShow" :persistent="true" :width="'30%'" transition="dialog-top-transition">
    <template #default>
      <v-card class="formCard">
        <v-toolbar :title="`${$t('router.sideBar.roleMenu')}`" color="white"></v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
            <v-select
              v-model="data.form.userrole_id"
              :disabled="data.dialogTitle === 'update'"
              :items="data.combobox.role_name"
              :label="$t('base.roleMenu.role_name')"
              :rules="data.rules.role_name"
              clearable
              item-title="label"
              item-value="value"
              variant="outlined"
            ></v-select>
          </v-form>
          <vxe-table
            ref="xTable"
            :checkbox-config="{checkRowKeys: data.menusSelectedList}"
            :data="data.combobox.menu_name"
            :radio-config="{labelField: 'value'}"
            :row-config="{isHover: true, keyField:'value'}"
            align="center"
            max-height="500"
            row-id="value"
            @checkbox-all="method.selectAllEvent"
            @checkbox-change="method.selectChangeEvent"
          >
            <vxe-column type="checkbox" width="60"></vxe-column>
            <vxe-column :title="$t('base.roleMenu.menu_name')" field="label"></vxe-column>
          </vxe-table>
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="method.closeDialog">{{ $t('system.page.close') }}</v-btn>
          <v-btn color="primary" variant="text" @click="method.submit">{{ $t('system.page.submit') }}</v-btn>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>
</template>

<script lang="ts" setup>
import { computed, reactive, ref, watch } from 'vue'
import { RoleMenuDetailVo, RoleMenuVO } from '@/types/Base/RoleMenu'
import { UserRoleVO } from '@/types/Base/UserRoleSetting'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { addRoleMenu, getMenus, updateRoleMenu } from '@/api/base/roleMenu'
import { getUserRoleAll } from '@/api/base/userRoleSetting'
import { MenuItem } from '@/types/System/Store'
import { actionDict } from './actionList'
import { removeObjectNull } from '@/utils/common'

const formRef = ref()
const emit = defineEmits(['close', 'saveSuccess'])

const props = defineProps<{
  showDialog: boolean
  form: RoleMenuVO
}>()
const isShow = computed(() => props.showDialog)
const xTable = ref()
const data = reactive({
  dialogTitle: '',
  form: ref<RoleMenuVO>({
    userrole_id: 0,
    role_name: '',
    detailList: []
  }),
  // Multiple drop-down box values, mainly used to temporarily save the selected values
  menusSelectedList: ref<any[]>([]),
  // If it is modified, you need to pass a negative ID to the API to delete the existing details
  removeDetailList: ref<RoleMenuDetailVo[]>([]),
  // Drop down box options
  combobox: ref<{
    role_name: {
      label: string
      value: number
    }[]
    menu_name: {
      label: string
      value: number
      menu_name: string
    }[]
  }>({
    role_name: [],
    menu_name: []
  }),
  rules: {
    role_name: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.roleMenu.role_name') }!`],
    menu_name: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.roleMenu.menu_name') }!`]
  }
})

const method = reactive({
  // Get dialog type, add or update
  getDialogType: () => {
    // This is special because this document is not actually a table,
    // but the data associated with another table.
    // so directly use the details to determine whether it is a new document
    if (props.form.detailList.length > 0) {
      data.dialogTitle = 'update'
    } else {
      data.dialogTitle = 'add'
    }
  },
  // Get the options required by the drop-down box
  getCombobox: async () => {
    data.combobox.role_name = []
    const { data: res } = await getUserRoleAll()
    if (!res.isSuccess) {
      return
    }
    const roleNameList: UserRoleVO[] = res.data
    for (const role of roleNameList) {
      data.combobox.role_name.push({
        label: role.role_name,
        value: role.id
      })
    }
    // data.combobox.menu_name = []
    const { data: menuRes } = await getMenus()
    if (!menuRes.isSuccess) {
      return
    }
    const menus: MenuItem[] = menuRes.data
    // for (const menu of menus) {
    //   data.combobox.menu_name.push({
    //     label: i18n.global.t(`router.sideBar.${ menu.menu_name }`),
    //     value: menu.id,
    //     menu_name: menu.menu_name
    //   })
    // }
    data.combobox.menu_name = menus.map(item => ({
      label: i18n.global.t(`router.sideBar.${ item.menu_name }`),
      value: item.id,
      menu_name: item.menu_name
    }))
  },
  closeDialog: () => {
    emit('close')
  },
  // Details verification before document submission
  beforeSubmit: (): boolean => {
    let flag = true
    // Process the new dropdown options
    for (const item of data.menusSelectedList) {
      if (data.form.detailList.findIndex((dl) => dl.menu_id === item) < 0) {
        data.form.detailList.push({
          id: 0,
          menu_id: item,
          menu_actions_authority: []
        })
      }
    }
    if (data.dialogTitle === 'update') {
      for (const item of data.form.detailList) {
        if (!data.menusSelectedList.includes(item.menu_id)) {
          // Delete if ID is negative
          item.id = -item.id
        }
      }
    }
    if (data.form.detailList.length === 0) {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('system.tips.detailLengthIsZero')
      })
      flag = false
    }
    return flag
  },
  submit: async () => {
    const beforeSubmitFlag = method.beforeSubmit()
    if (!beforeSubmitFlag) {
      return
    }
    const { valid } = await formRef.value.validate()
    if (valid) {
      let form = { ...data.form }

      form.detailList = form.detailList.map((item: RoleMenuDetailVo) => {
        if (!item.id || item.id === 0) {
          const index = data.combobox.menu_name.findIndex((fi: { value: number }) => fi.value === item.menu_id)

          item.menu_actions_authority = index > -1 ? actionDict[data.combobox.menu_name[index].menu_name] : []
        }
        return item
      })

      form = removeObjectNull(form)

      const { data: res } = data.dialogTitle === 'add'
        ? await addRoleMenu(form)
        : await updateRoleMenu({
          ...form,
          detailList: [...form.detailList, ...data.removeDetailList]
        }) // Merge the deleted list and the original list
      if (!res.isSuccess) {
        hookComponent.$message({
          type: 'error',
          content: res.errorMessage
        })
        return
      }
      hookComponent.$message({
        type: 'success',
        content: `${ i18n.global.t('system.page.submit') }${ i18n.global.t('system.tips.success') }`
      })
      emit('saveSuccess')
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('system.checkText.checkFormFail')
      })
    }
  },
  selectAllEvent: ({ checked }) => {
    checked
      ? data.menusSelectedList = data.combobox.menu_name.map(item => item.value)
      : data.menusSelectedList = []
  },
  selectChangeEvent: () => {
    const records = xTable.value.getCheckboxRecords()
    data.menusSelectedList = records.map(item => item.value)
  }
})

watch(
  () => props.showDialog,
  (val) => {
    if (val) {
      method.getDialogType()
      method.getCombobox()
      data.form = props.form
      data.menusSelectedList = data.form.detailList.map((item) => item.menu_id)
    }
  }
)
</script>

<style lang="less" scoped></style>
