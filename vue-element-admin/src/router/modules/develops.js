import Layout from '@/layout'

let chartsRouter = [{
  path: '/charts',
  component: Layout,
  redirect: 'noRedirect',
  name: 'Charts',
  meta: {
    title: 'Charts',
    icon: 'chart'
  },
  children: [{
      path: 'keyboard',
      component: () => import('@/views/charts/keyboard'),
      name: 'KeyboardChart',
      meta: {
        title: 'Keyboard Chart',
        noCache: true
      }
    },
    {
      path: 'line',
      component: () => import('@/views/charts/line'),
      name: 'LineChart',
      meta: {
        title: 'Line Chart',
        noCache: true
      }
    },
    {
      path: 'mix-chart',
      component: () => import('@/views/charts/mix-chart'),
      name: 'MixChart',
      meta: {
        title: 'Mix Chart',
        noCache: true
      }
    }
  ]
}]

let examplesRoutes = [{
  path: '/example',
  component: Layout,
  redirect: '/example/list',
  name: 'Example',
  meta: {
    title: 'Example',
    icon: 'example'
  },
  children: [{
      path: 'create',
      component: () => import('@/views/example/create'),
      name: 'CreateArticle',
      meta: {
        title: 'Create Article',
        icon: 'edit'
      }
    },
    {
      path: 'edit/:id(\\d+)',
      component: () => import('@/views/example/edit'),
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
      component: () => import('@/views/example/list'),
      name: 'ArticleList',
      meta: {
        title: 'Article List',
        icon: 'list'
      }
    }
  ]
}];

let componentsRouter = [{
  path: '/components',
  component: Layout,
  redirect: 'noRedirect',
  name: 'ComponentDemo',
  meta: {
    title: 'Components',
    icon: 'component'
  },
  children: [{
      path: 'tinymce',
      component: () => import('@/views/components-demo/tinymce'),
      name: 'TinymceDemo',
      meta: {
        title: 'Tinymce'
      }
    },
    {
      path: 'markdown',
      component: () => import('@/views/components-demo/markdown'),
      name: 'MarkdownDemo',
      meta: {
        title: 'Markdown'
      }
    },
    {
      path: 'json-editor',
      component: () => import('@/views/components-demo/json-editor'),
      name: 'JsonEditorDemo',
      meta: {
        title: 'JSON Editor'
      }
    },
    {
      path: 'split-pane',
      component: () => import('@/views/components-demo/split-pane'),
      name: 'SplitpaneDemo',
      meta: {
        title: 'SplitPane'
      }
    },
    {
      path: 'avatar-upload',
      component: () => import('@/views/components-demo/avatar-upload'),
      name: 'AvatarUploadDemo',
      meta: {
        title: 'Upload'
      }
    },
    {
      path: 'dropzone',
      component: () => import('@/views/components-demo/dropzone'),
      name: 'DropzoneDemo',
      meta: {
        title: 'Dropzone'
      }
    },
    {
      path: 'sticky',
      component: () => import('@/views/components-demo/sticky'),
      name: 'StickyDemo',
      meta: {
        title: 'Sticky'
      }
    },
    {
      path: 'count-to',
      component: () => import('@/views/components-demo/count-to'),
      name: 'CountToDemo',
      meta: {
        title: 'Count To'
      }
    },
    {
      path: 'mixin',
      component: () => import('@/views/components-demo/mixin'),
      name: 'ComponentMixinDemo',
      meta: {
        title: 'Component Mixin'
      }
    },
    {
      path: 'back-to-top',
      component: () => import('@/views/components-demo/back-to-top'),
      name: 'BackToTopDemo',
      meta: {
        title: 'Back To Top'
      }
    },
    {
      path: 'drag-dialog',
      component: () => import('@/views/components-demo/drag-dialog'),
      name: 'DragDialogDemo',
      meta: {
        title: 'Drag Dialog'
      }
    },
    {
      path: 'drag-select',
      component: () => import('@/views/components-demo/drag-select'),
      name: 'DragSelectDemo',
      meta: {
        title: 'Drag Select'
      }
    },
    {
      path: 'dnd-list',
      component: () => import('@/views/components-demo/dnd-list'),
      name: 'DndListDemo',
      meta: {
        title: 'Dnd List'
      }
    },
    {
      path: 'drag-kanban',
      component: () => import('@/views/components-demo/drag-kanban'),
      name: 'DragKanbanDemo',
      meta: {
        title: 'Drag Kanban'
      }
    }
  ]
}];

let othersRoutes = [{
  path: '/tab',
  component: Layout,
  children: [{
    path: 'index',
    component: () => import('@/views/tab/index'),
    name: 'Tab',
    meta: {
      title: 'Tab',
      icon: 'tab'
    }
  }]
}]

let nestedRouter = [{
  path: '/nested',
  component: Layout,
  redirect: '/nested/menu1/menu1-1',
  name: 'Nested',
  meta: {
    title: 'Nested Routes',
    icon: 'nested'
  },
  children: [{
      path: 'menu1',
      component: () => import('@/views/nested/menu1/index'), // Parent router-view
      name: 'Menu1',
      meta: {
        title: 'Menu 1'
      },
      redirect: '/nested/menu1/menu1-1',
      children: [{
          path: 'menu1-1',
          component: () => import('@/views/nested/menu1/menu1-1'),
          name: 'Menu1-1',
          meta: {
            title: 'Menu 1-1'
          }
        },
        {
          path: 'menu1-2',
          component: () => import('@/views/nested/menu1/menu1-2'),
          name: 'Menu1-2',
          redirect: '/nested/menu1/menu1-2/menu1-2-1',
          meta: {
            title: 'Menu 1-2'
          },
          children: [{
              path: 'menu1-2-1',
              component: () => import('@/views/nested/menu1/menu1-2/menu1-2-1'),
              name: 'Menu1-2-1',
              meta: {
                title: 'Menu 1-2-1'
              }
            },
            {
              path: 'menu1-2-2',
              component: () => import('@/views/nested/menu1/menu1-2/menu1-2-2'),
              name: 'Menu1-2-2',
              meta: {
                title: 'Menu 1-2-2'
              }
            }
          ]
        },
        {
          path: 'menu1-3',
          component: () => import('@/views/nested/menu1/menu1-3'),
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
      component: () => import('@/views/nested/menu2/index'),
      meta: {
        title: 'Menu 2'
      }
    }
  ]
}];

const tablesRouter = [{
  path: '/table',
  component: Layout,
  redirect: '/table/complex-table',
  name: 'Table',
  meta: {
    title: 'Table',
    icon: 'table'
  },
  children: [{
      path: 'dynamic-table',
      component: () => import('@/views/table/dynamic-table/index'),
      name: 'DynamicTable',
      meta: {
        title: 'Dynamic Table'
      }
    },
    {
      path: 'drag-table',
      component: () => import('@/views/table/drag-table'),
      name: 'DragTable',
      meta: {
        title: 'Drag Table'
      }
    },
    {
      path: 'inline-edit-table',
      component: () => import('@/views/table/inline-edit-table'),
      name: 'InlineEditTable',
      meta: {
        title: 'Inline Edit'
      }
    },
    {
      path: 'complex-table',
      component: () => import('@/views/table/complex-table'),
      name: 'ComplexTable',
      meta: {
        title: 'Complex Table'
      }
    }
  ]
}];

const developsRouter = [
  ...chartsRouter,
  ...examplesRoutes,
  ...othersRoutes,
  ...componentsRouter,
  ...nestedRouter,
  ...tablesRouter
];

export default developsRouter;
