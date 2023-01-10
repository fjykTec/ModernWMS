<template>
  <div class="languageIcon">
    <v-menu>
      <template #activator="{ props }">
        <v-icon icon="mdi-web" v-bind="props"></v-icon>
      </template>
      <v-list>
        <v-list-item
          v-for="(item, index) in data.languageList"
          :key="index"
          :value="index"
          @click="method.changeLanguage(item.value)"
        >
          <v-list-item-title>{{ item.title }}</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-menu>
  </div>
</template>

<script lang="ts" setup>
import { reactive } from 'vue'
import { useI18n } from 'vue-i18n'
import { useLocale } from 'vuetify'
import { getSelcectedLang } from '@/languages/method/index'
import { getSelcectedLangForVuetify } from '@/plugins/vuetify/method/index'
import { store } from '@/store'

const { locale } = useI18n()
const { current } = useLocale()

const data = reactive({
  showLanguage: false,
  languageList: [
    { title: '简体中文', value: 'zh' },
    { title: 'Englist', value: 'en' }
  ]
})

const method = reactive({
  changeLanguage: (lang: string) => {
    localStorage.setItem('language', lang)
    store.commit('system/setLanguage', lang) // set store
    locale.value = getSelcectedLang(lang) // global
    current.value = getSelcectedLangForVuetify(lang) // vuetify
  }
})
</script>

<style lang="less" scoped>
.languageIcon {
  height: 30px;
  width: 40px;
  font-size: 16px;
  display: flex;
  justify-content: center;
  align-items: center;
}
</style>
