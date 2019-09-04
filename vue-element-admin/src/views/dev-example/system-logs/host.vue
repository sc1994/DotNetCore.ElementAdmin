<template>
  <div class="app-container">
    <el-card class="box-card" shadow="hover" style>
      <el-row :gutter="5">
        <el-col :span="1">Content&nbsp;</el-col>
        <el-col :span="11">
          <el-cascader
            :options="classifies"
            clearable
            style="width:100%"
            filterable
            :debounce="500"
            :filter-method="cascaderFilterMethod"
            v-model="filtrate.classifies"
            placeholder="Log Content"
            :props="{ checkStrictly: true }"
          ></el-cascader>
        </el-col>
        <!-- <el-col :span="2">级别：</el-col>
        <el-col :span="3">
          <el-select v-model="filtrate.lv" clearable>
            <el-option label="Information" value="Information"></el-option>
            <el-option label="Warning" value="Warning"></el-option>
            <el-option label="Error" value="Error"></el-option>
            <el-option label="Debug" value="Debug"></el-option>
            <el-option label="Fatal" value="Fatal"></el-option>
          </el-select>
        </el-col>
        <el-col :span="1">时间：</el-col>
        <el-col :span="6">
          <el-date-picker
            v-if="switchTime"
            v-model="filtrate.times"
            type="datetimerange"
            range-separator="至"
            start-placeholder="开始日期"
            end-placeholder="结束日期"
            align="right"
            value-format="yyyy-MM-dd HH:mm:ss"
          ></el-date-picker>
          <el-select v-else v-model="filtrate.timeSelect">
            <el-option label="最近10分钟" value="10"></el-option>
            <el-option label="最近30分钟" value="30"></el-option>
            <el-option label="最近1小时" value="60"></el-option>
            <el-option label="最近8小时" value="480"></el-option>
          </el-select>
          <el-button icon="el-icon-full-screen" circle @click="switchTime = !switchTime"></el-button>
        </el-col>
      </el-row>
      <br />
      <el-row :gutter="5">
        <el-col :span="1">过滤：</el-col>
        <el-col :span="3">
          <el-input v-model="filtrate.filter1" placeholder="过滤1"></el-input>
        </el-col>
        <el-col :span="4">
          <el-input v-model="filtrate.filter2" placeholder="过滤2"></el-input>
        </el-col>
        <el-col :span="1">Ip：</el-col>
        <el-col :span="3">
          <el-select v-model="filtrate.ip" clearable style="width:100%">
            <el-option v-for="item in ips" :key="item" :label="item" :value="item"></el-option>
          </el-select>
        </el-col> -->
        <el-col :span="2">KeyWord&nbsp;</el-col>
        <el-col :span="10">
          <el-input
            v-model="filtrate.msg"
            placeholder="此项将会扰乱时间顺序, 以输入内容的匹配度为排序依据! 请严格限制时间范围, 否则搜索结果将偏差较多"
          ></el-input>
        </el-col>
      </el-row>
      <br />
      <el-row :gutter="5">
        <el-button
          type="primary"
          icon="el-icon-filtrate"
          @click="search(1)"
          :loading="loading && !autoRefresh"
        >搜索</el-button>
        <el-button
          type="primary"
          icon="el-icon-refresh rotate"
          @click="autoRefresh = false"
          v-if="autoRefresh"
          :loading="loading && autoRefresh"
        ></el-button>
        <el-button type="primary" icon="el-icon-refresh" @click="autoRefresh = true" v-else></el-button>
      </el-row>
    </el-card>
    <el-divider content-position="left">搜索结果</el-divider>
    <el-table
      :data="tableData"
      border
      style="width: 100%"
      @row-dblclick="showDetail"
      v-loading="loading"
      :row-class-name="tableRowClassName"
    >
      >
      <el-table-column prop="timestamp" label="时间" width="195"></el-table-column>
      <el-table-column prop="level" label="等级" width="120">
        <template slot-scope="scope">
          <span>{{scope.row.level}}</span>
        </template>
      </el-table-column>
      <el-table-column prop="module" label="项目/模块/大类/小类" width="600">
        <template slot-scope="scope">
          <span
            class="span-long-text"
            :title="scope.row.app + '/' + scope.row.module + '/' + scope.row.category + '/' +
                        scope.row.sub_category"
          >
            <span>{{scope.row.app}}</span>/
            <span>{{scope.row.module}}</span>/
            <span>{{scope.row.category}}</span>/
            <span>{{scope.row.sub_category}}</span>
          </span>
        </template>
      </el-table-column>

      <el-table-column prop="msg" label="Msg">
        <template slot-scope="scope">
          <span
            class="span-long-text"
            v-if="scope.row['fields.msg'] && scope.row['fields.msg'][0]"
            v-html="scope.row['fields.msg'][0]"
          ></span>
          <span class="span-long-text" v-else>{{scope.row.msg}}</span>
        </template>
      </el-table-column>
      <el-table-column prop="ip" label="Ip" width="150"></el-table-column>
    </el-table>
    <div style="padding: 20px;text-align: right">
      <el-pagination
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
        :current-page.sync="currentPage"
        :page-sizes="[5, 10, 50, 100]"
        :page-size="pageSize"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total"
      ></el-pagination>
    </div>
    <br />
    <br />
    <el-dialog
      :title="current.module+' / '+current.category+' / '+current.sub_category"
      :visible.sync="dialogVisible"
      width="60%"
      height="95%"
    >
      <el-table :data="[{...current}]" style="width: 100%">
        <el-table-column prop="timestamp" label="时间戳"></el-table-column>
        <el-table-column prop="app" label="项目"></el-table-column>
        <el-table-column prop="level" label="等级"></el-table-column>
        <el-table-column prop="filter1" label="过滤1" v-if="current.filter1"></el-table-column>
        <el-table-column prop="filter2" label="过滤2" v-if="current.filter2"></el-table-column>
      </el-table>
      <div v-if="current['fields.msg']">
        <el-divider content-position="left">含有关键字片段</el-divider>
        <p v-for="item in current['fields.msg']" :key="item" v-html="item"></p>
      </div>
      <br />
      <el-input
        type="textarea"
        :autosize="{ minRows: 1, maxRows: 25 }"
        :value="toJsonFormat(current.msg)"
        spellcheck="true"
      ></el-input>
    </el-dialog>
  </div>
