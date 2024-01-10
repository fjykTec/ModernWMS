<template>
  <v-dialog v-model="data.showDialog" width="248px" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-card-text>
          <div id="printArea" class="printArea">
            <div class="printTopContainer">
              <div class="printLabel" style="flex: 1">
                <div>
                  <span class="labelTitle">{{ $t('wms.deliveryManagement.dispatch_no') }}:</span>{{ data.printData.dispatch_no }}
                </div>
                <vue-qr :key="data.printText" size="140" :margin="0" :auto-color="true" :dot-scale="1" :text="data.printText"></vue-qr>
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
    dispatch_no: ''
  } as any,
  printText: ''
})

const method = reactive({
  openDialog: (row: any) => {
    data.printData = row

    data.printText = JSON.stringify(data.printData)

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
  width: 200px;

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
  justify-content: center;

  box-sizing: border-box;

  .labelTitle {
    opacity: 0.7;
    font-weight: 600;
  }

  div {
    width: 100%;
    text-align: center;
    margin-bottom: 5px;
  }
}
</style>
