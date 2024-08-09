<template>
  <v-dialog v-model="isShow" :width="'80%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card class="formCard">
        <v-toolbar color="white" :title="`${$t('wms.deliveryManagement.confirmOrder')}`"></v-toolbar>
        <v-card-text>
          <v-row>
            <v-col :cols="5">
              <vxe-table
                ref="xTable"
                keep-source
                :column-config="{ minWidth: '100px' }"
                :data="data.treeData"
                :height="'600px'"
                align="center"
                :cell-style="method.cellStyle"
                @cell-click="method.cellClick"
              >
                <template #empty>
                  {{ i18n.global.t('system.page.noData') }}
                </template>
                <vxe-column type="seq" width="40"></vxe-column>
                <vxe-column width="40">
                  <template #default="{ row }">
                    <div style="height: 100%; display: flex; align-items: center; justify-content: center">
                      <!-- <input v-model="row.confirm" class="checkboxClass" type="checkbox" /> -->
                      <CustomCheckbox v-model:value="row.confirm" />
                    </div>
                  </template>
                </vxe-column>
                <vxe-column field="spu_name" :title="$t('wms.deliveryManagement.spu_name')"> </vxe-column>
                <vxe-column field="spu_code" :title="$t('wms.deliveryManagement.spu_code')"> </vxe-column>
                <vxe-column field="sku_code" :title="$t('wms.deliveryManagement.sku_code')">
                  <template #default="{ row }">
                    <span :style="data.validList.includes(row.sku_code) ? 'color: red' : ''">{{ row.sku_code }}</span>
                  </template>
                </vxe-column>
                <vxe-column field="qty" :title="$t('wms.deliveryManagement.qty')"> </vxe-column>
              </vxe-table>
              <!-- <v-card :height="tableHeight"> -->
              <!-- <NavListVue
                  :list-data="data.treeData"
                  :title="data.navListOptions.title"
                  :label-key="data.navListOptions.labelKey"
                  :index-key="data.navListOptions.indexKey"
                  :index-value="data.navListOptions.indexValue"
                  @item-click="method.navListClick"
                /> -->
              <!-- </v-card> -->
            </v-col>
            <v-col :cols="7">
              <vxe-table
                ref="detailXTable"
                keep-source
                :column-config="{ minWidth: '100px' }"
                :data="data.tableData"
                :height="'600px'"
                align="center"
                :edit-config="{ trigger: 'click', mode: 'cell' }"
                :edit-rules="data.validRules"
              >
                <template #empty>
                  {{ i18n.global.t('system.page.noData') }}
                </template>
                <vxe-column type="seq" width="60"></vxe-column>
                <vxe-column field="warehouse_name" :title="$t('wms.stock.warehouse')"> </vxe-column>
                <vxe-column field="location_name" :title="$t('wms.stock.location_name')"> </vxe-column>
                <vxe-column field="goods_owner_name" :title="$t('base.ownerOfCargo.goods_owner_name')"> </vxe-column>
                <vxe-column field="qty_available" :title="$t('wms.deliveryManagement.qty_available')"></vxe-column>
                <vxe-column field="series_number" :title="$t('wms.stockLocation.series_number')"></vxe-column>
                <vxe-column field="pick_qty" :title="$t('wms.deliveryManagement.detailQty')" :edit-render="{}">
                  <template #edit="{ row }">
                    <vxe-input v-model="row.pick_qty" type="text"></vxe-input>
                  </template>
                </vxe-column>
                <vxe-column field="price" :title="$t('wms.stockAsnInfo.price')"></vxe-column>
                <vxe-date-column field="expiry_date" :title="$t('wms.stockAsnInfo.expiry_date')"></vxe-date-column>
                <vxe-date-column field="putaway_date" :title="$t('wms.stockAsnInfo.putaway_date')"></vxe-date-column>                
              </vxe-table>
            </v-col>
          </v-row>
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
import { ConfirmOrderVO, ConfirmOrderPickListVO } from '@/types/DeliveryManagement/DeliveryManagement'
import i18n from '@/languages/i18n'
import { getConfirmOrderInfoAndStock, confirmOrder } from '@/api/wms/deliveryManagement'
import { hookComponent } from '@/components/system/index'
import { isInteger } from '@/utils/dataVerification/tableRule'
import CustomCheckbox from '@/components/custom-checkbox.vue'
import { httpCodeJudge } from '@/utils/http/httpCodeJudge'

