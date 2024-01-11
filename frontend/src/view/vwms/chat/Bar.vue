<!--
 @name : Bar.vue
 @Description : 
 @author : saal
 @date : 2023/10/9
-->
<template>
  <div class="VWms-card">
    <div>{{ props.barData?.barTitle }}</div>
    <v-divider></v-divider>
    <div ref="chatRef" class="chat"></div>
  </div>
</template>

<script lang="ts" setup>
import { onMounted, ref, watch } from 'vue'
import * as echarts from 'echarts'

const props = defineProps({
  barData: Object,
})

onMounted(() => {
  initCharts()
})
const chatRef = ref()
let chat:any
const initCharts = () => {
  if (chat) {
    chat.dispose()
  }
  chat = echarts.init(chatRef.value)
  chat.setOption({
    tooltip: {
      trigger: 'item'
    },
    series: [
      {
        name: '个数',
        type: 'pie',
        radius: '50%',
        data: props.barData?.barGroupsData,
        emphasis: {
          itemStyle: {
            shadowBlur: 10,
            shadowOffsetX: 0,
            shadowColor: 'rgba(0, 0, 0, 0.5)'
          }
        },
        label: {
          show: true,
          position: 'outside',
          formatter: '{b} : {d}%'
        },
      },
    ]
  })
}
watch(() => props.barData, initCharts)
</script>

<style lang="less" scoped>
.chat{
  display: flex;
  align-items: center;
  justify-content: center;
  width:100%;
  height: 250px;
}
</style>