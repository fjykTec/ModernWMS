<!-- Warehouse Taking Operate Dialog -->
<template>
  <v-dialog v-model="isShow" :width="'30%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('router.sideBar.warehouseTaking')}`">
          <custom-qrcode :value="qrCodeValue" />
        </v-toolbar>
        <v-card-text>
          <div class="header">
            <div class="headerBtn">
              <tooltip-btn
                size="x-small"
                icon="mdi-home-plus-outline"
                :tooltip-text="$t('wms.warehouseWorking.warehouseTaking.addFromStock')"
                @click="method.openCommoditySelect()"
              ></tooltip-btn>
              <tooltip-btn
                size="x-small"
                icon="mdi-store-plus-outline"
                :tooltip-text="$t('wms.warehouseWorking.warehouseTaking.addFromCommodity')"
                :disabled="isFromStock"
                @click="method.openSkuSelect()"
              ></tooltip-btn>
            </div>
            <div class="headerTips">
              <v-tooltip :text="$t('wms.warehouseWorking.warehouseTaking.addTips')">
                <template #activator="{ props }">
                  <v-btn v-bind="props" size="x-small" variant="text" icon="mdi-help-circle" color="blue-lighten-2"></v-btn>
                </template>
              </v-tooltip>
            </div>
          </div>
          <v-form ref="formRef">
            <v-text-field
              v-model="data.form.spu_code"
              :label="$t('base.commodityManagement.spu_code')"
              :rules="data.rules.spu_code"
              variant="outlined"
              readonly
              clearable
              @click:clear="method.clearCommodity"
            ></v-text-field>
            <v-text-field
              v-model="data.form.spu_name"
              :label="$t('base.commodityManagement.spu_name')"
              :rules="data.rules.spu_name"
              variant="outlined"
              disabled
            ></v-text-field>
            <v-text-field
              v-model="data.form.sku_code"
              :label="$t('base.commodityManagement.sku_code')"
              :rules="data.rules.sku_code"
              variant="outlined"
              disabled
            ></v-text-field>
            <v-text-field v-model="data.form.series_number" :label="$t('wms.stockLocation.series_number')" variant="outlined" disabled></v-text-field>
            <v-text-field
              v-model="data.form.warehouse_name"
              :label="$t('wms.warehouseWorking.warehouseTaking.warehouse')"
              :rules="data.rules.warehouse_name"
              variant="outlined"
              disabled
            ></v-text-field>
            <v-text-field
              v-model="data.form.location_name"
              :label="$t('wms.warehouseWorking.warehouseTaking.location_name')"
              :rules="data.rules.location_name"
              variant="outlined"
              :disabled="isFromStock"
              @click="method.openLocationSelect"
              @click:clear="method.clearLocation"
            ></v-text-field>
            <v-text-field
              v-model="data.form.price"
              :label="$t('wms.warehouseWorking.warehouseTaking.price')"
              :rules="data.rules.price"
              variant="outlined"
              :disabled="isFromStock"
            ></v-text-field>
            <v-text-field
              v-model="data.form.expiry_date"
              :label="$t('wms.warehouseWorking.warehouseTaking.expiry_date')"
              variant="outlined"
              type="date"
              :disabled="isFromStock"
            ></v-text-field>
            <v-text-field
              v-model="data.form.putaway_date"
              :label="$t('wms.warehouseWorking.warehouseTaking.putaway_date')"
              variant="outlined"
              type="date"
              :disabled="isFromStock"
            ></v-text-field>
            <v-text-field
              v-model="data.form.book_qty"
              :label="$t('wms.warehouseWorking.warehouseTaking.book_qty')"
              :rules="data.rules.book_qty"
              variant="outlined"
              disabled
            ></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="method.closeDialog">{{ $t('system.page.close') }}</v-btn>
          <v-btn color="primary" variant="text" :disabled="operateDisabled" @click="method.submit">{{ $t('system.page.submit') }}</v-btn>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>

  <commodity-select
    :show-dialog="data.showCommodityDialogSelect"
    @close="method.closeCommodityDialogSelect"
    @sureSelect="method.sureSelectCommodity"
  />
  <sku-select :show-dialog="data.showSkuDialogSelect" @close="method.closeDialogSelectSku" @sureSelect="method.sureSelectSku" />
  <location-select :show-dialog="data.showLocationDialogSelect" @close="method.closeDialogSelectLocation" @sureSelect="method.sureSelectLocation" />
