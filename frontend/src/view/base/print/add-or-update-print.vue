<template>
  <v-dialog v-model="isShow" fullscreen :scrim="false" transition="dialog-bottom-transition">
    <v-card>
      <div style="padding: 15px 15px">
        <v-row no-gutters>
          <v-col cols="4">
            <v-row no-gutters>
              <v-col :cols="4">
                <v-select
                  v-model="data.form.vue_path"
                  :items="menuList"
                  item-title="label"
                  item-value="value"
                  :label="$t('base.printSolution.vue_path')"
                  density="compact"
                  hide-details
                  clearable
                  variant="outlined"
                  :disabled="props.form.id > 0"
                  @update:model-value="method.handleChangePath()"
                ></v-select>
              </v-col>
              <v-col cols="4">
                <div style="padding-left: 5px">
                  <v-select
                    v-model="data.form.tab_page"
                    :items="data.pageList"
                    item-title="label"
                    item-value="value"
                    :label="$t('base.printSolution.tab_page')"
                    density="compact"
                    hide-details
                    :disabled="props.form.id > 0"
                    clearable
                    variant="outlined"
                    @update:model-value="method.handleChangePage()"
                  ></v-select>
                </div>
              </v-col>
              <v-col cols="4">
                <div style="padding-left: 5px">
                  <v-text-field
                    v-model="data.form.solution_name"
                    :label="$t('base.printSolution.solution_name')"
                    density="compact"
                    hide-details
                    clearable
                    variant="outlined"
                    :disabled="props.form.id > 0"
                  ></v-text-field>
                </div>
              </v-col>
            </v-row>
          </v-col>
          <v-col cols="7">
            <div class="center-btn-box">
              <div>
                <v-btn
                  v-for="(value, type) in data.paperTypes"
                  :key="type"
                  :color="curPaperType === type ? 'indigo-darken-3' : 'white'"
                  @click="method.handleSetPaper(type, value)"
                >
                  {{ type }}
                </v-btn>
                <v-btn
                  icon="mdi-magnify-minus-outline"
                  style="font-size: 14px; margin-left: 10px"
                  size="small"
                  @click="method.handleChangeScale(false)"
                ></v-btn>
                <v-btn disabled size="small" style="margin-left: 10px; width: 70px">{{ scaleValueStr }}%</v-btn>
                <v-btn
                  icon="mdi-magnify-plus-outline"
                  style="font-size: 14px; margin-left: 10px"
                  size="small"
                  @click="method.handleChangeScale(true)"
                ></v-btn>
              </div>
              <div>
                <v-btn style="font-size: 14px; margin-left: 10px" @click="method.handlePreView()">
                  {{ $t('system.page.preView') }}
                </v-btn>
                <v-btn
                  style="font-size: 14px; margin-left: 10px"
                  :disabled="!data.form.solution_name || !data.form.tab_page"
                  color="primary"
                  @click="method.handleSubmit"
                >
                  {{ $t('system.page.submit') }}
                </v-btn>
              </div>
            </div>
          </v-col>
          <v-col cols="1">
            <div class="right-top-box">
              <v-btn icon style="font-size: 14px; margin-left: 10px" size="small" @click="method.handleClose()">
                <v-icon>mdi-close</v-icon>
              </v-btn>
            </div>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="2">
            <div class="left-box">
              <div id="hiprintEpContainer" class="rect-printElement-types hiprintEpContainer"> </div>
            </div>
          </v-col>
          <v-col cols="7">
            <div class="center-box card-design">
              <div id="hiprint-printTemplate" style="margin-top: 20px" class="hiprint-printTemplate"></div>
            </div>
          </v-col>
          <v-col cols="3">
            <div class="right-box">
              <div id="PrintElementOptionSetting"></div>
            </div>
          </v-col>
        </v-row>
      </div>
    </v-card>

    <preViewDialog ref="preViewDialogRef" />
  </v-dialog>
