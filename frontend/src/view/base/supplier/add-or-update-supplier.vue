<template>
  <v-dialog v-model="isShow" :width="'30%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('router.sideBar.supplier')}`"></v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
            <v-text-field
              v-model="data.form.supplier_name"
              :label="$t('base.supplier.supplier_name')"
              :rules="data.rules.supplier_name"
              variant="outlined"
            ></v-text-field>
            <v-text-field v-model="data.form.city" :label="$t('base.supplier.city')" :rules="data.rules.city" variant="outlined"></v-text-field>
            <v-text-field v-model="data.form.email" :label="$t('base.supplier.email')" :rules="data.rules.email" variant="outlined"></v-text-field>
            <v-text-field
              v-model="data.form.address"
              :label="$t('base.supplier.address')"
              :rules="data.rules.address"
              variant="outlined"
            ></v-text-field>
            <v-text-field
              v-model="data.form.manager"
              :label="$t('base.supplier.manager')"
              :rules="data.rules.manager"
              variant="outlined"
            ></v-text-field>
            <v-text-field
              v-model="data.form.contact_tel"
              :label="$t('base.supplier.contact_tel')"
              :rules="data.rules.contact_tel"
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
import { SupplierVO } from '@/types/Base/Supplier'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { addSupplier, updateSupplier } from '@/api/base/supplier'
import { removeObjectNull } from '@/utils/common'

const formRef = ref()
const emit = defineEmits(['close', 'saveSuccess'])

const props = defineProps<{
  showDialog: boolean
  form: SupplierVO
}>()

const isShow = computed(() => props.showDialog)

const dialogTitle = computed(() => {
  if (props.form.id && props.form.id > 0) {
    return 'update'
  }
  return 'add'
})

const data = reactive({
  form: ref<SupplierVO>({
    id: 0,
    supplier_name: '',
    city: '',
    address: '',
    manager: '',
    email: '',
    contact_tel: '',
    is_valid: true
  }),
  rules: {
    supplier_name: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.supplier.supplier_name') }!`],
    city: [],
    address: [],
    manager: [],
    email: [],
    contact_tel: [],
    is_valid: []
  }
})

const method = reactive({
  closeDialog: () => {
    emit('close')
  },
  submit: async () => {
    const { valid } = await formRef.value.validate()
    if (valid) {
      const form = removeObjectNull(data.form)
      const { data: res } = dialogTitle.value === 'add' ? await addSupplier(form) : await updateSupplier(form)
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
