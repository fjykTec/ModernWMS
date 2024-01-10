<template>
  <v-dialog v-model="data.showDialog" :width="'50%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card class="formCard">
        <v-toolbar color="white" :title="`${$t('wms.stockAsnInfo.unloadTime')}`"></v-toolbar>
        <v-card-text>
          <v-text-field
            v-model="data.unloadPerson"
            variant="outlined"
            :label="$t('wms.stockAsnInfo.unloadPerson')"
            append-inner-icon="mdi-magnify"
            single-line
            readonly
            @click:append-inner="method.handleSelectPerson"
          ></v-text-field>
          <v-text-field v-model="data.unloadTime" type="date" variant="outlined" :label="$t('wms.stockAsnInfo.unloadTime')"></v-text-field>
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="method.closeDialog">{{ $t('system.page.close') }}</v-btn>
          <v-btn color="primary" variant="text" @click="method.submit">{{ $t('system.page.submit') }}</v-btn>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>

  <EmployeeSelect ref="EmployeeSelectRef" @sure-select="method.employeeSureBack" />
</template>

<script lang="ts" setup>
import { reactive, ref } from 'vue'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { formatDate } from '@/utils/format/formatSystem'
import EmployeeSelect from '@/components/select/employee-select.vue'
import { UserVO } from '@/types/Base/UserManagement'
import { store } from '@/store'

const emit = defineEmits(['sure'])
const EmployeeSelectRef = ref()

const data = reactive({
  showDialog: false,
  unloadTime: '',
  unloadPerson: '',
  unloadPersonID: 0
})

const method = reactive({
  handleSelectPerson: () => {
    EmployeeSelectRef.value.openDialog()
  },
  employeeSureBack: (list: UserVO[]) => {
    if (list.length > 0) {
      data.unloadPerson = list[0].user_name
      data.unloadPersonID = list[0].id

      EmployeeSelectRef.value.closeDialog()
    }
  },
  openDialog: () => {
    data.unloadTime = formatDate(new Date(), 'yyyy-MM-dd')

    const userInfo = store.getters['user/userInfo']

    data.unloadPerson = userInfo.user_name
    data.unloadPersonID = userInfo.user_id

    data.showDialog = true
  },
  closeDialog: () => {
    data.showDialog = false
  },
  submit: async () => {
    if (data.unloadTime) {
      emit('sure', {
        unloadTime: data.unloadTime,
        unloadPerson: data.unloadPerson,
        unloadPersonID: data.unloadPersonID
      })
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('wms.stockAsnInfo.mustInputErrorMsg')
      })
    }
  }
})

defineExpose({
  openDialog: method.openDialog,
  closeDialog: method.closeDialog
})
</script>

<style scoped lang="less"></style>
