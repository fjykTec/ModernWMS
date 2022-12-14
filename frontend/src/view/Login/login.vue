<template>
  <div class="container">
    <img class="SysTitleLogo" src="@/assets/img/WMSTitle.png" />
    <div class="loginLeft">
      <img src="../../assets/img/loginLeft3.png" />
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
import LoginForm from '@/components/login/loginForm.vue'
import LanguagesSwitch from '@/components/common/languages.vue'
</script>

<style scoped lang="less">
.SysTitleLogo {
  position: absolute;
  top: 20px;
  left: 20px;
  height: 80px;
}
.LanguagesSwitchContainer {
  position: absolute;
  right: 10px;
  top: 10px;
}
.container {
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
    background-color: #f4f5fa;
    display: flex;
    align-items: center;
  }
}
</style>
