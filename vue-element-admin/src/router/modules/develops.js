import Layout from '@/layout'

const chartsRouter = [{
  path: 'charts',
  component: () => import("@/views/dev-example/index"),
  name: 'Charts',
  meta: {
    title: 'Charts',
    icon: 'chart'
  },
  children: [{
      path: 'keyboard',
      component: () => import('@/views/dev-example/charts/keyboard'),
      name: 'KeyboardChart',
      meta: {
        title: 'Keyboard Chart',
        noCache: true
      }
    },
    {
      path: 'line',
      component: () => import('@/views/dev-example/charts/line'),
      name: 'LineChart',
      meta: {
        title: 'Line Chart',
        noCache: true
      }
    },
    {
      path: 'mix-chart',
      component: () => import('@/views/dev-example/charts/mix-chart'),
      name: 'MixChart',
      meta: {
        title: 'Mix Chart',
        noCache: true
      }
    }
  ]
}]

const examplesRoutes = [{
  path: '/example',
  component: () => import("@/views/dev-example/index"),
  redirect: '/example/list',
  name: 'Example',
  meta: {
    title: 'Example',
    icon: 'example'
  },
  children: [{
      path: 'create',
      component: () => import('@/views/dev-example/example/create'),
      name: 'CreateArticle',
      meta: {
        title: 'Create Article',
        icon: 'edit'
      }
    },
    {
      path: 'edit/:id(\\d+)',
      component: () => import('@/views/dev-example/example/edit'),
      name: 'EditArticle',
      meta: {
        title: 'Edit Article',
        noCache: true,
        activeMenu: '/example/list'
      },
      hidden: true
    },
    {
      path: 'list',
      component: () => import('@/views/dev-example/example/list'),
      name: 'ArticleList',
      meta: {
        title: 'Article List',
        icon: 'list'
      }
    }
  ]
}];

const componentsRouter = [{
  path: '/components',
  component: () => import("@/views/dev-example/index"),
  redirect: 'noRedirect',
  name: 'ComponentDemo',
  meta: {
    title: 'Components',
    icon: 'component'
  },
  children: [{
      path: 'tinymce',
      component: () => import('@/views/dev-example/components-demo/tinymce'),
      name: 'TinymceDemo',
      meta: {
        title: 'Tinymce'
      }
    },
    {
      path: 'markdown',
      component: () => import('@/views/dev-example/components-demo/markdown'),
      name: 'MarkdownDemo',
      meta: {
        title: 'Markdown'
      }
    },
    {
      path: 'json-editor',
      component: () => import('@/views/dev-example/components-demo/json-editor'),
      name: 'JsonEditorDemo',
      meta: {
        title: 'JSON Editor'
      }
    },
    {
      path: 'split-pane',
      component: () => import('@/views/dev-example/components-demo/split-pane'),
      name: 'SplitpaneDemo',
      meta: {
        title: 'SplitPane'
      }
    },
    {
      path: 'avatar-upload',
      component: () => import('@/views/dev-example/components-demo/avatar-upload'),
      name: 'AvatarUploadDemo',
      meta: {
        title: 'Upload'
      }
    },
    {
      path: 'dropzone',
      component: () => import('@/views/dev-example/components-demo/dropzone'),
      name: 'DropzoneDemo',
      meta: {
        title: 'Dropzone'
      }
    },
    {
      path: 'sticky',
      component: () => import('@/views/dev-example/components-demo/sticky'),
      name: 'StickyDemo',
      meta: {
        title: 'Sticky'
      }
    },
    {
      path: 'count-to',
      component: () => import('@/views/dev-example/components-demo/count-to'),
      name: 'CountToDemo',
      meta: {
        title: 'Count To'
      }
    },
    {
      path: 'mixin',
      component: () => import('@/views/dev-example/components-demo/mixin'),
      name: 'ComponentMixinDemo',
      meta: {
        title: 'Component Mixin'
      }
    },
    {
      path: 'back-to-top',
      component: () => import('@/views/dev-example/components-demo/back-to-top'),
      name: 'BackToTopDemo',
      meta: {
        title: 'Back To Top'
      }
    },
    {
      path: 'drag-dialog',
      component: () => import('@/views/dev-example/components-demo/drag-dialog'),
      name: 'DragDialogDemo',
      meta: {
        title: 'Drag Dialog'
      }
    },
    {
      path: 'drag-select',
      component: () => import('@/views/dev-example/components-demo/drag-select'),
      name: 'DragSelectDemo',
      meta: {
        title: 'Drag Select'
      }
    },
    {
      path: 'dnd-list',
      component: () => import('@/views/dev-example/components-demo/dnd-list'),
      name: 'DndListDemo',
      meta: {
        title: 'Dnd List'
      }
    },
    {
      path: 'drag-kanban',
      component: () => import('@/views/dev-example/components-demo/drag-kanban'),
      name: 'DragKanbanDemo',
      meta: {
        title: 'Drag Kanban'
      }
    }
  ]
}];

