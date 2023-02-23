<template>
  <v-dialog v-model="data.isShow" :width="'30%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card class="formCard">
        <v-toolbar color="white" :title="`${$t('system.homeHeader.changePwd')}`"></v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
            <v-text-field
              v-model="data.form.old_password"
              :label="$t('system.homeHeader.oldPwd')"
              :rules="data.rules.old_password"
              variant="outlined"
              :autocomplete="false"
              :append-inner-icon="data.showPassword_old ? 'mdi-eye' : 'mdi-eye-off'"
              :type="data.showPassword_old ? 'text' : 'password'"
              @click:append-inner="method.handleShowPassword('old')"
            ></v-text-field>
            <v-text-field
              v-model="data.form.new_password"
              :label="$t('system.homeHeader.newPwd')"
              :rules="data.rules.new_password"
              variant="outlined"
              :autocomplete="false"
              :append-inner-icon="data.showPassword_new ? 'mdi-eye' : 'mdi-eye-off'"
              :type="data.showPassword_new ? 'text' : 'password'"
              @click:append-inner="method.handleShowPassword('new')"
            ></v-text-field>
            <v-text-field
              v-model="data.form.confirm_new_password"
              :label="$t('system.homeHeader.confrimNewPwd')"
              :rules="data.rules.confirm_new_password"
              variant="outlined"
              :autocomplete="false"
              :append-inner-icon="data.showPassword_confirm ? 'mdi-eye' : 'mdi-eye-off'"
              :type="data.showPassword_confirm ? 'text' : 'password'"
              @click:append-inner="method.handleShowPassword('confirm')"
            ></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="method.close">{{ $t('system.page.close') }}</v-btn>
          <v-btn color="primary" variant="text" @click="method.submit">{{ $t('system.page.submit') }}</v-btn>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>
</template>

<script lang="ts" setup>
import { reactive, ref } from 'vue'
import { Md5 } from 'ts-md5'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { changePassword } from '@/api/base/userManagement'
import { store } from '@/store'

const formRef = ref()

const data = reactive({
  isShow: false,
  showPassword_old: false,
  showPassword_new: false,
  showPassword_confirm: false,
  form: {
    old_password: '',
    new_password: '',
    confirm_new_password: ''
  },
  rules: {
    old_password: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('system.homeHeader.oldPwd') }!`],
    new_password: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('system.homeHeader.newPwd') }!`],
    confirm_new_password: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('system.homeHeader.confrimNewPwd') }!`,
      (val: string) => method.verifyPwd(val)
    ]
  }
})

const method = reactive({
  handleShowPassword: (flag: string) => {
    switch (flag) {
      case 'old':
        data.showPassword_old = !data.showPassword_old
        break
      case 'new':
        data.showPassword_new = !data.showPassword_new
        break
      case 'confirm':
        data.showPassword_confirm = !data.showPassword_confirm
        break
    }
  },
  open: () => {
    data.form = {
      old_password: '',
      new_password: '',
      confirm_new_password: ''
    }
    data.isShow = true
  },
  close: () => {
    data.isShow = false
  },
  submit: async () => {
    const { valid } = await formRef.value.validate()
    if (valid) {
      const user = store.getters['user/userInfo']
      const form = JSON.parse(
        JSON.stringify({
          id: user.user_id,
          old_password: Md5.hashStr(data.form.old_password),
          new_password: Md5.hashStr(data.form.new_password)
        })
      )
      const { data: res } = await changePassword(form)
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
      // Get remember username and password
      const rememberJSON = localStorage.getItem('userLoginInfo')
      if (rememberJSON) {
        const rememberJSON = JSON.stringify({
          userName: window.btoa(encodeURIComponent(user.user_name)),
          password: window.btoa(encodeURIComponent(data.form.new_password))
        })
        localStorage.setItem('userLoginInfo', rememberJSON)
      }
      method.close()
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('system.checkText.checkFormFail')
      })
    }
  },
  // Verify whether the passwords are the same
  verifyPwd: (val: string) => {
    if (val !== data.form.new_password) {
      return i18n.global.t('system.tips.verifyPwd')
    }
    return true
  }
})

defineExpose({
  open: method.open,
  close: method.close
})
</script>

<style scoped lang="less">
.v-form {
  div {
    margin-bottom: 7px;
  }
}
</style>
