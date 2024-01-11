<!--
 @name : Chart.vue
 @Description : 
 @author : linxingmin
 @date : 2023/10/26
-->
<template>
  <div class="chat-container">
    <v-select
      v-model="selectType"
      :items="selectList"
      :label="$t('base.printSolution.vue_path')"
      class="select"
      item-title="label"
      item-value="value"
      variant="outlined"
    >
    </v-select>
    <div ref="chatRef" class="chat" />
  </div>
</template>

<script lang="ts" setup>
import { onMounted, ref, watch } from 'vue'
import * as echarts from 'echarts'
import { EChartsOption, EChartsType } from 'echarts'
import _ from 'lodash'
import { StockAsnVO } from '@/types/WMS/StockAsn'
import { DeliveryStatisticVo } from '@/types/WMS/DeliveryStatistic'
import i18n from '@/languages/i18n'

const props = defineProps<{
  chartData: StockAsnVO[] | DeliveryStatisticVo[],
  type: string
}>()

onMounted(() => {
  handleSelectChange()
  initCharts()
})
const selectList = [{
  label: i18n.global.t('wms.deliveryStatistic.goods_owner_name'),
  value: 'goodsOwnerName'
},
  {
    label: i18n.global.t('wms.deliveryStatistic.category_name'),
    value: 'categoryName'
  }]
const chatRef = ref()
const selectType = ref('goodsOwnerName')

let chat: EChartsType
const initCharts = () => {
  if (chat) chat.dispose()
  chat = echarts.init(chatRef.value)
  // chat.on('legendselectchanged', (params:any) => {
  //   const legendName = params.name
  //   const yAxisName = legendName === '数量' ? '数量' : '金额(元)'
  //   chat.setOption({
  //     yAxis: {
  //       name: yAxisName
  //     }
  //   })
  // })
  const option: EChartsOption = {
    tooltip: {
      trigger: 'axis',
      axisPointer: {
        type: 'shadow',
        label: {
          show: true
        }
      }
    },
    toolbox: {
      show: true,
      feature: {
        mark: { show: true },
        dataView: {
          show: true,
          readOnly: false
        },
        magicType: {
          show: true,
          type: ['line', 'bar']
        },
        restore: { show: true },
        saveAsImage: { show: true }
      }
    },
    calculable: true,
    legend: {
      data: [i18n.global.t('wms.deliveryStatistic.amount'), i18n.global.t('wms.deliveryStatistic.amount_money')],
      itemGap: 5,
      selectedMode: 'single'
    },
    xAxis: [
      {
        type: 'category',
        data: xAxisData.value
      }
    ],
    yAxis: [
      {
        type: 'value',
        // name: '数量',
        axisLabel: {
          formatter(a) {
            return a.toLocaleString()
          }
        }
      }
    ],
    dataZoom: [
      {
        show: true,
        start: 0,
        end: 100
      }
    ],
    series: [
      {
        name: i18n.global.t('wms.deliveryStatistic.amount'),
        type: 'bar',
        data: sumAmountData.value
      },
      {
        name: i18n.global.t('wms.deliveryStatistic.amount_money'),
        type: 'bar',
        data: sumAmountMoneyData.value
      }
    ]
  }
  chat.setOption(option)
}

const xAxisData = ref<string[]>([])
const sumAmountData = ref<number[]>([])
const sumAmountMoneyData = ref<number[]>([])
const handleSelectChange = () => {
  xAxisData.value = []
  sumAmountData.value = []
  sumAmountMoneyData.value = []
  if (props.type === 'delivery') {
    const filterCondition = selectType.value === 'goodsOwnerName' ? 'goods_owner_name' : 'sku_name'
    const groupedData = _.groupBy(props.chartData, filterCondition)
    _.forEach(groupedData, (group, key) => {
      const sumAmount = _.sumBy(group, 'delivery_qty')
      const sumAmountMoney = _.sumBy(group, 'delivery_amount')
      xAxisData.value.push(key)
      sumAmountData.value.push(sumAmount)
      sumAmountMoneyData.value.push(sumAmountMoney)
    })
  } else {
    const filterCondition = selectType.value === 'goodsOwnerName' ? 'goods_owner_name' : 'sku_name'
    const groupedData = _.groupBy(props.chartData, filterCondition)
    _.forEach(groupedData, (group, key) => {
      const sumAmount = _.sumBy(group, 'actual_qty')
      xAxisData.value.push(key)
      sumAmountData.value.push(sumAmount)
      sumAmountMoneyData.value = []
    })
  }
  initCharts()
}
watch(selectType, handleSelectChange)
watch(() => props.chartData, handleSelectChange)

</script>

<style lang="less" scoped>
.chat-container {
  width: 100%;
  height: 100%;
  display: grid;
  grid-template-rows: 60px 1fr;

  .select {
    width: 240px;
  }
}

.chat {
  width: 100%;
  height: 100%;
}
</style>