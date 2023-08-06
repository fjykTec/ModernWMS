<template>
  <v-tooltip location="bottom">
    <template #activator="{ props }">
      <v-btn :disabled="disabled" :flat="flat" class="mr-3" :size="size" icon v-bind="props" @click="method.clickEvent">
        <v-icon :color="iconColor"> {{ icon }} </v-icon>
      </v-btn>
    </template>
    <span>{{ tooltipText }}</span>
  </v-tooltip>
</template>
<script setup lang="ts">
import { reactive } from 'vue'

const emit = defineEmits(['click'])

defineProps({
  icon: {
    type: String,
    default: ''
  },
  tooltipText: {
    type: String,
    default: ''
  },
  size: {
    type: String,
    default: 'small'
  },
  // Whether show button shadow. 
  // It's hasn't shadow when 'flat = true'.
  flat: {
    type: Boolean,
    default: false
  },
  iconColor: {
    type: String,
    default: ''
  },
  disabled: {
    type: Boolean,
    default: false
  }
})

const method = reactive({
  clickEvent: () => {
    emit('click')
  }
})
</script>
<style lang="less"></style>
