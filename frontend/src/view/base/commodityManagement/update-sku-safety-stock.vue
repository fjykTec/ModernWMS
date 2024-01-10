<template>
  <v-dialog v-model="data.showDialog" width="50%" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('router.sideBar.commodityManagement')}`"></v-toolbar>
        <v-card-text>
          <div>
            <v-row>
              <v-col cols="6">
                <v-text-field
                  v-model="data.skuInfo.sku_code"
                  :label="$t('base.commodityManagement.sku_code')"
                  variant="outlined"
                  density="compact"
                  readonly
                ></v-text-field>
              </v-col>
              <v-col cols="6">
                <v-text-field
                  v-model="data.skuInfo.sku_name"
                  :label="$t('base.commodityManagement.sku_name')"
                  variant="outlined"
                  density="compact"
                  readonly
                ></v-text-field>
              </v-col>
            </v-row>
          </div>
          <div>
            <v-form ref="formRef">
              <v-row v-for="(item, index) of data.detailList" :key="index" style="margin-top: 5px">
                <v-col :cols="7">
                  <v-text-field
                    v-model="item.warehouse_name"
                    :label="$t('wms.deliveryManagement.warehouse_name')"
                    :rules="data.rules.warehouse_name"
                    variant="outlined"
                    readonly
                  ></v-text-field>
                </v-col>
                <v-col :cols="3">
                  <v-text-field
                    v-model="item.safety_stock_qty"
                    :rules="data.rules.safety_stock_qty"
                    :label="$t('wms.deliveryManagement.detailQty')"
                    variant="outlined"
                    clearable
                  ></v-text-field>
                </v-col>
                <v-col :cols="2">
                  <div class="detailBtnContainer">
                    <tooltip-btn
                      :flat="true"
                      icon="mdi-delete-outline"
                      :tooltip-text="$t('system.page.delete')"
                      :icon-color="errorColor"
                      @click="method.deleteRow(index, item)"
                    ></tooltip-btn>
                  </div>
                </v-col>
              </v-row>
            </v-form>

            <v-btn style="font-size: 20px; margin-bottom: 15px; margin-top: 10px; float: right" color="primary" :width="40" @click="method.addNewRow">
              +
            </v-btn>
          </div>
        </v-card-text>

        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="method.closeDialog">{{ $t('system.page.close') }}</v-btn>
          <v-btn color="primary" variant="text" @click="method.sureBack">{{ $t('system.page.submit') }}</v-btn>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>

  <warehouse-select ref="warehouseSelectRef" @sure-select="method.warehouseSureSelect" />
</template>

<script setup lang="tsx">
import { reactive, ref, nextTick } from 'vue'
import { CommodityDetailVO, UpdateSaftyStockReqBodyVO } from '@/types/Base/CommodityManagement'
import { errorColor } from '@/constant/style'
import tooltipBtn from '@/components/tooltip-btn.vue'
import { hookComponent } from '@/components/system/index'
import i18n from '@/languages/i18n'
import warehouseSelect from '@/components/select/warehouse-select.vue'
import { WarehouseVO } from '@/types/Base/Warehouse'
import { IsInteger } from '@/utils/dataVerification/formRule'

const emit = defineEmits(['sure'])

const warehouseSelectRef = ref()
const formRef = ref()

const data = reactive({
  showDialog: false as boolean,
  detailList: [] as UpdateSaftyStockReqBodyVO[],
  rules: {
    warehouse_name: [],
    safety_stock_qty: [
      (val: number) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.deliveryManagement.detailQty') }!`,
      (val: number) => IsInteger(val, 'nonNegative') === '' || IsInteger(val, 'nonNegative')
    ]
  },
  skuInfo: {} as CommodityDetailVO,
  cacheDeleteDetail: [] as UpdateSaftyStockReqBodyVO[]
})

const method = reactive({
  openDialog: (row: CommodityDetailVO) => {
    data.cacheDeleteDetail = []

    data.showDialog = true
    nextTick(() => {
      data.skuInfo = row

      data.detailList = row.detailList
    })
  },
  closeDialog: () => {
    data.showDialog = false
  },
  addNewRow: () => {
    warehouseSelectRef.value.openDialog()
  },
  deleteRow: (index: number, item: UpdateSaftyStockReqBodyVO) => {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteDetailMessage'),
      handleConfirm: async () => {
        if (item.id && item.id > 0) {
          const delItem = { ...item }
          delItem.id = 0 - item.id
          data.cacheDeleteDetail.push(delItem)
        }

        data.detailList.splice(index, 1)
      }
    })
  },
  // After selection
  warehouseSureSelect: (list: WarehouseVO[]) => {
    for (const item of list) {
      const fl = data.detailList.filter((i: UpdateSaftyStockReqBodyVO) => i.warehouse_id === item.id)

      // If it already exists, do not insert it
      if (fl.length === 0) {
        data.detailList.push({
          id: 0,
          sku_id: data.skuInfo.id,
          warehouse_id: item.id,
          warehouse_name: item.warehouse_name,
          safety_stock_qty: 0
        })
      }
    }
  },
  // Confirm Save
  sureBack: async () => {
    const { valid } = await formRef.value.validate()

    if (valid) {
      emit('sure', {
        sku_id: data.skuInfo.id,
        detailList: [...data.detailList, ...data.cacheDeleteDetail]
      })
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('system.checkText.checkFormFail')
      })
    }
  }
})

defineExpose({
  openDialog: method.openDialog,
  closeDialog: method.closeDialog
})
</script>

<style scoped lang="less">
.mainForm {
  background-color: #f9f9f9;
  border-radius: 5px;
  padding: 20px;
  box-sizing: border-box;
  overflow: auto;
}
.detailBtnContainer {
  height: 56px;
  display: flex;
  justify-content: center;
  align-items: center;
}
</style>
