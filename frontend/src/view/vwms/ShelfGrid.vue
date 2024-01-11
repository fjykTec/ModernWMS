<!--
 @name : grid.vue
 @Description : 
 @author : saal
 @date : 2023/10/10
-->
<template>
  <div class="shelf-container">
    <div> {{ props.shelfGridData?.shelf_name }}</div>
    <div class="info">货架规格：{{ props.shelfGridData?.layer }} * {{ props.shelfGridData?.column }}</div>
    <div :style="styleObject" class="shelf-grid" @click="selectProduct">
      <div
        v-for="(index) in formatIndex"
        :key="index"
        :class="{'have':props.shelfGridData?.products.includes(index), 'selected':index === selectIndex}"
        :data-index="index"
        class="product-grid"
      ></div>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  shelfGridData: {
    type: Object
  },
  selectIndex: {
    type: Number
  }
})
const emit = defineEmits(['selectShelfItem'])
const styleObject = computed(() => ({
  gridTemplateColumns: `repeat(${ props.shelfGridData.column }, 1fr)`,
  gridTemplateRows: `repeat(${ props.shelfGridData.layer }, 1fr)`
}))

const formatIndex = computed(() => {
  const arr = Array.from({ length: props.shelfGridData.layer * props.shelfGridData.column }, (_, index) => index + 1)
    .reverse()
  const result = []
  for (let i = 0; i < arr.length; i += props.shelfGridData.column) {
    result.push(arr.slice(i, i + props.shelfGridData.column)
      .reverse())
  }
  return result.flat()
})

const selectProduct = (e) => {
  const index = parseInt(e.target.dataset.index)
  emit('selectShelfItem', index)
}
</script>

<style lang="less" scoped>

.shelf-container {
  position: absolute;
  left: 10px;
  top: 90px;
  padding: 15px;
  background-color: white;
  transition: all .5s ease-out;
  overflow: hidden;

  .header {
    display: flex;
    align-items: center;
    justify-content: space-between;
  }

  .shelf-grid {
    display: grid;
    margin: 10px 0;
    grid-gap: 2px;

    .product-grid {
      width: 40px;
      height: 40px;
      background-color: #d7d7d7;
      cursor: pointer;

      &:hover {
        box-shadow: rgba(50, 50, 93, 0.25) 0 50px 100px -20px, rgba(0, 0, 0, 0.3) 0px 30px 60px -30px, rgba(10, 37, 64, 0.35) 0px -2px 6px 0px inset;
      }
    }
  }
}

.have {
  background-color: #54a432 !important;
}

.selected {
  border: 3px solid #56a0ff;
}

</style>