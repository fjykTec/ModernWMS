<!-- Warehouse Taking Number Input -->
<template>
  <v-dialog v-model="isShow" :width="'20%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('router.sideBar.warehouseTaking')}`"></v-toolbar>
        <v-card-text>
          <p class="mb-4" style="color: #999999">{{ $t('wms.warehouseWorking.warehouseTaking.book_qty') }}：{{ data.form.book_qty }}</p>
          <v-form ref="formRef">
            <v-text-field
              v-model="data.form.counted_qty"
              :label="$t('wms.warehouseWorking.warehouseTaking.counted_qty')"
              :rules="data.rules.counted_qty"
              variant="outlined"
              clearable
            ></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="method.closeDialog">{{ $t('system.page.close') }}</v-btn>
          <v-btn color="primary" variant="text" @click="method.submit">{{ $t('system.page.submit') }}</v-btn>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>
</template>

<script lang="ts" setup>
import { reactive, computed, ref, watch } from 'vue'
import { hookComponent } from '@/components/system/index'
import { confirmStockTaking } from '@/api/wms/warehouseTaking'
import { WarehouseTakingVO } from '@/types/WarehouseWorking/WarehouseTaking'
import { removeObjectNull } from '@/utils/common'
import { TAKING_JOB_FINISH } from '@/constant/warehouseWorking'
import i18n from '@/languages/i18n'
import { IsInteger } from '@/utils/dataVerification/formRule'

const formRef = ref()
const emit = defineEmits(['close', 'saveSuccess'])

const props = defineProps<{
  showDialog: boolean
  form: WarehouseTakingVO
}>()

const isShow = computed(() => props.showDialog)

const data = reactive({
  showCommodityDialogSelect: false,
  showLocationDialogSelect: false,

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
    warehouse_name: '',
    location_name: '',
    adjust_status: false,
    price: 0,
    expiry_date: '',
    putaway_date: '',
    handler: '',
    handle_time: '',
    series_number: ''
  }),
  rules: {
    book_qty: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.warehouseWorking.warehouseTaking.book_qty') }!`
    ],
    counted_qty: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.warehouseWorking.warehouseTaking.counted_qty') }!`,
      (val: number) => IsInteger(val, 'nonNegative') === '' || IsInteger(val, 'nonNegative')
    ]
  }
})

const method = reactive({
  closeDialog: () => {
    emit('close')
  },

  initForm: () => {
    data.form = props.form
  },

  submit: async () => {
    const { valid } = await formRef.value.validate()

    const form = method.constructFormBody()

    if (valid) {
      const { data: res } = await confirmStockTaking(form)
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
