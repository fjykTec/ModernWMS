<!-- Warehouse Setting Operate Dialog -->
<template>
  <v-dialog v-model="isShow" :width="'30%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('router.sideBar.warehouseSetting')}`"></v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
            <v-text-field
              v-model="data.form.warehouse_name"
              :label="$t('base.warehouseSetting.warehouse_name')"
              :rules="data.rules.warehouse_name"
              variant="outlined"
              clearable
            ></v-text-field>
            <v-text-field
              v-model="data.form.city"
              :label="$t('base.warehouseSetting.city')"
              :rules="data.rules.city"
              variant="outlined"
              clearable
            ></v-text-field>
            <v-text-field
              v-model="data.form.address"
              :label="$t('base.warehouseSetting.address')"
              :rules="data.rules.address"
              variant="outlined"
              clearable
            ></v-text-field>
            <v-text-field
              v-model="data.form.contact_tel"
              :label="$t('base.warehouseSetting.contact_tel')"
              :rules="data.rules.contact_tel"
              variant="outlined"
              clearable
            ></v-text-field>
            <v-text-field
              v-model="data.form.email"
              :label="$t('base.warehouseSetting.email')"
              :rules="data.rules.email"
              variant="outlined"
              clearable
            ></v-text-field>
            <v-text-field
              v-model="data.form.manager"
              :label="$t('base.warehouseSetting.manager')"
              :rules="data.rules.manager"
              variant="outlined"
              clearable
            ></v-text-field>
            <v-switch
              v-model="data.form.is_valid"
              color="primary"
              :label="$t('base.warehouseSetting.is_valid')"
              :rules="data.rules.is_valid"
            ></v-switch>
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
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { addWarehouse, updateWarehouse } from '@/api/base/warehouseSetting'
import { WarehouseVO } from '@/types/Base/Warehouse'
import { StringLength } from '@/utils/dataVerification/formRule'
import { removeObjectNull } from '@/utils/common'

const formRef = ref()
const emit = defineEmits(['close', 'saveSuccess'])

const props = defineProps<{
  showDialog: boolean
  form: WarehouseVO
}>()

const isShow = computed(() => props.showDialog)

const dialogTitle = computed(() => {
  if (props.form.id && props.form.id > 0) {
    return 'update'
  }
  return 'add'
})

const data = reactive({
  form: ref<WarehouseVO>({
    id: 0,
    warehouse_name: '',
    city: '',
    address: '',
    contact_tel: '',
    email: '',
    manager: '',
    is_valid: true
  }),
  rules: {
    warehouse_name: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.warehouseSetting.warehouse_name') }!`,
      (val: string) => StringLength(val, 0, 32) === '' || StringLength(val, 0, 32)
    ],
    city: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.warehouseSetting.city') }!`,
      (val: string) => StringLength(val, 0, 128) === '' || StringLength(val, 0, 128)
    ],
    address: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.warehouseSetting.address') }!`,
      (val: string) => StringLength(val, 0, 256) === '' || StringLength(val, 0, 256)
    ],
    contact_tel: [(val: string) => StringLength(val, 0, 64) === '' || StringLength(val, 0, 64)],
    email: [(val: string) => StringLength(val, 0, 128) === '' || StringLength(val, 0, 128)],
    manager: [(val: string) => StringLength(val, 0, 64) === '' || StringLength(val, 0, 64)],
    is_valid: []
  }
})

const method = reactive({
  closeDialog: () => {
    emit('close')
  },
  initForm: () => {
    data.form = props.form
  },
  submit: async () => {
    const { valid } = await formRef.value.validate()
    if (valid) {
      const form = removeObjectNull(data.form)
      const { data: res } = dialogTitle.value === 'add' ? await addWarehouse(form) : await updateWarehouse(form)
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
      method.initForm()
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