</template>
<script>
export default {
  data() {
    return {
      filtrate: {
        times: [],
        timeSelect: "10"
      },
      switchTime: false,
      classifies: [],
      ips: [],
      tableData: [],
      total: 0,
      dialogVisible: false,
      current: {},
      currentPage: 1,
      pageSize: 5,
      autoRefresh: false,
      timer: {},
      loading: false
    };
  },
  methods: {
    async search(pageIndex) {
      this.currentPage = pageIndex;
      this.loading = true;
      var res = await axios.post("api/search", {
        ...this.filtrate,
        pageIndex,
        pageSize: this.pageSize
      });
      var content = res.data[0];
      this.tableData = content.hits.hits.map(x => {
        return {
          timestamp: new Date(x._source["@timestamp"]).format(
            "yyyy-MM-dd hh:mm:ss.S"
          ),
          level: x._source.level,
          ...x._source.fields,
          ...x.highlight
        };
      });
      this.total = content.hits.total;
      this.loading = false;
      console.log(res.data);
    },
    showDetail(row, column, event) {
      this.dialogVisible = true;
      this.current = row;
      console.log(row, column, event);
    },
    tableRowClassName({ row }) {
      if (row.level === "Warning") {
        return "warning-row";
      } else if (row.level === "Error" || row.level == "Fatal") {
        return "error-row";
      }
      return "";
    },
    async handleCurrentChange(val) {
      await this.search(val);
    },
    async handleSizeChange(val) {
      this.pageSize = val;
      await this.search(1);
    },
    toJsonFormat(str) {
      try {
        return JSON.stringify(JSON.parse(str), null, 2);
      } catch (e) {
        return str;
      }
    },
    cascaderFilterMethod(node, keyword) {
      var arr = node.text.split(" / ");
      var where = arr.filter(x => !!x).map(x => x.toLowerCase());
      var result =
        where.filter(x => x.indexOf(keyword.toLowerCase()) > -1).length > 0;
      return result;
    }
  },
  watch: {
    switchTime(val) {
      if (val) {
        this.filtrate.timeSelect = "0";
      } else {
        this.filtrate.timeSelect = "10";
        this.filtrate.times = [];
      }
    },
    autoRefresh(val) {
      if (val) {
        this.timer = setInterval(() => {
          this.search(1);
        }, 1500);
      } else {
        window.clearInterval(this.timer);
      }
    },
    dialogVisible() {
      $("textarea").scrollTop(0);
    }
  },
  async mounted() {
    // var res = await axios.get("api/search/aggregation");
    // this.classifies = res.data.item1;
    // this.ips = res.data.item2;
    // await this.search(1); // todo 测试
  }
};
</script>

<style lang="css">
.clearfix:before,
.clearfix:after {
  display: table;
  content: "";
}

.clearfix:after {
  clear: both;
}

.box-card {
  width: 100%;
}

.el-col-2 {
  text-align: right;
  padding-top: 9px;
}

.el-col-1 {
  text-align: right;
  padding-top: 9px;
}

.rotate {
  animation: rotating 2s linear infinite;
}

.el-table .warning-row {
  background: rgb(250, 236, 216);
}

.el-table .error-row {
  background: rgb(253, 226, 226);
}

.span-long-text {
  line-height: 30px;
  text-align: center;
  text-overflow: ellipsis;
  white-space: nowrap;
  overflow: hidden;
}

textarea {
  background-color: #000033 !important;
  color: #eeeeee !important;
}

.el-dialog {
  margin-top: 6vh !important;
}

.el-dialog__body {
  padding-top: 5px;
}

em {
  color: red;
}

.el-dialog__body .el-row {
  padding-top: 3px;
  padding-bottom: 3px;
}
</style>