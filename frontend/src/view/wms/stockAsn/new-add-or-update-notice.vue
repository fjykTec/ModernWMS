<template>
  <v-dialog v-model="isShow" :width="'80%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card class="formCard">
        <v-toolbar color="white" :title="`${$t('wms.stockAsn.tabNotice')}`"></v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
            <v-row>
              <v-col :cols="4">
                <!-- <v-select
                  v-model="data.form.goods_owner_name"
                  :items="data.combobox.goods_owner_name"
                  item-title="label"
                  item-value="label"
                  :rules="data.rules.goods_owner_name"
                  :label="$t('wms.stockAsnInfo.goods_owner_name')"
                  variant="outlined"
                  clearable
                  filterable
                  @update:model-value="method.goodsOwnerNameChange"
                ></v-select> -->
                <customFilterSelect
                  v-model="data.form"
                  :items="data.combobox.goods_owner_name"
                  item-title="label"
                  :rules="data.rules.goods_owner_name"
                  :label="$t('wms.stockAsnInfo.goods_owner_name')"
                  :mapping="[
                    {
                      in: 'goods_owner_id',
                      out: 'value'
                    },
                    {
                      in: 'goods_owner_name',
                      out: 'label'
                    }
                  ]"
                />
              </v-col>
              <v-col :cols="4">
                <v-text-field
                  v-model="data.form.asn_batch"
                  :label="$t('wms.stockAsnInfo.asn_batch')"
                  :rules="data.rules.asn_batch"
                  variant="outlined"
                ></v-text-field>
              </v-col>
              <v-col :cols="4">
                <v-text-field
                  v-model="data.form.estimated_arrival_time"
                  type="date"
                  variant="outlined"
                  :label="$t('wms.stockAsnInfo.estimated_arrival_time')"
                ></v-text-field>
              </v-col>
            </v-row>

            <v-row v-for="(item, index) of data.form.detailList" :key="index" style="margin-top: 5px">
              <v-col :cols="11">
                <v-row>
                  <v-col :cols="2">
                    <v-text-field v-model="item.spu_name" :label="$t('wms.stockAsnInfo.spu_name')" variant="outlined" readonly></v-text-field>
                  </v-col>
                  <v-col :cols="2">
                    <v-text-field
                      v-model="item.spu_code"
                      :label="$t('wms.stockAsnInfo.spu_code')"
                      :rules="data.rules.spu_code"
                      variant="outlined"
                      readonly
                    ></v-text-field>
                  </v-col>
                  <v-col :cols="2">
                    <v-text-field v-model="item.sku_code" :label="$t('wms.stockAsnInfo.sku_code')" variant="outlined" readonly></v-text-field>
                  </v-col>
                  <v-col :cols="2">
                    <!-- <v-select
                      v-model="item.supplier_name"
                      :items="data.combobox.supplier_name"
                      item-title="label"
                      item-value="label"
                      :rules="data.rules.supplier_name"
                      :label="$t('wms.stockAsnInfo.supplier_name')"
                      variant="outlined"
                      clearable
                      @update:model-value="method.supplierNameChange(item)"
                    ></v-select> -->
                    <customFilterSelect
                      v-model="data.form.detailList[index]"
                      :items="data.combobox.supplier_name"
                      item-title="label"
                      :rules="data.rules.supplier_name"
                      :label="$t('wms.stockAsnInfo.supplier_name')"
                      :mapping="[
                        {
                          in: 'supplier_name',
                          out: 'label'
                        },
                        {
                          in: 'supplier_id',
                          out: 'value'
                        }
                      ]"
                    />
                  </v-col>
                  <v-col :cols="2">
                    <v-text-field
                      v-model="item.asn_qty"
                      :rules="data.rules.asn_qty"
                      :label="$t('wms.stockAsnInfo.asn_qty')"
                      variant="outlined"
                      clearable
                    ></v-text-field>
                  </v-col>
                  <v-col :cols="2">
                    <v-text-field
                      v-model="item.price"
                      :rules="data.rules.price"
                      :label="$t('wms.stockAsnInfo.price')"
                      variant="outlined"
                      clearable
                    ></v-text-field>
                  </v-col>
                </v-row>
              </v-col>
              <v-col :cols="1">
                <div class="detailComtainer">
                  <div class="detailchbContainer">
                    <v-checkbox v-model="item.is_check"></v-checkbox>
                  </div>
                  <div class="detailBtnContainer">
                    <tooltip-btn
                      :flat="true"
                      icon="mdi-delete-outline"
                      :tooltip-text="$t('system.page.delete')"
                      :icon-color="errorColor"
                      @click="method.removeItem(index, item)"
                    ></tooltip-btn>
                  </div>
                </div>
              </v-col>
            </v-row>
            <v-btn
              style="font-size: 20px; margin-bottom: 15px; margin-top: 10px; float: right"
              color="primary"
              :width="40"
              @click="method.openSelect('target')"
            >
              +
            </v-btn>
          </v-form>
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn v-show="showQRPrint" color="primary" variant="text" @click="method.printQrCode">
            {{ $t('base.commodityManagement.printQrCode') }}
          </v-btn>
          <v-btn variant="text" @click="method.closeDialog">{{ $t('system.page.close') }}</v-btn>
          <v-btn color="primary" variant="text" @click="method.submit">{{ $t('system.page.submit') }}</v-btn>
        </v-card-actions>
      </v-card>
      <skuSelect :show-dialog="data.showSkuDialogSelect" @close="method.closeDialogSelect('target')" @sureSelect="method.sureSelect" />
      <!-- Print QR code -->
      <qr-code-dialog ref="qrCodeDialogRef" :menu="'stockAsnInfo-notice'">
        <template #left="{ slotData }">
          <p>{{ $t('wms.stockAsnInfo.num') }}:{{ slotData.asn_no }}</p>
          <p>{{ $t('wms.stockAsnInfo.spu_name') }}:{{ slotData.spu_name }}</p>
          <p>{{ $t('wms.stockAsnInfo.sku_code') }}:{{ slotData.sku_code }}</p>
        </template>
      </qr-code-dialog>
    </template>
  </v-dialog>
