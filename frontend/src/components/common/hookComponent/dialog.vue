<template>
  <v-dialog v-model="data.dialogVisible" persistent :width="dialogWidth">
    <v-card>
      <v-card-title class="text-h5"> {{ title }} </v-card-title>
      <v-card-text>{{ content }}</v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="green-darken-1" variant="text" @click="data.dialogVisible = false">
          {{ cancleText }}
        </v-btn>
        <v-btn
          v-if="typeof dialogProps.handleConfirm === 'function'"
          color="green-darken-1"
          variant="text"
          @click="method._sure()"
        >
          {{ confirmText }}
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts" setup>
import { watch, reactive } from 'vue'
import i18n from '@/languages/i18n'

const dialogProps = defineProps({
  visible: {
    type: Boolean,
    default: false
  },
  confirmText: {
    type: String,
    default: i18n.global.t('system.hookComponent.dialog.defaultConfirm')
  },
  cancleText: {
    type: String,
    default: i18n.global.t('system.hookComponent.dialog.defaultClose')
  },
  handleConfirm: {
    type: Function,
    default: null
  },
  // Remove dom function
  removeComponent: {
    type: Function,
    default: null
  },
  content: {
    type: String,
    default: ''
  },
  title: {
    type: String,
    default: i18n.global.t('system.hookComponent.dialog.defaultTitle')
  },
  dialogWidth: {
    type: [String, Number],
    default: 700
  }
})

const data = reactive({
  dialogVisible: false,
  dialogText: ''
})

// Init dialog visible
data.dialogVisible = Boolean(dialogProps.visible) // Avoid shallow copies

const method = reactive({
  _sure: async () => {
    await dialogProps.handleConfirm()
    data.dialogVisible = false
  }
})

// Shut dialog run remove function
watch(
  () => data.dialogVisible,
  (val) => {
    !val && dialogProps.removeComponent()
  }
)
</script>
