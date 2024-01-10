import axios from 'axios' // 引入axios
import { store } from '@/store'
import { emitter } from '@/utils/bus'
import { router } from '@/router'
import { hookComponent } from '@/components/system'
import i18n from '@/languages/i18n'

// Basis of axios
const SERVER_URL = `${ import.meta.env.VITE_BASE_PATH }:${ import.meta.env.VITE_SERVER_PORT }`
axios.defaults.baseURL = SERVER_URL
const http = axios.create({
  baseURL: SERVER_URL,
  timeout: 10000
})

// The interface array request failed
let subscribesArr: Array<any> = []

// The count of current request
let acitveAxios = 0

function pushSubscribeInterface(cb: any) {
  subscribesArr.push(cb)
}

function reloadSubscribesWithNewToken(token: string) {
  subscribesArr.map((cb) => cb(token))
}

function resetSubscribes() {
  subscribesArr = []
}

/**
 * expired or will expired
 * @returns {boolean}
 */
function isTokenExpired() {
  const expiredTime = store.getters['user/expirationTime']
  if (expiredTime) {
    // Distance and x seconds is judged to be due
    const willExpiredSecond = 10 * 60
    const nowTime = new Date().getTime()
    const willExpired = (expiredTime - nowTime) / 1000 < willExpiredSecond

    return willExpired
  }
  return false
}

function rediretToLogin() {
  store.commit('system/clearOpenedMenu')
  store.commit('system/setCurrentRouterPath', '')

  clearLoading() // Clear all loads

  router.push('/login')
}

const showLoading = () => {
  acitveAxios++
  if (acitveAxios > 0) {
    emitter.emit('showLoading')
  }
}

const closeLoading = () => {
  acitveAxios--
  if (acitveAxios <= 0) {
    emitter.emit('closeLoading')
  }
}

const clearLoading = () => {
  acitveAxios = 0
  emitter.emit('closeLoading')
}

const handleRefreshToken = (token: string) => {
  const refreshToken = store.getters['user/refreshToken']
  store.commit('user/setIsRefreshingToken', true)
  axios
    .post('/refresh-token', {
      accessToken: token,
      refreshToken
    })
    .then(({ data: res }) => {
      if (res.isSuccess) {
        const tokenVo = res.data
        const expiredTime = new Date().getTime() + store.getters['user/effectiveMinutes'] * 60 * 1000

        store.commit('user/setToken', tokenVo)
        store.commit('user/setExpirationTime', expiredTime)

        // With the new token request those suspended interface
        reloadSubscribesWithNewToken(tokenVo)
      } else {
        return Promise.reject()
      }
    })
    .catch(() => {
      resetSubscribes()
      rediretToLogin()
    })
    .finally(() => {
      store.commit('user/setIsRefreshingToken', false)
    })
}

http.interceptors.request.use(
  (config: any) => {
    const donNeedTokenApi = ['/login', '/user/register']
    const token = store.getters['user/token']

    let culture = 'en-us'
    switch (store.getters['system/language']) {
      case 'zh':
        culture = 'zh-cn'
        break
      case 'en':
        culture = 'en-us'
        break
    }
    config.params ? (config.params.culture = culture) : (config.params = { culture })

    if (!config.hideLoading) {
      showLoading()
    }

    // It don't need token to request with some apis.
    if (donNeedTokenApi.includes(config.url)) {
      return config
    }

    // 1.Logout when token isn't exist
    if (!token) {
      rediretToLogin()
      return config
    }

    // 2.Request normally when token is exist and in valid date
    if (!isTokenExpired() || config.url === '/refresh-token') {
      config.headers = {
        'Content-Type': 'application/json',
        ...config.headers
      }
      if (config.url && !donNeedTokenApi.includes(config.url)) {
        config.headers.Authorization = `Bearer ${ token }`
      }

      return config
    }

    // 3.Take a 'refresh token' request when it not in the refreshing.
    if (!store.getters['user/isRefreshingToken']) {
      handleRefreshToken(token)
    }

    // 4.Put the fail requests up and initiate them after refresh token
    const retry = new Promise((resolve) => {
      pushSubscribeInterface((newToken: string) => {
        config.headers.Authorization = `Bearer ${ newToken }`
        resolve(config)
      })
    })
    return retry
  },
  (error) => {
    closeLoading()
    return error
  }
)

http.interceptors.response.use(
  (response) => {
    closeLoading()
    if (response.data.code === 0 || response.headers.success === 'true') {
      if (response.headers.msg) {
        response.data.msg = decodeURI(response.headers.msg)
      }
      return response.data
    }
    return response.data.msg ? response.data : response
  },
  (error) => {
    closeLoading()
    // 1.There isn't 'error.response' object when request timeout
    if (!error.response) {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('system.tips.requestTimeout')
      })
      return
    }

    // 2.There is response status when request fail but not timeout
    switch (error.response.status) {
      case 500:
        console.error('error：', 500)
        break
      case 404:
        console.error('error：', 404)
        break
    }

    hookComponent.$message({
      type: 'error',
      content: i18n.global.t('system.tips.requestFail')
    })
    return error
  }
)

export default http
