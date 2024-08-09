<template>
  <div class="screen-top-right2">
    <div class="screen-top-header flex-l">
      <div class="header-left flex-c">
        <i class="iconfont icon-chart-pie" />
      </div>
      <div class="header-right flex-l">
        <span class="header-title">{{ $t('router.sideBar.stockageStatistic') }}</span>
        <dv-decoration-1 class="dv-dec-1" />
      </div>
    </div>
    <div class="screen-top-chart">
      <div ref="chatRef" class="chat" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive, onMounted, ref } from 'vue'
import _ from 'lodash'
import * as echarts from 'echarts'
import { EChartsOption, EChartsType } from 'echarts'
import { list as getStockAgeStatisticList } from '@/api/wms/stockageStatistic'

const chatRef = ref()
let chat: EChartsType
const chartData = reactive({
  category: [] as Array<string>,
  countData: [] as Array<any>
})

const method = reactive({
  initCharts: () => {
    if (chat) chat.dispose()
    chat = echarts.init(chatRef.value)
    const option: EChartsOption = {
      tooltip: {
        trigger: 'item',
        formatter: '{b} : {c} ({d}%)'
      },
      toolbox: {
        show: true
      },
      calculable: true,
      legend: {
        orient: 'horizontal',
        icon: 'circle',
        bottom: 0,
        data: chartData.category,
        textStyle: {
          color: '#fff'
        }
      },
      series: [
        {
          type: 'pie',
          radius: '55%',
          // roseType: 'area',
          // center: ['50%', '40%'],
          itemStyle: {
            borderRadius: 5
          },
          data: chartData.countData,
          label: {
            show: true,
            position: 'outside',
            formatter: '{b} : {d}%',
            color: '#fff'
          }
          // emphasis: {
          //   itemStyle: {
          //     shadowBlur: 10,
          //     shadowOffsetX: 0,
          //     shadowColor: 'rgba(0, 0, 0, 0.5)'
          //   }
          // }
        }
      ]
    }
    chat.setOption(option)
  },
  getStockAgeStatisticList: async () => {
    chartData.category = []
    chartData.countData = []
    const { data: res } = await getStockAgeStatisticList({
      pageIndex: 1,
      pageSize: 99999999
    })
    if (res.isSuccess) {
      res.data.rows.forEach((item) => {
        if (item.stock_age < 30) {
          item.category = '<30(day)'
        } else if (item.stock_age < 61) {
          item.category = '30-60(day)'
        } else if (item.stock_age < 181) {
          item.category = '61-180(day)'
        } else if (item.stock_age < 361) {
          item.category = '181-360(day)'
        } else {
          item.category = '>360(day)'
        }
      })
      const groupedData = _.groupBy(res.data.rows, 'category')
      _.forEach(groupedData, (group, key) => {
        const sumAmount = _.sumBy(group, 'qty')
        chartData.category.push(key)
        chartData.countData.push({ name: key, value: sumAmount })
      })
    }
    method.initCharts()
  }
})
onMounted(() => {
  method.getStockAgeStatisticList()
  setInterval(() => {
    method.getStockAgeStatisticList()
  }, 10 * 60 * 1000)
})
</script>

<style lang="less" scoped>
.screen-top-right2 {
  height: 405px;
  background-color: #0f1325;
  margin: 10px 10px;
  border-radius: 20px;

  .screen-top-header {
    height: 40px;

    .header-left {
      width: 30px;
    }

    .header-right {
      width: calc(100% - 30px);

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

  .screen-top-chart {
    .chat {
      width: 100%;
      height: 340px;
      display: flex;
      justify-content: center;
      align-items: center;
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
.iconfont {
  font-size: 20px !important;
  color: #5cd9e8;
}
</style>
