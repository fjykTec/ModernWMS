<template>
  <div class="btn-group col">
    <tooltip-btn
      v-for="(bItem, index) of btnList"
      :key="index"
      :disabled="Boolean(bItem.code) && !authorityList.includes(bItem.code)"
      :icon="bItem.icon"
      :tooltip-text="bItem.name"
      @click="bItem.click"
    ></tooltip-btn>
  </div>
</template>

<script setup lang="tsx">
// import { onMounted, reactive } from 'vue'
import tooltipBtn from '@/components/tooltip-btn.vue'

interface BtnItem {
  name: string
  icon: string
  code: string // Reserved for permission control
  click: () => void
}

defineProps<{
  btnList: BtnItem[]
  authorityList: string[]
}>()
</script>

<style scoped lang="less">
.col {
  display: flex;
  align-items: center;
}
</style>
