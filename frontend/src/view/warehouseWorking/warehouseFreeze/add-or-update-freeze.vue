<!-- Warehouse Freeze Operate Dialog -->
<template>
  <v-dialog v-model="isShow" :width="'30%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar class="" color="white" :title="`${jobTypeComp}`"></v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
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
              v-model="data.form.location_name"
              :label="$t('wms.warehouseWorking.warehouseFreeze.location_name')"
              :rules="data.rules.location_name"
              variant="outlined"
              disabled
            ></v-text-field>
            <v-text-field
              v-model="data.form.warehouse_name"
              :label="$t('wms.warehouseWorking.warehouseFreeze.warehouse')"
              :rules="data.rules.warehouse_name"
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
    :sql-title="commoditySqlTitle"
    @close="method.closeCommodityDialogSelect"
    @sureSelect="method.sureSelectCommodity"
  />
  <!-- <location-select :show-dialog="data.showLocationDialogSelect" @close="method.closeLocationDialogSelect" @sureSelect="method.sureSelectLocation" /> -->
</template>

<script lang="ts" setup>
import { reactive, computed, ref, watch } from 'vue'
import { FREEZE_JOB_FREEZE, FREEZE_JOB_UNFREEZE } from '@/constant/warehouseWorking'
import { hookComponent } from '@/components/system/index'
import { addStockFreeze } from '@/api/wms/warehouseFreeze'
import { WarehouseFreezeVO } from '@/types/WarehouseWorking/WarehouseFreeze'
import { removeObjectNull } from '@/utils/common'
import commoditySelect from '@/components/select/commodity-select.vue'
import i18n from '@/languages/i18n'

const formRef = ref()
const emit = defineEmits(['close', 'saveSuccess'])
const isUpdate = computed(() => props.form.id && props.form.id > 0)
const operateDisabled = computed(() => !!isUpdate.value)

const props = defineProps<{
  showDialog: boolean
  form: WarehouseFreezeVO
  freezeType: boolean
}>()

const isShow = computed(() => props.showDialog)
const jobTypeComp = computed(() => (data.form.job_type === FREEZE_JOB_FREEZE
    ? i18n.global.t('base.roleMenu.operationTitle.stock') + i18n.global.t('wms.warehouseWorking.warehouseFreeze.freeze')
    : i18n.global.t('base.roleMenu.operationTitle.stock') + i18n.global.t('wms.warehouseWorking.warehouseFreeze.unfreeze')))

// Unfreeze should be filter freezed.
const commoditySqlTitle = computed(() => (data.form.job_type === FREEZE_JOB_UNFREEZE ? 'frozen' : ''))

const data = reactive({
  showCommodityDialogSelect: false,
  showLocationDialogSelect: false,

  form: ref<WarehouseFreezeVO>({
    id: 0,
    job_code: '',
    job_type: FREEZE_JOB_FREEZE,
    sku_id: 0,
    goods_owner_id: 0,
    goods_location_id: 0,
    handler: '',
    handle_time: '',
    last_update_time: '',
    tenant_id: 0,
    warehouse_name: '',
    location_name: '',
    spu_code: '',
    spu_name: '',
    sku_code: '',
    series_number: '',
    creator: '',
    create_time: ''
  }),
  rules: {
    warehouse_name: [],
    location_name: [],
    spu_code: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.spu_code') }!`],
    spu_name: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.spu_name') }!`],
    sku_code: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.sku_code') }!`]
  }
})

const method = reactive({
  closeDialog: () => {
    emit('close')
  },
  initForm: () => {
    data.form = props.form
    data.form.job_type = props.freezeType
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
      data.form.spu_code = selectRecords[0].spu_code
      data.form.spu_name = selectRecords[0].spu_name
      data.form.sku_code = selectRecords[0].sku_code
      data.form.series_number = selectRecords[0].series_number

      data.form.goods_owner_id = selectRecords[0].goods_owner_id
      data.form.goods_location_id = selectRecords[0].goods_location_id
      data.form.warehouse_name = selectRecords[0].warehouse_name
      data.form.location_name = selectRecords[0].location_name
    } catch (error) {
      console.error(error)
    }
  },

  clearCommodity: () => {
    data.form.sku_id = 0
    data.form.spu_code = ''
    data.form.spu_name = ''
    data.form.sku_code = ''
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
      const { data: res } = await addStockFreeze(form)
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
