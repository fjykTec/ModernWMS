<template>
  <v-dialog v-model="isShow" :width="'30%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card class="formCard">
        <v-toolbar color="white" :title="`${$t('wms.deliveryManagement.setFreight')}`"></v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
            <v-text-field
              v-model="data.form.waybill_no"
              :rules="data.rules.waybill_no"
              :label="$t('wms.deliveryManagement.waybill_no')"
              variant="outlined"
              clearable
            ></v-text-field>
            <v-text-field
              v-model="data.form.carrier"
              :label="$t('wms.deliveryManagement.carrier')"
              variant="outlined"
              readonly
              clearable
              @click="method.openSelect()"
              @click:clear="method.valClearable"
            ></v-text-field>
            <v-text-field
              v-model="data.form.departure_city"
              :label="$t('wms.deliveryManagement.departure_city')"
              variant="outlined"
              readonly
            ></v-text-field>
            <v-text-field
              v-model="data.form.arrival_city"
              :label="$t('wms.deliveryManagement.arrival_city')"
              variant="outlined"
              readonly
            ></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="method.closeDialog">{{ $t('system.page.close') }}</v-btn>
          <v-btn color="primary" variant="text" @click="method.submit">{{ $t('system.page.submit') }}</v-btn>
        </v-card-actions>
      </v-card>
      <freight-select :show-dialog="data.dialogSelect" @close="method.closeDialogSelect()" @sureSelect="method.sureSelect" />
    </template>
  </v-dialog>
</template>

<script lang="ts" setup>
import { reactive, computed, watch } from 'vue'
import { hookComponent } from '@/components/system/index'
import i18n from '@/languages/i18n'
import freightSelect from '@/components/select/freight-select.vue'
import { FreightVO } from '@/types/Base/Freight'
import { StringLength } from '@/utils/dataVerification/formRule'

const emit = defineEmits(['close', 'submit'])

const props = defineProps<{
  showDialog: boolean
}>()

const isShow = computed(() => props.showDialog)

const data = reactive({
  dialogSelect: false,
  form: { carrier: '', freightfee_id: 0, departure_city: '', arrival_city: '', waybill_no: '' },
  combobox: {
    freightfee_id: []
  },
  rules: {
    waybill_no: [
      (val: string) => StringLength(val, 0, 64) === '' || StringLength(val, 0, 64)
    ]
  }
})

const method = reactive({
  openSelect: () => {
    data.dialogSelect = true
  },
  closeDialogSelect: () => {
    data.dialogSelect = false
  },
  sureSelect: (selectRecords: FreightVO[]) => {
    if (selectRecords.length > 0) {
      data.form.carrier = selectRecords[0].carrier
      data.form.freightfee_id = selectRecords[0].id ? selectRecords[0].id : 0
      data.form.departure_city = selectRecords[0].departure_city
      data.form.arrival_city = selectRecords[0].arrival_city
    }
  },
  valClearable: () => {
    data.form = { carrier: '', freightfee_id: 0, departure_city: '', arrival_city: '', waybill_no: data.form.waybill_no }
  },
  closeDialog: () => {
    emit('close')
  },
  submit: () => {
    if (data.form.carrier) {
      emit('submit', data.form)
    } else {
      hookComponent.$message({
        type: 'error',
        content: `${ i18n.global.t('system.checkText.mustSelect') }${ i18n.global.t('wms.deliveryManagement.carrier') }!`
      })
    }
  }
})

watch(
  () => isShow.value,
  (val) => {
    if (val) {
      data.form = { carrier: '', freightfee_id: 0, departure_city: '', arrival_city: '', waybill_no: '' }
    }
  }
)
</script>

<style scoped lang="less"></style>