</template>

<script lang="ts" setup>
import { reactive, computed, ref, watch } from 'vue'
import i18n from '@/languages/i18n'
import { errorColor } from '@/constant/style'
import { hookComponent } from '@/components/system/index'
import { addAsnNew, updateAsnNew } from '@/api/wms/stockAsn'
import tooltipBtn from '@/components/tooltip-btn.vue'
import skuSelect from '@/components/select/sku-select.vue'
import { CommodityDetailJoinMainVO } from '@/types/Base/CommodityManagement'
import { IsInteger, IsDecimal, StringLength } from '@/utils/dataVerification/formRule'
import { StockAsnVO, StockAsnDetailVO } from '@/types/WMS/StockAsn'
import { getSupplierAll } from '@/api/base/supplier'
import { getOwnerOfCargoAll } from '@/api/base/ownerOfCargo'
import { checkDetailRepeatGetBool } from '@/utils/dataVerification/page'
import { formatDate } from '@/utils/format/formatSystem'
import customFilterSelect from '@/components/custom-filter-select.vue'
import { removeObjectNull } from '@/utils/common'
import QrCodeDialog from '@/components/codeDialog/qrCodeDialog.vue'

const formRef = ref()
const qrCodeDialogRef = ref()
const emit = defineEmits(['close', 'saveSuccess'])

const props = defineProps<{
  showDialog: boolean
  form: StockAsnVO
}>()

const isShow = computed(() => props.showDialog)
const showQRPrint = computed(() => data.form.detailList.length > 0 && (data.form?.id as number) > 0)

