<template>
  <img
    class="SysTitleLogo"
    :style="{ height: height + 'px', top: top + 'px', left: left + 'px' }"
    src="@/assets/img/WMSTitle.png"
  />
</template>
<script setup lang="ts">
defineProps({
  height: {
    type: Number,
    default: 80
  },
  top: {
    type: Number,
    default: 20
  },
  left: {
    type: Number,
    default: 20
  }
})
</script>
<style lang="less">
.SysTitleLogo {
  position: absolute;
}
</style>
