<template>
  <v-dialog v-model="data.showDialog" :persistent="true" transition="dialog-top-transition" width="auto">
    <template #default>
      <v-card :title="$t('system.page.preView')">
        <v-card-text>
          <div class="print-area-container">
            <div class="print-area-scroll" @click="method.resetIndex()">
              <div
                id="printArea"
                :style="{
                  'grid-template-columns': `repeat(${data.rowsNumber},1fr)`,
                  rowGap: `${data.gridRowGap}px`,
                  columnGap: `${data.gridColumnGap}px`
                }"
                class="printArea"
              >
                <div
                  v-for="(item, index) in data.printData"
                  :key="index"
                  class="code-container"
                  :class="{ 'code-click': data.clickIndex === index }"
                  @click.stop="method.setIndex(index)"
                >
                  <svg :id="'printBarCode' + item.id"></svg>
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
            <v-btn v-print="'#printArea'" color="primary" :disabled="isDisabled" variant="text" @click="method.resetIndex()">{{ $t('system.page.print') }}</v-btn>
          </div>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>
</template>

<script lang="ts" setup>
import { computed, nextTick, reactive, watch } from 'vue'
import JsBarcode from 'jsbarcode'
import { setStorage, getStorage } from '@/utils/common'

const props = defineProps<{
  menu: string
}>()

const data = reactive({
  showDialog: false,
  printData: [] as any,
  rowsNumber: 5,
  gridColumnGap: 15,
  gridRowGap: 15,
  clickIndex: -1,
  count: 1
})
const isDisabled = computed(() => data.printData.length === 0)
watch(
  () => data.printData,
  () => {
    if (data.printData.length < 5) {
      data.rowsNumber = data.printData.length
    } else {
      data.rowsNumber = 5
    }
  }
)

const method = reactive({
  changePrintParams: () => {
    const storage = {
      cols: data.rowsNumber,
      rs: data.gridRowGap
    }

    setStorage(`BarCode-${ props.menu }`, storage)
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
      const storage: any = getStorage(`BarCode-${ props.menu }`)

      if (storage) {
        data.rowsNumber = storage.cols
        data.gridRowGap = storage.rs
      }
    } catch {
      data.rowsNumber = 5
      data.gridRowGap = 15
    }

    data.printData = row

    data.showDialog = true

    nextTick(() => {
      for (const item of data.printData) {
        JsBarcode(`#printBarCode${ item.id }`, item.id, {
          fontSize: 12,
          width: 2,
          height: 40,
          displayValue: true
        })
      }
    })
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

<style lang="less" scoped>
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
  align-items: center;
  // grid-gap: 15px;

  .code-container {
    display: flex;
    align-items: center;
    justify-content: center;
  }
}
.code-click {
  border: 2px dashed #7ebdaa !important;
}

.row-number-input {
  width: 350px;
  padding: 0 16px;
}

.colText {
  box-sizing: border-box;
  padding: 0 5px;
}

.padding-lr-16 {
  padding: 0 16px;
}
</style>
