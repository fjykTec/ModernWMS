<template>
  <v-dialog v-model="isShow" width="80%" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="jobTypeComp">
          <custom-qrcode :value="qrCodeValue" />
        </v-toolbar>
        <v-card-text>
          <v-row>
            <!-- Source Table -->
            <v-col cols="6">
              <div class="dataTable">
                <div class="toolbar">
                  <div class="toolbarTitle">
                    <p style="color: #999">{{ $t('wms.warehouseWorking.warehouseProcessing.source') }}</p>
                  </div>
                  <tooltip-btn
                    icon="mdi-plus"
                    :tooltip-text="$t('system.page.add')"
                    size="x-small"
                    :disabled="operateDisabled"
                    @click="method.openSelect('source')"
                  ></tooltip-btn>
                </div>
                <vxe-table
                  ref="xTableSource"
                  :column-config="{ minWidth: '100px' }"
                  :data="data.form.source_detail_list"
                  :height="SYSTEM_HEIGHT.SELECT_TABLE"
                  :edit-config="{ trigger: 'click', mode: 'cell' }"
                  :edit-rules="data.validRulesSource"
                  align="center"
                >
                  <template #empty>
                    {{ i18n.global.t('system.page.noData') }}
                  </template>
                  <vxe-column type="seq" width="60"></vxe-column>
                  <vxe-column field="operate" width="50" :title="$t('system.page.operate')" :resizable="false">
                    <template #default="{ row }">
                      <tooltip-btn
                        :flat="true"
                        icon="mdi-delete-outline"
                        :tooltip-text="$t('system.page.delete')"
                        :icon-color="errorColor"
                        @click="method.deleteRowSource(row)"
                      ></tooltip-btn>
                    </template>
                  </vxe-column>
                  <vxe-column field="spu_code" :title="$t('wms.warehouseWorking.warehouseProcessing.spu_code')"></vxe-column>
                  <vxe-column field="spu_name" :title="$t('wms.warehouseWorking.warehouseProcessing.spu_name')"></vxe-column>
                  <vxe-column field="sku_code" :title="$t('wms.warehouseWorking.warehouseProcessing.sku_code')"></vxe-column>
                  <vxe-column
                    field="qty"
                    :title="$t('wms.warehouseWorking.warehouseProcessing.qty')"
                    :edit-render="{ autofocus: '.vxe-input--inner' }"
                  >
                    <template #edit="{ row }">
                      <vxe-input v-model="row.qty" type="text"></vxe-input>
                    </template>
                  </vxe-column>
                  <vxe-column field="unit" :title="$t('wms.warehouseWorking.warehouseProcessing.unit')"></vxe-column>
                  <vxe-column field="series_number" :title="$t('wms.stockLocation.series_number')"></vxe-column>
                  <vxe-column field="price" :title="$t('wms.warehouseWorking.warehouseProcessing.price')"></vxe-column>
                  <vxe-date-column field="expiry_date" :title="$t('wms.warehouseWorking.warehouseProcessing.expiry_date')"></vxe-date-column>
                  <vxe-date-column field="putaway_date" :title="$t('wms.warehouseWorking.warehouseProcessing.putaway_date')"></vxe-date-column>
                </vxe-table>
              </div>
            </v-col>

            <!-- Target Table -->
            <v-col cols="6">
              <div class="dataTable">
                <div class="toolbar">
                  <div class="toolbarTitle">
                    <p style="color: #999">{{ $t('wms.warehouseWorking.warehouseProcessing.target') }}</p>
                  </div>
                  <tooltip-btn
                    icon="mdi-plus"
                    :tooltip-text="$t('system.page.add')"
                    size="x-small"
                    :disabled="operateDisabled"
                    @click="method.openSelect('target')"
                  ></tooltip-btn>
                </div>
                <vxe-table
                  ref="xTableTarget"
                  :column-config="{ minWidth: '100px' }"
                  :data="data.form.target_detail_list"
                  :height="SYSTEM_HEIGHT.SELECT_TABLE"
                  :edit-config="{ trigger: 'click', mode: 'cell' }"
                  :edit-rules="data.validRulesTarget"
                  align="center"
                >
                  <template #empty>
                    {{ i18n.global.t('system.page.noData') }}
                  </template>
                  <vxe-column type="seq" width="60"></vxe-column>
                  <vxe-column field="operate" width="50" :title="$t('system.page.operate')" :resizable="false">
                    <template #default="{ row }">
                      <tooltip-btn
                        :flat="true"
                        icon="mdi-delete-outline"
                        :tooltip-text="$t('system.page.delete')"
                        :icon-color="errorColor"
                        @click="method.deleteRowTarget(row)"
                      ></tooltip-btn>
                    </template>
                  </vxe-column>
                  <vxe-column field="spu_code" :title="$t('wms.warehouseWorking.warehouseProcessing.spu_code')"></vxe-column>
                  <vxe-column field="spu_name" :title="$t('wms.warehouseWorking.warehouseProcessing.spu_name')"></vxe-column>
                  <vxe-column field="sku_code" :title="$t('wms.warehouseWorking.warehouseProcessing.sku_code')"></vxe-column>
                  <vxe-column
                    field="expiry_date"
                    :title="$t('wms.warehouseWorking.warehouseProcessing.expiry_date')"
                    :edit-render="{ autofocus: '.vxe-input--inner' }"
                  >
                    <template #edit="{ row }">
                      <vxe-input v-model="row.expiry_date" type="date"></vxe-input>
                    </template>
                  </vxe-column>
                  <!-- <vxe-column
                    field="putaway_date"
                    :title="$t('wms.warehouseWorking.warehouseProcessing.putaway_date')"
                    :edit-render="{ autofocus: '.vxe-input--inner' }"
                  >
                    <template #edit="{ row }">
                      <vxe-input v-model="row.putaway_date" type="date"></vxe-input>
                    </template>
                  </vxe-column> -->
                  <vxe-column
                    field="qty"
                    :title="$t('wms.warehouseWorking.warehouseProcessing.qty')"
                    :edit-render="{ autofocus: '.vxe-input--inner' }"
                  >
                    <template #edit="{ row }">
                      <vxe-input v-model="row.qty" type="text"></vxe-input>
                    </template>
                  </vxe-column>
                  <vxe-column
                    field="price"
                    :title="$t('wms.warehouseWorking.warehouseProcessing.price')"
                    :edit-render="{ autofocus: '.vxe-input--inner' }"
                  >
                    <template #edit="{ row }">
                      <vxe-input v-model="row.price" type="text"></vxe-input>
                    </template>
                  </vxe-column>

                  <vxe-column field="unit" width="60" :title="$t('wms.warehouseWorking.warehouseProcessing.unit')"></vxe-column>
                  <vxe-column field="location_name" :title="$t('wms.warehouseWorking.warehouseProcessing.target_location')" :edit-render="{}">
                    <template #edit="{ row }">
                      <vxe-input v-model="row.location_name" readonly type="search" @search-click="method.openLocationSelect(row)"></vxe-input>
                    </template>
                  </vxe-column>
                </vxe-table>
              </div>
            </v-col>
          </v-row>
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="method.closeDialog">{{ $t('system.page.close') }}</v-btn>
          <v-btn color="primary" variant="text" :disabled="operateDisabled" @click="method.submit">{{ $t('system.page.submit') }}</v-btn>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>

  <commodity-select :show-dialog="data.showCommodityDialogSelect" @close="method.closeDialogSelect('source')" @sureSelect="method.sureSelect" />
  <sku-select :show-dialog="data.showSkuDialogSelect" @close="method.closeDialogSelect('target')" @sureSelect="method.sureSelect" />
  <location-select :show-dialog="data.showLocationDialogSelect" @close="method.closeLocationDialogSelect" @sureSelect="method.sureSelectLocation" />
