<template>
  <v-dialog v-model="data.showDialog" width="448px" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-card-text>
          <div id="printArea" class="printArea">
            <div class="printTopContainer">
              <div>
                <vue-qr :key="data.printText" size="140" :margin="0" :auto-color="true" :dot-scale="1" :text="data.printText"></vue-qr>
              </div>

              <div class="printLabel" style="flex: 1">
                <div>
                  <span class="labelTitle">{{ $t('wms.stockAsnInfo.num') }}:</span> &nbsp;{{ data.printData.asn_no }}
                </div>
                <div><span class="labelTitle">{{ $t('wms.stockAsnInfo.asn_batch') }}:</span> &nbsp;{{ data.printData.asn_batch }}</div>
              </div>
            </div>
          </div>
        </v-card-text>

        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="method.closeDialog">{{ $t('system.page.close') }}</v-btn>
          <v-btn v-print="'#printArea'" color="primary" variant="text">{{ $t('system.page.print') }}</v-btn>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>
</template>

<script setup lang="tsx">
import { reactive } from 'vue'
import VueQr from 'vue-qr/src/packages/vue-qr.vue'

const data = reactive({
  showDialog: false,
  printData: {
    asn_id: 0,
    asn_no: '',
    asn_batch: ''
  } as any,
  printText: ''
})

const method = reactive({
  openDialog: (row: any) => {
    data.printData = row

    data.printText = JSON.stringify({
      asn_id: row.asn_id,
      type: row.type
    })

    data.showDialog = true
  },
  closeDialog: () => {
    data.showDialog = false
  }
})

defineExpose({
  openDialog: method.openDialog,
  closeDialog: method.closeDialog
})
</script>

<style scoped lang="less">
.printArea {
  width: 400px;

  display: flex;
  flex-wrap: wrap;
  align-items: center;
  justify-content: center;

  box-sizing: border-box;
  padding: 10px;

  .printTopContainer {
    display: flex;
    justify-content: space-between;
  }
}
.printLabel {
  font-size: 14px;
  font-weight: bold;

  display: flex;
  flex-wrap: wrap;
  align-items: center;

  box-sizing: border-box;
  padding-left: 30px;

  .labelTitle {
    opacity: 0.7;
    font-weight: 600;
  }

  div {
    width: 100%;
  }
}
</style>
