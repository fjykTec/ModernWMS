<template>
  <v-dialog v-model="isShow" :width="'30%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('router.sideBar.companySetting')}`"></v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
            <v-text-field
              v-model="data.form.company_name"
              :label="$t('base.companySetting.company_name')"
              :rules="data.rules.company_name"
              variant="outlined"
            ></v-text-field>
            <v-text-field v-model="data.form.city" :label="$t('base.companySetting.city')" :rules="data.rules.city" variant="outlined"></v-text-field>
            <v-text-field
              v-model="data.form.address"
              :label="$t('base.companySetting.address')"
              :rules="data.rules.address"
              variant="outlined"
            ></v-text-field>
            <v-text-field
              v-model="data.form.manager"
              :label="$t('base.companySetting.manager')"
              :rules="data.rules.manager"
              variant="outlined"
            ></v-text-field>
            <v-text-field
              v-model="data.form.contact_tel"
              :label="$t('base.companySetting.contact_tel')"
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
import { CompanyVO } from '@/types/Base/CompanySetting'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { addCompany, updateCompany } from '@/api/base/companySetting'
import { StringLength } from '@/utils/dataVerification/formRule'
import { removeObjectNull } from '@/utils/common'

const formRef = ref()
const emit = defineEmits(['close', 'saveSuccess'])

const props = defineProps<{
  showDialog: boolean
  form: CompanyVO
}>()

const isShow = computed(() => props.showDialog)

const dialogTitle = computed(() => {
  if (props.form.id && props.form.id > 0) {
    return 'update'
  }
  return 'add'
})

const data = reactive({
  form: ref<CompanyVO>({
    id: 0,
    company_name: '',
    city: '',
    address: '',
    manager: '',
    contact_tel: ''
  }),
  rules: {
    company_name: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.companySetting.company_name') }!`,
      (val: string) => StringLength(val, 0, 256) === '' || StringLength(val, 0, 256)
    ],
    city: [(val: string) => StringLength(val, 0, 128) === '' || StringLength(val, 0, 128)],
    address: [(val: string) => StringLength(val, 0, 256) === '' || StringLength(val, 0, 256)],
    manager: [(val: string) => StringLength(val, 0, 64) === '' || StringLength(val, 0, 64)],
    contact_tel: [(val: string) => StringLength(val, 0, 64) === '' || StringLength(val, 0, 64)]
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
      const { data: res } = dialogTitle.value === 'add' ? await addCompany(form) : await updateCompany(form)
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
