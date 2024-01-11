<template>
  <v-dialog v-model="isShow" :width="'30%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card class="formCard">
        <v-toolbar color="white" :title="`${$t('wms.deliveryManagement.weigh')}`"></v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
            <v-text-field
              v-model="data.form.qty"
              style="margin-bottom: 5px"
              :rules="data.rules.qty"
              :label="$t('wms.deliveryManagement.detailQty')"
              variant="outlined"
              clearable
            ></v-text-field>
            <v-text-field
              v-model="data.form.weight"
              :label="$t('wms.deliveryManagement.detailWeight') + '(' + weightUnit + ')'"
              :rules="data.rules.weight"
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
import { reactive, computed, watch } from 'vue'
import { hookComponent } from '@/components/system/index'
import i18n from '@/languages/i18n'
import { IsInteger, IsDecimal } from '@/utils/dataVerification/formRule'

const emit = defineEmits(['close', 'submit'])

const props = defineProps<{
  showDialog: boolean
  maxQty: number
  defaultWeight: number
  weightUnit: string
}>()

const isShow = computed(() => props.showDialog)

const data = reactive({
  form: { qty: 0, weight: 0 },
  rules: {
    qty: [
      (val: number) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.deliveryManagement.detailQty') }!`,
      (val: number) => IsInteger(val, 'greaterThanZero') === '' || IsInteger(val, 'greaterThanZero')
    ],
    weight: [
      (val: number) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.deliveryManagement.detailQty') }!`,
      (val: number) => IsDecimal(val, 'nonNegative', 15, 3) === '' || IsDecimal(val, 'nonNegative', 15, 3)
    ]
  }
})

const method = reactive({
  closeDialog: () => {
    emit('close')
  },
  submit: () => {
    if (
      IsInteger(data.form.qty, 'greaterThanZero') === ''
      && IsDecimal(data.form.weight, 'nonNegative', 15, 3) === ''
      && Number(data.form.qty) > 0
      && Number(data.form.qty) <= props.maxQty
      && Number(data.form.weight) > 0
    ) {
      emit('submit', { weighing_qty: data.form.qty, weighing_weight: data.form.weight })
    } else {
      hookComponent.$message({
        type: 'error',
        content: `${ i18n.global.t('wms.deliveryManagement.validQtyMsgPackage') }${ props.maxQty }!`
      })
    }
  }
})

watch(
  () => isShow.value,
  (val) => {
    if (val) {
      data.form.qty = props.maxQty
      data.form.weight = props.defaultWeight
    }
  }
)
</script>

<style scoped lang="less"></style>
