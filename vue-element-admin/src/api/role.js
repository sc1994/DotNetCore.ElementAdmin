import request from '@/utils/request'
import {
  pageToSkipCount
} from "@/utils";

export function getRoles(page, size = 10) {
  return request({
    url: `/services/app/Role/GetAll?SkipCount=${pageToSkipCount(page, size)}&MaxResultCount=${size}`,
    method: 'get'
  })
}

export function addRole(data) {
  return request({
    url: '/services/app/Role/Create',
    method: 'post',
    data
  })
}

export function updateRole(data) {
  return request({
    url: "/services/app/Role/Update",
    method: 'put',
    data
  })
}

export function deleteRole(id) {
  return request({
    url: `/services/app/Role/Delete?Id=${id}`,
    method: 'delete'
  })
}

export function getAllPermissions() {
  return request({
    url: "/services/app/Role/GetAllPermissions",
    method: "get"
  })
}

export function getRoleForEdit(id) {
  return request({
    url: `/services/app/Role/GetRoleForEdit?Id=${id}`,
    method: "get"
  })
}
