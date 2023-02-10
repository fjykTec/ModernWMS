<template>
  <div v-if="props.value" class="checkbox checkboxConfirm" @click="method.clickCheckBox()">
    <v-icon style="font-size: 12px"> mdi-check </v-icon>
  </div>
  <div v-else class="checkbox" @click="method.clickCheckBox()"></div>
</template>

<script lang="ts" setup>
import { reactive } from 'vue'

const emit = defineEmits(['update:value'])

const props = defineProps<{
  value: boolean
}>()

const method = reactive({
  clickCheckBox: () => {
    emit('update:value', !props.value)
  }
})
</script>

<style scoped lang="less"></style>
