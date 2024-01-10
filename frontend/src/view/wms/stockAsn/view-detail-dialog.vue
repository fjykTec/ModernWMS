<template>
  <v-dialog v-model="data.showDialog" :width="'70%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('wms.stockAsnInfo.viewDetail')}`"></v-toolbar>
        <v-card-text>
          <vxe-table
            ref="xTable"
            keep-source
            :column-config="{ minWidth: '100px' }"
            :data="data.tableData"
            :height="SYSTEM_HEIGHT.SELECT_TABLE"
            align="center"
            :mouse-config="{ selected: true }"
            :keyboard-config="{ isArrow: true, isDel: true, isEnter: true, isTab: true, isEdit: true, isChecked: true }"
          >
            <template #empty>
              {{ i18n.global.t('system.page.noData') }}
            </template>
            <vxe-column type="seq" width="60"></vxe-column>
            <vxe-column field="goods_owner_name" :title="$t('wms.stockAsnInfo.goods_owner_name')"> </vxe-column>
            <vxe-column field="series_number" :title="$t('wms.stockAsnInfo.series_number')"> </vxe-column>
          </vxe-table>
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="method.closeDialog">{{ $t('system.page.close') }}</v-btn>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>
</template>

<script lang="ts" setup>
import { reactive, ref } from 'vue'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { SYSTEM_HEIGHT } from '@/constant/style'
import { getGrouding } from '@/api/wms/stockAsn'

const xTable = ref()

const data = reactive({
  showDialog: false,
  tableData: []
})

const method = reactive({
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
    data.tableData = res.data
  },

  openDialog: async (id: number) => {
    // Initialized Data
    method.initDialogData(id)

    data.showDialog = true
  },
  closeDialog: () => {
    data.showDialog = false
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