const data = reactive({
  curSelectType: '',
  showSkuDialogSelect: false,
  dialogTitle: '',
  form: ref<StockAsnVO>({
    id: 0,
    asn_no: '',
    asn_batch: '',
    estimated_arrival_time: '',
    // asn_status: 0,
    // weight: 0,
    // volume: 0,
    // goods_owner_id: 0,
    // goods_owner_name: '',
    // creator: '',
    // create_time: '',
    // last_update_time: '',
    detailList: []
  }),
  removeDetailList: ref<StockAsnDetailVO[]>([]),
  combobox: ref<{
    supplier_name: {
      label: string
      value: number
    }[]
    goods_owner_name: {
      label: string
      value: number
    }[]
  }>({
    supplier_name: [],
    goods_owner_name: []
  }),
  rules: {
    goods_owner_name: [],
    estimated_arrival_time: [],
    supplier_name: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.stockAsnInfo.supplier_name') }!`],
    spu_code: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.deliveryManagement.sku_code') }!`],
    asn_qty: [
      (val: number) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.stockAsnInfo.asn_qty') }!`,
      (val: number) => IsInteger(val, 'greaterThanZero') === '' || IsInteger(val, 'greaterThanZero')
    ],
    price: [(val: number) => IsDecimal(val, 'nonNegative', 10, 2) === '' || IsDecimal(val, 'nonNegative', 10, 2)],
    asn_batch: [
      // (val: string) => !!val || `${i18n.global.t('system.checkText.mustInput')}${i18n.global.t('wms.stockAsnInfo.asn_batch')}!`,
      (val: string) => StringLength(val, 0, 64) === '' || StringLength(val, 0, 64)
    ]
  }
})

