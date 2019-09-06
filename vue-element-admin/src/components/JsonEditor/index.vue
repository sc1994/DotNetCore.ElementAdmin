<template>
  <div class="json-editor">
    <textarea ref="textarea" />
  </div>
</template>

<script>
import CodeMirror from "codemirror";
import "codemirror/addon/lint/lint.css";
import "codemirror/lib/codemirror.css";
import "codemirror/theme/panda-syntax.css";
import "codemirror/lib/codemirror.css";
import "codemirror/addon/fold/foldgutter.css";
import "codemirror/addon/hint/show-hint.css";

require("script-loader!jsonlint");

import "codemirror/addon/lint/lint";
import "codemirror/addon/lint/json-lint";
import "codemirror/addon/edit/matchbrackets";
import "codemirror/addon/fold/foldcode";
import "codemirror/addon/fold/foldgutter";
import "codemirror/addon/fold/indent-fold";
import "codemirror/addon/fold/brace-fold";
import "codemirror/addon/hint/show-hint.js";
import "codemirror/mode/javascript/javascript";

export default {
  name: "JsonEditor",
  /* eslint-disable vue/require-prop-types */
  props: {
    value: [String, Object],
    readonly: {
      type: Boolean,
      default: false
    },
    lineNumber: {
      type: Boolean,
      default: true
    }
  },
  data() {
    return {
      jsonEditor: false
    };
  },
  watch: {
    value(value) {
      const editorValue = this.jsonEditor.getValue();
      if (value !== editorValue) {
        this.jsonEditor.setValue(JSON.stringify(this.value, null, 2));
      }
    }
  },
  mounted() {
    this.jsonEditor = CodeMirror.fromTextArea(this.$refs.textarea, {
      foldGutter: true,
      mode: "application/json",
      gutters: [
        "CodeMirror-lint-markers",
        "CodeMirror-linenumbers",
        "CodeMirror-foldgutter"
      ],
      lineNumbers: this.lineNumbers,
      theme: "panda-syntax",
      lint: true,
      readOnly: this.readonly,
      matchBrackets: true
    });

    this.jsonEditor.setValue(JSON.stringify(this.value, null, 2));
    this.jsonEditor.on("change", cm => {
      this.$emit("changed", cm.getValue());
      this.$emit("input", cm.getValue());
    });
  },
  methods: {
    getValue() {
      return this.jsonEditor.getValue();
    }
  }
};
</script>

<style scoped>
.json-editor {
  height: 100%;
  position: relative;
}
.json-editor >>> .CodeMirror {
  height: auto;
  min-height: 300px;
}
.json-editor >>> .CodeMirror-scroll {
  min-height: 300px;
}
.json-editor >>> .cm-s-rubyblue span.cm-string {
  color: #f08047;
}
</style>
