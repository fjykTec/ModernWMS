<template>
  <v-dialog v-model="isShow" width="20%" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('login.register')}`"></v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
            <v-text-field
              v-model="data.form.user_name"
              :label="$t('base.userManagement.user_register_name')"
              :rules="data.rules.user_name"
              variant="outlined"
            ></v-text-field>
            <v-text-field
              v-model="data.form.auth_string"
              :label="$t('base.userManagement.auth_string')"
              :rules="data.rules.auth_string"
              variant="outlined"
              :append-icon="data.isShowPassword ? 'mdi-eye' : 'mdi-eye-off'"
              :type="data.isShowPassword ? 'text' : 'password'"
              @click:append="data.isShowPassword = !data.isShowPassword"
            ></v-text-field>
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
              v-model="data.form.email"
              :label="$t('base.userManagement.email')"
              :rules="data.rules.email"
              variant="outlined"
            ></v-text-field>
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
import { Md5 } from 'ts-md5'
import { UserVO } from '@/types/Base/UserManagement'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { registerUser } from '@/api/base/userManagement'

const formRef = ref()
const emit = defineEmits(['close', 'saveSuccess'])

const props = defineProps<{
  showDialog: boolean
  form: UserVO
}>()

const isShow = computed(() => props.showDialog)

const data = reactive({
  isShowPassword: false,
  form: ref<UserVO>({
    id: 0,
    user_num: '',
    user_name: '',
    auth_string: '',
    email: '',
    sex: '',
    is_valid: true
  }),
  rules: {
    user_num: [],
    user_name: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.userManagement.user_register_name') }!`
    ],
    auth_string: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.userManagement.auth_string') }!`],
    email: [(val: string) => method.verifyMailbox(val)],
    sex: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.userManagement.sex') }!`],
    contact_tel: [],
    is_valid: []
  },
  combobox: ref<{
    sex: {
      label: string
      value: string
    }[]
  }>({
    sex: []
  })
})

const method = reactive({
  // Verify mailbox
  verifyMailbox: (val: string) => {
    if (!val) {
      return true
    }
    const RE = new RegExp(
      /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/
    )
    if (RE.test(val)) {
      return true
    }
    return i18n.global.t('system.tips.vaildEmail')
  },
  // Get the options required by the drop-down box
  getCombobox: () => {
    // Static drop-down box
    const sexOptions = ['male', 'female']
    data.combobox.sex = []
    for (const sex of sexOptions) {
      data.combobox.sex.push({
        label: i18n.global.t(`system.combobox.sex.${ sex }`),
        value: sex
      })
    }
  },
  closeDialog: () => {
    emit('close')
  },
  submit: async () => {
    const { valid } = await formRef.value.validate()

    if (valid) {
      const form = {
        id: 0,
        user_num: '',
        user_name: data.form.user_name,
        auth_string: Md5.hashStr(data.form.auth_string as string),
        email: data.form.email,
        sex: data.form.sex,
        is_valid: true
      }

      const { data: res } = await registerUser(form)
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
