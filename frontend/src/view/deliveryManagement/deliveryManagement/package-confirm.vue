<template>
  <v-dialog v-model="data.showDialog" :width="'70%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card class="formCard">
        <v-toolbar color="white" :title="dialogTitle"></v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
            <v-row v-for="(item, index) of data.list" :key="index">
              <v-col :cols="3">
                <v-text-field v-model="item.spu_name" :label="$t('wms.deliveryManagement.spu_name')" variant="outlined" readonly></v-text-field>
              </v-col>
              <v-col :cols="3">
                <v-text-field v-model="item.spu_code" :label="$t('wms.deliveryManagement.spu_code')" variant="outlined" readonly></v-text-field>
              </v-col>
              <v-col :cols="isWeight ? 2 : 3">
                <v-text-field v-model="item.sku_code" :label="$t('wms.deliveryManagement.sku_code')" variant="outlined" readonly></v-text-field>
              </v-col>
              <v-col :cols="isWeight ? 2 : 3">
                <v-text-field
                  v-model="item.qty"
                  :rules="data.rules.qty"
                  :label="isSignIn ? $t('wms.deliveryManagement.damagedQuantity') : $t('wms.deliveryManagement.detailQty')"
                  variant="outlined"
                  clearable
                ></v-text-field>
              </v-col>
              <v-col v-if="isWeight" :cols="2">
                <v-text-field
                  v-model="item.weight"
                  :rules="data.rules.weight"
                  :label="$t('wms.deliveryManagement.detailWeight') + '(' + item.weight_unit + ')'"
                  variant="outlined"
                  clearable
                ></v-text-field>
              </v-col>
            </v-row>
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
import { ref, reactive } from 'vue'
import { hookComponent } from '@/components/system/index'
import i18n from '@/languages/i18n'
import { IsInteger, IsDecimal } from '@/utils/dataVerification/formRule'
import { ConfirmItem } from '@/types/DeliveryManagement/DeliveryManagement'

const emit = defineEmits(['close', 'submit'])

const props = defineProps<{
  // showDialog: boolean
  // dataList: ConfirmItem[]
  isWeight: boolean
  isSignIn?: boolean
  dialogTitle: string
}>()

// const isShow = computed(() => props.showDialog)

const data = reactive({
  showDialog: false as boolean,
  list: ref<ConfirmItem[]>([]),
  rules: {
    qty: [],
    weight: [
      (val: number) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.deliveryManagement.detailQty') }!`,
      (val: number) => IsDecimal(val, 'nonNegative', 15, 3) === '' || IsDecimal(val, 'nonNegative', 15, 3)
    ]
  } as any
})

const method = reactive({
  openDialog: (dataList: ConfirmItem[] = []) => {
    data.list = dataList

    if (!props.isSignIn) {
      data.rules.qty = [
        (val: number) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.deliveryManagement.detailQty') }!`,
        (val: number) => IsInteger(val, 'greaterThanZero') === '' || IsInteger(val, 'greaterThanZero')
      ]
    } else {
      data.rules.qty = []
    }

    data.showDialog = true
  },
  closeDialog: () => {
    // emit('close')
    data.showDialog = false
  },
  submit: () => {
    if (props.isSignIn) {
      const verificationFailedList = data.list.filter(
        (item) => IsInteger(item.qty, 'nonNegative') !== '' || Number(item.qty) < 0 || Number(item.qty) > item.maxQty
      )
      if (verificationFailedList.length > 0) {
        const errMsgStrList = verificationFailedList.map(
          (item) => `${ item.sku_code }: ${ i18n.global.t('wms.deliveryManagement.exceedingQuantity') }${ item.maxQty }`
        )
        hookComponent.$message({
          type: 'error',
          content: `${ i18n.global.t('wms.deliveryManagement.invalidValue') }; ${ errMsgStrList.join('; ') }`
        })
      } else {
        hookComponent.$dialog({
          content: `${ i18n.global.t('wms.deliveryManagement.irreversible') }, ${ i18n.global.t('wms.deliveryManagement.confirmSignIn') }?`,
          handleConfirm: async () => {
            emit('submit', data.list)
          }
        })
      }
    } else {
      const verificationFailedList = data.list.filter(
        (item) => IsInteger(item.qty, 'greaterThanZero') !== ''
          || Number(item.qty) <= 0
          || Number(item.qty) > item.maxQty
          || (item.weight && IsDecimal(item.weight, 'nonNegative', 15, 3) !== '')
      )
      if (verificationFailedList.length > 0) {
        const errMsgStrList = verificationFailedList.map(
          (item) => `${ item.sku_code }: ${ i18n.global.t('wms.deliveryManagement.exceedingQuantity') }${ item.maxQty }`
        )
        hookComponent.$message({
          type: 'error',
          content: `${ i18n.global.t('wms.deliveryManagement.invalidValue') }; ${ errMsgStrList.join('; ') }`
        })
      } else {
        emit('submit', data.list)
      }
    }
  }
})

// watch(
//   () => isShow.value,
//   (val) => {
//     if (val) {
//       data.list = props.dataList
//     }
//   }
// )

defineExpose({
  openDialog: method.openDialog,
  closeDialog: method.closeDialog
})
</script>

<style scoped lang="less"></style>
