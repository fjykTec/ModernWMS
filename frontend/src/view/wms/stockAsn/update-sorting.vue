<template>
  <v-dialog v-model="data.showDialog" :width="'70%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('wms.stockAsnInfo.editSorting')}`"></v-toolbar>
        <v-card-text>
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
            <vxe-column field="series_number" :title="$t('wms.stockAsnInfo.series_number')"> </vxe-column>
            <vxe-column field="sorted_qty" :title="$t('wms.stockAsnInfo.sorted_qty')" :edit-render="{ autofocus: '.vxe-input--inner' }">
              <template #edit="{ row }">
                <vxe-input v-model="row.sorted_qty" type="text"></vxe-input>
              </template>
            </vxe-column>
            <vxe-column field="expiry_date" :title="$t('wms.stockAsnInfo.expiry_date')" :edit-render="{ autofocus: '.vxe-input--inner' }">
              <template #edit="{ row }">
                <vxe-input v-model="row.expiry_date" type="date"></vxe-input>
              </template>
            </vxe-column>
            <vxe-column field="creator" :title="$t('wms.deliveryManagement.creator')"> </vxe-column>
            <vxe-date-column field="create_time" :title="$t('wms.deliveryManagement.create_time')"> </vxe-date-column>
            <vxe-column field="operate" :title="$t('system.page.operate')" width="100" :resizable="false" show-overflow>
              <template #default="{ row }">
                <tooltip-btn
                  :flat="true"
                  icon="mdi-delete-outline"
                  :tooltip-text="$t('system.page.delete')"
                  :icon-color="errorColor"
                  @click="method.deleteRow(row)"
                ></tooltip-btn>
              </template>
            </vxe-column>
          </vxe-table>
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
import { reactive, ref } from 'vue'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { errorColor, SYSTEM_HEIGHT } from '@/constant/style'
import { getSorting } from '@/api/wms/stockAsn'
import { UpdateSortingVo } from '@/types/WMS/StockAsn'
import tooltipBtn from '@/components/tooltip-btn.vue'
import { isInteger } from '@/utils/dataVerification/tableRule'
import { formatDate } from '@/utils/format/formatSystem'

const xTable = ref()

const emit = defineEmits(['sure'])

const data = reactive({
  showDialog: false,
  tableData: [] as any[],
  validRules: {
    sorted_qty: [
      { required: true, message: `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.stockAsnInfo.sorted_qty') }` },
      {
        validator: isInteger,
        validNumerical: 'nonNegative',
        trigger: 'change'
      }
    ],
    series_number: [
      {
        type: 'string',
        min: 0,
        max: 64,
        message: `${ i18n.global.t('system.checkText.lengthValid') }${ 0 }-${ 64 }`,
        trigger: 'change'
      }
    ]
  } as any
})

const method = reactive({
  activeMethod({ row, column }: any) {
    if (!row.series_number && column.field === 'series_number') {
      return false
    }
    return true
  },
  // delete row
  deleteRow: (row: UpdateSortingVo) => {
    const $table = xTable.value
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteDetailMessage'),
      handleConfirm: async () => {
        $table.remove(row)
      }
    })
  },
  // Initialize window data
  initDialogData: async (id: number) => {
    const { data: res } = await getSorting(id)
    if (!res.isSuccess) {
      hookComponent.$message({
        type: 'error',
        content: res.errorMessage
      })
      data.showDialog = false
      return
    }
    data.tableData = res.data

    // 240507 刘福: 处理一下操作列的日期数据
    if (data.tableData?.length > 0) {
      data.tableData = data.tableData.map((item: any) => {
        item.expiry_date = formatDate(item.expiry_date, 'yyyy-MM-dd')
        return item
      })
    }
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
      // Find the deleted data, assign a negative id to represent deletion
      const removeTableData = xTable.value.getRemoveRecords()

      // Traversing and modifying the original data, as the data is a shallow copy, there is no need to assign a value
      if (removeTableData) {
        for (const item of removeTableData) {
          item.id = 0 - item.id
        }
      }

      const tableData: any = data.tableData
      emit('sure', tableData)
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

<style scoped lang="less"></style>
