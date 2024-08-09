<template>
  <div class="loginForm">
    <div class="titleText">
      <h5>{{ $t('login.welcomeTitle') }}</h5>
    </div>
    <div class="formContainer">
      <v-form ref="VFormRef" v-model="data.valid" lazy-validation @keydown.enter.prevent="method.login()">
        <v-text-field v-model="data.userName" required :rules="data.userNameVaildRules" :label="$t('login.userName')" variant="solo"></v-text-field>
        <v-text-field
          v-model="data.password"
          required
          :rules="data.passwordVaildRules"
          :autocomplete="false"
          :append-inner-icon="data.showPassword ? 'mdi-eye' : 'mdi-eye-off'"
          :type="data.showPassword ? 'text' : 'password'"
          :label="$t('login.password')"
          variant="solo"
          @click:append-inner="method.handleShowPassword()"
        ></v-text-field>
        <v-checkbox v-model="data.remember" :label="$t('login.rememberTips')"></v-checkbox>
        <v-btn color="purple" class="loginBtn" @click="method.login()">{{ $t('login.mainButtonLabel') }}</v-btn>
        <!-- <v-btn class="mt-2" color="#666" variant="plain" @click="method.openRegisterDialog">
          {{ i18n.global.t('login.registerTips') }}
        </v-btn> -->
      </v-form>
    </div>
    <!-- <userRegisterForm :show-dialog="data.showDialog" :form="data.dialogForm" @close="method.closeDialog" @saveSuccess="method.saveSuccess" /> -->
  </div>
</template>

<script lang="ts" setup>
import { reactive, ref, onMounted } from 'vue'
import { Md5 } from 'ts-md5'
import i18n from '@/languages/i18n'
import { login, getUserAuthority } from '@/api/sys/login'
import { store } from '@/store'
import { hookComponent } from '@/components/system'
import { router } from '@/router/index'
// import userRegisterForm from './user-register-form.vue'

// 加解密算法
function simpleEncrypt(text: string, key: string) {
  let encrypted = ''
  for (let i = 0; i < text.length; i++) {
    encrypted += String.fromCharCode(text.charCodeAt(i) ^ key.charCodeAt(i % key.length))
  }
  return encrypted
}

function simpleDecrypt(encryptedText: string, key: string) {
  return simpleEncrypt(encryptedText, key) // 因为异或运算具有对称性，加密和解密过程相同
}

// Get v-form ref
const VFormRef = ref()

const data = reactive({
  showDialog: false,
  valid: true,
  showPassword: false,
  userName: 'admin', // 240507 刘福: 默认账号 admin 1
  password: '1',
  remember: false,
  dialogForm: {
    id: 0,
    user_num: '',
    user_name: '',
    auth_string: '',
    email: '',
    // sex: '',
    is_valid: true
  },
  userNameVaildRules: [(v: string) => !!v || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('login.userName') }!`],
  passwordVaildRules: [(v: string) => !!v || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('login.password') }!`],
  encryption: 'ModernWMS2024'
})

