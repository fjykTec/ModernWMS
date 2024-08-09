<template>
  <v-dialog v-model="data.showDialog" width="auto" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card :title="$t('system.page.preView')">
        <v-card-text>
          <div class="print-area-container">
            <div class="print-area-scroll" @click="method.resetIndex()">
              <div
                id="printArea"
                class="printArea"
                :style="{
                  'grid-template-columns': `repeat(${data.rowsNumber},1fr)`,
                  rowGap: `${data.gridRowGap}px`,
                  columnGap: `${data.gridColumnGap}px`
                }"
              >
                <div
                  v-for="(item, index) in data.printData"
                  :key="index"
                  class="code-container"
                  :class="{ 'code-click': data.clickIndex === index }"
                  @click.stop="method.setIndex(index)"
                >
                  <div>
                    <vue-qr :key="index" :size="150" :auto-color="true" :dot-scale="1" :text="method.formatPrintData(item)"></vue-qr>
                  </div>
                  <div class="printLabel">
                    <slot name="left" :slotData="item"></slot>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </v-card-text>

        <v-card-actions class="justify-space-between">
          <div class="row-number-input">
            <v-row no-gutters>
              <v-col cols="4" class="col">
                <div class="colText">
                  <v-text-field
                    v-model.number="data.rowsNumber"
                    hide-details="auto"
                    :label="$t('system.page.rowsNumber')"
                    @update:model-value="method.changePrintParams"
                  ></v-text-field>
                </div>
              </v-col>
              <v-col cols="4" class="col">
                <div class="colText">
                  <v-text-field
                    v-model.number="data.gridRowGap"
                    hide-details="auto"
                    :label="$t('system.page.gridRowGap')"
                    @update:model-value="method.changePrintParams"
                  ></v-text-field>
                </div>
              </v-col>
              <v-col cols="4" class="col">
                <div class="colText">
                  <v-text-field
                    v-model.number="data.count"
                    hide-details="auto"
                    :label="$t('system.page.count')"
                    :disabled="data.clickIndex === -1"
                    @keydown="method.handleKeyDown"
                  ></v-text-field>
                </div>
              </v-col>
            </v-row>
          </div>
          <div class="padding-lr-16">
            <v-btn variant="text" @click="method.closeDialog">{{ $t('system.page.close') }}</v-btn>
            <v-btn v-print="'#printArea'" color="primary" :disabled="isDisabled" variant="text" @click="method.resetIndex()">
              {{ $t('system.page.print') }}
            </v-btn>
          </div>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>
</template>

<script setup lang="ts">
import { computed, reactive } from 'vue'
import VueQr from 'vue-qr/src/packages/vue-qr.vue'
import { setStorage, getStorage } from '@/utils/common'

const props = defineProps<{
  menu: string
}>()

const data = reactive({
  showDialog: false,
  printData: [] as any,
  printText: '',
  clickIndex: -1,
  rowsNumber: 5,
  gridColumnGap: 15,
  gridRowGap: 15,
  count: 1
})

const isDisabled = computed(() => data.printData.length === 0)

const method = reactive({
  changePrintParams: () => {
    const storage = {
      cols: data.rowsNumber,
      rs: data.gridRowGap
    }

    setStorage(`QRCode-${ props.menu }`, storage)
  },
  setIndex: (index: number) => {
    data.clickIndex = index
  },
  resetIndex: () => {
    data.clickIndex = -1
  },
  handleKeyDown: (event: any) => {
    if (event.key === 'Enter') {
      if (data.count > 1) {
        const object = data.printData[data.clickIndex]
        while (data.count > 1) {
          data.printData.splice(data.clickIndex, 0, object)
          data.count--
        }
        method.resetIndex()
      }
    }
  },
  openDialog: (row: any) => {
    try {
      const storage: any = getStorage(`QRCode-${ props.menu }`)

      if (storage) {
        data.rowsNumber = storage.cols
        data.gridRowGap = storage.rs
      }
    } catch {
      data.rowsNumber = 5
      data.gridRowGap = 15
    }
    data.clickIndex = -1
    data.printData = row
    data.showDialog = true
  },
  formatPrintData: (val: any) => {
    const form = {
      id: val.id,
      type: val.type
    } as any
    if (val.hasOwnProperty('sku_id')) {
      form.sku_id = val.sku_id
    }
    if (val.hasOwnProperty('asn_id')) {
      form.asn_id = val.asn_id
    }
    if (val.hasOwnProperty('asnmaster_id')) {
      form.asnmaster_id = val.asnmaster_id
    }
    if (val.hasOwnProperty('series_number')) {
      form.SN = val.series_number
    }
    return JSON.stringify(form)
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
.print-area-container {
  background-color: #efefef;
  padding: 15px;
}

.print-area-scroll {
  overflow: auto;
  max-width: 900px;
  min-width: 300px;
  height: 500px;
}

.printArea {
  display: grid;
  // grid-gap: 15px;

  .code-container {
    width: 183px;
    box-sizing: border-box;
    padding: 2px 10px;
    padding-bottom: 10px;

    border: 1px solid #000;

    background-color: #fff;

    display: flex;
    flex-wrap: wrap;
    align-items: center;
    justify-content: center;
  }
}
.code-click {
  border: 2px dashed #7ebdaa !important;
}
.row-number-input {
  width: 450px;
  padding: 0 16px;
}

.colText {
  box-sizing: border-box;
  padding: 0 5px;
}

.padding-lr-16 {
  padding: 0 16px;
}
.printLabel {
  width: 100%;
  text-align: center;

  font-size: 12px;
  font-weight: bold;
  // padding-left: 15px;
  white-space: nowrap;
  overflow: hidden; /* 隐藏溢出的文本 */
  text-overflow: ellipsis; /* 显示省略号 */

  .labelTitle {
    opacity: 0.7;
    font-weight: 600;
  }

  div {
    width: 100%;
  }
}
</style>
