<!-- Warehouse Move Operate Dialog -->
<template>
  <v-dialog v-model="isShow" :width="'60%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('router.sideBar.warehouseMove')}`">
          <custom-qrcode :value="qrCodeValue" />
        </v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
            <v-row>
              <v-col :cols="6">
                <v-text-field
                  v-model="data.form.spu_code"
                  :label="$t('base.commodityManagement.spu_code')"
                  :rules="data.rules.spu_code"
                  variant="outlined"
                  readonly
                  clearable
                  @click="method.openCommoditySelect"
                  @click:clear="method.clearCommodity"
                ></v-text-field>
              </v-col>
              <v-col :cols="6">
                <v-text-field
                  v-model="data.form.spu_name"
                  :label="$t('base.commodityManagement.spu_name')"
                  :rules="data.rules.spu_name"
                  variant="outlined"
                  disabled
                ></v-text-field>
              </v-col>
            </v-row>

            <v-row>
              <v-col :cols="6">
                <v-text-field
                  v-model="data.form.sku_code"
                  :label="$t('base.commodityManagement.sku_code')"
                  :rules="data.rules.sku_code"
                  variant="outlined"
                  disabled
                ></v-text-field>
              </v-col>
              <v-col :cols="6">
                <v-text-field
                  v-model="data.form.series_number"
                  :label="$t('wms.stockLocation.series_number')"
                  variant="outlined"
                  disabled
                ></v-text-field>
              </v-col>
            </v-row>

            <v-row>
              <v-col :cols="6">
                <v-text-field
                  v-model="data.form.orig_goods_warehouse"
                  :label="$t('wms.warehouseWorking.warehouseMove.orig_goods_warehouse')"
                  :rules="data.rules.orig_goods_warehouse"
                  variant="outlined"
                  disabled
                ></v-text-field>
              </v-col>
              <v-col :cols="6">
                <v-text-field
                  v-model="data.form.orig_goods_location_name"
                  :label="$t('wms.warehouseWorking.warehouseMove.orig_goods_location_name')"
                  :rules="data.rules.orig_goods_location_name"
                  variant="outlined"
                  disabled
                ></v-text-field>
              </v-col>
            </v-row>

            <v-row>
              <v-col :cols="6">
                <v-text-field
                  v-model="data.form.dest_googs_warehouse"
                  :label="$t('wms.warehouseWorking.warehouseMove.dest_googs_warehouse')"
                  :rules="data.rules.dest_googs_warehouse"
                  variant="outlined"
                  disabled
                ></v-text-field>
              </v-col>
              <v-col :cols="6">
                <v-text-field
                  v-model="data.form.dest_googs_location_name"
                  :label="$t('wms.warehouseWorking.warehouseMove.dest_googs_location_name')"
                  :rules="data.rules.dest_googs_location_name"
                  variant="outlined"
                  readonly
                  clearable
                  @click="method.openLocationSelect"
                  @click:clear="method.clearLocation"
                ></v-text-field>
              </v-col>
            </v-row>

            <v-row>
              <v-col :cols="12">
                <v-text-field
                  v-model="data.form.qty"
                  :label="$t('wms.warehouseWorking.warehouseMove.qty')"
                  :rules="data.rules.qty"
                  variant="outlined"
                ></v-text-field>
              </v-col>
            </v-row>
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

  <location-select :show-dialog="data.showLocationDialogSelect" @close="method.closeLocationDialogSelect" @sureSelect="method.sureSelectLocation" />
</template>

<script lang="ts" setup>
import { reactive, computed, ref, watch } from 'vue'
import { log } from 'console'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { addStockMove } from '@/api/wms/warehouseMove'
import { WarehouseMoveVO, MoveStatus } from '@/types/WarehouseWorking/WarehouseMove'
import { removeObjectNull } from '@/utils/common'
import commoditySelect from '@/components/select/commodity-select.vue'
import locationSelect from '@/components/select/location-select.vue'
import customQrcode from '@/components/custom-qrcode.vue'
import { IsInteger } from '@/utils/dataVerification/formRule'

const formRef = ref()
const emit = defineEmits(['close', 'saveSuccess'])
const isUpdate = computed(() => props.form.id && props.form.id > 0)
const operateDisabled = computed(() => !!isUpdate.value)
const qrCodeValue = computed(() => JSON.stringify({
    id: data.form.id.toString(),
    type: 'warehouseMove'
  }))

const props = defineProps<{
  showDialog: boolean
  form: WarehouseMoveVO
}>()

const isShow = computed(() => props.showDialog)

