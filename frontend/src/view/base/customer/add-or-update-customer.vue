<template>
  <v-dialog v-model="isShow" :width="'30%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('router.sideBar.customer')}`"></v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
            <v-text-field
              v-model="data.form.customer_name"
              :label="$t('base.customer.customer_name')"
              :rules="data.rules.customer_name"
              variant="outlined"
            ></v-text-field>
            <v-text-field v-model="data.form.city" :label="$t('base.customer.city')" :rules="data.rules.city" variant="outlined"></v-text-field>
            <v-text-field
              v-model="data.form.email"
              :label="$t('base.customer.email')"
              :rules="data.rules.email"
              variant="outlined"
            ></v-text-field>
            <v-text-field
              v-model="data.form.address"
              :label="$t('base.customer.address')"
              :rules="data.rules.address"
              variant="outlined"
            ></v-text-field>
            <v-text-field
              v-model="data.form.manager"
              :label="$t('base.customer.manager')"
              :rules="data.rules.manager"
              variant="outlined"
            ></v-text-field>
            <v-text-field
              v-model="data.form.contact_tel"
              :label="$t('base.customer.contact_tel')"
              :rules="data.rules.contact_tel"
              variant="outlined"
            ></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="method.closeDialog">{{ $t('system.page.close') }}</v-btn>
          <v-btn color="primary" variant="text" @click="method.submit">{{ $t('system.page.submit') }}</v-btn>
          <v-btn color="primary" variant="text" @click="method.print">{{ $t('system.page.print') }}</v-btn>
        </v-card-actions>
      </v-card>
      <hprintDialog ref="hprintDialogRef" :form="data.form" :tab-page="'print_page_detail'" />
    </template>
  </v-dialog>
</template>

<script lang="ts" setup>
import { reactive, computed, ref, watch } from 'vue'
import { CustomerVO } from '@/types/Base/Customer'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { addCustomer, updateCustomer } from '@/api/base/customer'
import hprintDialog from '@/components/hiprint/hiprintFast.vue'
import { removeObjectNull } from '@/utils/common'

const formRef = ref()
const hprintDialogRef = ref()
const emit = defineEmits(['close', 'saveSuccess'])

const props = defineProps<{
  showDialog: boolean
  form: CustomerVO
}>()

const isShow = computed(() => props.showDialog)

const dialogTitle = computed(() => {
  if (props.form.id && props.form.id > 0) {
    return 'update'
  }
  return 'add'
})

const data = reactive({
  form: ref<CustomerVO>({
    id: 0,
    customer_name: '',
    city: '',
    address: '',
    manager: '',
    email: '',
    contact_tel: '',
    is_valid: true
  }),
  rules: {
    customer_name: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.customer.customer_name') }!`],
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
      const { data: res } = dialogTitle.value === 'add' ? await addCustomer(form) : await updateCustomer(form)
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
  },
    print() {
    const ref = hprintDialogRef.value
    ref.method.handleOpen()
  },
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
