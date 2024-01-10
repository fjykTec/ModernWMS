<template>
  <v-dialog v-model="isShow" :width="'30%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <!-- <v-toolbar color="white" :title="`${$t('wms.stockAsn.tabNotice')}`"></v-toolbar> -->
        <v-card-text>
          <v-list-item two-line>
            <v-list-item-content>
              <v-list-item-title>{{ i18n.global.t('wms.skuInfo.spu_name') + ':' + data.tableData.spu_name }}</v-list-item-title>
              <v-list-item-subtitle>{{ i18n.global.t('wms.skuInfo.sku_code') + ':' + data.tableData.sku_code }}</v-list-item-subtitle>
              <v-list-item-subtitle>{{ i18n.global.t('wms.skuInfo.sku_name') + ':' + data.tableData.sku_name }}</v-list-item-subtitle>
              <v-list-item-subtitle>{{ i18n.global.t('wms.skuInfo.spu_description') + ':' + data.tableData.spu_description }}</v-list-item-subtitle>
              <v-list-item-subtitle>{{ i18n.global.t('wms.skuInfo.category_name') + ':' + data.tableData.category_name }}</v-list-item-subtitle>
              <v-list-item-subtitle>{{ i18n.global.t('wms.skuInfo.brand') + ':' + data.tableData.brand }}</v-list-item-subtitle>
              <v-list-item-subtitle>{{ i18n.global.t('wms.skuInfo.origin') + ':' + data.tableData.origin }}</v-list-item-subtitle>
              <v-list-item-subtitle>{{ i18n.global.t('wms.skuInfo.unit') + ':' + data.tableData.unit }}</v-list-item-subtitle>
              <v-list-item-subtitle>{{ i18n.global.t('wms.skuInfo.cost') + ':' + data.tableData.cost }}</v-list-item-subtitle>
              <v-list-item-subtitle>{{ i18n.global.t('wms.skuInfo.bar_code') + ':' + data.tableData.bar_code }}</v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="method.closeDialog">{{ $t('system.page.close') }}</v-btn>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>
</template>

<script lang="ts" setup>
import { reactive, computed, ref, watch } from 'vue'
import { SkuInfoVo } from '@/types/WMS/StockAsn'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { getSkuInfo } from '@/api/wms/stockAsn'

const formRef = ref()
const emit = defineEmits(['close'])

const props = defineProps<{
  showDialog: boolean
  sku_id: number
}>()

const isShow = computed(() => props.showDialog)

const data = reactive({
  id: 0,
  tableData: ref<SkuInfoVo>({
    spu_id: 0,
    spu_code: '',
    spu_name: '',
    category_id: 0,
    category_name: '',
    spu_description: '',
    bar_code: '',
    supplier_id: 0,
    supplier_name: '',
    brand: '',
    origin: '',
    length_unit: 0,
    volume_unit: 0,
    weight_unit: 0,
    sku_id: 0,
    sku_code: '',
    sku_name: '',
    weight: 0,
    lenght: 0,
    width: 0,
    height: 0,
    volume: 0,
    unit: '',
    cost: 0,
    price: 0
  })
})

const method = reactive({
  closeDialog: () => {
    emit('close')
  },
  getStockAsnList: async () => {
    const { data: res } = await getSkuInfo(data.id)
    if (!res.isSuccess) {
      hookComponent.$message({
        type: 'error',
        content: res.errorMessage
      })
      return
    }
    data.tableData = res.data
  }
})

watch(
  () => isShow.value,
  (val) => {
    if (val) {
      data.id = props.sku_id
      method.getStockAsnList()
    }
  }
)
</script>

<style scoped lang="less">
.v-form {
  div {
    margin-bottom: 7px;
  }
}
</style>