const method = reactive({
  handleShowPassword: () => {
    data.showPassword = !data.showPassword
  },
  login: async () => {
    const { valid } = await VFormRef.value.validate()
    if (!valid) {
      return
    }
    const { data: loginRes } = await login({
      user_name: data.userName,
      password: Md5.hashStr(data.password)
    })

    if (loginRes.isSuccess) {
      const expiredTime = new Date().getTime() + loginRes.data.expire * 60 * 1000

      store.commit('user/setToken', loginRes.data.access_token)
      store.commit('user/setRefreshToken', loginRes.data.refresh_token)
      store.commit('user/setExpirationTime', expiredTime)
      store.commit('user/setEffectiveMinutes', loginRes.data.expire)
      store.commit('user/setUserInfo', loginRes.data)

      const { data: authorityRes } = await getUserAuthority(loginRes.data.userrole_id)
      if (!authorityRes.isSuccess) {
        hookComponent.$message({
          type: 'error',
          content: authorityRes.errorMessage
        })
        return
      }
      if (authorityRes.data.length <= 0) {
        hookComponent.$message({
          type: 'error',
          content: i18n.global.t('login.notAuthority')
        })
        return
      }

      const authorityList = authorityRes.data

      // test
      // authorityList.push({
      //   id: 112999,
      //   menu_name: 'test',
      //   module: 'baseModule',
      //   vue_path: 'test',
      //   vue_path_detail: '',
      //   vue_directory: 'test/test',
      //   sort: 2
      // })

      store.commit('user/setUserMenuList', authorityList)

      hookComponent.$message({
        type: 'success',
        content: i18n.global.t('login.loginSuccess')
      })

      // Remember user login info
      if (data.remember) {
        const rememberJSON = JSON.stringify({
          user_num: simpleEncrypt(data.userName, data.encryption),
          password: simpleEncrypt(data.password, data.encryption)
        })
        localStorage.setItem('userLoginInfo', rememberJSON)
      } else {
        localStorage.setItem('userLoginInfo', '')
      }

      // Jump home
      store.commit('system/setCurrentRouterPath', 'homepage')
      router.push('home')
    } else {
      hookComponent.$message({
        type: 'error',
        content: loginRes.errorMessage
      })
    }
  },
  openRegisterDialog: () => {
    data.dialogForm = {
      id: 0,
      user_num: '',
      user_name: '',
      auth_string: '',
      email: '',
      // sex: '',
      is_valid: true
    }
    data.showDialog = true
  },
  // Shut add or update dialog
  closeDialog: () => {
    data.showDialog = false
  },
  // after Add or update success.
  saveSuccess: () => {
    method.closeDialog()
  }
})

onMounted(() => {
  // Get remember username and password
  const rememberJSON = localStorage.getItem('userLoginInfo')
  if (rememberJSON) {
    const obj = JSON.parse(rememberJSON)
    data.remember = true

    try {
      data.userName = simpleDecrypt(obj.user_num, data.encryption)
      data.password = simpleDecrypt(obj.password, data.encryption)
    } catch {
      // Compatible with old encrypted data
      try {
        data.userName = decodeURIComponent(window.atob(obj.userName))
        data.password = decodeURIComponent(window.atob(obj.password))
      } catch {
        // Compatible with old encrypted data
        try {
          data.userName = window.atob(obj.userName)
          data.password = window.atob(obj.password)
        } catch {
          data.userName = ''
          data.password = ''
          data.remember = false
        }
      }
    }
  }
  // 旧的加密数据
})
</script>

<style scoped lang="less">
.loginForm {
  // min-height: ;
  height: 50%;
  width: 100%;
  box-sizing: border-box;
  padding: 16px;
  .titleText {
    box-sizing: border-box;
    padding: 20px;
    h5 {
      font-size: 1.5rem !important;
      font-weight: 500;
      line-height: 2rem;
      letter-spacing: normal !important;
      font-family: inter, sans-serif, -apple-system, blinkmacsystemfont, Segoe UI, roboto, Helvetica Neue, arial, sans-serif, 'Apple Color Emoji',
        'Segoe UI Emoji', Segoe UI Symbol !important;
      text-transform: none !important;
    }
  }
  .formContainer {
    box-sizing: border-box;
    padding: 12px 20px;
    .v-btn {
      width: 100%;
    }
    .v-text-field {
      margin-top: 10px;
    }
    .v-checkbox {
      color: #b2b0b5;
      margin-inline-start: -0.5625rem;
      margin-top: -10px;
      height: 60px;
    }
  }
  // There is style pollution Or vuetify itself has problems, replace the required verification color manually
  :deep(.v-messages) {
    color: #b00020 !important;
  }
}

.loginBtn {
  height: 45px;
}
</style>
