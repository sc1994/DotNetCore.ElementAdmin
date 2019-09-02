import axios from 'axios'
import {
  // MessageBox,
  Message,
  Notification
} from 'element-ui'
import store from '@/store'
import {
  getToken
} from '@/utils/auth'

// create an axios instance
const service = axios.create({
  baseURL: process.env.VUE_APP_BASE_API, // url = base url + request url
  withCredentials: true, // send cookies when cross-domain requests
  timeout: 5000 // request timeout
})

// request interceptor
service.interceptors.request.use(
  config => {
    // do something before request is sent

    if (store.getters.token) {
      // let each request carry token
      // ['X-Token'] is a custom headers key
      // please modify it according to the actual situation
      config.headers['Authorization'] = 'Bearer ' + getToken()
    }
    return config
  },
  error => {
    // do something with request error
    console.log(error) // for debug
    return Promise.reject(error)
  }
)

// response interceptor
service.interceptors.response.use(
  /**
   * If you want to get http information such as headers or status
   * Please return  response => response
   */

  /**
   * Determine the request status by custom code
   * Here is just an example
   * You can also judge the status by HTTP Status Code
   */
  response => {
    const res = response.data

    // if the custom code is not 20000, it is judged as an error.
    if (!res.success) {
      Message({
        message: res.error.message || 'Error',
        type: 'error',
        duration: 5 * 1000
      })

      return Promise.reject(new Error(res.error.message || 'Error'))
    } else {
      return res
    }
  },
  error => {
    console.log('err' + error) // for debug

    if (error.response.status == 401) { // 重新登录逻辑
      MessageBox.confirm('You have been logged out, you can cancel to stay on this page, or log in again', 'Confirm logout', {
        confirmButtonText: 'Re-Login',
        cancelButtonText: 'Cancel',
        type: 'warning'
      }).then(() => {
        store.dispatch('user/resetToken').then(() => {
          location.reload()
        })
      })
    } else {
      let html = "";
      if (error.response.data.error.details) {
        let arr = error.response.data.error.details.split("\r\n");
        for (let item of arr) {
          if (!item || item == undefined + "") continue;
          html += `<div>${item}</div>`;
        }
      }
      Notification({
        title: error.response.data.error.message,
        dangerouslyUseHTMLString: true,
        message: html,
        type: "error"
      });
    }

    return Promise.reject(error)
  }
)

export default service
