<template>
  <div class="app-container">
    <el-button type="primary" @click="handleAddRole">New Role</el-button>

    <el-table :data="rolesList" style="width: 100%;margin-top:30px;" border>
      <el-table-column align="center" label="Role Key" width="220">
        <template slot-scope="scope">{{ scope.row.key }}</template>
      </el-table-column>
      <el-table-column align="center" label="Role Name" width="220">
        <template slot-scope="scope">{{ scope.row.name }}</template>
      </el-table-column>
      <el-table-column align="header-center" label="Description">
        <template slot-scope="scope">{{ scope.row.description }}</template>
      </el-table-column>
      <el-table-column align="center" label="Operations">
        <template slot-scope="scope">
          <el-button type="primary" size="small" @click="handleEdit(scope)">Edit</el-button>
          <el-button type="danger" size="small" @click="handleDelete(scope)">Delete</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination background layout="total, prev, pager, next" :total="totalCount"></el-pagination>

    <el-dialog :visible.sync="dialogVisible" :title="dialogType==='edit'?'Edit Role':'New Role'">
      <el-form :model="role" label-width="100px" label-position="left">
        <el-form-item label="Name">
          <el-input v-model="role.name" placeholder="Role Name" />
        </el-form-item>
        <el-form-item label="Desc">
          <el-input
            v-model="role.description"
            :autosize="{ minRows: 2, maxRows: 4}"
            type="textarea"
            placeholder="Role Description"
          />
        </el-form-item>
        <el-form-item label="Permissions">
          <el-checkbox
            :indeterminate="isPermissionsIndeterminate"
            v-model="isPermissionsAll"
            @change="handlePermissionsCheckAllChange"
          >All</el-checkbox>
          <el-checkbox-group v-model="role.grantedPermissionNames">
            <el-checkbox
              v-for="item in role.permissions"
              :label="item.name"
              :key="item.name"
            >{{item.name}}</el-checkbox>
          </el-checkbox-group>
        </el-form-item>
        <el-form-item label="Menus">
          <el-tree
            ref="tree"
            :check-strictly="checkStrictly"
            :data="routesData"
            :props="defaultProps"
            show-checkbox
            node-key="key"
            class="permission-tree"
          />
        </el-form-item>
      </el-form>
      <div style="text-align:right;">
        <el-button type="danger" @click="dialogVisible=false">Cancel</el-button>
        <el-button type="primary" @click="confirmRole">Confirm</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import path from "path";
import { deepClone } from "@/utils";
import { asyncRoutes, constantRoutes } from "@/router";
import {
  getRoutes,
  getRoles,
  addRole,
  deleteRole,
  updateRole,
  getRoleForEdit
} from "@/api/role";

const defaultRole = {
  key: "",
  name: "",
  description: "",
  routes: []
};

