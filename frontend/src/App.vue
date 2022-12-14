<template>
  <div>
    <v-dialog v-model="loadingFlag" :scrim="false" persistent max-width="200">
      <v-card color="primary">
        <v-card-text>
          Loading...
          <v-progress-linear indeterminate color="white"></v-progress-linear>
        </v-card-text>
      </v-card>
    </v-dialog>
    <router-view></router-view>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { emitter } from './utils/bus'

const loadingFlag = ref(false)

onMounted(() => {
  // const customerLang = localStorage.getItem('language')
  // if (customerLang) {
  //   console.log(customerLang)
  // }

  emitter.on('showLoading', () => {
    // console.log('showloading')
    loadingFlag.value = true
  })
  emitter.on('closeLoading', () => {
    // console.log('closeLoading')
    loadingFlag.value = false
  })
})
</script>

<style scoped>
div {
  height: 100%;
  width: 100%;
}
</style>