</template>
<script lang="ts" setup>
import { reactive, ref, computed, watch, nextTick, getCurrentInstance, ComponentInternalInstance } from 'vue'
import { hookComponent } from '@/components/system/index'
import { addPrintSolution, updatePrintSolution } from '@/api/base/printSolution'
import { PrintSolutionVO } from '@/types/Base/PrintSolution'
import i18n from '@/languages/i18n'
import preViewDialog from '@/components/hiprint/preView.vue'
import { PRINT_MENU } from '@/constant/print'

const logoRef = ref()
const { appContext } = getCurrentInstance() as ComponentInternalInstance
const proxy = appContext.config.globalProperties
const { hiprint } = proxy
const emit = defineEmits(['close', 'saveSuccess'])

interface paperTypeData {
  width: number
  height: number
}
interface tableInterFace {
  name: string
  field: string
  columns: Array<string>
}
interface selectInterFace {
  value: string
  label: string
  form: any
}
const props = defineProps<{
  showDialog: boolean
  form: PrintSolutionVO
}>()
const isShow = computed(() => props.showDialog)
const preViewDialogRef = ref()
const data = reactive({
  form: {
    id: 0,
    solution_name: '',
    config_json: '',
    report_length: 0,
    report_width: 0,
    report_direction: 'A4',
    tenant_id: 0
  } as PrintSolutionVO,
  table: [] as tableInterFace[],
  i18nName: '',
  pageForm: {} as any,
  hiprintTemplate: {} as any,
  mode: '',
  modeList: [] as string[],
  pageList: [] as selectInterFace[],
  // 当前纸张
  curPaper: {
    type: 'A4',
    width: 210,
    height: 296.6
  },
  // 纸张类型
  paperTypes: {
    A3: {
      width: 420,
      height: 296.6
    },
    A4: {
      width: 210,
      height: 296.6
    },
    A5: {
      width: 210,
      height: 147.6
    },
    B3: {
      width: 500,
      height: 352.6
    },
    B4: {
      width: 250,
      height: 352.6
    },
    B5: {
      width: 250,
      height: 175.6
    }
  },
  scaleValue: 1,
  scaleMax: 5,
  scaleMin: 0.5,
  panel: {}
})

