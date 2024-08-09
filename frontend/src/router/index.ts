// index.ts
import { createRouter, createWebHashHistory, RouteRecordRaw } from 'vue-router'
import { store } from '@/store'
import { CustomerRouterProps } from '@/types/System/Router'
import { menusToRouter } from '@/utils/router'

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
    component: () => import('@/view/login/login.vue')
  },
  {
    name: 'home',
    path: '/home',
    redirect: 'homepage',
    component: () => import('@/view/home/home.vue'),
    children: [
      {
        name: 'vwms',
        path: '/vwms',
        component: () => import('@/view/vwms/VWms.vue'),
        meta: {
          menuPath: 'vwms'
        }
      },
      // {
      //   name: 'largeScreen',
      //   path: '/largeScreen',
      //   component: () => import('@/view/largeScreen/largeScreen.vue'),
      //   meta: {
      //     menuPath: 'largeScreen'
      //   }
      // }
      
    ]
  },
]

// create router
const router = createRouter({
  // mode is hash
  history: createWebHashHistory(),
  routes
  // Return to top after route jump
  // scrollBehavior() {
  //   return { top: 0 }
  // }
})

// vite2 dynamic import
const modules = import.meta.glob('../view/*/*/*.vue')

// load router function
function loadRouter() {
  dynamicRouter = menusToRouter(store.getters['user/menulist'])
  dynamicRouter.push(
    {
      name: 'homepage',
      path: '/homepage',
      directory: 'home/homepage',
      redirect: '',
      component: null
    }
  )
  // dynamicRouter.push(
  //   {
  //     name: 'test',
  //     path: '/test',
  //     directory: 'test/test',
  //     redirect: '',
  //     component: null
  //   }
  // )
  // Clear the loaded routes before creating them, mainly for logged out users
  for (const item of loadedRouter) {
    router.removeRoute(item)
  }
  loadedRouter = []
  // This works normally, but it doesn't seem right when getting router
  for (const item of dynamicRouter) {
    item.component = modules[`../view/${ item.directory }${ item.path }.vue`]
    item.meta = {
      menuModule: item.module,
      menuPath: item.name
    }
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
  if (to.path === '/vwms') {
    next()
  }
  // dont have token, back login
  if (!store.getters['user/token']) {
    return next('/login')
  }

  if (dynamicRouter.length === 0) {
    loadRouter()
    return next(to.path)
  }

  next()
})

// Route interceptor, after jump
router.afterEach(() => {
  // disable back or forward
  window.history.pushState(null, '', window.location.href)
})

export { router }
