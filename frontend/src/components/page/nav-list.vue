<template>
  <div style="height: 100%">
    <!-- title -->
    <div class="nav-list-title">{{ title }}</div>

    <!-- list -->
    <div class="nav-list">
      <div
        v-for="(item, index) of listData"
        :key="index"
        class="nav-item"
        :class="[indexValue === String(item[indexKey]) ? 'is-active-item' : '']"
        @click.stop="method.itemClick(item)"
      >
        <div class="nav-item__name">
          {{ item[labelKey] }}
        </div>
        <v-icon v-if="indexValue === String(item[indexKey])" class="nav-item__icon" icon="mdi-menu-left"></v-icon>
      </div>
    </div>

    <!-- no data -->
    <div v-if="!listData.length" class="null-data">{{ $t('system.tips.noData') }}</div>
  </div>
</template>

<script lang="ts" setup>
import { reactive } from 'vue'

const emit = defineEmits(['itemClick'])

const props = defineProps<{
  title: string // Sidebar Title
  listData: any[] // Sidebar Data
  labelKey: string // Key displayed in the sidebar
  // The selected key and value in the sidebar
  // For example, if a List is [{a: 1, b: 2}, {a: 3, b: 4}], I want to activate the row with b=4. Then indexKey=b indexValue=4
  indexKey: string
  indexValue: string
}>()

const method = reactive({
  itemClick: (item: any) => {
    if (props.indexValue === String(item[props.indexKey])) {
      return
    }
    emit('itemClick', item)
  }
})
</script>

<style lang="less" scoped>
@normal-transition: all 0.2s ease;
.nav-list-title {
  height: 35px;
  width: 100%;
  box-sizing: border-box;
  padding-left: 10px;
  background-color: #f8f8f9;
  color: #444;
  display: flex;
  justify-content: flex-start;
  align-items: center;
}

.nav-list {
  height: 100%;
  overflow-y: scroll;
}
.nav-list::-webkit-scrollbar {
  display: none;
}

.nav-item {
  box-sizing: border-box;
  width: 93%;
  min-height: 40px;
  padding: 10px 20px;
  margin: 8px auto;
  transition: @normal-transition;
  border-radius: 5px;
  display: flex;
  justify-content: flex-start;
  align-items: center;
}

.nav-item:hover {
  cursor: pointer;
  background-color: #e3ecfb;
  transition: @normal-transition;
}

.nav-item__name {
  width: 80%;
}

.is-active-item {
  background-color: #e3ecfb;
  color: #9C27B0;
  transition: @normal-transition;
  position: relative;
  // justify-content: space-between;

  // .nav-item__name {
  //   width: 80%;
  // }
  .nav-item__icon {
    width: 20px;
    position: absolute;
    right: 5px;
    > img {
      width: 15px;
      height: 12px;
    }
  }
}

.null-data {
  font-size: 12px;
  color: #606266;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
}
</style>
