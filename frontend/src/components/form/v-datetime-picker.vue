<template>
  <div class="pickerContainer">
    <v-text-field
      v-model="textVal"
      clearable
      hide-details
      density="comfortable"
      class="searchInput ml-5 mt-1"
      :label="props.label"
      variant="solo"
      :readonly="data.readOnly"
      :persistent-clear="props.disabled ? false : true"
      :disabled="props.disabled"
    >
    </v-text-field>
    <div class="handleClickBlock">
      <vxe-input v-model="textVal" type="datetime" transfer></vxe-input>
    </div>
  </div>
</template>

<script setup lang="tsx">
import { reactive, computed } from 'vue'

const emit = defineEmits(['update:modelValue'])

const props = defineProps<{
  label: string
  disabled?: boolean
  modelValue: string
}>()

const data = reactive({
  val1: '',
  readOnly: true
})

const textVal = computed({
  get: () => props.modelValue,
  set: (val: string) => {
    emit('update:modelValue', val)
  }
})
</script>

<style scoped lang="less">
.vxe-input {
  opacity: 0 !important;
  height: 50px;
  width: 100% !important;
}
:deep(.vxe-input--inner) {
  border: none !important;
  padding-right: 0 !important;
  // width: calc(100% - 36px) !important;
}

:deep(.vxe-input--extra-suffix) {
  display: none !important;
}

.pickerContainer {
  position: relative;
  // display: inline-block;
  width: 100%;
}
.handleClickBlock {
  position: absolute;
  top: 0px;
  left: 20px;
  width: calc(100% - 36px - 20px - 10px) !important;
  height: 100%;
}
</style>
