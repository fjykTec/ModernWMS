<template>
  <v-dialog v-model="isShow" :width="'30%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('router.sideBar.supplier')}`"></v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
            <v-text-field
              v-model="data.form.dateFrom"
              type="date"
              variant="outlined"
              :label="$t('wms.deliveryBatchAllocation.dateFrom')"
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
import { DeliveryBatchAllocationVO } from '@/types/DeliveryManagement/DeliveryManagement'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'

const formRef = ref()
const emit = defineEmits(['close', 'saveSuccess'])

const props = defineProps<{
  showDialog: boolean
  form: DeliveryBatchAllocationVO
}>()

const isShow = computed(() => props.showDialog)

// const dialogTitle = computed(() => {
//   if (props.form.id && props.form.id > 0) {
//     return 'update'
//   }
//   return 'add'
// })

const data = reactive({
  form: ref<DeliveryBatchAllocationVO>({
    id: 0,
    dateFrom: '',
    dateTo: '',
    allocationRule: '',
    is_valid: true
  }),
  rules: {
    dateFrom: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.deliveryBatchAllocation.dateFrom') }!`],
    dateTo: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.deliveryBatchAllocation.dateTo') }!`],
    allocationRule: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.deliveryBatchAllocation.allocationRule') }!`],
  }
})

const method = reactive({
  closeDialog: () => {
    emit('close')
  },
  submit: async () => {
    const { valid } = await formRef.value.validate()
    if (valid) {
      // const { data: res } = dialogTitle.value === 'add' ? await addSupplier(data.form) : await updateSupplier(data.form)
      // if (!res.isSuccess) {
      //   hookComponent.$message({
      //     type: 'error',
      //     content: res.errorMessage
      //   })
      //   return
      // }
      // hookComponent.$message({
      //   type: 'success',
      //   content: `${i18n.global.t('system.page.submit')}${i18n.global.t('system.tips.success')}`
      // })
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