const othersRoutes = [{
  path: '/tab',
  component: () => import("@/views/dev-example/index"),
  children: [{
    path: 'index',
    component: () => import('@/views/dev-example/tab/index'),
    name: 'Tab',
    meta: {
      title: 'Tab',
      icon: 'tab'
    }
  }]
}, {
  path: '/icon',
  component: () => import("@/views/dev-example/index"),
  children: [{
    path: 'index',
    component: () => import('@/views/dev-example/icons/index'),
    name: 'Icons',
    meta: {
      title: 'Icons',
      icon: 'icon',
      noCache: true
    }
  }]
}, {
  path: '/pdf',
  component: () => import("@/views/dev-example/index"),
  redirect: '/pdf/index',
  children: [{
    path: 'index',
    component: () => import('@/views/dev-example/pdf/index'),
    name: 'PDF',
    meta: {
      title: 'PDF',
      icon: 'pdf'
    }
  }]
}, {
  path: '/pdf/download',
  component: () => import('@/views/dev-example/pdf/download'),
  hidden: true
}, {
  path: '/theme',
  component: () => import("@/views/dev-example/index"),
  children: [{
    path: 'index',
    component: () => import('@/views/dev-example/theme/index'),
    name: 'Theme',
    meta: {
      title: 'Theme',
      icon: 'theme'
    }
  }]
}]

const nestedRouter = [{
  path: '/nested',
  component: () => import("@/views/dev-example/index"),
  redirect: '/nested/menu1/menu1-1',
  name: 'Nested',
  meta: {
    title: 'Nested Routes',
    icon: 'nested'
  },
  children: [{
      path: 'menu1',
      component: () => import('@/views/dev-example/nested/menu1/index'), // Parent router-view
      name: 'Menu1',
      meta: {
        title: 'Menu 1'
      },
      redirect: '/nested/menu1/menu1-1',
      children: [{
          path: 'menu1-1',
          component: () => import('@/views/dev-example/nested/menu1/menu1-1'),
          name: 'Menu1-1',
          meta: {
            title: 'Menu 1-1'
          }
        },
        {
          path: 'menu1-2',
          component: () => import('@/views/dev-example/nested/menu1/menu1-2'),
          name: 'Menu1-2',
          redirect: '/nested/menu1/menu1-2/menu1-2-1',
          meta: {
            title: 'Menu 1-2'
          },
          children: [{
              path: 'menu1-2-1',
              component: () => import('@/views/dev-example/nested/menu1/menu1-2/menu1-2-1'),
              name: 'Menu1-2-1',
              meta: {
                title: 'Menu 1-2-1'
              }
            },
            {
              path: 'menu1-2-2',
              component: () => import('@/views/dev-example/nested/menu1/menu1-2/menu1-2-2'),
              name: 'Menu1-2-2',
              meta: {
                title: 'Menu 1-2-2'
              }
            }
          ]
        },
        {
          path: 'menu1-3',
          component: () => import('@/views/dev-example/nested/menu1/menu1-3'),
          name: 'Menu1-3',
          meta: {
            title: 'Menu 1-3'
          }
        }
      ]
    },
    {
      path: 'menu2',
      name: 'Menu2',
      component: () => import('@/views/dev-example/nested/menu2/index'),
      meta: {
        title: 'Menu 2'
      }
    }
  ]
}];

