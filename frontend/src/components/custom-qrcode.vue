<template>
  <qrcode-vue class="qrcode" :style="{ 'margin-right': '20px' }" :value="props.value" :size="30" :foreground="props.foreground" @click="data.qrCodeDialog = !data.qrCodeDialog"></qrcode-vue>
  
  <v-dialog v-model="data.qrCodeDialog" max-width="290">
    <v-card>
      <qrcode-vue :margin="5" :value="props.value" :size="300" :foreground="props.foreground"></qrcode-vue>
    </v-card>
  </v-dialog>
</template>
<script setup lang="ts">
import { reactive } from 'vue'
import QrcodeVue from 'qrcode.vue'

const props = defineProps({
  value: {
    type: String,
    default: ''
  },
  foreground: {
    type: String,
    default: '#999'
  }
})

const data = reactive({
  qrCodeDialog: false,
})

</script>
<style lang="less" scoped>

.qrcode:hover{
  cursor: pointer;
  opacity: 0.8;
}
</style>
