// index.ts
import { createRouter, createWebHashHistory, RouteRecordRaw } from 'vue-router'
import { store } from '@/store'
import { CustomerRouterProps } from '@/types/system/router'

// array => dynamic router
let dynamicRouter: CustomerRouterProps[] = []
let loadedRouter: string[] = []

// default router
const routes: RouteRecordRaw[] = [
  {
    path: '/',
    redirect: '/login'
  },
  {
    path: '/login',
    name: 'login',
    component: () => import('@/view/Login/login.vue')
  },
  {
    name: 'home',
    path: '/home',
    component: () => import('@/view/Home/home.vue'),
    children: []
  }
]

// create router
const router = createRouter({
  // mode is hash
  history: createWebHashHistory(),
  routes,
  // Return to top after route jump
  // scrollBehavior() {
  //   return { top: 0 }
  // }
})

// vite2 dynamic import
const modules = import.meta.glob('../view/*/*/*.vue')

// load router function
export function loadRouter() {
  dynamicRouter = store.getters['user/menulist']
  // Clear the loaded routes before creating them, mainly for logged out users
  for (const item of loadedRouter) {
    router.removeRoute(item)
  }
  loadedRouter = []
  // This works normally, but it doesn't seem right when getting router
  for (const item of dynamicRouter) {
    item.component = modules[`../view/${ item.directory }${ item.path }.vue`]
    router.addRoute('home', item)
    loadedRouter.push(item.name) // cache dynamic router
  }
}

// Set the front routing guard
router.beforeEach((to, from, next) => {
  // join system
  if (to.path === '/login') {
    dynamicRouter = []
    return next()
  }

  // dont have token, back login
  // if (!store.getters['user/token']) {
  //   return next('/login')
  // }

  if (dynamicRouter.length === 0) {
    loadRouter()
    return next(to.path)
  }

  next()
})

// disable back or forward
// router.afterEach(() => {
//   window.history.pushState(null, '', window.location.href)
// })

export { router }