</template>

<script lang="ts" setup>
import { reactive, computed, ref, watch } from 'vue'
import { hookComponent } from '@/components/system/index'
import { addStockTaking } from '@/api/wms/warehouseTaking'
import { WarehouseTakingVO } from '@/types/WarehouseWorking/WarehouseTaking'
import { removeObjectNull } from '@/utils/common'
import { TAKING_JOB_FINISH } from '@/constant/warehouseWorking'
import i18n from '@/languages/i18n'
import commoditySelect from '@/components/select/commodity-select.vue'
import tooltipBtn from '@/components/tooltip-btn.vue'
import locationSelect from '@/components/select/location-select.vue'
import skuSelect from '@/components/select/sku-select.vue'
import customQrcode from '@/components/custom-qrcode.vue'
import { IsDecimal } from '@/utils/dataVerification/formRule'
import { formatDate } from '@/utils/format/formatSystem'

const formRef = ref()
const emit = defineEmits(['close', 'saveSuccess'])
const isUpdate = computed(() => props.form.id && props.form.id > 0)
const operateDisabled = computed(() => !!isUpdate.value)
const isFromStock = computed(() => data.curStockID > 0)
const qrCodeValue = computed(() => JSON.stringify({
    id: data.form.id.toString(),
    type: 'warehouseTaking'
  }))

const props = defineProps<{
  showDialog: boolean
  form: WarehouseTakingVO
}>()

const isShow = computed(() => props.showDialog)

const data = reactive({
  showCommodityDialogSelect: false,
  showLocationDialogSelect: false,
  showSkuDialogSelect: false,

  // There has value when choose from stock.
  curStockID: 0,

  form: ref<WarehouseTakingVO>({
    id: 0,
    job_code: '',
    job_status: TAKING_JOB_FINISH,
    sku_id: 0,
    goods_owner_id: 0,
    goods_location_id: 0,
    book_qty: 0,
    counted_qty: 0,
    difference_qty: 0,
    spu_code: '',
    spu_name: '',
    sku_code: '',
    series_number: '',
    warehouse_name: '',
    location_name: '',
    handler: '',
    handle_time: '',
    adjust_status: false,
    creator: '',
    create_time: '',
    price: 0,
    expiry_date: '',
    putaway_date: ''
  }),
  rules: {
    job_type: [],
    book_qty: [],
    warehouse_name: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.warehouseWorking.warehouseTaking.warehouse') }!`
    ],
    location_name: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.warehouseWorking.warehouseTaking.location_name') }!`
    ],
    price: [(val: number) => IsDecimal(val, 'nonNegative', 10, 2) === '' || IsDecimal(val, 'nonNegative', 10, 2)],
    spu_code: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.spu_code') }!`],
    spu_name: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.spu_name') }!`],
    sku_code: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.sku_code') }!`],
    sku_name: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.sku_name') }!`]
  }
})

