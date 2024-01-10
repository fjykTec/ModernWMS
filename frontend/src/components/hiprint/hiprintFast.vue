<template>
  <v-dialog v-model="data.formVisible" width="260px">
    <v-card>
      <div style="padding: 5px 15px">
        <v-row>
          <v-col cols="12">
            <div class="left-top-box">
              <v-select
                v-model="data.mode"
                :items="data.modeList"
                density="compact"
                hide-details
                clearable
                @update:model-value="method.handleChangeMode"
              ></v-select>
            </div>
          </v-col>
        </v-row>
        <!-- <v-row v-show="false">
          <v-col cols="12">
            <div class="center-box card-design">
              <div id="hiprint-printTemplate" style="margin-top: 20px" class="hiprint-printTemplate"></div>
            </div>
          </v-col>
        </v-row> -->
      </div>
      <v-card-actions class="justify-end">
        <v-btn variant="text" @click="method.handleClose">{{ $t('system.page.close') }}</v-btn>
        <v-btn color="primary" variant="text" @click="method.handlePrint">{{ $t('system.page.print') }}</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
<script lang="ts" setup>
import { reactive, computed, watch, nextTick, getCurrentInstance, ComponentInternalInstance } from 'vue'
import { useRouter } from 'vue-router'
import i18n from '@/languages/i18n'
import { listByPath } from '@/api/base/printSolution'
import { PrintSolutionVO, PrintSolutionGetByPathVo } from '@/types/Base/PrintSolution'
import { hookComponent } from '@/components/system/index'

const { appContext } = getCurrentInstance() as ComponentInternalInstance
const proxy = appContext.config.globalProperties
const { hiprint } = proxy
const props = defineProps<{
  form: object
  tabPage: string
}>()
const routerInfo = useRouter()
const data = reactive({
  formVisible: false,
  hiprintTemplate: {} as any,
  mode: '',
  modeList: [] as string[],
  panel: {},
  printSolutionList: [] as PrintSolutionVO[]
})
const method = reactive({
  initTemplate() {
    const dom = document.getElementById('hiprint-printTemplate')
    if (dom !== null) {
      dom.innerHTML = ''
    }
    data.hiprintTemplate = new hiprint.PrintTemplate({
      template: data.panel,
      dataMode: 1, // 1:getJson 其他：getJsonTid 默认1
      history: false, // 是否需要 撤销重做功能
      // settingContainer: '#PrintElementOptionSetting',
      // paginationContainer: '.hiprint-printPagination'
    })
    // data.hiprintTemplate.design('#hiprint-printTemplate')
    if (data.modeList.length === 1) {
      method.handlePrint()
    } else {
      data.formVisible = true
    }
  },
  async init() {
    const form = {
      vue_path: vuePath.value,
      tab_page: props.tabPage
    } as PrintSolutionGetByPathVo
    const { data: res } = await listByPath(form)
    if (res.isSuccess) {
      data.printSolutionList = res.data
      data.modeList = data.printSolutionList.map((item) => item.solution_name)
    }
    if (!data.mode && data.modeList.length === 0) {
      hookComponent.$message({
        type: 'error',
        content: `${ i18n.global.t('system.hiprint.emptyScheme') }`
      })
      method.handleClose()
    } else {
      data.mode = data.printSolutionList[0].solution_name
      method.handleChangeMode()
    }
  },
  handleChangeMode() {
    const option = data.printSolutionList.filter((item) => item.solution_name === data.mode)
    if (option.length > 0) {
      data.panel = JSON.parse(option[0].config_json)
    } else {
      data.panel = {}
    }
    method.initTemplate()
  },
  handleClose() {
    data.formVisible = false
  },
  handleOpen() {
    method.init()
  },
  handlePrint() {
    data.hiprintTemplate.print(
      props.form,
      {},
      {
        callback: () => {
          // data.waitShowPrinter = false
        }
      }
    )
  }
})
const vuePath = computed(() => {
  const vuePath = routerInfo.currentRoute.value.name?.toString() as string
  return vuePath
})

defineExpose({
  method
})
</script>
<style scoped lang="less">
.center-btn-box {
  margin-bottom: 5px;
  display: flex;
  justify-content: space-between;
}

.center-box {
  height: calc(100vh - 83px);
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
  height: calc(100vh - 83px);
  border: 2px solid #e8e8e8;
  box-sizing: border-box;
  border-radius: 2px;
  padding: 0 5px;
  display: flex;
  justify-content: center;
  /* 水平居中对齐 */
  margin-top: 5px;
}

.right-top-box {
  display: flex;
  justify-content: flex-end;
}

.right-box {
  height: calc(100vh - 83px);
  border: 2px solid #e8e8e8;
  box-sizing: border-box;
  border-radius: 2px;
  padding: 0 5px;
  display: flex;
  justify-content: center;
  /* 水平居中对齐 */
  margin-top: 5px;
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