const method = reactive({
  provider() {
    const elementList = [
      new hiprint.PrintElementTypeGroup(i18n.global.t('system.hiprint.routine'), [
        {
          tid: 'providerModule.customText',
          title: i18n.global.t('system.hiprint.text'),
          customText: i18n.global.t('system.hiprint.text'),
          custom: true,
          type: 'text'
        },
        {
          tid: 'providerModule.image',
          title: 'Logo',
          custom: true,
          type: 'image'
        }
      ])
    ]
    const userList = [] as any[]
    for (const key in data.pageForm) {
      const i18nName = `${ data.i18nName }.${ key }`
      const labelName = i18n.global.t(i18nName)
      if (labelName !== i18nName && !key.includes('detailList')) {
        userList.push({
          tid: `providerModule.${ key }`,
          title: labelName,
          data: labelName,
          type: 'text',
          options: {
            field: key,
            testData: labelName,
            height: 16,
            fontSize: 6.75,
            fontWeight: '700',
            textAlign: 'left',
            textContentVerticalAlign: 'middle'
          }
        })
      }
    }
    let index = 1
    data.table.forEach((item) => {
      const object = {
        tid: `providerModule.table${ index.toString() }`,
        title: item.name,
        type: 'table',
        options: {
          field: item.field,
          fields: [{ text: '名称', field: 'NAME' }]
        },
        editable: true,
        columnDisplayEditable: true, // 列显示是否能编辑
        columnDisplayIndexEditable: true, // 列顺序显示是否能编辑
        columnTitleEditable: true, // 列标题是否能编辑
        columnResizable: true, // 列宽是否能调整
        columnAlignEditable: true, // 列对齐是否调整
        columns: [[{ title: '部门', align: 'center', field: 'dept', width: 50 }]]
      }
      object.columns[0] = []
      object.options.fields = []
      item.columns.forEach((col) => {
        const i18nName = `${ data.i18nName }.${ col }`
        const labelName = i18n.global.t(i18nName)
        if (i18nName !== labelName) {
          object.columns[0].push({ title: labelName, align: 'center', field: col, width: 50 })
          object.options.fields.push({ text: labelName, field: col })
        }
      })
      userList.push(object)
      index += 1
    })
    if (userList.length > 0) {
      elementList.push(new hiprint.PrintElementTypeGroup(i18n.global.t('system.hiprint.field'), userList))
    }
    elementList.push(
      new hiprint.PrintElementTypeGroup(i18n.global.t('system.hiprint.auxiliary'), [
        {
          tid: 'providerModule.hline',
          title: i18n.global.t('system.hiprint.hline'),
          type: 'hline'
        },
        {
          tid: 'providerModule.vline',
          title: i18n.global.t('system.hiprint.vline'),
          type: 'vline'
        },
        {
          tid: 'providerModule.rect',
          title: i18n.global.t('system.hiprint.rect'),
          type: 'rect'
        },
        {
          tid: 'providerModule.oval',
          title: i18n.global.t('system.hiprint.oval'),
          type: 'oval'
        }
      ])
    )
    const addElementTypes = function (context: any) {
      context.removePrintElementTypes('providerModule')
      context.addPrintElementTypes('providerModule', elementList)
    }
    return {
      addElementTypes
    }
  },
  initProvier() {
    hiprint.init({
      providers: [method.provider()]
    })
    // $('.hiprintEpContainer').empty()
    const dom = document.getElementById('hiprintEpContainer')
    if (dom !== null) {
      dom.innerHTML = ''
    }
    hiprint.PrintElementTypeManager.build('.hiprintEpContainer', 'providerModule')
  },
  initTemplate() {
    // const provider = providers[0]
    // $('#hiprint-printTemplate').empty()
    const dom = document.getElementById('hiprint-printTemplate')
    if (dom !== null) {
      dom.innerHTML = ''
    }
    data.hiprintTemplate = new hiprint.PrintTemplate({
      template: data.panel,
      dataMode: 1, // 1:getJson 其他：getJsonTid 默认1
      history: false, // 是否需要 撤销重做功能
      // onDataChanged: (type, json) => {
      //   console.log(type) // 新增、移动、删除、修改(参数调整)、大小、旋转
      //   console.log(json) // 返回 template
      //   // 更新模板
      //   // hiprintTemplate.update(json)
      //   // console.log(hiprintTemplate.historyList)
      // },
      onImageChooseClick: (target) => {
        // 创建input，模拟点击，然后 target.refresh 更新
        const input = document.createElement('input')
        input.setAttribute('type', 'file')
        input.click()
        input.onchange = function (evnt: any) {
          const file = evnt?.target?.files[0]
          if (file) {
            const reader = new FileReader()
            // 通过文件流行文件转换成Base64字行串
            reader.readAsDataURL(file)
            // 转换成功后
            reader.onloadend = function () {
              // 通过 target.refresh 更新图片元素
              target.refresh(reader.result)
            }
          }
        }
        input.remove()
      },
      settingContainer: '#PrintElementOptionSetting',
      paginationContainer: '.hiprint-printPagination'
    })
    data.hiprintTemplate.design('#hiprint-printTemplate')
  },
  handleSetPaper(type: string, value: paperTypeData) {
    try {
      if (Object.keys(data.paperTypes).includes(type)) {
        data.curPaper = { type, width: value.width, height: value.height }
      } else {
        data.curPaper = { type: 'other', width: value.width, height: value.height }
      }
      if (data.hiprintTemplate) {
        data.hiprintTemplate.setPaper(value.width, value.height)
      }
    } catch (error) {
      // console.log(error)
    }
  },
  handleChangeScale(big: boolean) {
    let scaleValue = data.scaleValue
    if (big) {
      scaleValue += 0.1
      if (scaleValue > data.scaleMax) scaleValue = 5
    } else {
      scaleValue -= 0.1
      if (scaleValue < data.scaleMin) scaleValue = 0.5
    }
    scaleValue = Number(scaleValue.toFixed(1))
    if (data.hiprintTemplate) {
      // scaleValue: 放大缩小值, false: 不保存(不传也一样), 如果传 true, 打印时也会放大
      data.hiprintTemplate.zoom(scaleValue, true)
      data.scaleValue = scaleValue
    }
  },
  handleChangePath() {
    data.form.tab_page = ''
    method.initPageList()
  },
  initPageList() {
    data.pageList = []
    const option = PRINT_MENU.filter((item) => item.vue_path === data.form.vue_path)
    if (option.length > 0) {
      data.i18nName = option[0].i18nName
      data.pageList = option[0].children.map((item) => {
        let label = item.tab_page
        if (item.tab_page === option[0].vue_path) {
          label = i18n.global.t(`router.sideBar.${ item.tab_page }`)
        } else {
          label = i18n.global.t(`${ data.i18nName }.${ item.tab_page }`)
        }
        return { value: item.tab_page, label, form: item.form }
      })
    }
  },
  handleChangePage() {
    const option = data.pageList.filter((item) => item.value === data.form.tab_page)
    data.table = []
    if (option.length > 0) {
      data.pageForm = option[0].form
      for (const key in data.pageForm) {
        if (key.indexOf('detailList') > -1) {
          const detailList = data.pageForm[key]
          const name = i18n.global.t(`${ data.i18nName }.${ detailList.name }`)
          data.table.push({ name, field: key, columns: detailList.columns })
        }
      }
    }
    method.initProvier()
    method.initTemplate()
  },
  async handleSubmit() {
    if (data.hiprintTemplate) {
      const jsonOut = JSON.stringify(data.hiprintTemplate.getJson() || {})
      const form = {
        id: data.form.id,
        vue_path: data.form.vue_path,
        tab_page: data.form.tab_page,
        solution_name: data.form.solution_name,
        config_json: jsonOut,
        report_length: data.curPaper.height,
        report_width: data.curPaper.width,
        report_direction: data.curPaper.type,
        tenant_id: 0
      } as PrintSolutionVO

      const { data: res } = form.id === 0 ? await addPrintSolution(form) : await updatePrintSolution(form)
      if (!res.isSuccess) {
        hookComponent.$message({
          type: 'error',
          content: res.errorMessage
        })
        return
      }
      hookComponent.$message({
        type: 'success',
        content: `${ i18n.global.t('system.page.submit') }${ i18n.global.t('system.tips.success') }`
      })
      emit('saveSuccess')
    }
  },
  handleClose() {
    emit('close')
  },
  handlePreView() {
    const ref = preViewDialogRef.value
    ref.data.width = data.curPaper.width
    ref.data.hiprintTemplate = data.hiprintTemplate
    const printData = {}
    for (const key in data.pageForm) {
      if (!key.includes('detailList')) {
        let value = key
        const i18nName = `${ data.i18nName }.${ key }`
        const labelName = i18n.global.t(i18nName)
        if (labelName !== i18nName) {
          value = labelName
        }
        printData[key] = value
      } else {
        const detailList = data.pageForm[key]
        const obj = {}
        detailList.columns.forEach((col) => {
          const i18nName = `${ data.i18nName }.${ col }`
          const labelName = i18n.global.t(i18nName)
          if (i18nName !== labelName) {
            obj[col] = labelName
          } else {
            obj[col] = col
          }
        })
        printData[key] = [obj]
      }
    }
    ref.data.printData = printData
    ref.method.show()
  }
})

