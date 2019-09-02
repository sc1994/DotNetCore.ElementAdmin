import {
  asyncRoutes,
  constantRoutes
} from '@/router'

import user from "./user"

/**
 * Filter asynchronous routing tables by recursion
 * @param routes asyncRoutes
 * @param roles
 */
export function filterAsyncRoutes(routes) {
  const res = []

  routes.forEach(route => {
    const tmp = {
      ...route
    }
    if (tmp.children) {
      tmp.children = filterAsyncRoutes(tmp.children)
    } else {
      tmp.children = [];
    }
    if (!tmp.meta && tmp.children && tmp.children.length > 0) {
      // let t = {
      //   ...tmp.children[0],
      //   ...tmp
      // };
      // t.children = [];
      // res.push(t)
      console.log(tmp);
      res.push(tmp)
    } else if (user.state.menus.indexOf(tmp.name) > -1)
      res.push(tmp)
  })
  return res
}

const state = {
  routes: [],
  addRoutes: []
}

const mutations = {
  SET_ROUTES: (state, routes) => {
    state.addRoutes = routes
    state.routes = constantRoutes.concat(routes)
  }
}

const actions = {
  generateRoutes({
    commit
  }) {
    return new Promise(resolve => {
      let accessedRoutes = filterAsyncRoutes(asyncRoutes)
      commit('SET_ROUTES', accessedRoutes)
      resolve(accessedRoutes)
    })
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}
