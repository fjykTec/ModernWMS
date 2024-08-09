<template>
  <div class="screen-bottom-left">
    <div class="screen-bottom-header flex-l">
      <div class="header-left flex-c">
        <i class="iconfont icon-chart-line" />
      </div>
      <div class="header-right flex-l">
        <span class="header-title">{{ $t('router.sideBar.deliveryStatistic') }}</span>
        <dv-decoration-3 class="dv-dec-1" />
      </div>
    </div>
    <div class="screen-bottom-chart">
      <div ref="chatRef" class="chat" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive, onMounted, ref } from 'vue'
import _ from 'lodash'
import * as echarts from 'echarts'
import { EChartsOption, EChartsType } from 'echarts'
import { formatDate } from '@/utils/format/formatSystem'
import { list as getDeliveryStatisticList } from '@/api/wms/deliveryStatistic'
import i18n from '@/languages/i18n'

const chatRef = ref()
let chat: EChartsType
const chartData = reactive({
  category: [] as Array<string>,
  countData: [] as Array<number>,
  moneyData: [] as Array<number>
})

const method = reactive({
  initCharts: () => {
    if (chat) chat.dispose()
    chat = echarts.init(chatRef.value)
    const option: EChartsOption = {
      tooltip: {
        show: true,
        trigger: 'item',
        axisPointer: {
          type: 'shadow',
          label: {
            show: true
          }
        }
      },
      legend: {
        show: true,
        textStyle: {
          color: 'white',
          fontSize: '14px'
        }
      },
      grid: {
        width: '88%',
        // top: '5%',
        bottom: '7%'
      },

      xAxis: [
        {
          axisLine: {
            lineStyle: {
              color: '#ffffff'
            }
          },
          axisTick: {
            show: false
          },
          data: chartData.category
        }
      ],
      yAxis: [
        {
          type: 'value',
          axisLine: {
            lineStyle: {
              color: '#ffffff'
            }
          },
          axisLabel: {
            formatter(a) {
              return a.toLocaleString()
            }
          }
        }
      ],
      series: [
        {
          name: i18n.global.t('wms.deliveryStatistic.amount'),
          type: 'bar',
          barWidth: 20,
          // type: 'line',
          // smooth: true,
          // showAllSymbol: true,
          // symbol: 'emptyCircle',
          // itemStyle: {
          //   color: '#5cd9e8'
          // },
          itemStyle: {
            // normal: {
            //   barBorderRadius: 5,
            color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [
              { offset: 0, color: '#FFAE8B' },
              { offset: 1, color: '#FFF065' }
            ])
            // }
          },
          data: chartData.countData
        }
      ]
    }
    chat.setOption(option)
  },
  getDeliveryStatisticList: async () => {
    chartData.category = []
    chartData.countData = []
    const myDate = new Date()
    const nowDate = formatDate(myDate, 'yyyy-MM-dd HH:mm:ss')
    const year = myDate.getFullYear()
    const startDate = `${ year }-01-01`
    const { data: res } = await getDeliveryStatisticList({ pageIndex: 1, pageSize: 999999, delivery_date_from: startDate, delivery_date_to: nowDate })
    if (res.isSuccess) {
      const groupedData = _.groupBy(res.data.rows, 'sku_name')
      _.forEach(groupedData, (group, key) => {
        const sumQty = _.sumBy(group, 'delivery_qty')
        const sumAmount = _.sumBy(group, 'delivery_amount')
        chartData.category.push(key)
        chartData.countData.push(sumQty)
        chartData.moneyData.push(sumAmount)
      })
    }
    method.initCharts()
  }
})
// 生命周期
onMounted(() => {
  method.getDeliveryStatisticList()
  setInterval(() => {
    method.getDeliveryStatisticList()
  }, 10 * 60 * 1000)
})
</script>

<style lang="less" scoped>
.screen-bottom-left {
  height: 450px;
  background-color: #0f1325;
  margin: 15px 5px;
  border-radius: 20px;

  .screen-bottom-header {
    height: 40px;

    .header-left {
      width: 40px;
    }

    .header-right {
      width: calc(100% - 40px);

      .header-title {
        color: #fff;
        font-size: 13px;
      }

      .dv-dec-1 {
        width: 100px;
        height: 20px;
        margin-left: 10px;
      }
    }
  }
}
.flex-l {
  display: flex;
  flex-wrap: wrap;
  justify-content: flex-start;
  align-items: center;
}

.flex-c {
  display: flex;
  justify-content: center;
  align-items: center;
}
// 图标
.iconfont {
  font-size: 20px !important;
  color: #5cd9e8;
}
.chat {
  height: 400px;
  width: 100%;
}
</style>