watch(
  () => isShow.value,
  (val) => {
    if (val) {
      nextTick(() => {
        if (props.form.id > 0) {
          data.form = props.form
          if (data.form.config_json) {
            data.panel = JSON.parse(data.form.config_json)
          } else {
            data.panel = {}
          }
          method.initPageList()
          method.handleChangePage()
          if (Object.keys(data.paperTypes).includes(data.form.report_direction)) {
            data.curPaper = {
              type: data.form.report_direction,
              width: data.paperTypes[data.form.report_direction].width,
              height: data.paperTypes[data.form.report_direction].height
            }
            method.handleSetPaper(data.curPaper.type, data.curPaper)
          }
        } else {
          data.form = {
            id: 0,
            solution_name: '',
            config_json: '',
            report_length: 0,
            report_width: 0,
            report_direction: 'st',
            tenant_id: 0
          } as PrintSolutionVO
          data.curPaper = {
            type: 'A4',
            width: 210,
            height: 296.6
          }
          method.handleSetPaper(data.curPaper.type, data.curPaper)
          const dom = document.getElementById('hiprintEpContainer')
          if (dom !== null) {
            dom.innerHTML = ''
          }
          const dom1 = document.getElementById('hiprint-printTemplate')
          if (dom1 !== null) {
            dom1.innerHTML = ''
          }
          data.panel = {}
        }
      })
    }
  }
)
const scaleValueStr = computed(() => {
  let scaleValue = data.scaleValue * 100
  scaleValue = Number(scaleValue.toFixed(0))
  return scaleValue
})
const curPaperType = computed(() => {
  let type = 'other'
  const types = data.paperTypes
  for (const key in types) {
    const item = types[key]
    const { width, height } = data.curPaper
    if (item.width === width && item.height === height) {
      type = key
    }
  }
  return type
})
const menuList = computed(() => PRINT_MENU.map((item) => ({ value: item.vue_path, label: i18n.global.t(`router.sideBar.${ item.vue_path }`) })))

