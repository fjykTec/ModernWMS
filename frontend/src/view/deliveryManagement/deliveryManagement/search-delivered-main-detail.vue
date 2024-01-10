<template>
  <v-dialog v-model="isShow" :width="'70%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card class="formCard">
        <v-toolbar color="white" :title="`${$t('wms.deliveryManagement.ViewInventoryDetails')}`"></v-toolbar>
        <v-card-text>
          <vxe-table
            ref="detailXTable"
            keep-source
            :column-config="{ minWidth: '100px' }"
            :data="data.tableData"
            :height="'500px'"
            align="center"
            :edit-config="{ trigger: 'click', mode: 'cell' }"
          >
            <template #empty>
              {{ i18n.global.t('system.page.noData') }}
            </template>
            <vxe-column type="seq" width="60"></vxe-column>
            <vxe-column field="spu_code" :title="$t('wms.deliveryManagement.spu_code')"></vxe-column>
            <vxe-column field="spu_name" :title="$t('wms.deliveryManagement.spu_name')"></vxe-column>
            <vxe-column field="spu_description" width="200px" :title="$t('wms.deliveryManagement.spu_description')"></vxe-column>
            <vxe-column field="bar_code" :title="$t('wms.deliveryManagement.bar_code')"></vxe-column>
            <vxe-column field="sku_code" :title="$t('wms.deliveryManagement.sku_code')"></vxe-column>
            <vxe-column field="sku_name" :title="$t('wms.deliveryManagement.sku_name')"></vxe-column>
            <vxe-column field="unit" :title="$t('wms.deliveryManagement.unit')"></vxe-column>
            <vxe-column field="qty" :title="$t('wms.deliveryManagement.qty')"></vxe-column>
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
import { reactive, computed, watch } from 'vue'
import { hookComponent } from '@/components/system/index'
import { viewDeliveryMainDetail } from '@/api/wms/deliveryManagement'
import i18n from '@/languages/i18n'

const emit = defineEmits(['close', 'submit'])

const props = defineProps<{
  showDialog: boolean
  dispatchNo: string
}>()

const isShow = computed(() => props.showDialog)

const data = reactive({
  tableData: []
})

const method = reactive({
  getTableData: async () => {
    const { data: res } = await viewDeliveryMainDetail(props.dispatchNo)
    if (!res.isSuccess) {
      hookComponent.$message({
        type: 'error',
        content: res.errorMessage
      })
      return
    }
    data.tableData = res.data
  },
  closeDialog: () => {
    emit('close')
  }
})

watch(
  () => isShow.value,
  (val) => {
    if (val) {
      method.getTableData()
    }
  }
)
</script>

<style scoped lang="less"></style>
