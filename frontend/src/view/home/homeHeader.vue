<template>
  <div class="homeHeader">
    <div class="menuTitle">
      <v-breadcrumbs :items="data.breadcrumbItems">
        <template #prepend>
          <v-icon :color="lightGrey" size="small" icon="mdi-form-select"></v-icon>
        </template>
      </v-breadcrumbs>
    </div>
    <div class="toolsBar">
      <div class="gitSrc mr-4">
        <img src="@/assets/img/gitee.png" alt="Gitee" @click="method.toGit('gitee')" />
      </div>
      <div class="gitSrc mr-4">
        <img src="@/assets/img/github.png" alt="Gitee" @click="method.toGit('github')" />
      </div>
      <div class="gitSrc mr-4">
        <img src="@/assets/img/apifox.png" alt="API" @click="method.toGit('apifox')" />
      </div>
      <LanguagesSwitch />
      <v-menu>
        <template #activator="{ props }">
          <div class="toolItems headPortrait ml-4" v-bind="props">
            <p class="firstName">{{ firstName }}</p>
            <div class="alive"></div>
          </div>
        </template>
        <v-list>
          <v-list-item v-for="(item, index) in data.userOperationMenu" :key="index" :value="index" @click="method.operation(item.value)">
            <v-list-item-title :title="item.title">{{ item.title }}</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </div>
    <ChangePwd ref="ChangePwdRef" />

    <ViewLogDialog ref="ViewLogDialogRef" />
  </div>
</template>

<script lang="ts" setup>
import { reactive, watch, computed, ref } from 'vue'
import { useRouter } from 'vue-router'
import LanguagesSwitch from '@/components/system/languages.vue'
import { lightGrey } from '@/constant/style'
import i18n from '@/languages/i18n'
import { router } from '@/router'
import { store } from '@/store'
import { DataProps } from '@/types/Home/HomeHeader'
import ChangePwd from '@/components/system/change-pwd.vue'
import ViewLogDialog from '@/components/system/view-log-dialog.vue'

const routerInfo = useRouter()
const ChangePwdRef = ref()
const ViewLogDialogRef = ref()

const data: DataProps = reactive({
  breadcrumbItems: [],
  // User Action List
  userOperationMenu: [
    {
      title: i18n.global.t('system.homeHeader.changePwd'),
      value: 'changePwd'
    },
    {
      title: i18n.global.t('system.homeHeader.log'),
      value: 'viewLog'
    },
    {
      title: i18n.global.t('system.homeHeader.logout'),
      value: 'logout'
    }
  ]
})

// Get current page route info
watch(
  () => routerInfo,
  (newValue) => {
    const MODULE = i18n.global.t(`router.sideBar.${ newValue.currentRoute.value.meta.menuModule }`)
    const PATH = i18n.global.t(`router.sideBar.${ newValue.currentRoute.value.meta.menuPath }`)

    data.breadcrumbItems = [
      {
        title: PATH,
        disabled: true
      }
    ]

    if (newValue.currentRoute.value.meta.menuModule) {
      data.breadcrumbItems.unshift({
        title: MODULE,
        disabled: true
      })
    }
  },
  { immediate: true, deep: true }
)

const method = reactive({
  // User operation method
  operation: (value: string) => {
    if (value === 'logout') {
      store.commit('system/clearOpenedMenu')
      store.commit('system/setCurrentRouterPath', '')

      router.push('/login')
    } else if (value === 'changePwd') {
      ChangePwdRef.value.open()
    } else if (value === 'viewLog') {
      ViewLogDialogRef.value.openDialog()
    }
  },
  toGit: (type: string) => {
    if (type === 'gitee') {
      window.open('https://gitee.com/modernwms/ModernWMS', '_blank')
    } else if (type === 'github') {
      window.open('https://github.com/fjykTec/ModernWMS', '_blank')
    } else if (type === 'apifox') {
      window.open('https://apifox.com/apidoc/shared-c34f3f10-1982-4d24-8214-a8c2490fd02e', '_blank')
    }
  }
})

const firstName = computed(() => {
  const userInfo = store.getters['user/userInfo']
  return userInfo?.user_name?.charAt(0) || ''
})
</script>

<style scoped lang="less">
@headerHeight: 60px;

.homeHeader {
  width: 100%;
  height: @headerHeight;
  position: relative;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0 20px;
  box-sizing: border-box;
  border-radius: 0 0 10px 10px;
  transition: all 0.5s;

  box-shadow: 0 3px 3px -2px var(--v-shadow-key-umbra-opacity, rgba(0, 0, 0, 0.2)),
    0 3px 4px 0 var(--v-shadow-key-penumbra-opacity, rgba(0, 0, 0, 0.14)), 0 1px 8px 0 var(--v-shadow-key-penumbra-opacity, rgba(0, 0, 0, 0.12));
  background-color: rgba(255, 255, 255, 0.5);

  .menuTitle {
    display: flex;

    .v-icon {
      margin-right: 10px;
    }

    span {
      color: #b9b8bc;
    }
  }

  .gitSrc {
    display: flex;
    align-items: center;
    justify-content: flex-end;

    > img {
      width: 24px;
      height: 24px;
      opacity: 0.5;
    }

    :hover {
      cursor: pointer;
      opacity: 1;
      scale: 1.1;
      transition: all ease-in-out 0.2s;
    }
  }
  .toolsBar {
    display: flex;
    align-items: center;
    justify-content: flex-end;

    .toolItems {
      height: 40px;
      width: 40px;
      font-size: 16px;
      display: flex;
      justify-content: center;
      align-items: center;
      transition: all 0.5s;
      border-radius: 40px;
      margin-right: 10px;
      cursor: pointer;

      &:hover {
        background-color: #e4e5ea;
      }

      &:active {
        background-color: #cdced3;
      }
    }

    .bell {
      position: relative;

      .hasNotify {
        margin: 0;
        width: 11px;
        height: 11px;
        border-radius: 11px;
        background-color: #ff4c51;
        border: 1px solid #fff;
        position: absolute;
        top: 5px;
        right: 7px;
      }
    }

    .headPortrait {
      border-radius: 40px;
      width: 40px;
      height: 40px;
      background-color: #e8e2fb;
      position: relative;

      .alive {
        margin: 0;
        width: 11px;
        height: 11px;
        border-radius: 11px;
        background-color: #56ca00;
        border: 1px solid #fff;
        position: absolute;
        bottom: 1px;
        right: 1px;
      }
    }
  }
}

.firstName {
  font-weight: 600;
  color: #9c27b0;
}
</style>
