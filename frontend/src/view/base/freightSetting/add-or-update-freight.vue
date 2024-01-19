<!-- Freight Setting Operate Dialog -->
<template>
  <v-dialog v-model="isShow" :width="'30%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('router.sideBar.freightSetting')}`"></v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
            <v-text-field
              v-model="data.form.carrier"
              :label="$t('base.freightSetting.carrier')"
              :rules="data.rules.carrier"
              variant="outlined"
            ></v-text-field>
            <v-text-field
              v-model="data.form.departure_city"
              :label="$t('base.freightSetting.departure_city')"
              :rules="data.rules.departure_city"
              variant="outlined"
            ></v-text-field>
            <v-text-field
              v-model="data.form.arrival_city"
              :label="$t('base.freightSetting.arrival_city')"
              :rules="data.rules.arrival_city"
              variant="outlined"
            ></v-text-field>
            <v-text-field
              v-model="data.form.price_per_weight"
              :label="$t('base.freightSetting.price_per_weight')"
              :rules="data.rules.price_per_weight"
              variant="outlined"
            ></v-text-field>
            <v-text-field
              v-model="data.form.price_per_volume"
              :label="$t('base.freightSetting.price_per_volume')"
              :rules="data.rules.price_per_volume"
              variant="outlined"
            ></v-text-field>
            <v-text-field
              v-model="data.form.min_payment"
              :label="$t('base.freightSetting.min_payment')"
              :rules="data.rules.min_payment"
              variant="outlined"
            ></v-text-field>
            <v-switch
              v-model="data.form.is_valid"
              color="primary"
              :label="$t('base.freightSetting.is_valid')"
              :rules="data.rules.is_valid"
            ></v-switch>
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
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { addFreight, updateFreight } from '@/api/base/freightSetting'
import { FreightVO } from '@/types/Base/Freight'
import { StringLength, IsDecimal } from '@/utils/dataVerification/formRule'
import { removeObjectNull } from '@/utils/common'

const formRef = ref()
const emit = defineEmits(['close', 'saveSuccess'])

const props = defineProps<{
  showDialog: boolean
  form: FreightVO
}>()

const isShow = computed(() => props.showDialog)

const dialogTitle = computed(() => {
  if (props.form.id && props.form.id > 0) {
    return 'update'
  }
  return 'add'
})

const data = reactive({
  form: ref<FreightVO>({
    id: 0,
    carrier: '',
    departure_city: '',
    arrival_city: '',
    price_per_weight: 0,
    price_per_volume: 0,
    min_payment: 0,
    is_valid: true
  }),
  rules: {
    carrier: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.freightSetting.carrier') }!`,
      (val: string) => StringLength(val, 0, 256) === '' || StringLength(val, 0, 256)
    ],
    departure_city: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.freightSetting.departure_city') }!`,
      (val: string) => StringLength(val, 0, 128) === '' || StringLength(val, 0, 128)
    ],
    arrival_city: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.freightSetting.arrival_city') }!`,
      (val: string) => StringLength(val, 0, 128) === '' || StringLength(val, 0, 128)
    ],
    price_per_weight: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.freightSetting.price_per_weight') }!`,
      (val: number) => IsDecimal(val, 'nonNegative', 6, 2) === '' || IsDecimal(val, 'nonNegative', 6, 2)
    ],
    price_per_volume: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.freightSetting.price_per_volume') }!`,
      (val: number) => IsDecimal(val, 'nonNegative', 6, 2) === '' || IsDecimal(val, 'nonNegative', 6, 2)
    ],
    min_payment: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.freightSetting.min_payment') }!`,
      (val: number) => IsDecimal(val, 'nonNegative', 8, 2) === '' || IsDecimal(val, 'nonNegative', 8, 2)
    ],
    is_valid: []
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
    if (valid) {
      const form = removeObjectNull(data.form)
      const { data: res } = dialogTitle.value === 'add' ? await addFreight(form) : await updateFreight(form)
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