const method = reactive({
  closeDialog: () => {
    emit('close')
  },

  initForm: () => {
    data.form = props.form
    data.curStockID = 0
    if (data.form.expiry_date) {
      data.form.expiry_date = formatDate(data.form.expiry_date, 'yyyy-MM-dd')
      data.form.putaway_date = formatDate(data.form.putaway_date, 'yyyy-MM-dd')
    }
  },

  openCommoditySelect: () => {
    data.showCommodityDialogSelect = true
  },

  closeCommodityDialogSelect: () => {
    data.showCommodityDialogSelect = false
  },

  sureSelectCommodity: (selectRecords: any) => {
    try {
      data.curStockID = selectRecords[0].id
      data.form.sku_id = selectRecords[0].sku_id
      data.form.goods_owner_id = selectRecords[0].goods_owner_id
      data.form.goods_location_id = selectRecords[0].goods_location_id
      data.form.warehouse_name = selectRecords[0].warehouse_name
      data.form.location_name = selectRecords[0].location_name
      data.form.spu_code = selectRecords[0].spu_code
      data.form.spu_name = selectRecords[0].spu_name
      data.form.sku_code = selectRecords[0].sku_code
      data.form.series_number = selectRecords[0].series_number
      data.form.book_qty = selectRecords[0].qty_available
      data.form.price = selectRecords[0].price
      data.form.expiry_date = selectRecords[0].expiry_date
      data.form.putaway_date = selectRecords[0].putaway_date
      if (data.form.expiry_date) {
        data.form.expiry_date = formatDate(data.form.expiry_date, 'yyyy-MM-dd')
      }
      if (data.form.putaway_date) {
        data.form.putaway_date = formatDate(data.form.putaway_date, 'yyyy-MM-dd')
      }
    } catch (error) {
      // console.error(error)
    }
  },

  clearCommodity: () => {
    data.curStockID = 0
    data.form.sku_id = 0
    data.form.goods_location_id = 0
    data.form.warehouse_name = ''
    data.form.location_name = ''
    data.form.spu_code = ''
    data.form.spu_name = ''
    data.form.sku_code = ''
    data.form.book_qty = 0
  },

  openSkuSelect: () => {
    data.showSkuDialogSelect = true
  },

  closeDialogSelectSku: () => {
    data.showSkuDialogSelect = false
  },

  sureSelectSku: (selectRecords: any) => {
    if (selectRecords.length > 0) {
      data.form.sku_id = selectRecords[0].sku_id
      data.form.goods_owner_id = selectRecords[0].goods_owner_id
      data.form.goods_location_id = selectRecords[0].goods_location_id
      // The book qty should zero when the data from commodity.
      data.form.book_qty = 0
      data.form.spu_code = selectRecords[0].spu_code
      data.form.spu_name = selectRecords[0].spu_name
      data.form.sku_code = selectRecords[0].sku_code
    }
  },

  openLocationSelect: () => {
    data.showLocationDialogSelect = true
  },

  closeDialogSelectLocation: () => {
    data.showLocationDialogSelect = false
  },

  sureSelectLocation: (selectRecords: any) => {
    if (selectRecords.length > 0) {
      data.form.goods_location_id = selectRecords[0].id
      data.form.warehouse_name = selectRecords[0].warehouse_name
      data.form.location_name = selectRecords[0].location_name
    }
  },

  clearLocation: () => {
    data.form.goods_location_id = 0
    data.form.warehouse_name = ''
    data.form.location_name = ''
  },

  submit: async () => {
    const { valid } = await formRef.value.validate()

    const form = method.constructFormBody()

    if (valid) {
      const { data: res } = await addStockTaking(form)
      if (!res.isSuccess) {
        hookComponent.$message({
          type: 'error',
          content: res.errorMessage
        })
        return
      }
      hookComponent.$message({
        type: 'success',
        content: `${ i18n.global.t('system.page.submit') }${ i18n.global.t('system.tips.success') }`
      })
      emit('saveSuccess')
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('system.checkText.checkFormFail')
      })
    }
  },

  constructFormBody: () => {
    let form = { ...data.form }
    form = removeObjectNull(form)

    return form
  }
})

watch(
  () => isShow.value,
  (val) => {
    if (val) {
      method.initForm()
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

.header {
  display: flex;
  margin-bottom: 15px;
  justify-content: space-between;
}

.headerBtn {
}
.headerTips {
  display: flex;
  align-items: center;
}
</style>
