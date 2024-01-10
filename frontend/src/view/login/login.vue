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

    <div class="copyRight">
      <a href="https://modernwms.ikeyly.com" target="_blank">CopyRight © 2023 ModernWMS All Rights Reserve</a>
    </div>
    <div class="ICP" @click="method.toICP"> 闽ICP备2021008548号 </div>
  </div>
</template>

<script lang="ts" setup>
import { reactive, onMounted } from 'vue'
import LoginForm from '@/components/login/login-form.vue'
import LanguagesSwitch from '@/components/system/languages.vue'
import Logo from '@/components/system/logo.vue'
import { emitter } from '@/utils/bus'

const method = reactive({
  toICP: () => {
    window.open('https://beian.miit.gov.cn/', '_blank')
  }
})

// Return to the login interface to clear the status
onMounted(() => {
  emitter.emit('closeLoading')
})
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
  // position: relative;
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

.copyRight {
  width: 100%;
  color: #bbb;
  display: flex;
  justify-content: center;
  font-size: 12px;
  position: absolute;
  bottom: 30px;
  left: 50%;
  transform: translate(-50%, -50%);
}

.ICP {
  color: #999;
  font-size: 12px;
  position: absolute;
  bottom: 10px;
  left: 50%;
  transform: translate(-50%, -50%);

  &:hover {
    cursor: pointer;
    color: #aaa;
  }
}
</style>
