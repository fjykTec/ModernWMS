import { createStore } from 'vuex'
import VuexPersistence from 'vuex-persist'

import { user } from '@/store/module/user'
import { system } from '@/store/module/system'

const vuexLocal = new VuexPersistence({
  storage: window.localStorage,
  modules: ['user', 'system']
})

export const store = createStore({
  modules: {
    user,
    system
  },
  plugins: [vuexLocal.plugin]
})
