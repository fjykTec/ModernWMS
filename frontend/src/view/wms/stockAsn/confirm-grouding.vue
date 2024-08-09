<template>
  <v-dialog v-model="data.showDialog" :width="'70%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('wms.stockAsnInfo.editGrounding')}`"></v-toolbar>
        <v-card-text>
          <v-row no-gutters>
            <v-col cols="12" class="col">
              <tooltip-btn
                icon="mdi-warehouse"
                :tooltip-text="$t('wms.stockAsnInfo.selectLocation')"
                @click="method.handleSelectLocation()"
              ></tooltip-btn>
            </v-col>
          </v-row>

          <vxe-table
            ref="xTable"
            keep-source
            :column-config="{ minWidth: '100px' }"
            :data="data.tableData"
            :height="SYSTEM_HEIGHT.SELECT_TABLE"
            align="center"
            :edit-rules="data.validRules"
            :edit-config="{ trigger: 'click', mode: 'cell', activeMethod: method.activeMethod }"
            :mouse-config="{ selected: true }"
            :keyboard-config="{ isArrow: true, isDel: true, isEnter: true, isTab: true, isEdit: true, isChecked: true }"
          >
            <template #empty>
              {{ i18n.global.t('system.page.noData') }}
            </template>
            <vxe-column type="seq" width="60"></vxe-column>
            <vxe-column type="checkbox" width="50"></vxe-column>
            <vxe-column field="goods_owner_name" :title="$t('wms.stockAsnInfo.goods_owner_name')"> </vxe-column>
            <vxe-column field="series_number" :title="$t('wms.stockAsnInfo.series_number')"> </vxe-column>
            <vxe-column field="location_name" :title="$t('base.warehouseSetting.location_name')" :edit-render="{ autofocus: '.vxe-input--inner' }">
            </vxe-column>

            <vxe-column field="sorted_qty" :title="$t('wms.stockAsnInfo.can_putaway_qty')"> </vxe-column>
            <vxe-column field="putaway_qty" :title="$t('wms.stockAsnInfo.putaway_qty')" :edit-render="{ autofocus: '.vxe-input--inner' }">
              <template #edit="{ row }">
                <vxe-input v-model="row.putaway_qty" type="text"></vxe-input>
              </template>
            </vxe-column>
            <!-- <vxe-column field="operate" :title="$t('system.page.operate')" width="100" :resizable="false" show-overflow>
              <template #default="{ row }">
                <tooltip-btn
                  :flat="true"
                  icon="mdi-warehouse"
                  :tooltip-text="$t('base.warehouseSetting.location_name')"
                  @click="method.handleSelectLocation(row)"
                ></tooltip-btn>
              </template>
            </vxe-column> -->
          </vxe-table>
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="method.closeDialog">{{ $t('system.page.close') }}</v-btn>
          <v-btn color="primary" variant="text" @click="method.submit">{{ $t('system.page.submit') }}</v-btn>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>

  <locationSelect :show-dialog="data.showSkuDialogSelect" @close="method.closeDialogSelect()" @sureSelect="method.sureSelect" />
</template>

<script lang="ts" setup>
import { reactive, ref } from 'vue'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { SYSTEM_HEIGHT } from '@/constant/style'
import { getGrouding } from '@/api/wms/stockAsn'
import { isInteger } from '@/utils/dataVerification/tableRule'
import tooltipBtn from '@/components/tooltip-btn.vue'
import locationSelect from '@/components/select/location-select.vue'

const xTable = ref()

const emit = defineEmits(['sure'])

// Verify if it exceeds the available shelving quantity
const validQty = ({ cellValue, row }: { cellValue: number; row: { sorted_qty: number } }) => {
  if (!Number.isNaN(cellValue) && !Number.isNaN(row.sorted_qty) && cellValue > row.sorted_qty) {
    return new Error(i18n.global.t('wms.stockAsnInfo.exceedTip'))
  }
}

// If the number of listings is greater than 0, a storage location code must be selected for verification
const validLocationName = ({ cellValue, row }: { cellValue: string; row: { putaway_qty: number } }) => {
  if (row.putaway_qty && !cellValue) {
    return new Error(i18n.global.t('wms.stockAsnInfo.notLocation'))
  }
}

const data = reactive({
  showDialog: false,
  showSkuDialogSelect: false,
  tableData: [],
  validRules: {
    putaway_qty: [
      { required: true, message: `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.stockAsnInfo.putaway_qty') }` },
      {
        validator: isInteger,
        validNumerical: 'greaterThanZero',
        trigger: 'change'
      },
      { validator: validQty, trigger: 'change' }
    ],
    series_number: [],
    location_name: [
    { required: true, message: `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.warehouseSetting.location_name') }` },
    { validator: validLocationName, trigger: 'change' }]
  } as any
})

const method = reactive({
  // Table activation rules
  activeMethod: ({ column }: any) => {
    if (column.field === 'location_name') {
      return false
    }
    return true
  },
  // Select storage location
  handleSelectLocation: () => {
    const checkRecords = xTable.value.getCheckboxRecords()

    if (checkRecords.length > 0) {
      data.showSkuDialogSelect = true
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('wms.stockAsnInfo.selectOne')
      })
    }
  },
  // Close the location selection box
  closeDialogSelect: () => {
    data.showSkuDialogSelect = false
  },
  // Select inventory location callback
  sureSelect: (selectRecords: any) => {
    if (selectRecords.length > 0) {
      const checkRecords = xTable.value.getCheckboxRecords()

      for (const item of checkRecords) {
        item.goods_location_id = selectRecords[0].id
        item.location_name = selectRecords[0].location_name
      }
    }
  },
  // Initialize window data
  initDialogData: async (id: number) => {
    const { data: res } = await getGrouding(id)
    if (!res.isSuccess) {
      hookComponent.$message({
        type: 'error',
        content: res.errorMessage
      })
      data.showDialog = false
      return
    }
    data.tableData = res.data.map((item: any) => ({
      ...item,
      putaway_qty: item.sorted_qty
    }))
  },

  openDialog: async (id: number) => {
    // Initialized Data
    method.initDialogData(id)

    data.showDialog = true
  },
  closeDialog: () => {
    data.showDialog = false
  },
  submit: async () => {
    const $table = xTable.value
    const errMap = await $table.validate(true)
    if (!errMap) {
      emit('sure', data.tableData)
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
.col {
  display: flex;
  align-items: center;
  margin-bottom: 10px;
}
</style>
