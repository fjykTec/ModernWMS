<template>
  <div class="loginContainer">
    <Logo />
    <div class="loginLeft">
      <img src="../../assets/img/loginLeft.png" style="width: 80%" />
    </div>
    <div class="loginRight">
      <div class="LanguagesSwitchContainer">
        <LanguagesSwitch />
      </div>
      <LoginForm />
    </div>
  </div>
</template>

<script lang="ts" setup>
import LoginForm from '@/components/login/login-form.vue'
import LanguagesSwitch from '@/components/system/languages.vue'
import Logo from '@/components/system/logo.vue'
</script>

<style scoped lang="less">
.LanguagesSwitchContainer {
  position: absolute;
  right: 10px;
  top: 10px;
}
.loginContainer {
  width: 100%;
  height: 100%;
  display: flex;
  background-color: #fff;
  .loginLeft {
    width: 70%;
    display: flex;
    align-items: center;
    justify-content: center;
  }
  .loginRight {
    width: 30%;
    background-color: #fafafa;
    display: flex;
    align-items: center;
  }
}
</style>
