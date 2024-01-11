<template>
  <v-dialog v-model="data.showDialog" :width="'30%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('wms.stockAsn.tabNotice')}`"></v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
            <!-- <v-text-field
              v-model="data.formPutaway.location_name"
              :label="$t('base.warehouseSetting.location_name')"
              :rules="data.rules.location_name"
              variant="outlined"
              readonly
              clearable
              @click="method.openSelect(index)"
              @click:clear="method.clearCommodity"
            ></v-text-field>
            <v-text-field
              v-model="data.formPutaway.putaway_qty"
              :label="$t('wms.stockAsnInfo.actual_qty')"
              :rules="data.rules.putaway_qty"
              variant="outlined"
            ></v-text-field> -->
          </v-form>
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="method.closeDialog">{{ $t('system.page.close') }}</v-btn>
          <v-btn color="primary" variant="text" @click="method.submit">{{ $t('system.page.submit') }}</v-btn>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>
  <locationSelect :show-dialog="data.showSkuDialogSelect" @close="method.closeDialogSelect()" @sureSelect="method.sureSelect" />
</template>

<script lang="ts" setup>
import { reactive, ref } from 'vue'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { getSorting } from '@/api/wms/stockAsn'
import locationSelect from '@/components/select/location-select.vue'

const formRef = ref()
// const emit = defineEmits(['sure'])

const data = reactive({
  showDialog: false,
  tableData: [],
  rules: {
    putaway_qty: [(val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.stockAsnInfo.actual_qty') }!`],
    location_name: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.warehouseSetting.location_name') }!`
    ]
  },
  showSkuDialogSelect: false,
  cacheItemIndex: -1
})

const method = reactive({
  // Initialized Data
  initDialogData: async (id: number) => {
    const { data: res } = await getSorting(id)
    if (!res.isSuccess) {
      hookComponent.$message({
        type: 'error',
        content: res.errorMessage
      })
      method.closeDialog()
      return
    }
    data.tableData = res.data
  },
  openDialog: (id: number) => {
    method.initDialogData(id)

    data.showDialog = true
  },
  closeDialog: () => {
    data.showDialog = false
  },
  submit: async () => {
    const { valid } = await formRef.value.validate()
    if (valid) {
      console.log()
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('system.checkText.checkFormFail')
      })
    }
  },

  clearCommodity: (item: { asn_id: number; location_name: string; goods_location_id: number; putaway_qty: number }) => {
    item.goods_location_id = 0
    item.location_name = ''
  },
  openSelect: (index: number) => {
    data.cacheItemIndex = index

    data.showSkuDialogSelect = true
  },
  closeDialogSelect: () => {
    data.showSkuDialogSelect = false
  },
  sureSelect: (selectRecords: any) => {
    console.log(selectRecords)
    // data.formPutaway.goods_location_id = selectRecords[0].id
    // data.formPutaway.location_name = selectRecords[0].location_name
  }
})
</script>

<style scoped lang="less">
.v-form {
  div {
    margin-bottom: 7px;
  }
}
</style>
