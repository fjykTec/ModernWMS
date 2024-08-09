<template>
  <div class="screen-top-right2">
    <div class="screen-top-header flex-l">
      <div class="header-left flex-c">
        <i class="iconfont icon-chart-location" />
      </div>
      <div class="header-right flex-l">
        <span class="header-title">{{ $t('wms.stockManagement.stockLocation') }}</span>
        <dv-decoration-1 class="dv-dec-1" />
      </div>
    </div>
    <div class="screen-top-chart">
      <dv-scroll-board class="dv-scr-board" :config="config" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive, onMounted } from 'vue'
import { getStockLocationList } from '@/api/wms/stockManagement'
import { StockLocationVO } from '@/types/WMS/StockManagement'
import i18n from '@/languages/i18n'

const config = reactive({
  header: [
    i18n.global.t('wms.stockLocation.warehouse_name'),
    i18n.global.t('wms.stockLocation.location_name'),
    i18n.global.t('wms.stockLocation.spu_code'),
    i18n.global.t('wms.stockLocation.spu_name'),
    i18n.global.t('wms.stockLocation.sku_code'),
    i18n.global.t('wms.stockLocation.qty')
  ] as string[],
  data: [],
  rowNum: 7, // Table Rows
  headerHeight: 35,
  headerBGC: '#0f1325', // Meter header
  oddRowBGC: '#0f1325', // Odd Rows
  evenRowBGC: '#171c33', // Even rows
  index: true,
  columnWidth: [50],
  align: ['center']
})
const method = reactive({
  getStockLocationList: async () => {
    const { data: res } = await getStockLocationList({ total: 0, pageIndex: 1, pageSize: 99999 })
    config.data = res.data.rows.map((item: StockLocationVO) => [item.warehouse_name, item.location_name, item.spu_code, item.spu_name, item.sku_code, item.qty])
  }
})
onMounted(() => {
  method.getStockLocationList()
    setInterval(() => {
    method.getStockLocationList()
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
    .dv-scr-board {
      width:100%;
      // width: calc(100% - 30px);
      height: 340px;
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