</template>

<script lang="ts" setup>
import { reactive, computed, ref, watch } from 'vue'
import { VxeTablePropTypes } from 'vxe-table'
import { privateDecrypt } from 'crypto'
import { WarehouseProcessingVO, WarehouseProcessingDetailVO } from '@/types/WarehouseWorking/WarehouseProcessing'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { addStockProcess } from '@/api/wms/warehouseProcessing'
import { SYSTEM_HEIGHT, errorColor } from '@/constant/style'
import { removeObjectNull, removeArrayNull } from '@/utils/common'
import { PROCESS_JOB_COMBINE, PROCESS_JOB_SPLIT } from '@/constant/warehouseWorking'
import commoditySelect from '@/components/select/commodity-select.vue'
import locationSelect from '@/components/select/location-select.vue'
import skuSelect from '@/components/select/sku-select.vue'
import tooltipBtn from '@/components/tooltip-btn.vue'
import customQrcode from '@/components/custom-qrcode.vue'
import { exportData } from '@/utils/exportTable'
import { isInteger, isDecimal } from '@/utils/dataVerification/tableRule'

const emit = defineEmits(['close', 'saveSuccess'])
const xTableSource = ref()
const xTableTarget = ref()

const props = defineProps<{
  showDialog: boolean
  form: WarehouseProcessingVO
  processType: boolean
}>()

