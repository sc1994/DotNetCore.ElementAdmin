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
          <el-tag v-for="name in scope.row.roleNames" style="margin-right:5px;">{{ name }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column align="center" label="Creation Time" width="220">
        <template slot-scope="scope">{{ new Date(scope.row.creationTime).toLocaleString() }}</template>
      </el-table-column>
      <el-table-column align="center" label="Status" width="180">
        <template slot-scope="scope">
          <el-tag type="success" v-if="scope.row.isActive">Active</el-tag>
          <el-tag type="success" v-else>Slumber</el-tag>
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
        <el-form-item label="Initial Password" v-if="dialogType==='new'">
          <el-input :value="user.password" disabled />
        </el-form-item>
        <el-form-item label="Roles">
          <el-transfer v-model="user.roleNames" :data="allRoles" :titles="['Roles', 'Selected']"></el-transfer>
        </el-form-item>
        <el-form-item label="Status" v-if="dialogType == 'edit'">
          <!-- <el-input v-model="user.name" placeholder="Email Address" /> -->
          TODO:
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
import { getUsers, addUser } from "@/api/user";
import { deepClone, randomCipher } from "@/utils";
import { getRoles } from "@/api/role";
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
      allRoles: []
    };
  },
  methods: {
    async confirmUser() {
      this.user.name = this.user.userName;
      this.user.surname = this.user.userName;
      if (this.dialogType == "new") {
        this.isActive = true;
        await addUser(this.user);
      }
      this.dialogVisible = false;
      this.$notify({
        title: "Success",
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