const data = reactive({
  showCommodityDialogSelect: false,
  showLocationDialogSelect: false,

  // Available Qty
  curAvailableQty: 0,

  form: ref<WarehouseMoveVO>({
    id: 0,
    job_code: '',
    move_status: MoveStatus.UNADJUST,
    sku_id: 0,
    orig_goods_location_id: 0,
    dest_googs_location_id: 0,
    qty: 0,
    goods_owner_id: 0,
    handler: '',
    handle_time: '',
    orig_goods_warehouse: '',
    orig_goods_location_name: '',
    dest_googs_warehouse: '',
    dest_googs_location_name: '',
    spu_code: '',
    spu_name: '',
    sku_code: '',
    sku_name: '',
    series_number: '',
    creator: '',
    create_time: '',
    price: 0,
    expiry_date: '',
    putaway_date: ''
  }),
  rules: {
    qty: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.warehouseWorking.warehouseMove.qty') }!`,
      (val: number) => IsInteger(val, 'greaterThanZero') === '' || IsInteger(val, 'greaterThanZero'),
      (val: string) => method.validQty(val)
    ],
    orig_goods_warehouse: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.warehouseWorking.warehouseMove.orig_goods_warehouse') }!`
    ],
    orig_goods_location_name: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.warehouseWorking.warehouseMove.orig_goods_location_name') }!`
    ],
    dest_googs_warehouse: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.warehouseWorking.warehouseMove.dest_googs_warehouse') }!`
    ],
    dest_googs_location_name: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.warehouseWorking.warehouseMove.dest_googs_location_name') }!`
    ],
    spu_code: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.spu_code') }!`],
    spu_name: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.spu_name') }!`],
    sku_code: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.sku_code') }!`],
    sku_name: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.sku_name') }!`]
  }
})

const method = reactive({
  // The 'qty' can't more than 'qty_available'
  validQty: (value: string): boolean | string => {
    let inputQty = Number(value)
    if (Number.isNaN(inputQty)) {
      inputQty = 0
    }

    if (inputQty > data.curAvailableQty) {
      return `${ i18n.global.t('wms.warehouseWorking.warehouseProcessing.qtyMoreThanAvailable') }${ data.curAvailableQty }`
    }
    return true
  },
  closeDialog: () => {
    emit('close')
  },
  initForm: () => {
    data.form = props.form
  },

  openCommoditySelect: () => {
    // Open select modal after UI rendered.
    setTimeout(() => {
      data.showCommodityDialogSelect = true
    }, 100)
  },

  closeCommodityDialogSelect: () => {
    data.showCommodityDialogSelect = false
  },

  openLocationSelect: () => {
    setTimeout(() => {
      data.showLocationDialogSelect = true
    }, 100)
  },

  closeLocationDialogSelect: () => {
    data.showLocationDialogSelect = false
  },

  sureSelectCommodity: (selectRecords: any) => {
    try {
      data.form.sku_id = selectRecords[0].sku_id
      data.form.orig_goods_location_id = selectRecords[0].goods_location_id
      data.form.orig_goods_warehouse = selectRecords[0].warehouse_name
      data.form.orig_goods_location_name = selectRecords[0].location_name
      data.form.goods_owner_id = selectRecords[0].goods_owner_id
      data.form.spu_code = selectRecords[0].spu_code
      data.form.spu_name = selectRecords[0].spu_name
      data.form.sku_code = selectRecords[0].sku_code
      data.form.sku_name = selectRecords[0].sku_name
      data.form.series_number = selectRecords[0].series_number
      data.form.price = selectRecords[0].price
      data.form.expiry_date = selectRecords[0].expiry_date
      data.form.putaway_date = selectRecords[0].putaway_date

      data.curAvailableQty = selectRecords[0].qty_available
    } catch (error) {
      console.error(error)
    }
  },

  sureSelectLocation: (selectRecords: any) => {
    try {
      data.form.dest_googs_location_id = selectRecords[0].id
      data.form.dest_googs_warehouse = selectRecords[0].warehouse_name
      data.form.dest_googs_location_name = selectRecords[0].location_name
    } catch (error) {
      console.error(error)
    }
  },

  clearCommodity: () => {
    data.form.sku_id = 0
    data.form.orig_goods_location_id = 0
    data.form.orig_goods_warehouse = ''
    data.form.orig_goods_location_name = ''
    data.form.spu_code = ''
    data.form.spu_name = ''
    data.form.sku_code = ''
    data.form.sku_name = ''
    data.curAvailableQty = 0
  },

  clearLocation: () => {
    data.form.dest_googs_location_id = 0
    data.form.dest_googs_warehouse = ''
    data.form.dest_googs_location_name = ''
  },

  submit: async () => {
    const { valid } = await formRef.value.validate()

    const form = method.constructFormBody()

    if (valid) {
      const { data: res } = await addStockMove(form)
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

<style scoped lang="less"></style>