const isShow = computed(() => props.showDialog)
const isUpdate = computed(() => props.form.id && props.form.id > 0)
const jobTypeComp = computed(() => (data.form.job_type === PROCESS_JOB_COMBINE
    ? i18n.global.t('wms.warehouseWorking.warehouseProcessing.process_combine')
    : i18n.global.t('wms.warehouseWorking.warehouseProcessing.process_split')))
const operateDisabled = computed(() => !!isUpdate.value)
const qrCodeValue = computed(() => JSON.stringify({
    id: data.form.id.toString(),
    type: 'warehouseProcessing'
  }))

const method = reactive({
  initForm: () => {
    data.form = props.form
    data.form.job_type = props.processType
  },

  closeDialog: () => {
    emit('close')
  },

  openSelect: (type: string) => {
    data.curSelectType = type

    if (type === 'source') {
      data.showCommodityDialogSelect = true
    } else if (type === 'target') {
      // Target should select the data with 'sku-select-modal'
      data.showSkuDialogSelect = true
    }
  },

  closeDialogSelect: (type: string) => {
    if (type === 'source') {
      data.showCommodityDialogSelect = false
    } else if (type === 'target') {
      // Target should select the data with 'sku-select-modal'
      data.showSkuDialogSelect = false
    }
  },

  sureSelect: (selectRecords: any) => {
    if (data.curSelectType === 'source') {
      method.insertSourceData(selectRecords)
    } else if (data.curSelectType === 'target') {
      method.insertTargetData(selectRecords)
    }
  },

  openLocationSelect: (row: WarehouseProcessingDetailVO) => {
    data.curSelectRow = row
    data.showLocationDialogSelect = true
  },

  closeLocationDialogSelect: () => {
    data.showLocationDialogSelect = false
  },

  sureSelectLocation: (selectRecords: any) => {
    if (selectRecords.length > 0) {
      const $table = xTableTarget.value
      const tableData = $table.getTableData().fullData
      tableData.forEach((row: WarehouseProcessingDetailVO) => {
        if (data.curSelectRow.sku_id === row.sku_id) {
          row.goods_location_id = selectRecords[0].id
          row.location_name = selectRecords[0].location_name
        }
      })
      // Tips: Must to reload!
      $table.reloadData(tableData)
    }
  },

  insertSourceData: (selectRecords: any) => {
    const $table = xTableSource.value
    const tableData = $table.getTableData().fullData

    // Combine: That can select more source commodity
    if (data.form.job_type === PROCESS_JOB_COMBINE) {
      for (const record of selectRecords) {
        const isRepeat = tableData.some((data: WarehouseProcessingDetailVO) => data.sku_id === record.sku_id)
        if (isRepeat) {
          continue
        }

        $table.insertAt(
          {
            id: 0,
            stock_process_id: 0,
            sku_id: record.sku_id,
            goods_owner_id: record.goods_owner_id,
            goods_location_id: record.goods_location_id,
            qty: record.qty_available || 0,
            tenant_id: 0,
            is_source: true,
            spu_code: record.spu_code,
            spu_name: record.spu_name,
            sku_code: record.sku_code,
            series_number: record.series_number,
            unit: record.unit,
            is_update_stock: false,
            qty_available: record.qty_available,
            price: record.price,
            expiry_date: record.expiry_date,
            putaway_date: record.putaway_date
          },
          -1
        )
        // })
      }
    } else if (data.form.job_type === PROCESS_JOB_SPLIT) {
      // Split: That just can select one source commodity.
      // It should remove all data before insert.
      $table.remove()
      $table.insertAt(
        {
          id: 0,
          stock_process_id: 0,
          sku_id: selectRecords[0].sku_id,
          goods_owner_id: selectRecords[0].goods_owner_id,
          goods_location_id: selectRecords[0].goods_location_id,
          qty: selectRecords[0].qty_available || 0,
          tenant_id: 0,
          is_source: true,
          spu_code: selectRecords[0].spu_code,
          spu_name: selectRecords[0].spu_name,
          sku_code: selectRecords[0].sku_code,
          series_number: selectRecords[0].series_number,
          unit: selectRecords[0].unit,
          is_update_stock: false,
          qty_available: selectRecords[0].qty_available,
          price: selectRecords[0].price,
          expiry_date: selectRecords[0].expiry_date,
          putaway_date: selectRecords[0].putaway_date
        },
        -1
      )
    }
  },

  insertTargetData: (selectRecords: any) => {
    const $table = xTableTarget.value
    const tableData = $table.getTableData().fullData

    // Combine: That just can select one target commodity
    if (data.form.job_type === PROCESS_JOB_COMBINE) {
      // It should remove all data before insert.
      $table.remove()
      $table.insertAt(
        {
          id: 0,
          stock_process_id: 0,
          sku_id: selectRecords[0].sku_id,
          goods_owner_id: 0,
          goods_location_id: 0,
          qty: 0,
          tenant_id: 0,
          is_source: false,
          spu_code: selectRecords[0].spu_code,
          spu_name: selectRecords[0].spu_name,
          sku_code: selectRecords[0].sku_code,
          unit: selectRecords[0].unit,
          is_update_stock: false,
          price: 0,
          expiry_date: '',
          putaway_date: ''
        },
        -1
      )
    } else if (data.form.job_type === PROCESS_JOB_SPLIT) {
      // Split: That can select more target commodity
      for (const record of selectRecords) {
        const isRepeat = tableData.some((data: WarehouseProcessingDetailVO) => data.sku_id === record.sku_id)
        if (isRepeat) {
          continue
        }
        $table.insertAt(
          {
            id: 0,
            stock_process_id: 0,
            sku_id: record.sku_id,
            goods_owner_id: 0,
            goods_location_id: 0,
            qty: 0,
            tenant_id: 0,
            is_source: false,
            spu_code: record.spu_code,
            spu_name: record.spu_name,
            sku_code: record.sku_code,
            unit: record.unit,
            is_update_stock: false,
            price: 0,
            expiry_date: '',
            putaway_date: ''
          },
          -1
        )
      }
    }
  },

  // Export table
  exportTable: (type: string) => {
    const $table = type === 'source' ? xTableSource.value : xTableTarget.value
    exportData({
      table: $table,
      filename: i18n.global.t('router.sideBar.commodityManagement'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },

  submit: async () => {
    const validSource = await method.validSourceTable()
    const validTarget = await method.validTargetTable()
    
    if (!validSource || !validTarget) {
      return
    }

    const form = method.constructFormBody()

    const { data: res } = await addStockProcess(form)
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
  },

  constructFormBody: () => {
    const $tableSource = xTableSource.value
    const $tableTarget = xTableTarget.value

    const tableSourceRecords = $tableSource.getTableData().fullData
    const tableTargetRecords = $tableTarget.getTableData().fullData

    // Need combine source and target to server.
    let form = { ...data.form }
    form.detailList = [...tableSourceRecords, ...tableTargetRecords]
    form = removeObjectNull(form)
    form.detailList = removeArrayNull(form.detailList)

    delete form.source_detail_list
    delete form.target_detail_list

    return form
  },

  validSourceTable: async () => {
    const $table = xTableSource.value
    const tableData = $table.getTableData().fullData

    // 1.The table must have data.
    if (!tableData.length) {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('system.tips.detailLengthIsZero')
      })
      return false
    }

    // 2.The properties valid.
    const errMap = await $table.validate(true)
    
    if (errMap) {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('system.checkText.checkFormFail')
      })
      return false
    }

    return true
  },

  validTargetTable: async () => {
    const $table = xTableTarget.value
    const tableData = $table.getTableData().fullData

    // 1.The table must have data.
    if (!tableData.length) {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('system.tips.detailLengthIsZero')
      })
      return false
    }

    // 2.The properties valid.
    const errMap = await $table.validate(true)
    
    if (errMap) {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('system.checkText.checkFormFail')
      })
      return false
    }

    return true
  },

  // The 'qty' can't more than 'qty_available'
  validQty: ({ cellValue, row }: any) => {
    const qty = cellValue || 0
    const qtyAvailable = row.qty_available || 0

    if (qty > qtyAvailable) {
      return new Error(`${ i18n.global.t('wms.warehouseWorking.warehouseProcessing.qtyMoreThanAvailable') } ${ qtyAvailable }`)
    }
  },

  deleteRowSource: (row: WarehouseProcessingDetailVO) => {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteDetailMessage'),
      handleConfirm: async () => {
        if (row) {
          const $table = xTableSource.value
          $table.remove(row)
        }
      }
    })
  },

  deleteRowTarget: (row: WarehouseProcessingDetailVO) => {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteDetailMessage'),
      handleConfirm: async () => {
        if (row) {
          const $table = xTableTarget.value
          $table.remove(row)
        }
      }
    })
  }
})

