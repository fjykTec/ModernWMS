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
            <vxe-column field="series_number" :title="$t('wms.stockLocation.series_number')"></vxe-column>
            <vxe-column field="goods_owner_name" :title="$t('base.ownerOfCargo.goods_owner_name')"></vxe-column>
            <vxe-column field="warehouse_name" :title="$t('wms.stockLocation.warehouse_name')"></vxe-column>
            <vxe-column field="warehouse_area_name" :title="$t('base.warehouseSetting.area_name')"></vxe-column>
            <vxe-column field="location_name" :title="$t('base.warehouseSetting.location_name')"></vxe-column>
            <!-- <vxe-column field="pick_qty" :title="$t('wms.deliveryManagement.unpicked_qty')"></vxe-column> -->
            <vxe-column field="picked_qty" :title="$t('wms.deliveryManagement.picked_qty')"></vxe-column>
            <vxe-column field="picker" :title="$t('wms.deliveryManagement.picker')"></vxe-column>
            <vxe-column
              v-if="sourceType === 'picking'"
              field="operate"
              :title="$t('system.page.operate')"
              width="120"
              :resizable="false"
              show-overflow
            >
              <template #default="{ row }">
                <div style="width: 100%; display: flex; justify-content: center">
                  <tooltip-btn
                    :disabled="!data.authorityList.includes('picked-confirm') || row.picker_id > 0"
                    :flat="true"
                    icon="mdi-cart-arrow-down"
                    :tooltip-text="$t('base.roleMenu.opeartionFunctionName.deliveryManagement.picked')"
                    @click="method.pickRow(row)"
                  ></tooltip-btn>
                </div>
              </template>
            </vxe-column>
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
import { reactive, computed, watch, withDefaults } from 'vue'
import { hookComponent } from '@/components/system/index'
import { getMenuAuthorityList } from '@/utils/common'
import { viewInventoryDetails, confirmPickingDetail } from '@/api/wms/deliveryManagement'
import i18n from '@/languages/i18n'
import tooltipBtn from '@/components/tooltip-btn.vue'

const emit = defineEmits(['close', 'submit'])

const props = withDefaults(
  defineProps<{
    showDialog: boolean
    id: number
    sourceType: string
  }>(),
  {
    sourceType: ''
  }
)

const isShow = computed(() => props.showDialog)

const data = reactive({
  tableData: [],
  // Menu operation permissions
  authorityList: getMenuAuthorityList()
})

const method = reactive({
  pickRow: async (row) => {
    hookComponent.$dialog({
      content: `${ i18n.global.t('base.roleMenu.opeartionFunctionName.deliveryManagement.picked') }?`,
      handleConfirm: async () => {
        const { data: res } = await confirmPickingDetail([row.id])
        if (!res.isSuccess) {
          hookComponent.$message({
            type: 'error',
            content: res.errorMessage
          })
          return
        }
        hookComponent.$message({
          type: 'success',
          content: res.data
        })
        method.getTableData()
      }
    })
  },
  getTableData: async () => {
    const { data: res } = await viewInventoryDetails(props.id)
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
