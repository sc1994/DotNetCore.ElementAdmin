import request from '@/utils/request'
import {
  pageToSkipCount
} from "@/utils";

export function login(data) {
  return request({
    url: '/TokenAuth/Authenticate',
    method: 'post',
    data
  })
}

export function getInfo(token) {
  return request({
    url: '/services/app/Session/GetCurrentLoginInformations',
    method: 'get'
  })
}

export function logout() {
  return request({
    url: '/user/logout',
    method: 'post'
  })
}

export function getUsers(key, page) {
  let skip = pageToSkipCount(page, 10);
  return request({
    url: `/services/app/User/GetAll?Keyword=${key}&SkipCount=${skip}&MaxResultCount=10`,
    method: 'get'
  })
}

export function addUser(data) {
  return request({
    url: "/services/app/User/Create",
    method: 'post',
    data
  })
}
