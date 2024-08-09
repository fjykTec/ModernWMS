<template>
  <v-row no-gutters @keyup.enter="method.sureSearch">
    <v-col v-if="[1].includes(searchSetting.length)" cols="4"> </v-col>
    <v-col v-if="[1, 2].includes(searchSetting.length)" cols="4"> </v-col>
    <v-col v-for="(item, index) of searchSetting" :key="index" cols="4">
      <template v-if="item.type === 'string'">
        <v-text-field
          v-model="searchForm[item.name]"
          clearable
          hide-details
          density="comfortable"
          class="searchInput ml-5 mt-1"
          :label="$t(`${props.i18nPrefix}.${item.name}`)"
          variant="solo"
        >
        </v-text-field>
      </template>
      <template v-if="item.type === 'number'">
        <v-text-field
          v-model="searchForm[item.name]"
          clearable
          hide-details
          density="comfortable"
          class="searchInput ml-5 mt-1"
          :label="$t(`${props.i18nPrefix}.${item.name}`)"
          variant="solo"
          type="input"
        >
        </v-text-field>
      </template>
      <template v-else-if="item.type === 'datetime'">
        <v-datetime-picker v-model="searchForm[item.name]" :label="$t(`${props.i18nPrefix}.${item.name}`)" />
      </template>
    </v-col>
  </v-row>

  <set-search ref="SetSearchRef" :i18n-key="props.i18nPrefix" :options="data.searchSetting" @success="method.refreshSearchSetting()" />
</template>

<script setup lang="tsx">
import { reactive, computed, ref, onMounted } from 'vue'
import { getMenuSearchSetting } from '@/utils/common'
import vDatetimePicker from '../form/v-datetime-picker.vue'
import searchSettingSet from '@/constant/searchSettingSet'
import SetSearch from '@/components/system/set-search.vue'

const SetSearchRef = ref()

const emit = defineEmits(['sureSearch', 'update:modelValue'])

const props = defineProps<{
  modelValue: any
  // searchSetting: any[]
  menuName: string
  i18nPrefix: string
}>()

const data = reactive({
  searchSetting: [] as string[],
  initForm: null
})

const method = reactive({
  sureSearch: () => {
    emit('sureSearch')
  },
  // 刷新查询条件
  refreshSearchSetting: () => {
    // 不加ref不会触发响应式
    searchForm.value = ref(JSON.parse(JSON.stringify(data.initForm)))

    // Obtain query condition settings
    const searchSetting = getMenuSearchSetting(props.menuName)
    if (searchSetting.length > 0) {
      data.searchSetting = searchSetting
    } else {
      data.searchSetting = searchSettingSet[props.menuName].default
    }
  }
})

onMounted(() => {
  data.initForm = JSON.parse(JSON.stringify(props.modelValue))

  method.refreshSearchSetting()
})

const searchForm = computed({
  get: () => props.modelValue,
  set: (val: any) => {
    emit('update:modelValue', val)
  }
})

const searchSetting = computed(() => {
  const result = searchSettingSet[props.menuName].list.filter((item: any) => data.searchSetting.includes(item.name))
  return result
})

defineExpose({
  openDialog: () => {
    SetSearchRef.value.openDialog(props.menuName)
  }
})
</script>

<style scoped lang="less"></style>
