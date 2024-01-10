<template>
  <v-dialog v-model="data.showDialog" :width="'50%'" transition="dialog-top-transition" :persistent="true">
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
            <v-row v-for="(snNum, index) of data.SNList" :key="index" style="margin-top: 5px">
              <v-col :cols="10">
                <v-text-field
                  v-model="snNum.snNum"
                  :label="$t('wms.stockAsnInfo.series_number')"
                  :rules="data.rules.series_number"
                  variant="outlined"
                ></v-text-field>
              </v-col>
              <!-- <v-col :cols="3">
                <v-text-field v-model="data.staticDetailQty" readonly :label="$t('wms.stockAsnInfo.sorted_qty')" variant="outlined"></v-text-field>
              </v-col> -->
              <v-col :cols="2">
                <div class="detailBtnContainer">
                  <tooltip-btn
                    :flat="true"
                    icon="mdi-delete-outline"
                    :tooltip-text="$t('system.page.delete')"
                    :icon-color="errorColor"
                    @click="method.removeItem(index)"
                  ></tooltip-btn>
                </div>
              </v-col>
            </v-row>
            <v-btn
              style="font-size: 20px; margin-bottom: 15px; margin-top: 10px; float: right"
              color="primary"
              :width="40"
              @click="method.insertSNData()"
            >
              +
            </v-btn>
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
import { reactive, ref } from 'vue'
import { SortingVo } from '@/types/WMS/StockAsn'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { IsInteger, StringLength } from '@/utils/dataVerification/formRule'
import { errorColor } from '@/constant/style'
import tooltipBtn from '@/components/tooltip-btn.vue'

const formRef = ref()
const emit = defineEmits(['sure'])

const data = reactive({
  showDialog: false,
  staticDetailQty: 1,
  form: ref<SortingVo>({
    asn_id: 0,
    sorted_qty: 0
  }),
  SNList: [] as { snNum: string }[],
  rules: {
    sorted_qty: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.stockAsnInfo.sorted_qty') }!`,
      (val: number) => IsInteger(val, 'greaterThanZero') === '' || IsInteger(val, 'greaterThanZero')
    ],
    series_number: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('wms.stockAsnInfo.series_number') }!`,
      (val: string) => StringLength(val, 0, 64) === '' || StringLength(val, 0, 64)
    ]
  }
})

const method = reactive({
  // Delete data
  removeItem: (index: number) => {
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteDetailMessage'),
      handleConfirm: async () => {
        data.SNList.splice(index, 1)
      }
    })
  },
  insertSNData: () => {
    data.SNList.push({ snNum: '' })
  },
  openDialog: async (id: number) => {
    data.form = {
      asn_id: id,
      sorted_qty: 0
    }

    data.SNList = []

    data.showDialog = true
  },
  closeDialog: () => {
    data.showDialog = false
  },
  submit: async () => {
    if (data.SNList.length > data.form.sorted_qty) {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('wms.stockAsnInfo.exceedingPrompt')
      })
      return
    }
    const { valid } = await formRef.value.validate()
    if (valid) {
      const reqData: any = []

      for (const item of data.SNList) {
        reqData.push({
          asn_id: data.form.asn_id,
          series_number: item.snNum,
          sorted_qty: 1
        })
      }

      // If there is still remaining after removing the SN code, add details without the SN code
      if (data.SNList.length !== data.form.sorted_qty) {
        const margin = data.form.sorted_qty - data.SNList.length
        reqData.push({
          asn_id: data.form.asn_id,
          series_number: '',
          sorted_qty: margin
        })
      }

      emit('sure', reqData)
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('system.checkText.checkFormFail')
      })
    }
  }
})

defineExpose({
  openDialog: method.openDialog,
  closeDialog: method.closeDialog
})
</script>

<style scoped lang="less">
.detailBtnContainer {
  height: 56px;
  display: flex;
  justify-content: center;
  align-items: center;
}
</style>
