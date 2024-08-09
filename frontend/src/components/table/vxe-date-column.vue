<template>
  <vxe-column :field="field" :title="title" :width="width">
    <template #default="{ row, column }">
      <span>{{ formatDate(row[column.property], format) }}</span>
    </template>
  </vxe-column>
</template>

<script setup lang="tsx">
import { defineProps, withDefaults } from 'vue'
import { formatDate } from '@/utils/format/formatSystem'

withDefaults(
  defineProps<{
    field: string
    title: string
    width?: string | number
    format?: string
  }>(),
  {
    format: 'yyyy-MM-dd',
    width: ''
  }
)
</script>

<style scoped lang="less"></style>
