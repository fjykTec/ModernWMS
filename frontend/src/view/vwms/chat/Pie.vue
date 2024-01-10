<!--
 @name : Pie.vue
 @Description : 
 @author : saal
 @date : 2023/10/9
-->
<template>
  <div class="VWms-card">
    <div>{{ props.pieData?.pieTitle }}</div>
    <v-divider></v-divider>
    <div ref="chatRef" class="chat"></div>
  </div>
</template>

<script lang="ts" setup>
import { onMounted, ref, watch } from 'vue'
import * as echarts from 'echarts'

const props = defineProps({
  pieData: Object,
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
    xAxis: {
      type: 'category',
      data: props.pieData?.xData,
    },
    yAxis: {
      type: 'value'
    },
    tooltip: {
      trigger: 'axis',
      axisPointer: {
        type: 'shadow'
      }
    },
    series: [
      {
        data: props.pieData?.yData,
        type: 'bar',
        showBackground: true,
        backgroundStyle: {
          color: 'rgba(180, 180, 180, 0.2)'
        },
        tooltip: {
          valueFormatter(value) {
            return `存货数量：${ value }`
          }
        },
        markLine: {
          data: [{ type: 'average', name: 'Avg' }]
        },
      }
    ]
  })
}
watch(() => props.pieData, initCharts)
</script>

<style lang="less" scoped>
.chat{
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  height: 300px;
}
</style>