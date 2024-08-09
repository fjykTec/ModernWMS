<template>
  <div id="container" ref="screenContainerRef" :data-screen="fullScreen" class="screen-container" :class="{ 'screen-container-full': fullScreen }">
    <div class="screenfullbtn">
      <v-btn icon variant="text" @click="toggleFullScreen">
        <v-tooltip activator="parent" location="bottom">{{ $t('system.page.fullScreen') }}</v-tooltip>
        <v-icon :icon="`mdi-arrow-${fullScreen ? 'collapse' : 'expand'}`" color="white"></v-icon>
      </v-btn>
    </div>
    <div v-if="loading" class="mask flex-c">
      <dv-loading> </dv-loading>
    </div>
    <div ref="screenRef" class="screen-content">
      <div class="header-section">
        <ScreenHeader></ScreenHeader>
      </div>
      <div :key="key_section1" class="screen-chart-section1">
        <dv-border-box-12>
          <ScreenTopLeft></ScreenTopLeft>
        </dv-border-box-12>
        <dv-border-box-8 :dur="10">
          <ScreenTopCenter></ScreenTopCenter>
        </dv-border-box-8>
        <dv-border-box-13>
          <ScreenTopRight></ScreenTopRight>
        </dv-border-box-13>
      </div>
      <div :key="key_section2" class="screen-chart-section2">
        <dv-border-box-12>
          <ScreenBottomLeft></ScreenBottomLeft>
        </dv-border-box-12>
        <dv-border-box-13>
          <ScreenBottomRight></ScreenBottomRight>
        </dv-border-box-13>
      </div>
      <div class="footer-section">
        <ScreenFooter></ScreenFooter>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { onMounted, onUnmounted, ref } from 'vue'
import screenfull from 'screenfull'
import windowResize from '@/utils/largeScreenResize'
import ScreenHeader from './ScreenHeader.vue'
import ScreenTopLeft from './ScreenTopLeft.vue'
import ScreenTopCenter from './ScreenTopCenter.vue'
import ScreenTopRight from './ScreenTopRight.vue'
import ScreenBottomLeft from './ScreenBottomLeft.vue'
import ScreenBottomRight from './ScreenBottomRight.vue'
import ScreenFooter from './ScreenFooter.vue'

const { screenContainerRef, screenRef, calcRate, windowDraw, unWindowDraw } = windowResize()
const loading = ref(true)
onMounted(() => {
  // Monitor browser window size changes
  windowDraw()
  calcRate()
  setTimeout(() => {
    loading.value = false
  }, 2000)
})

onUnmounted(() => {
  unWindowDraw()
})
const fullScreen = ref(false)

const key_section1 = ref('section1')
const key_section2 = ref('section2')
const toggleFullScreen = () => {
  if (screenfull.isEnabled) {
    fullScreen.value = !fullScreen.value
    setTimeout(() => {
      const time = Date.now()
      key_section1.value = `${ time }_1`
      key_section2.value = `${ time }_2`
      setTimeout(() => {
        calcRate()
      }, 200)
    }, 0)
  }
}
</script>
<style lang="less" scoped>
.screen-container {
  width: 100%;
  height: calc(100vh - 75px);
  background-color: #020308;
  display: flex;
  justify-content: center;
  align-items: center;
  position: relative;
  .mask {
    position: absolute;
    width: 100%;
    height: 100%;
    z-index: 999;
    background-color: #020308;
    background-image: url('@/assets/img/home_bg.png');
  }
  .screenfullbtn {
    position: absolute;
    top: 10px;
    right: 10px;
    z-index: 10;
    display: flex;
    align-items: center;
    padding: 5px;
    border-radius: 4px;
  }
  .screen-content {
    flex: none;
    width: 1920px ;
    height: 1080px ;
    box-sizing: border-box;
    padding: 12px;
    background-image: url('@/assets/img/home_bg.png');
    transition: all 0.2s ease-in-out;

    .screen-chart-section1 {
      margin-top: 10px;
      display: grid;
      grid-template-columns: 2fr 3fr 2fr;
      grid-column-gap: 5px;
    }

    .screen-chart-section2 {
      margin-top: 5px;
      display: grid;
      grid-template-columns: 5fr 5fr;
      grid-column-gap: 5px;
    }
  }
}
.screen-container-full {
  z-index: 999;
  position: absolute;
  left: 0px;
  top: 0px;
  // width: 100vw;
  height: 100vh;
  overflow: hidden;
}
</style>
