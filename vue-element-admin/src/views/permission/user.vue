<template>
  <div class="app-container">
    <el-form :inline="true" :model="formSearch" class="demo-form-inline">
      <el-form-item label="User Name">
        <el-input
          v-model="formSearch.key"
          placeholder="Search User Name KeyWord"
          style="width:270px;"
        ></el-input>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="getUsers(1)">Search</el-button>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="handleAddUser">New User</el-button>
      </el-form-item>
    </el-form>
    <el-table :data="usersDataTable" style="width: 100%;margin-top:30px;" border>
      <el-table-column align="center" label="User Id" width="150">
        <template slot-scope="scope">{{ scope.row.id }}</template>
      </el-table-column>
      <el-table-column align="center" label="User Name" width="220">
        <template slot-scope="scope">{{ scope.row.name }}</template>
      </el-table-column>
      <el-table-column align="center" label="Email Address">
        <template slot-scope="scope">{{ scope.row.emailAddress }}</template>
      </el-table-column>
      <el-table-column align="center" label="Roles">
        <template slot-scope="scope">
          <el-tag v-for="name in scope.row.roleNames" style="margin:2px;">{{ name }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column align="center" label="Creation Time" width="220">
        <template slot-scope="scope">{{ new Date(scope.row.creationTime).toLocaleString() }}</template>
      </el-table-column>
      <el-table-column align="center" label="Status" width="180">
        <template slot-scope="scope">
          <el-tag type="success" v-if="scope.row.isActive">Active</el-tag>
          <el-tag type="info" v-else>Slumber</el-tag>
        </template>
      </el-table-column>
      <el-table-column align="center" label="Operations">
        <template slot-scope="scope">
          <el-button
            type="primary"
            size="small"
            @click="handleEdit(scope)"
            :disabled="scope.row.id == 1"
          >Edit</el-button>
          <el-button
            type="danger"
            size="small"
            @click="handleDelete(scope)"
            :disabled="scope.row.id == 1"
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

    <el-dialog :visible.sync="dialogVisible" :title="dialogType==='edit'?'Edit User':'New User'">
      <el-form :model="user" label-width="120px" label-position="rigth">
        <el-form-item label="User Name">
          <el-input
            v-model="user.userName"
            placeholder="User Name"
            :disabled="dialogType==='edit'"
          />
        </el-form-item>
        <el-form-item label="Email Address">
          <el-input
            v-model="user.emailAddress"
            placeholder="Email Address"
            :disabled="dialogType==='edit'"
          />
        </el-form-item>
        <el-form-item label="Password">
          <el-input :value="user.password" disabled v-if="dialogType==='new'" />
          <el-button type="primary" plain v-else @click="resetPassword">Reset Password</el-button>
        </el-form-item>
        <el-form-item label="Roles">
          <el-transfer v-model="user.roleNames" :data="allRoles" :titles="['Roles', 'Selected']"></el-transfer>
        </el-form-item>
        <el-form-item label="Active" v-if="dialogType == 'edit'">
          <el-switch v-model="user.isActive" active-color="#13ce66" inactive-color="#ff4949"></el-switch>
        </el-form-item>
      </el-form>
      <div style="text-align:right;">
        <el-button type="danger" @click="dialogVisible=false">Cancel</el-button>
        <el-button type="primary" @click="confirmUser">Confirm</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import {
  getUsers,
  addUser,
  updateUser,
  resetPassword,
  deleteUser
} from "@/api/user";
import { deepClone, randomCipher } from "@/utils";
import { getRoles } from "@/api/role";
import { async } from "q";
const defaultUser = {
  userName: "",
  emailAddress: "",
  password: "",
  roleNames: []
};

export default {
  data() {
    return {
      formSearch: {},
      totalCount: 0,
      currentPage: 1,
      usersDataTable: [],
      dialogVisible: false,
      user: deepClone(defaultUser),
      dialogType: "",
      allRoles: [],
      adminPassword: ""
    };
  },
  methods: {
    resetPassword() {
      this.$msgbox({
        title: "Reset Password",
        showInput: true,
        inputPlaceholder: "Verify Admin Password",
        inputType: "password",
        showCancelButton: true,
        confirmButtonText: "Ok",
        cancelButtonText: "Off",
        beforeClose: async (action, instance, done) => {
          done();
        }
      })
        .then(async (input, instance) => {
          let newPassword = randomCipher();
          await resetPassword({
            adminPassword: input.value,
            userId: this.user.id,
            newPassword: newPassword
          });
          this.$notify({
            title: "Success",
            message: "New Passwor is " + newPassword,
            type: "success"
          });
        })
        .catch(error => {
          console.log(error);
        });
    },
    async confirmUser() {
      this.user.name = this.user.userName;
      this.user.surname = this.user.userName;
      if (this.dialogType == "new") {
        this.user.isActive = true;
        await addUser(this.user);
      } else {
        await updateUser(this.user);
      }
      this.dialogVisible = false;
      this.$notify({
        title: "Confirm Success",
        dangerouslyUseHTMLString: true,
        message: `
            <div>User Name: ${this.user.userName}</div>
            <div>Email Address: ${this.user.emailAddress}</div>
          `,
        type: "success"
      });
      await this.getUsers(this.currentPage);
    },
    handleAddUser() {
      this.user = deepClone(defaultUser);
      this.user.password = randomCipher();
      this.dialogType = "new";
      this.dialogVisible = true;
    },
    handleEdit(scope) {
      this.user = deepClone(scope.row);
      this.dialogType = "edit";
      this.dialogVisible = true;
    },
    async handleDelete(scope) {
      await deleteUser(scope.row.id);
      this.$notify({
        title: "Delete Success",
        dangerouslyUseHTMLString: true,
        message: `
            <div>User Name: ${scope.row.userName}</div>
            <div>Email Address: ${scope.row.emailAddress}</div>
          `,
        type: "success"
      });
      await this.getUsers(this.currentPage);
    },
    async getUsers(page = 1) {
      let { result } = await getUsers(this.formSearch.key || "", page);
      this.usersDataTable = result.items;
      this.totalCount = result.totalCount;
    },
    async getAllRoles() {
      let { result } = await getRoles(1, 9999);

      this.allRoles = result.items.map(x => {
        return {
          label: x.normalizedName,
          key: x.normalizedName
        };
      });
    }
  },
  async created() {
    await this.getUsers();
    await this.getAllRoles();
  }
};
</script>
    
<style>
</style>