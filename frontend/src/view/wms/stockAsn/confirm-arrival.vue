<template>
  <v-dialog v-model="data.showDialog" :width="'50%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card class="formCard">
        <v-toolbar color="white" :title="`${$t('wms.stockAsnInfo.arrival_time')}`"></v-toolbar>
        <v-card-text>
          <v-text-field v-model="data.arrival_time" type="date" variant="outlined" :label="$t('wms.stockAsnInfo.arrival_time')"></v-text-field>
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
import { reactive } from 'vue'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { formatDate } from '@/utils/format/formatSystem'

const emit = defineEmits(['sure'])

const data = reactive({
  showDialog: false,
  arrival_time: ''
})

const method = reactive({
  openDialog: () => {
    data.arrival_time = formatDate(new Date(), 'yyyy-MM-dd')

    data.showDialog = true
  },
  closeDialog: () => {
    data.showDialog = false
  },
  submit: async () => {
    if (data.arrival_time) {
      emit('sure', data.arrival_time)
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