const tablesRouter = [{
  path: '/table',
  component: () => import("@/views/dev-example/index"),
  redirect: '/table/complex-table',
  name: 'Table',
  meta: {
    title: 'Table',
    icon: 'table'
  },
  children: [{
      path: 'dynamic-table',
      component: () => import('@/views/dev-example/table/dynamic-table/index'),
      name: 'DynamicTable',
      meta: {
        title: 'Dynamic Table'
      }
    },
    {
      path: 'drag-table',
      component: () => import('@/views/dev-example/table/drag-table'),
      name: 'DragTable',
      meta: {
        title: 'Drag Table'
      }
    },
    {
      path: 'inline-edit-table',
      component: () => import('@/views/dev-example/table/inline-edit-table'),
      name: 'InlineEditTable',
      meta: {
        title: 'Inline Edit'
      }
    },
    {
      path: 'complex-table',
      component: () => import('@/views/dev-example/table/complex-table'),
      name: 'ComplexTable',
      meta: {
        title: 'Complex Table'
      }
    }
  ]
}];

const execlsRouter = [{
  path: '/excel',
  component: () => import("@/views/dev-example/index"),
  redirect: '/excel/export-excel',
  name: 'Excel',
  meta: {
    title: 'Excel',
    icon: 'excel'
  },
  children: [{
      path: 'export-excel',
      component: () => import('@/views/dev-example/excel/export-excel'),
      name: 'ExportExcel',
      meta: {
        title: 'Export Excel'
      }
    },
    {
      path: 'export-selected-excel',
      component: () => import('@/views/dev-example/excel/select-excel'),
      name: 'SelectExcel',
      meta: {
        title: 'Export Selected'
      }
    },
    {
      path: 'export-merge-header',
      component: () => import('@/views/dev-example/excel/merge-header'),
      name: 'MergeHeader',
      meta: {
        title: 'Merge Header'
      }
    },
    {
      path: 'upload-excel',
      component: () => import('@/views/dev-example/excel/upload-excel'),
      name: 'UploadExcel',
      meta: {
        title: 'Upload Excel'
      }
    }
  ]
}];

const zipsRouter = [{
  path: '/zip',
  component: () => import("@/views/dev-example/index"),
  redirect: '/zip/download',
  alwaysShow: true,
  name: 'Zip',
  meta: {
    title: 'Zip',
    icon: 'zip'
  },
  children: [{
    path: 'download',
    component: () => import('@/views/dev-example/zip/index'),
    name: 'ExportZip',
    meta: {
      title: 'Export Zip'
    }
  }]
}]

const errorsRouter = [{
  path: '/error',
  component: () => import("@/views/dev-example/index"),
  redirect: 'noRedirect',
  name: 'ErrorPages',
  meta: {
    title: 'Error Pages',
    icon: '404'
  },
  children: [{
      path: '401',
      component: () => import('@/views/error-page/401'),
      name: 'Page401',
      meta: {
        title: '401',
        noCache: true
      }
    },
    {
      path: '404',
      component: () => import('@/views/error-page/404'),
      name: 'Page404',
      meta: {
        title: '404',
        noCache: true
      }
    }
  ]
}]

const developsRouter = {
  path: "/dev-example",
  component: Layout,
  redirect: "/dev-example/documentation/index",
  alwaysShow: true,
  name: "Developer",
  meta: {
    title: "Developer",
    icon: "documentation"
  },
  children: [{
      path: "documentation/index",
      component: () => import("@/views/dev-example/documentation/index"),
      name: "Documentation",
      meta: {
        title: "Documentation",
        icon: "documentation"
      }
    },
    ...chartsRouter,
    ...examplesRoutes,
    ...othersRoutes,
    ...componentsRouter,
    ...nestedRouter,
    ...tablesRouter,
    ...execlsRouter,
    ...zipsRouter,
    ...errorsRouter
  ]
};
console.log(developsRouter);
export default developsRouter;