const data = reactive({
  tableData: [],
  // 'source' | 'target'
  curSelectType: '',

  showCommodityDialogSelect: false,
  showSkuDialogSelect: false,
  showLocationDialogSelect: false,

  form: ref<WarehouseProcessingVO>({
    id: 0,
    job_code: '',
    job_type: PROCESS_JOB_COMBINE,
    process_status: false,
    processor: '',
    process_time: '',
    source_detail_list: [],
    target_detail_list: []
  }),
  curSelectRow: ref<WarehouseProcessingDetailVO>({
    id: 0,
    stock_process_id: 0,
    sku_id: 0,
    goods_owner_id: 0,
    goods_location_id: 0,
    qty: 0,
    is_source: true,
    spu_code: '',
    spu_name: '',
    sku_code: '',
    unit: '',
    is_update_stock: false
  }),
  validRulesSource: ref<any>({
    qty: [
      { required: true, message: `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.warehouseWorking.warehouseProcessing.qty') }` },
      {
        validator: method.validQty
      },
      {
        validator: isInteger,
        validNumerical: 'greaterThanZero',
        trigger: 'change'
      }
    ]
  }),
  validRulesTarget: ref<any>({
    qty: [
      { required: true, message: `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.warehouseWorking.warehouseProcessing.qty') }` },
      {
        validator: isInteger,
        validNumerical: 'greaterThanZero'
      }
    ],
    location_name: [
      {
        required: true,
        message: `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.warehouseWorking.warehouseProcessing.target_location') }`
      }
    ],
    price: [
      {
        validator: isDecimal,
        validNumerical: 'nonNegative',
        length: 10,
        decimalLength: 2,
        trigger: 'change'
      }
    ]
  })
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
.mainForm {
  background-color: #f9f9f9;
  border-radius: 5px;
  padding: 20px;
  box-sizing: border-box;
  overflow: auto;
}

.toolbar {
  height: 40px;
  display: flex;
  justify-content: space-between;
}

.toolbarTitle {
  display: flex;
  // justify-content: center;
  // align-items: center;
}
</style>