const method = reactive({
  // Print QR code
  printQrCode: async () => {
    const records = data.form.detailList.filter((item: StockAsnDetailVO) => item.is_check) as any[]
    // data.selectRowData.length === 0 ? data.selectRowData = [row] : ''
    // const records:any[] = data.selectRowData

    if (records.length > 0) {
      // const list = records.map((item) => item.id)
      // const { data: res } = await getPrintAsnList(list)
      // if (!res.isSuccess) {
      //   hookComponent.$message({
      //     type: 'error',
      //     content: res.errorMessage
      //   })
      //   return
      // }
      const printList = records.map((item) => ({
        id: item.id,
        asn_id: item.id,
        asnmaster_id: item.asnmaster_id,
        type: 'asn',
        asn_no: data.form.asn_no,
        spu_name: item.spu_name,
        sku_code: item.sku_code,
        sku_id: item.sku_id
      }))
      // const printList = res.data.map((item) => ({ id: item.asn_id, type: 'asn', ...item }))
      qrCodeDialogRef.value.openDialog(printList)
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('base.userManagement.checkboxIsNull')
      })
    }
  },
  supplierNameChange: (dItem: StockAsnDetailVO) => {
    if (dItem.supplier_name) {
      dItem.supplier_id = data.combobox.supplier_name.filter((item) => item.label === dItem.supplier_name)[0].value
    } else {
      dItem.supplier_id = 0
    }
  },
  goodsOwnerNameChange: (val: string) => {
    if (!val) {
      data.form.goods_owner_id = 0
      data.form.goods_owner_name = ''
    } else {
      data.form.goods_owner_id = data.combobox.goods_owner_name.filter((item) => item.label === val)[0].value
    }
  },
  openSelect: (type: string) => {
    data.curSelectType = type

    if (type === 'target') {
      data.showSkuDialogSelect = true
    }
  },
  sureSelect: (selectRecords: CommodityDetailJoinMainVO[]) => {
    if (data.curSelectType === 'target') {
      // 获取最后一条有供应商的数据, 自动带到后面的数据
      let hasSupplierItem: any = null

      for (const item of data.form.detailList.reverse()) {
        if (item.supplier_id && item.supplier_id > 0) {
          hasSupplierItem = item
          break
        }
      }

      for (const item of selectRecords) {
        // const index = data.form.detailList.findIndex((fi) => fi.sku_id === item.sku_id)
        // if (index === -1) {
        const newItem: any = {
          spu_id: item.spu_id,
          spu_code: item.spu_code,
          spu_name: item.spu_name,
          sku_id: item.sku_id,
          sku_code: item.sku_code,
          sku_name: item.sku_name,
          origin: item.origin,
          // length_unit: '',
          // volume_unit: '',
          // weight_unit: '',
          asn_qty: 0,
          price: 0
          // actual_qty: '',
          // weight: '',
          // volume: '',
          // supplier_id: '',
          // supplier_name: ''
          // is_valid: ''
        }

        if (hasSupplierItem) {
          newItem.supplier_id = hasSupplierItem.supplier_id
          newItem.supplier_name = hasSupplierItem.supplier_name
        }

        data.form.detailList.push(newItem)
        // }
      }
    }
  },
  closeDialogSelect: (type: string) => {
    if (type === 'target') {
      data.showSkuDialogSelect = false
    }
  },
  // Get the options required by the drop-down box
  getCombobox: async () => {
    data.combobox.supplier_name = []
    data.combobox.goods_owner_name = []
    const { data: supplierRes } = await getSupplierAll()
    if (!supplierRes.isSuccess) {
      return
    }
    for (const item of supplierRes.data) {
      if (item.is_valid) {
        data.combobox.supplier_name.push({
          label: item.supplier_name,
          value: item.id
        })
      }
    }
    const { data: goodsOwnerRes } = await getOwnerOfCargoAll()
    if (!goodsOwnerRes.isSuccess) {
      return
    }
    for (const item of goodsOwnerRes.data) {
      if (item.is_valid) {
        data.combobox.goods_owner_name.push({
          label: item.goods_owner_name,
          value: item.id
        })
      }
    }
  },
  closeDialog: () => {
    emit('close')
  },
  // Details verification before document submission
  beforeSubmit: (): boolean => {
    let flag = true
    if (data.form.detailList.length === 0) {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('system.tips.detailLengthIsZero')
      })
      flag = false
    } else if (checkDetailRepeatGetBool(data.form.detailList, ['sku_id', 'supplier_id'])) {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('system.tips.detailHasItemRepeat')
      })
      flag = false
    }
    return flag
  },
  submit: async () => {
    const beforeSubmitFlag = method.beforeSubmit()
    if (!beforeSubmitFlag) {
      return
    }
    const { valid } = await formRef.value.validate()
    if (valid) {
      const form = removeObjectNull(data.form)
      const { data: res } = form.id && form.id > 0 ? await updateAsnNew({ ...form, detailList: [...form.detailList, ...data.removeDetailList] }) : await addAsnNew(form)
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
  // remove detail
  removeItem: (index: number, item: StockAsnDetailVO) => {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteDetailMessage'),
      handleConfirm: async () => {
        if (item.id && item.id > 0) {
          item.id = -item.id
          data.removeDetailList.push(item) // Cache remove row
        }
        data.form.detailList.splice(index, 1) // Remove row in detailList
      }
    })
  }
})

watch(
  () => isShow.value,
  (val) => {
    if (val) {
      method.getCombobox()
      data.form = props.form

      // Processing time is date
      if (data.form.estimated_arrival_time) {
        data.form.estimated_arrival_time = formatDate(data.form.estimated_arrival_time, 'yyyy-MM-dd')
      }
    }
  }
)
</script>

<style scoped lang="less">
// .v-form {
//   div {
//     margin-bottom: 7px;
//   }
// }
.detailComtainer {
  margin-left: -10px;
  width: 90px;
  display: flex;
  justify-content: space-between;
}
.detailchbContainer {
  height: 40px;
}
.detailBtnContainer {
  height: 56px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}
</style>
