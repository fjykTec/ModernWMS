<template>
  <v-dialog v-model="data.formVisible" :width="contentWidth" height="90%">
    <v-card>
      <v-toolbar color="white">
        <v-toolbar-title>{{ $t('system.page.preView') }}</v-toolbar-title>
        <v-spacer></v-spacer>
        <v-btn color="primary" variant="text" @click="method.print">{{ $t('system.page.print') }}</v-btn>
        <v-btn variant="text" @click="method.hideModal">{{ $t('system.page.close') }}</v-btn>
      </v-toolbar>
      <div class="preview_box">
        <v-row>
          <v-col cols="12">
            <div id="preview_content_custom" class="preview_content"></div>
          </v-col>
        </v-row>
      </div>
    </v-card>
  </v-dialog>
</template>
<script lang="ts" setup>
import { reactive, computed } from 'vue'
import $ from 'jquery'

const data = reactive({
  formVisible: false,
  waitShowPrinter: false,
  // 纸张宽 mm
  width: 0,
  // 模板
  hiprintTemplate: {} as any,
  hiprintData: {} as any,
  // 数据
  printData: {}
})
const method = reactive({
  hideModal() {
    data.formVisible = false
  },
  show() {
    data.formVisible = true
    setTimeout(() => {      
      $('#preview_content_custom').html(data.hiprintTemplate.getHtml(data.printData))
    }, 500)
  },
  print() {
    data.waitShowPrinter = true
    data.hiprintTemplate.print(
      data.printData,
      {},
      {
        callback: () => {
          data.waitShowPrinter = false
        }
      }
    )
  }
})
const contentWidth = computed(() => {
  let cwidth = (data.width * 3.78) as number
  cwidth += 25
  return `${ cwidth }px`
})
defineExpose({
  data,
  method
})
</script>
<style scoped lang="less">
.preview_box {
  display: flex;
  justify-content: center; /* 水平居中对齐 */
  border: 1px solid #e8e8e8;
  box-sizing: border-box;
  border-radius: 2px;
}
</style>