defineExpose({
  data
})
</script>
<style scoped lang="less">
.center-btn-box {
  margin-left: 10px;
  margin-bottom: 5px;
  display: flex;
  justify-content: start;
}

.center-box {
  height: calc(100vh - 100px);
  border: 2px solid #e8e8e8;
  box-sizing: border-box;
  border-radius: 2px;
  padding: 0 5px;
  display: flex;
  justify-content: center;
  /* 水平居中对齐 */
}

.left-top-box {
  display: flex;
  justify-content: space-between;
  /* 水平居中对齐 */
}

.left-box {
  height: calc(100vh - 100px);
  border: 2px solid #e8e8e8;
  box-sizing: border-box;
  border-radius: 2px;
  padding: 0 5px;
  display: flex;
  justify-content: center;
}

.right-top-box {
  display: flex;
  justify-content: flex-end;
}

.right-box {
  height: calc(100vh - 100px);
  border: 2px solid #e8e8e8;
  box-sizing: border-box;
  border-radius: 2px;
  padding: 0 5px;
  display: flex;
  justify-content: center;
}

.hiprintEpContainer {
  max-width: 200px;
}

:deep(.hiprint-printElement-type > li > ul > li > a) {
  padding: 4px 4px !important;
  color: white !important;
  background-color: #9c27b0 !important;
  font-family: 'Roboto', sans-serif !important;
  font-weight: 500 !important;
  line-height: normal !important;
  vertical-align: middle !important;
  justify-content: center !important;
  height: 30px !important;
  border-radius: 4px !important;
  border-style: none !important;
  text-overflow: ellipsis !important;
  font-size: 14px !important;
  box-shadow: 0 3px 1px -2px var(--v-shadow-key-umbra-opacity, rgba(0, 0, 0, 0.2)),
    0 2px 2px 0 var(--v-shadow-key-penumbra-opacity, rgba(0, 0, 0, 0.14)), 0 1px 5px 0 var(--v-shadow-key-penumbra-opacity, rgba(0, 0, 0, 0.12)) !important;
}

:deep(.hiprint-printElement-type > li > .title) {
  color: #000000de !important;
  font-family: 'Roboto', sans-serif !important;
  font-weight: 500 !important;
}

// 最右边样式，确认按钮背景色
:deep(.hiprint-option-item-settingBtn) {
  background: #9c27b0 !important;
}

// 最右边样式，删除按钮背景色
:deep(.hiprint-option-item-deleteBtn) {
  background: #8d9098 !important;
}

// 默认图片
// :deep(.hiprint-printElement-image-content) {
//   img {
//     content: url('@/assets/img/webLogoMini.png') !important;
//   }
// }

// 设计容器
.card-design {
  overflow: hidden;
  overflow-x: auto;
  overflow-y: auto;
}
</style>
