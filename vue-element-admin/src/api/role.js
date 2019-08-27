import request from '@/utils/request'

export function getRoutes() {
  return request({
    url: '/routes',
    method: 'get'
  })
}

export function getRoles() {
  return request({
    url: '/services/app/Role/GetAll',
    method: 'get'
  })
}

export function addRole(data) {
  return request({
    url: '/role',
    method: 'post',
    data
  })
}

export function updateRole(id, data) {
  return request({
    url: `/role/${id}`,
    method: 'put',
    data
  })
}

export function deleteRole(id) {
  return request({
    url: `/role/${id}`,
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
