import request from '@/utils/request'

export function postAggregation(data) {
  return request({
    url: "/services/app/Logs/PostAggregation",
    method: 'post',
    data
  })
}

export function postSearch(data) {
  return request({
    url: "/services/app/Logs/PostSearch",
    method: 'post',
    data
  })
}

export function getAllIndexTimes() {
  return request({
    url: "/services/app/Logs/GetAllIndexTimes",
    method: 'get'
  })
}
