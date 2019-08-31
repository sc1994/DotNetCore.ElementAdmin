<template>
  <div class="app-container">
    <el-button type="primary" @click="handleAddRole">New Role</el-button>

    <el-table :data="rolesList" style="width: 100%;margin-top:30px;" border>
      <el-table-column align="center" label="Role Id" width="220">
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
          <el-button
            type="primary"
            size="small"
            @click="handleEdit(scope)"
            :disabled="scope.row.key == 1"
          >Edit</el-button>
          <el-button
            type="danger"
            size="small"
            @click="handleDelete(scope)"
            :disabled="scope.row.key == 1"
          >Delete</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination
      background
      layout="total, prev, pager, next"
      :total="totalCount"
      :current-page.sync="currentPage"
    ></el-pagination>

    <el-dialog :visible.sync="dialogVisible" :title="dialogType==='edit'?'Edit Role':'New Role'">
      <el-form :model="role" label-width="100px" label-position="rigth">
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
        <el-form-item label="Permissions" v-if="role.permissions && role.permissions.length > 0">
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
            :data="routesData"
            :props="{ children: 'children', label: this.treeSelectLabel }"
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
  routes: [],
  grantedPermissionNames: []
};

export default {
  data() {
    return {
      role: Object.assign({}, defaultRole),
      routes: deepClone([...constantRoutes, ...asyncRoutes]),
      rolesList: [],
      dialogVisible: false,
      dialogType: "new",
      totalCount: 0,
      isPermissionsIndeterminate: false,
      isPermissionsAll: false,
      currentPage: 1
    };
  },
  computed: {
    routesData() {
      let map = x => {
        return {
          meta: x.meta,
          children: x.children,
          key: x.name
        };
      };
      let routes = this.routes
        .map(x => {
          if (x.hidden) return null;
          if (!x.meta && x.children && x.children.length > 0)
            return map(x.children[0]);
          return map(x);
        })
        .filter(x => x);
      return this.routeDataMap(routes);
    }
  },
  created() {
    this.getRoles();
  },
  methods: {
    // 映射到视图
    mapToViewRole(source) {
      return {
        id: source.id,
        key: source.id,
        name: source.normalizedName || source.name.toUpperCase(),
        description: source.description || "NULL PLACEHOLDERS !",
        routes: (source.menus || []).map(x => x.key),
        grantedPermissionNames: source.grantedPermissionNames || [],
        permissions: source.permissions || []
      };
    },
    // 映射到数据
    mapToDataRole(source) {
      return {
        name: source.name,
        displayName: source.name,
        normalizedName: source.name,
        description: source.description,
        grantedPermissions: source.grantedPermissionNames,
        id: source.id,
        grantedMenus: this.$refs.tree.getCheckedKeys()
      };
    },
    async getRoles(page = 1) {
      const { result } = await getRoles(page);
      this.totalCount = result.totalCount;
      this.rolesList = result.items.map(x => {
        return this.mapToViewRole(x);
      });
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
      let { result } = await getRoleForEdit(scope.row.id);
      this.role = this.mapToViewRole({
        ...result.role,
        permissions: result.permissions
      });

      this.dialogType = "edit";
      this.dialogVisible = true;

      this.$nextTick(() => {
        var keys = this.getRoutesByKeys(
          this.role.routes,
          deepClone(this.routesData)
        ); // 获取选中数节点
        this.$refs.tree.setCheckedNodes(keys); // 设置树结构的选中状态
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
          await this.getRoles(this.currentPage);
          this.$message({
            type: "success",
            message: "Delete succed!"
          });
        })
        .catch(err => {
          console.error(err);
        });
    },
    async confirmRole() {
      const isEdit = this.dialogType === "edit";

      if (isEdit) {
        await updateRole(this.mapToDataRole(this.role));
      } else {
        await addRole(this.mapToDataRole(this.role));
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
      await this.getRoles(this.currentPage);
    },
    // 映射路由数据(简化数据结构)
    routeDataMap(routes) {
      let result = [];
      for (let route of routes) {
        let t = {
          meta: route.meta,
          key: route.key || route.name || route.meta.title
        };
        if (route.children) {
          t.children = this.routeDataMap(route.children);
        }
        result.push(t);
      }
      return result;
    },
    // 根据key获取路由数据
    getRoutesByKeys(keys, routes) {
      if (!keys) return [];
      let result = [];
      for (let route of routes) {
        if (keys.indexOf(route.key) > -1) {
          result.push(route);
        }
        if (route.children) {
          var t = this.getRoutesByKeys(keys, route.children);
          result.push(...t);
        }
      }
      return result;
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
      this.isPermissionsAll =
        this.role.permissions && checkedCount === this.role.permissions.length;
      this.isPermissionsIndeterminate =
        this.role.permissions &&
        checkedCount > 0 &&
        checkedCount < this.role.permissions.length;
    },
    currentPage(val) {
      this.getRoles(val);
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
