<template>
  <div class="container">
    welcome Home!
    <RouterView v-slot="{ Component }">
      <keep-alive :include="openedMenus">
        <component :is="Component"></component>
      </keep-alive>
    </RouterView>
  </div>
</template>

<script lang="ts" setup>
import { computed, reactive } from 'vue'
import { router } from '@/router/index'
import { store } from '@/store'

const method = reactive({
  // Open menu
  openMenu: (menuName: string) => {
    if (menuName === 'login') {
      store.commit('system/clearOpenedMenu', menuName)
    } else {
      store.commit('system/addOpenedMenu', menuName)
    }
    router.push(menuName)
  }
})

const openedMenus = computed(() => store.getters['system/openedMenus'])
</script>

<style scoped lang="less"></style>
