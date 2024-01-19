<template>
  <v-dialog v-model="isShow" :width="'30%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('router.sideBar.commodityCategorySetting')}`"></v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
            <v-text-field
              v-model="data.form.category_name"
              :label="$t('base.commodityCategorySetting.category_name')"
              :rules="data.rules.category_name"
              variant="outlined"
            ></v-text-field>
            <v-select
              v-model="data.form.parent_id"
              :items="data.combobox.parent"
              item-title="label"
              item-value="value"
              :rules="data.rules.parent_id"
              :label="$t('base.commodityCategorySetting.parent_name')"
              variant="outlined"
              clearable
            ></v-select>
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
import { CategoryVO } from '@/types/Base/CommodityCategorySetting'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { addCategory, updateCategory, getCategoryAll } from '@/api/base/commodityCategorySetting'
import { StringLength } from '@/utils/dataVerification/formRule'
import { removeObjectNull } from '@/utils/common'

const formRef = ref()
const emit = defineEmits(['close', 'saveSuccess'])

const props = defineProps<{
  showDialog: boolean
  form: CategoryVO
}>()

const isShow = computed(() => props.showDialog)

const dialogTitle = computed(() => {
  if (props.form.id && props.form.id > 0) {
    return 'update'
  }
  return 'add'
})

const data = reactive({
  form: ref<CategoryVO>({
    id: 0,
    category_name: ''
  }),
  rules: {
    category_name: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityCategorySetting.category_name') }!`,
      (val: string) => StringLength(val, 0, 32) === '' || StringLength(val, 0, 32)
    ],
    parent_id: [],
    is_valid: []
  },
  combobox: ref<{
    parent: {
      label: string
      value: number
    }[]
  }>({
    parent: []
  })
})

const method = reactive({
  // Get the options required by the drop-down box
  getCombobox: async () => {
    data.combobox.parent = []
    const { data: res } = await getCategoryAll()
    if (!res.isSuccess) {
      return
    }
    for (const item of res.data) {
      data.combobox.parent.push({
        label: item.category_name,
        value: item.id
      })
    }
  },
  closeDialog: () => {
    emit('close')
  },
  submit: async () => {
    const { valid } = await formRef.value.validate()
    if (valid) {
      const form = removeObjectNull(data.form)
      const { data: res } = dialogTitle.value === 'add' ? await addCategory(form) : await updateCategory(form)
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
      method.getCombobox()
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