export default {
  data() {
    return {
      role: Object.assign({}, defaultRole),
      routes: deepClone([...constantRoutes, ...asyncRoutes]),
      rolesList: [],
      dialogVisible: false,
      dialogType: "new",
      checkStrictly: false,
      totalCount: 0,
      defaultProps: {
        children: "children",
        label: this.treeSelectLabel
      },
      isPermissionsIndeterminate: false,
      isPermissionsAll: false
    };
  },
  computed: {
    routesData() {
      let map = x => {
        return {
          meta: x.meta,
          children: x.children,
          key: `${x.path}_${x.name}`
        };
      };
      return this.routes
        .map(x => {
          if (x.hidden) return null;
          if (!x.meta && x.children && x.children.length > 0)
            return map(x.children[0]);
          return map(x);
        })
        .filter(x => x);
    }
  },
  created() {
    // Mock: get all routes and roles list from server
    // this.getRoutes() TODO: 暂时注释掉

    this.getRoles();
  },
  methods: {
    mapToRole(source) {
      return {
        id: source.id,
        key: source.name,
        name: source.name,
        description: source.description || "NULL PLACEHOLDERS !",
        routes: [], // TODO: 路由
        grantedPermissionNames: source.grantedPermissionNames || [],
        permissions: source.permissions || []
      };
    },
    async getRoutes() {
      const res = await getRoutes();
      this.serviceRoutes = res.data;
      this.routes = this.generateRoutes(res.data);
    },
    async getRoles() {
      const { result } = await getRoles();
      this.totalCount = result.totalCount;
      this.rolesList = result.items.map(x => {
        return this.mapToRole(x);
      });
    },
    // Reshape the routes structure so that it looks the same as the sidebar
    generateRoutes(routes, basePath = "/") {
      const res = [];

      for (let route of routes) {
        // skip some route
        if (route.hidden) {
          continue;
        }

        const onlyOneShowingChild = this.onlyOneShowingChild(
          route.children,
          route
        );

        if (route.children && onlyOneShowingChild && !route.alwaysShow) {
          route = onlyOneShowingChild;
        }

        const data = {
          path: path.resolve(basePath, route.path),
          title: route.meta && route.meta.title
        };

        // recursive child routes
        if (route.children) {
          data.children = this.generateRoutes(route.children, data.path);
        }
        res.push(data);
      }
      return res;
    },
    generateArr(routes) {
      let data = [];
      routes.forEach(route => {
        data.push(route);
        if (route.children) {
          const temp = this.generateArr(route.children);
          if (temp.length > 0) {
            data = [...data, ...temp];
          }
        }
      });
      return data;
    },
    handleAddRole() {
      this.role = Object.assign({}, defaultRole);
      if (this.$refs.tree) {
        this.$refs.tree.setCheckedNodes([]);
      }
      this.dialogType = "new";
      this.dialogVisible = true;
    },
    async handleEdit(scope) {
      this.dialogType = "edit";
      this.dialogVisible = true;
      this.checkStrictly = true;
      let { result } = await getRoleForEdit(scope.row.id);
      this.role = this.mapToRole({
        ...result.role,
        permissions: result.permissions,
        grantedPermissionNames: result.grantedPermissionNames
      });
      this.$nextTick(() => {
        // const routes = this.generateRoutes(this.role.routes); TODO:
        // this.$refs.tree.setCheckedNodes(this.generateArr(routes));TODO:
        // set checked state of a node not affects its father and child nodes
        this.checkStrictly = false;
      });
    },
    handleDelete({ $index, row }) {
      this.$confirm("Confirm to remove the role?", "Warning", {
        confirmButtonText: "Confirm",
        cancelButtonText: "Cancel",
        type: "warning"
      })
        .then(async () => {
          await deleteRole(row.key);
          this.rolesList.splice($index, 1);
          this.$message({
            type: "success",
            message: "Delete succed!"
          });
        })
        .catch(err => {
          console.error(err);
        });
    },
    generateTree(routes, basePath = "/", checkedKeys) {
      const res = [];

      for (const route of routes) {
        const routePath = path.resolve(basePath, route.path);

        // recursive child routes
        if (route.children) {
          route.children = this.generateTree(
            route.children,
            routePath,
            checkedKeys
          );
        }

        if (
          checkedKeys.includes(routePath) ||
          (route.children && route.children.length >= 1)
        ) {
          res.push(route);
        }
      }
      return res;
    },
    async confirmRole() {
      const isEdit = this.dialogType === "edit";

      const checkedKeys = this.$refs.tree.getCheckedKeys();
      this.role.routes = this.generateTree(
        deepClone(this.serviceRoutes),
        "/",
        checkedKeys
      );

      if (isEdit) {
        await updateRole(this.role.key, this.role);
        for (let index = 0; index < this.rolesList.length; index++) {
          if (this.rolesList[index].key === this.role.key) {
            this.rolesList.splice(index, 1, Object.assign({}, this.role));
            break;
          }
        }
      } else {
        const { data } = await addRole(this.role);
        this.role.key = data.key;
        this.rolesList.push(this.role);
      }

      const { description, key, name } = this.role;
      this.dialogVisible = false;
      this.$notify({
        title: "Success",
        dangerouslyUseHTMLString: true,
        message: `
            <div>Role Key: ${key}</div>
            <div>Role Nmae: ${name}</div>
            <div>Description: ${description}</div>
          `,
        type: "success"
      });
      handleCheckAllChange();
    },
    // reference: src/view/layout/components/Sidebar/SidebarItem.vue
    onlyOneShowingChild(children = [], parent) {
      let onlyOneChild = null;
      const showingChildren = children.filter(item => !item.hidden);

      // When there is only one child route, the child route is displayed by default
      if (showingChildren.length === 1) {
        onlyOneChild = showingChildren[0];
        onlyOneChild.path = path.resolve(parent.path, onlyOneChild.path);
        return onlyOneChild;
      }

      // Show parent if there are no child route to display
      if (showingChildren.length === 0) {
        onlyOneChild = { ...parent, path: "", noShowingChildren: true };
        return onlyOneChild;
      }

      return false;
    },
    // 树控件选择外显标题
    treeSelectLabel(data, node) {
      return data.meta.title;
    },
    handlePermissionsCheckAllChange(val) {
      this.role.grantedPermissionNames = val
        ? this.role.permissions.map(x => x.name)
        : [];
      this.isPermissionsIndeterminate = false;
    }
  },
  watch: {
    "role.grantedPermissionNames"(value) {
      let checkedCount = value.length;
      this.isPermissionsAll = checkedCount === this.role.permissions.length;
      this.isPermissionsIndeterminate =
        checkedCount > 0 && checkedCount < this.role.permissions.length;
    }
  }
};
</script>

<style lang="scss" scoped>
.app-container {
  .roles-table {
    margin-top: 30px;
  }
  .permission-tree {
    margin-bottom: 30px;
  }
}
</style>
