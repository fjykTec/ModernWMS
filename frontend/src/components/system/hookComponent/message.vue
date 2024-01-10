<template>
  <v-alert
    :type="type || 'info'"
    :variant="variant || 'flat'"
    :border="border || false"
    :color="color || undefined"
    :width="messageWidth"
    :min-width="250"
  >
    <!-- :max-height="60" -->
    {{ content }}
  </v-alert>
</template>

<script lang="ts" setup>
import { onBeforeMount } from 'vue'

// Vue3 does not support external interfaces
const messageProps = defineProps<{
  type?: 'success' | 'info' | 'warning' | 'error'
  variant?: 'flat' | 'elevated' | 'tonal' | 'outlined' | 'text' | 'plain'
  content: string
  border?: boolean | 'top' | 'end' | 'bottom' | 'start'
  color?: string
  messageWidth?: string | number // message width pixel or percentage
  removeComponent(): void
}>()

onBeforeMount(() => {
  messageProps.removeComponent()
})
</script>
<style lang="less" scoped></style>
