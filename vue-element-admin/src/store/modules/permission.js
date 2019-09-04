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
    }
    if (user.state.menus.indexOf(tmp.name) > -1 ||
      (tmp.children || []).length > 0 ||
      tmp.redirect == "/404" ||
      tmp.redirect == "/401") {
      res.push(tmp)
    }
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
    var routes = constantRoutes.concat(routes)
    state.routes = routes;
  }
}

const actions = {
  generateRoutes({
    commit
  }) {
    return new Promise(resolve => {
      let accessedRoutes
      if (user.state.roles.indexOf("ADMIN" > -1)) {
        accessedRoutes = asyncRoutes;
      } else {
        accessedRoutes = filterAsyncRoutes(asyncRoutes)
      }
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
