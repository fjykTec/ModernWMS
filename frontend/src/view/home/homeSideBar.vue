<template>
  <div style="height: 100%">
    <div class="homeSidebar">
      <div class="sideBarTitle">
        <Logo :height="50" :top="15" :left="5" />
      </div>
      <div class="sideBarMenus">
        <div v-for="(item, index) in data.menuList" :key="index">
          <div class="menuItems" :class="method.getItemClass(item)" @click="method.openMenu(item)">
            <div style="display: flex; align-items: center; height: 100%">
              <div class="menuIcon">
                <v-icon
                  :icon="item.icon ? 'mdi-' + item.icon : 'mdi-checkbox-blank-circle-outline'"
                  :size="18"
                  :color="currentRouterPath === item.routerPath ? '#fff' : '#524e59'"
                ></v-icon>
              </div>
              <div class="menuLabel">{{ item.lable }}</div>
            </div>
            <div v-if="item.children && item.children.length > 0" :class="item.showDetail && 'rotate90'">
              <v-icon icon="mdi-chevron-right" color="#524e59" :size="22"></v-icon>
            </div>
          </div>
          <!-- <Transition name="menu"> -->
          <v-expand-transition>
            <div v-show="item.showDetail">
              <div
                v-for="(detailItem, detailIndex) in item.children"
                :key="detailIndex"
                class="menuItems padding-l"
                :class="method.getItemClass(detailItem)"
                @click="method.openMenu(detailItem)"
              >
                <div style="display: flex; align-items: center; height: 100%">
                  <div class="menuIcon">
                    <v-icon
                      :icon="detailItem.icon ? `mdi-${detailItem.icon}` : 'mdi-checkbox-blank-circle-outline'"
                      :size="14"
                      :color="currentRouterPath === detailItem.routerPath ? '#fff' : '#524e59'"
                    ></v-icon>
                  </div>
                  <div class="menuLabel">{{ detailItem.lable }}</div>
                </div>
              </div>
            </div>
          </v-expand-transition>
          <!-- </Transition> -->
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { reactive, onMounted, computed } from 'vue'
import Logo from '@/components/system/logo.vue'
import { SideBarMenu, SideBarDataProps } from '@/types/Home/Home'
import { menusToSideBar } from '@/utils/router'
import { store } from '@/store'
import { router } from '@/router'

const data: SideBarDataProps = reactive({
  menuList: []
})

const method = reactive({
  // Open menu
  openMenu: (item: SideBarMenu) => {
    if (item.children && item.children.length > 0) {
      item.showDetail = !item.showDetail
    } else if (item.routerPath && currentRouterPath.value !== item.routerPath) {
      // If the selected menu is skipped, no action will be taken
      store.commit('system/setCurrentRouterPath', item.routerPath)
      store.commit('system/addOpenedMenu', item.routerPath)
      router.push(item.routerPath)
    }
    // if (menuName === 'login') {
    //   store.commit('system/clearOpenedMenu', menuName)
    // } else {
    //   store.commit('system/addOpenedMenu', menuName)
    // }
    // router.push(menuName)
  },
  // Get item class
  getItemClass: (item: SideBarMenu) => {
    if (item.children && item.children.length > 0 && item.showDetail) {
      return 'openedMenuItems'
    }
    if (currentRouterPath.value === item.routerPath) {
      return 'activeMenuItems'
    }
    return ''
  }
})

// Currently selected menu
const currentRouterPath = computed(() => store.getters['system/currentRouterPath'])

onMounted(() => {
  data.menuList = menusToSideBar()
})
</script>

<style scoped lang="less">
@headerHeight: 60px;
@sideBarWidth: 300px;
@sideBarTitleHeight: 70px;
.homeSidebar {
  width: @sideBarWidth;
  box-shadow: 5px 5px 5px #dbdce2;
  height: 100%;
  .sideBarTitle {
    height: @sideBarTitleHeight;
    position: relative;
  }
  .sideBarMenus {
    height: calc(100% - @sideBarTitleHeight);
    overflow: auto;
    .menuItems {
      // height: 42px;
      width: calc(100% - 20px);
      box-sizing: border-box;
      padding: 10px 0;
      padding-left: 22px;
      padding-right: 8px;
      border-radius: 0 50px 50px 0;
      margin-bottom: 7px;
      display: flex;
      justify-content: space-between;
      align-items: center;
      color: #696671;
      cursor: pointer;
      .menuIcon {
        height: 100%;
        width: 20px;
        margin-right: 10px;
        display: flex;
        justify-content: center;
        align-items: center;
        .v-icon {
          width: 10px;
          height: 10px;
        }
      }
      &:hover {
        background-color: #edeef3;
      }
    }

    .openedMenuItems {
      background-color: #e8e9ed;
    }
    .activeMenuItems {
      background: linear-gradient(to right, #af85fc, #9155fd);
      color: #fff;
      &:hover {
        background: linear-gradient(to right, #af85fc, #9155fd);
      }
    }

    &::-webkit-scrollbar {
      width: 7px;
    }

    &::-webkit-scrollbar-thumb {
      border-radius: 20px;
      background-color: #e5e5e9;
    }
  }
}

.rotate90 {
  transform: rotate(90deg);
}

.padding-l{
  padding-left: 44px !important;
}
</style>
