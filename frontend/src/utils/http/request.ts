import axios from 'axios' // 引入axios
import { store } from '@/store'
import { emitter } from '@/utils/bus.js'

// Basis of axios
const SERVER_URL = `${import.meta.env.VITE_BASE_PATH}:${import.meta.env.VITE_SERVER_PORT}`
axios.defaults.baseURL = SERVER_URL
const http = axios.create({
  baseURL: SERVER_URL,
  timeout: 20000
})

// The interface array request failed
let subscribesArr: Array<any> = []

// The count of current request
let acitveAxios = 0

function pushSubscribeInterface(cb: Function) {
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
  let expiredTime = store.getters['user/expirationTime']
  if (expiredTime) {
    // 距离到期还有x秒则判定为到期
    const willExpiredSecond = 10 * 60
    let nowTime = new Date().getTime()
    let willExpired = (expiredTime - nowTime) / 1000 < willExpiredSecond

    return willExpired
  }
  return false
}

function rediretToLogin() {
  // TODO 重定向回登录页
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

const handleRefreshToken = (token: string) => {
  let refreshToken = store.getters['user/refreshToken']
  store.commit('user/setIsRefreshingToken', true)
  axios
    .post('/refresh-token', {
      accessToken: token,
      refreshToken: refreshToken
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
    const donNeedTokenApi = ['/login']
    const token = store.getters['user/token']
    // const user = store.getters['user/userInfo']

    if (!config.hideLoading) {
      showLoading()
    }

    // 1.token不存在，退出登录
    if (!token) {
      rediretToLogin()
      return config
    }

    // 2.token存在且未过期，继续正常的请求
    if (!isTokenExpired() || config.url === '/refresh-token') {
      config.headers = {
        'Content-Type': 'application/json',
        ...config.headers
      }
      if (config.url && !donNeedTokenApi.includes(config.url)) {
        config.headers.Authorization = `Bearer ${token}`
      }

      return config
    }

    // 3.当前未在刷新token，则需要发起刷新token请求
    if (!store.getters['user/isRefreshingToken']) {
      handleRefreshToken(token)
    }

    // 4.将请求失败的接口挂起，等刷新token后补偿调用
    let retry = new Promise((resolve) => {
      pushSubscribeInterface((newToken: string) => {
        config.headers.Authorization = `Bearer ${newToken}`
        resolve(config)
      })
    })
    return retry
  },
  (error) => {
    closeLoading()
    // ElMessage({
    //     showClose: true,
    //     message: error,
    //     type: 'error'
    // })
    return error
  }
)

http.interceptors.response.use(
  (response) => {
    closeLoading()

    // TODO 根据实际业务修改
    if (response.data.code === 0 || response.headers.success === 'true') {
      if (response.headers.msg) {
        response.data.msg = decodeURI(response.headers.msg)
      }
      return response.data
    }
    // ElMessage({
    //     showClose: true,
    //     message: response.data.msg || decodeURI(response.headers.msg),
    //     type: 'error'
    // })
    if (response.data.data && response.data.data.reload) {
      // store.commit('user/LoginOut')
    }
    return response.data.msg ? response.data : response
  },
  (error) => {
    closeLoading()

    // 1.There isn't 'error.response' object when request timeout
    if (!error.response) {
      console.log('请求超时', error.message)
      return
    }

    // 2.There is response status when request fail but not timeout
    switch (error.response.status) {
      case 500:
        console.log('错误：', 500)
        break
      case 404:
        console.log('错误：', 404)
        break
    }

    return error
  }
)

export default http
