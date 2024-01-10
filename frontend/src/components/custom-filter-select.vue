<template>
  <div :id="`id${data.dynamicID}`">
    <v-combobox
      ref="comboBoxRef"
      v-model="data.cacheObj"
      :label="props.label"
      :items="cacheItems"
      :item-title="props.itemTitle"
      :rules="props.rules"
      variant="outlined"
      clearable
      autocomplete="off"
      return-object
      @click:clear="method.valueClear"
      @update:model-value="method.valueChange"
    >
    </v-combobox>
  </div>
</template>

<script setup lang="tsx">
import { reactive, ref, onMounted, nextTick, computed } from 'vue'
import { generateUUID } from '@/utils/common'

const emit = defineEmits(['update:modelValue'])

const props = defineProps<{
  modelValue: any
  items: any[]
  itemTitle: string
  label: string
  rules: any[]
  mapping: any
}>()

const comboBoxRef = ref()

const data = reactive({
  cacheObj: undefined as any,
  dynamicID: generateUUID()
})

const method = reactive({
  // 点击清空
  valueClear: () => {
    data.cacheObj = undefined

    const cache = { ...props.modelValue }

    for (const item of props.mapping) {
      cache[item.in] = ''
    }

    emit('update:modelValue', cache)
  },
  // 值改变
  valueChange: (val: any) => {
    if (val) {
      const cache = { ...props.modelValue }

      for (const item of props.mapping) {
        cache[item.in] = val[item.out]
      }
      emit('update:modelValue', cache)
    }
  },
  // 选择框光标消失
  selectBlur: () => {
    if (typeof data.cacheObj === 'string') {
      data.cacheObj = undefined
    }
  }
})

onMounted(() => {
  // 处理原始值
  let cache: any = {}

  for (const item of props.mapping) {
    if (props.modelValue[item.in] || props.modelValue[item.in] === 0) {
      cache[item.out] = props.modelValue[item.in]
    } else {
      cache[item.out] = ''
    }
  }

  if (Object.values(cache).filter((item) => item !== '' && item !== 0).length === 0) {
    cache = undefined
  }

  data.cacheObj = cache

  // 挂载失去标点参数
  nextTick(() => {
    const inputDom: any = document.querySelector(`#${ `id${ data.dynamicID }` } > div > div.v-input__control > div > div.v-field__field > div > input`)
    if (inputDom) {
      inputDom.onblur = method.selectBlur
    }
  })
})

const cacheItems = computed(() => {
  // 处理Items
  const cacheItems: any = [...props.items]

  for (const item of cacheItems) {
    for (const key of Object.keys(item)) {
      // 处理数据, 保证下拉框正常运行
      if (item[key] === 0) {
        continue
      }
      if (!item[key]) {
        item[key] = ''
      }
    }
  }

  return cacheItems
})
</script>

<style scoped lang="less"></style>
