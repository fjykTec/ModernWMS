<template>
  <v-dialog v-model="isShow" :width="'30%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('wms.stockAsn.tabNotice')}`"></v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
            <v-text-field
              v-model="data.form.sorted_qty"
              :label="$t('wms.stockAsnInfo.sorted_qty')"
              :rules="data.rules.sorted_qty"
              variant="outlined"
            ></v-text-field>
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
import { reactive, computed, ref, watch } from 'vue'
import { StockAsnVO, SortingVo } from '@/types/WMS/StockAsn'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { sortingAsn } from '@/api/wms/stockAsn'

const formRef = ref()
const emit = defineEmits(['close', 'saveSuccess'])

const props = defineProps<{
  showDialog: boolean
  form: StockAsnVO
}>()

const isShow = computed(() => props.showDialog)

const dialogTitle = computed(() => 'update')

const data = reactive({
  form: ref<StockAsnVO>({
    id: 0,
    asn_no: '',
    asn_status: 0,
    spu_id: 0,
    spu_code: '',
    spu_name: '',
    sku_id: 0,
    sku_code: '',
    sku_name: '',
    origin: '',
    length_unit: 0,
    volume_unit: 0,
    weight_unit: 0,
    asn_qty: 0,
    actual_qty: 0,
    sorted_qty: 0,
    shortage_qty: 0,
    more_qty: 0,
    damage_qty: 0,
    weight: 0,
    volume: 0,
    supplier_id: 0,
    supplier_name: '',
    goods_owner_id: 0,
    goods_owner_name: '',
    is_valid: true
  }),
  formSorting: ref<SortingVo>({
    asn_id: 0,
    sorted_qty: 0
  }),
  rules: {
    sorted_qty: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.stockAsnInfo.sorted_qty') }!`]
  }
})

const method = reactive({
  closeDialog: () => {
    emit('close')
  },
  submit: async () => {
    const { valid } = await formRef.value.validate()
    if (valid) {
      data.formSorting.asn_id = data.form.id
      data.formSorting.sorted_qty = data.form.sorted_qty
      const { data: res } = await sortingAsn(data.formSorting)
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
  }
})

watch(
  () => isShow.value,
  (val) => {
    if (val) {
      data.form = props.form
    }
  }
)
</script>

<style scoped lang="less">
.v-form {
  div {
    margin-bottom: 7px;
  }
}
</style>
