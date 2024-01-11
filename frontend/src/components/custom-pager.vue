<template>
  <div class="pagerContainer">
    <v-row>
      <v-col cols="8" class="fl-c">
        <v-pagination
          v-model="data.pageIndex"
          :total-visible="7"
          :length="PAGER_LENGTH"
          size="small"
          rounded="circle"
          :active-color="primaryColor"
          @update:modelValue="method.pageChange"
        ></v-pagination>
      </v-col>
      <v-col cols="2" class="fl-s">
        <v-select
          v-model="data.pageSizeSelect"
          hide-details
          variant="underlined"
          density="compact"
          :items="PAGER_SIZE_OPTION"
          item-title="label"
          item-value="value"
          @update:modelValue="method.changePageSizeSelect"
        ></v-select>
      </v-col>
      <v-col cols="2" class="fl-c">
        <div>{{ TOTAL_RECORD }}</div>
      </v-col>
    </v-row>
  </div>
</template>
<script setup lang="ts">
import { reactive, computed } from 'vue'
import { PAGE_SIZE, DEFAULT_PAGE_SIZE } from '@/constant/vxeTable'
import { primaryColor } from '@/constant/style'
import i18n from '@/languages/i18n'

const emit = defineEmits(['click', 'page-change'])

const props = defineProps({
  currentPage: {
    type: Number,
    default: 1
  },
  pageSize: {
    type: Number,
    default: DEFAULT_PAGE_SIZE
  },
  total: {
    type: Number,
    default: 0
  },
  pageSizes: {
    type: Array<number>,
    default: PAGE_SIZE
  },
  // ！For the time being is not enabled
  layouts: {
    type: Array<string>,
    default: ['PrevJump', 'PrevPage', 'Number', 'NextPage', 'NextJump', 'Sizes', 'FullJump', 'Total']
  },
  // ！For the time being is not enabled
  perfect: {
    type: Boolean,
    default: false
  }
})

const data = reactive({
  pageIndex: 1,
  pageSizeSelect: DEFAULT_PAGE_SIZE
})

const method = reactive({
  pageChange: (currentPage: number) => {
    emit('page-change', { currentPage, pageSize: props.pageSize })
  },
  changePageSizeSelect: (pageSize: number) => {
    emit('page-change', { pageIndex: data.pageIndex, pageSize })
  }
})

const PAGER_SIZE_OPTION = computed(() => {
  const res: any = []
  for (const pageSize of props.pageSizes) {
    res.push({ label: `${ pageSize }${ i18n.global.t('system.pager.pageSizes') }`, value: pageSize })
  }
  return res
})
const TOTAL_RECORD = computed(() => `${ i18n.global.t('system.pager.total') } ${ props.total } ${ i18n.global.t('system.pager.record') }`)
const PAGER_LENGTH = computed(() => {
  if (!props.total) {
    return 1
  }
  return Math.ceil(props.total / props.pageSize)
})
</script>
<style lang="less" scoped>
.pagerContainer {
  color: #666;
}

.fl-s {
  display: flex;
  align-items: flex-start;
}

.fl-c {
  display: flex;
  align-items: center;
  justify-content: flex-end;
}
</style>
