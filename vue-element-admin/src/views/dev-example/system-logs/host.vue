<template>
  <div class="app-container">
    <el-card class="box-card" shadow="hover" style>
      <el-row :gutter="5">
        <el-col :span="2">Request Path&nbsp;</el-col>
        <el-col :span="10">
          <el-cascader
            :options="requestPath"
            clearable
            style="width:100%"
            filterable
            :debounce="500"
            :filter-method="cascaderFilterMethod"
            v-model="filtrate.requestPath"
            placeholder="Select Request Path"
            :props="{ checkStrictly: true }"
          >
            <template slot-scope="{ node, data }">
              <span>{{ data.label }}</span>
              &nbsp;({{ data.docCount }})
            </template>
          </el-cascader>
        </el-col>
        <el-col :span="1">Level</el-col>
        <el-col :span="3">
          <el-select v-model="filtrate.lv" clearable placeholder="Select Level">
            <el-option label="Information" value="Information"></el-option>
            <el-option label="Warning" value="Warning"></el-option>
            <el-option label="Error" value="Error"></el-option>
            <el-option label="Debug" value="Debug"></el-option>
            <el-option label="Fatal" value="Fatal"></el-option>
          </el-select>
        </el-col>
        <el-col :span="2">Time Range</el-col>
        <el-col :span="6">
          <el-date-picker
            v-if="switchTime"
            v-model="filtrate.times"
            type="datetimerange"
            range-separator="~"
            start-placeholder="Start Time"
            end-placeholder="End Time"
            align="right"
            format="yyyy-MM-dd HH:mm"
            value-format="yyyy-MM-dd HH:mm"
            style="width:83%"
          ></el-date-picker>
          <el-select v-else v-model="filtrate.timeSelect">
            <el-option label="lately 10 minute" value="10"></el-option>
            <el-option label="lately 30 minute" value="30"></el-option>
            <el-option label="lately 1 hour" value="60"></el-option>
            <el-option label="lately 8 hour" value="480"></el-option>
          </el-select>
          <el-button icon="el-icon-full-screen" circle @click="switchTime = !switchTime"></el-button>
        </el-col>
      </el-row>
      <br />
      <!-- 
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
      </el-col>-->

      <el-row :gutter="5">
        <el-col :span="2">Context&nbsp;</el-col>
        <el-col :span="10">
          <el-cascader
            :options="context"
            clearable
            style="width:100%"
            filterable
            :debounce="500"
            :filter-method="cascaderFilterMethod"
            v-model="filtrate.context"
            placeholder="Select Context"
            :props="{ checkStrictly: true }"
          >
            <template slot-scope="{ node, data }">
              <span>{{ data.label }}</span>
              &nbsp;({{ data.docCount }})
            </template>
          </el-cascader>
        </el-col>
        <el-col :span="1">Search</el-col>
        <el-col :span="10">
          <el-input
            v-model="filtrate.msg"
            placeholder="This will disrupt the chronological order of the input based on how well it matches!Please strictly limit the time range, otherwise the search results will be more deviation"
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
        >Search</el-button>
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
    <el-divider content-position="left">Search Result</el-divider>
    <el-table
      :data="tableData"
      border
      style="width: 100%"
      @row-dblclick="showDetail"
      v-loading="loading"
      :row-class-name="tableRowClassName"
    >
      >
      <el-table-column prop="timestamp" label="Time" width="165"></el-table-column>
      <el-table-column prop="level" label="Level" width="110">
        <template slot-scope="scope">
          <span>{{scope.row.level}}</span>
        </template>
      </el-table-column>
      <el-table-column prop="RequestPath" label="RequestPath" width="280">
        <template slot-scope="scope">
          <span class="span-long-text" :title="scope.row.RequestPath">{{scope.row.RequestPath}}</span>
        </template>
      </el-table-column>
      <el-table-column prop="module" label="Context" width="380">
        <template slot-scope="scope">
          <span class="span-long-text" :title="scope.row.SourceContext">{{scope.row.SourceContext}}</span>
        </template>
      </el-table-column>
      <el-table-column prop="message" label="Message">
        <template slot-scope="scope">
          <!-- <span
            class="span-long-text"
            v-if="scope.row['fields.msg'] && scope.row['fields.msg'][0]"
            v-html="scope.row['fields.msg'][0]"
          ></span> TODO:不知道干嘛的逻辑-->
          <span class="span-long-text">{{scope.row.message}}</span>
        </template>
      </el-table-column>
      <el-table-column prop="RequestId" label="RequestId" width="220"></el-table-column>
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
import { postAggregation, postSearch } from "@/api/system-log";
import { parseTime } from "@/utils";

export default {
  data() {
    return {
      filtrate: {
        times: [],
        timeSelect: "10"
      },
      switchTime: false,
      requestPath: [],
      context: [],
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
      var { result } = await postSearch({
        ...this.filtrate,
        pageSize: this.pageSize,
        pageIndex
      });
      var content = JSON.parse(result);
      console.log(content);
      this.tableData = content.hits.hits.map(x => {
        return {
          timestamp: parseTime(x._source["@timestamp"]),
          level: x._source.level,
          message: x._source.message,
          ...x._source.fields,
          ...x.highlight
        };
      });
      this.total = content.hits.total;
      this.loading = false;
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
    },
    async getAggregation() {
      var { result } = await postAggregation(this.filtrate);
      console.log(result);
      this.requestPath = result.requestPath;
      this.context = result.context;
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
    },
    "filtrate.timeSelect"(val) {
      if (val > 0) this.getAggregation();
    },
    "filtrate.times"(val) {
      if (val.length == 2) this.getAggregation();
    },
    "filtrate.lv"(val) {
      this.getAggregation();
    }
  },
  async mounted() {
    await this.getAggregation();
    await this.search(1);
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