const xTable = ref()
const detailXTable = ref()

const emit = defineEmits(['close', 'saveSuccess'])

const props = defineProps<{
  showDialog: boolean
  confirmOrderNo: string
}>()

const isShow = computed(() => props.showDialog)

const data = reactive({
  navListOptions: {
    title: i18n.global.t('wms.deliveryManagement.orderDetail'),
    labelKey: 'sku_code',
    indexKey: 'sku_code',
    indexValue: ''
  },
  treeData: ref<ConfirmOrderVO[]>([]),
  tableData: ref<ConfirmOrderPickListVO[]>([]),
  // List of verification results for each commodity
  validList: ref<string[]>([]),
  validRules: ref<any>({
    pick_qty: [
      {
        validator: isInteger,
        validNumerical: 'nonNegative',
        trigger: 'change'
      }
    ]
  })
})

const method = reactive({
  closeDialog: () => {
    emit('close')
  },
  beforeSubmit: (): boolean => {
    // let flag = true
    data.validList = []
    const msgList: string[] = []
    let orderCount = 0
    if (data.treeData.filter((fl) => fl.confirm).length <= 0) {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('wms.deliveryManagement.NoItemSelected')
      })
      return false
    }
    for (const item of data.treeData) {
      if (item.confirm) {
        let count = 0
        for (const dItem of item.pick_list) {
          count += Number(dItem.pick_qty)
        }
        orderCount += count
        if (count > item.qty) {
          data.validList.push(item.sku_code)
          msgList.push(`${ item.sku_code } ${ i18n.global.t('wms.deliveryManagement.quantityOverflow') } ${ item.qty }`)
          // flag = false
        }
        if (count === 0) {
          if (!data.validList.includes(item.sku_code)) {
            data.validList.push(item.sku_code)
          }
          msgList.push(`${ item.sku_code } ${ i18n.global.t('wms.deliveryManagement.detailQuantityIsZero') }!`)
        }
      }
    }
    // return flag
    if (msgList.length > 0) {
      hookComponent.$message({
        type: 'error',
        content: msgList.join(';\n')
      })
      return false
    }
    if (orderCount <= 0) {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('wms.deliveryManagement.quantityIsZero')
      })
      return false
    }
    return true
  },
  submit: async () => {
    const $table = detailXTable.value
    const errMap = await $table.validate(true)
    if (errMap) {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('system.checkText.checkFormFail')
      })
      return
    }
    if (method.beforeSubmit()) {
      // console.log(data.treeData)
      const { data: res } = await confirmOrder(data.treeData)
      if (!res.isSuccess) {
        // 2023-12-06 Add automatic refresh of expired data
        if (httpCodeJudge(res.errorMessage, false)) {
          hookComponent.$message({
            type: 'error',
            content: i18n.global.t('system.tips.dataExpiration')
          })
          return
        }

        hookComponent.$message({
          type: 'error',
          content: res.errorMessage
        })
        return
      }
      hookComponent.$message({
        type: 'success',
        content: i18n.global.t('wms.deliveryManagement.confirmSuccess')
      })
      emit('saveSuccess')
    }
  },
  getInfoByNo: async () => {
    const { data: res } = await getConfirmOrderInfoAndStock(props.confirmOrderNo)
    if (!res.isSuccess) {
      return
    }
    data.treeData = res.data
    data.tableData = []
    if (data.treeData.length > 0) {
      method.cellClick({ row: data.treeData[0] })
    }
  },
  cellClick: ({ row }: { row: ConfirmOrderVO }) => {
    data.navListOptions.indexValue = row.sku_code
    data.tableData = row.pick_list
  },
  // When clicking the left tree
  navListClick: (item: ConfirmOrderVO) => {
    data.navListOptions.indexValue = item.sku_code
    data.tableData = item.pick_list
  },
  cellStyle: ({ row }: { row: ConfirmOrderVO }) => {
    const style: any = { cursor: 'pointer' }
    if (row.sku_code === data.navListOptions.indexValue) {
      style.backgroundColor = '#e6dafd'
    }
    return style
  }
})

watch(
  () => isShow.value,
  (val) => {
    if (val) {
      method.getInfoByNo()
    }
  }
)
</script>

<style scoped lang="less">
.detailBtnContainer {
  height: 56px;
  display: flex;
  justify-content: center;
  align-items: center;
}
</style>
