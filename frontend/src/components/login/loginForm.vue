<template>
  <div class="loginForm">
    <div class="titleText">
      <h5>{{ $t('login.welcomeTitle') }}</h5>
    </div>
    <div class="formContainer">
      <v-form ref="VFormRef" v-model="data.valid" lazy-validation>
        <v-text-field
          v-model="data.userName"
          required
          :rules="data.userNameVaildRules"
          :label="$t('login.userName')"
          variant="solo"
        ></v-text-field>
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
        <v-btn color="purple" @click="method.login()">{{ $t('login.mainButtonLabel') }}</v-btn>
        <!-- <v-btn color="purple" @click="method.testApi()">{{ '测试接口' }}</v-btn> -->
      </v-form>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { reactive, ref } from 'vue'
import { Md5 } from 'ts-md5'
import i18n from '@/languages/i18n'
import { login } from '@/api/sys/login'
// import { getNotify } from '@/api/sys/test'
import { store } from '@/store'

// Get v-form ref
const VFormRef = ref()

const data = reactive({
  valid: true,
  showPassword: false,
  userName: '',
  password: '',
  remember: false,
  userNameVaildRules: [(v: string) => !!v || i18n.global.t('login.userNameMustInput')],
  passwordVaildRules: [(v: string) => !!v || i18n.global.t('login.passwordMustInput')]
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
      userName: data.userName,
      password: Md5.hashStr(data.password)
    })

    if (loginRes.isSuccess) {
      // TODO 把用户权限信息加进来
      // let expiredTime = new Date().getTime() + loginRes.data.expire * 60 * 1000
      const expiredTime = new Date().getTime() + 1 * 60 * 1000

      store.commit('user/setToken', loginRes.data.accessToken)
      store.commit('user/setRefreshToken', loginRes.data.refreshToken)
      store.commit('user/setExpirationTime', expiredTime)
      store.commit('user/setEffectiveMinutes', loginRes.data.expire)
    }
  }
  // testApi: async () => {
  //   const { data: res } = await getNotify({
  //     ifall: true,
  //     iftodo: true
  //   })

  //   if (res.isSuccess) {
  //     console.log('成功！', res)
  //   }
  // }
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
      font-family: inter, sans-serif, -apple-system, blinkmacsystemfont, Segoe UI, roboto, Helvetica Neue, arial,
        sans-serif, 'Apple Color Emoji', 'Segoe UI Emoji', Segoe UI Symbol !important;
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
}
</style>
