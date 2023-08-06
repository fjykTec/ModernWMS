<!-- Warehouse Adjust Operate Dialog -->
<template>
  <v-dialog v-model="isShow" :width="'30%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar class="" color="white" :title="`${$t('router.sideBar.warehouseAdjust')}`"></v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
            <v-select
              v-model="data.form.job_type"
              :items="data.combobox.job_type"
              item-title="label"
              item-value="value"
              :rules="data.rules.job_type"
              :label="$t('wms.warehouseWorking.warehouseAdjust.job_type')"
              variant="outlined"
              clearable
              :disabled="isAssociatedJobType"
              @update:model-value="method.changeJobType"
            ></v-select>
            <v-text-field
              v-model="data.form.spu_code"
              :label="$t('base.commodityManagement.spu_code')"
              :rules="data.rules.spu_code"
              variant="outlined"
              readonly
              clearable
              :disabled="isAssociatedJobType"
              @click="method.openCommoditySelect"
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
            <v-text-field
              v-model="data.form.warehouse_name"
              :label="$t('wms.warehouseWorking.warehouseAdjust.warehouse')"
              :rules="data.rules.warehouse_name"
              variant="outlined"
              disabled
            ></v-text-field>
            <v-text-field
              v-model="data.form.location_name"
              :label="$t('wms.warehouseWorking.warehouseAdjust.location_name')"
              :rules="data.rules.location_name"
              variant="outlined"
              disabled
            ></v-text-field>
            <v-text-field
              v-model="data.form.qty"
              :label="$t('wms.warehouseWorking.warehouseAdjust.qty')"
              :rules="data.rules.qty"
              variant="outlined"
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
</template>

<script lang="ts" setup>
import { reactive, computed, ref, watch } from 'vue'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { addStockAdjust } from '@/api/wms/warehouseAdjust'
import { WarehouseAdjustVO, AdjustJobType } from '@/types/WarehouseWorking/WarehouseAdjust'
import { removeObjectNull } from '@/utils/common'
import commoditySelect from '@/components/select/commodity-select.vue'
import { IsInteger } from '@/utils/dataVerification/formRule'

const formRef = ref()
const emit = defineEmits(['close', 'saveSuccess'])
const isUpdate = computed(() => props.form.id && props.form.id > 0)
const operateDisabled = computed(() => !!isUpdate.value)

const props = defineProps<{
  showDialog: boolean
  form: WarehouseAdjustVO
  // If 'associatedJobType' has value, the 'job_type' and 'sku' should be disabled.
  associatedJobType?: AdjustJobType
}>()

const isShow = computed(() => props.showDialog)
const isAssociatedJobType = computed(() => !!(props.associatedJobType && props.associatedJobType > 0))

const data = reactive({
  showCommodityDialogSelect: false,
  showLocationDialogSelect: false,

  form: ref<WarehouseAdjustVO>({
    id: 0,
    // job_type: AdjustJobType.UNKNOW,
    job_code: '',
    sku_id: 0,
    goods_owner_id: 0,
    goods_location_id: 0,
    qty: 0,
    is_update_stock: false,
    source_table_id: 0,
    spu_code: '',
    spu_name: '',
    sku_code: '',
    warehouse_name: '',
    location_name: '',
    creator: '',
    create_time: ''
  }),
  rules: {
    job_type: [],
    qty: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.warehouseWorking.warehouseAdjust.qty') }!`,
      (val: number) => IsInteger(val, 'greaterThanZero') === '' || IsInteger(val, 'greaterThanZero')
    ],
    warehouse_name: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.warehouseWorking.warehouseAdjust.warehouse') }!`
    ],
    location_name: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.warehouseWorking.warehouseAdjust.location_name') }!`
    ],
    spu_code: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.spu_code') }!`],
    spu_name: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.spu_name') }!`],
    sku_code: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.sku_code') }!`],
    sku_name: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.sku_name') }!`]
  },
  combobox: ref<{
    job_type: {
      label: string
      value: AdjustJobType
    }[]
  }>({
    job_type: [
      {
        label: i18n.global.t('wms.warehouseWorking.warehouseAdjust.warehouseTake'),
        value: AdjustJobType.TAKE
      },
      {
        label: i18n.global.t('wms.warehouseWorking.warehouseAdjust.processCombine'),
        value: AdjustJobType.PROCESS_COMBINE
      },
      {
        label: i18n.global.t('wms.warehouseWorking.warehouseAdjust.processSplit'),
        value: AdjustJobType.PROCESS_SPLIT
      },
      {
        label: i18n.global.t('wms.warehouseWorking.warehouseAdjust.warehouseMove'),
        value: AdjustJobType.MOVE
      }
    ]
  })
})

const method = reactive({
  closeDialog: () => {
    emit('close')
  },

  initForm: () => {
    data.form = props.form

    if (isAssociatedJobType.value) {
      data.form.job_type = props.associatedJobType
    }
  },

  changeJobType: (value: any) => {
    // Find the ID corresponding value
    const jobType = data.combobox.job_type.find((item) => item.value === value)
    if (jobType) {
      data.form.job_type = jobType.value
    }
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

  sureSelectCommodity: (selectRecords: any) => {
    try {
      data.form.sku_id = selectRecords[0].sku_id
      data.form.goods_owner_id = selectRecords[0].goods_owner_id
      data.form.goods_location_id = selectRecords[0].goods_location_id
      data.form.warehouse_name = selectRecords[0].warehouse
      data.form.location_name = selectRecords[0].location_name
      data.form.spu_code = selectRecords[0].spu_code
      data.form.spu_name = selectRecords[0].spu_name
      data.form.sku_code = selectRecords[0].sku_code
    } catch (error) {
      // console.error(error)
    }
  },

  clearCommodity: () => {
    data.form.sku_id = 0
    data.form.goods_location_id = 0
    data.form.warehouse_name = ''
    data.form.location_name = ''
    data.form.spu_code = ''
    data.form.spu_name = ''
    data.form.sku_code = ''
  },

  submit: async () => {
    const { valid } = await formRef.value.validate()

    const form = method.constructFormBody()

    if (valid) {
      const { data: res } = await addStockAdjust(form)
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
</style>
