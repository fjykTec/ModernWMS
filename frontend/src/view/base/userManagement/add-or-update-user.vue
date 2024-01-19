<template>
  <v-dialog v-model="isShow" :width="'30%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('router.sideBar.userManagement')}`"></v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
            <v-text-field
              v-model="data.form.user_num"
              :label="$t('base.userManagement.user_num')"
              :rules="data.rules.user_num"
              variant="outlined"
            ></v-text-field>
            <v-text-field
              v-model="data.form.user_name"
              :label="$t('base.userManagement.user_name')"
              :rules="data.rules.user_name"
              variant="outlined"
            ></v-text-field>
            <v-select
              v-model="data.form.user_role"
              :items="data.combobox.user_role"
              :rules="data.rules.user_role"
              :label="$t('base.userManagement.user_role')"
              variant="outlined"
              clearable
            ></v-select>
            <v-select
              v-model="data.form.sex"
              :items="data.combobox.sex"
              item-title="label"
              item-value="value"
              :rules="data.rules.sex"
              :label="$t('base.userManagement.sex')"
              variant="outlined"
              clearable
            ></v-select>
            <v-text-field
              v-model="data.form.contact_tel"
              :label="$t('base.userManagement.contact_tel')"
              :rules="data.rules.contact_tel"
              variant="outlined"
            ></v-text-field>
            <v-switch
              v-model="data.form.is_valid"
              color="primary"
              :label="$t('base.userManagement.is_valid')"
              :rules="data.rules.is_valid"
            ></v-switch>
          </v-form>
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
import { reactive, computed, ref, watch } from 'vue'
import { UserVO } from '@/types/Base/UserManagement'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { addUser, updateUser, getSelectItem } from '@/api/base/userManagement'
import { StringLength } from '@/utils/dataVerification/formRule'
import { removeObjectNull } from '@/utils/common'

const formRef = ref()
const emit = defineEmits(['close', 'saveSuccess'])

const props = defineProps<{
  showDialog: boolean
  form: UserVO
}>()

const isShow = computed(() => props.showDialog)

const dialogTitle = computed(() => {
  if (props.form.id && props.form.id > 0) {
    return 'update'
  }
  return 'add'
})

const data = reactive({
  form: ref<UserVO>({
    id: 0,
    user_num: '',
    user_name: '',
    contact_tel: '',
    is_valid: true
  }),
  rules: {
    user_num: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.userManagement.user_num') }!`,
      (val: string) => StringLength(val, 0, 128) === '' || StringLength(val, 0, 128)
    ],
    user_name: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.userManagement.user_name') }!`,
      (val: string) => StringLength(val, 0, 128) === '' || StringLength(val, 0, 128)
    ],
    user_role: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.userManagement.user_role') }!`],
    sex: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.userManagement.sex') }!`],
    contact_tel: [(val: string) => StringLength(val, 0, 128) === '' || StringLength(val, 0, 64)],
    is_valid: []
  },
  combobox: ref<{
    sex: {
      label: string
      value: string
    }[]
    user_role: string[]
  }>({
    sex: [],
    user_role: []
  })
})

const method = reactive({
  // Get the options required by the drop-down box
  getCombobox: async () => {
    // Static drop-down box
    const sexOptions = ['male', 'female']
    data.combobox.sex = []
    for (const sex of sexOptions) {
      data.combobox.sex.push({
        label: i18n.global.t(`system.combobox.sex.${ sex }`),
        value: sex
      })
    }
    // Dynamic drop-down box
    data.combobox.user_role = []
    const { data: res } = await getSelectItem()
    if (!res.isSuccess) {
      return
    }
    for (const item of res.data) {
      switch (item.code) {
        case 'user_role':
          data.combobox.user_role.push(item.name)
          break
      }
    }
  },
  closeDialog: () => {
    emit('close')
  },
  submit: async () => {
    const { valid } = await formRef.value.validate()
    if (valid) {
      const form = removeObjectNull(data.form)
      const { data: res } = dialogTitle.value === 'add' ? await addUser(form) : await updateUser(form)
      if (!res.isSuccess) {
        hookComponent.$message({
          type: 'error',
          content: res.errorMessage
        })
        return
      }
      if (dialogTitle.value === 'add') {
        hookComponent.$dialog({
          content: i18n.global.t('base.userManagement.addSuccessTip') + res.errorMessage,
          handleConfirm: async () => {}
        })
      } else {
        hookComponent.$message({
          type: 'success',
          content: `${ i18n.global.t('system.page.submit') }${ i18n.global.t('system.tips.success') }`
        })
      }
      emit('saveSuccess')
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('system.checkText.checkFormFail')
      })
    }
  }
})

watch(
  () => isShow.value,
  (val) => {
    if (val) {
      method.getCombobox()
      data.form = props.form
    }
  }
)
</script>

<style scoped lang="less">
.v-form {
  div {
    margin-bottom: 7px;
  }
}
</style>
