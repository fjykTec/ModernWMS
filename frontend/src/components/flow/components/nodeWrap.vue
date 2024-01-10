<template>
  <div>
    <!-- 每一行内容 -->
    <div v-if="data.selfNodeConfig.type !== 4" class="node-wrap">
      <!-- 每一个卡片 -->
      <div
        v-if="data.selfNodeConfig.type && data.selfNodeConfig.type !== 0"
        class="node-wrap-box"
        :class="(data.selfNodeConfig.type === 1 ? 'start-node' : '') + (data.selfNodeConfig.error ? 'active error' : '')"
      >
        <div @click="method.handleClickCard">
          <!-- 卡片头部 -->
          <div class="title" :style="'background: rgb(' + ['87, 106, 149', '6, 135, 165'][data.selfNodeConfig.type - 1] + ');'">
            <span class="editable-title">{{ data.selfNodeConfig.nodeName }}</span>
            <i v-if="data.selfNodeConfig.type !== 1" class="el-icon-close close" @click.stop="method.delNode()"></i>
          </div>
          <!-- 卡片正文内容 -->
          <div class="content">
            <div v-if="data.selfNodeConfig.type === 1" class="text">
              {{ '类型1' }}
            </div>
            <div v-if="data.selfNodeConfig.type === 2" class="text">
              {{ '类型2' }}
            </div>
            <i class="el-icon-arrow-right arrow"></i>
          </div>
          <!-- 报错提示 -->
          <div v-if="data.selfNodeConfig.error" class="error_tip">
            <i class="el-icon-warning" style="color: rgb(242, 86, 67)"></i>
          </div>
        </div>
      </div>
      <!-- 新增节点元素 -->
      <AddNodeComponent v-model:child-node-p="data.selfNodeConfig.childNode" :tip="'审批节点'" />
    </div>
    <!-- "条件分支" - 类型4 -->
    <div v-if="data.selfNodeConfig.type === 4" class="branch-wrap">
      <div class="branch-box-wrap">
        <div class="branch-box">
          <!-- 添加条件按钮 -->
          <el-button class="add-branch" @click="method.addTerm"> 添加条件 </el-button>
          <!-- 多个条件 -->
          <div v-for="(item, index) in data.selfNodeConfig.conditionNodes" :key="index" class="col-box">
            <div class="condition-node">
              <div class="condition-node-box">
                <div class="auto-judge" :class="item.error ? 'error active' : ''" @click="method.handleClickCard(index)">
                  <div class="title-wrapper">
                    <span class="editable-title">{{ item.nodeName }}</span>
                    <i class="el-icon-close close" @click.stop="method.delCardItem(index)"></i>
                  </div>
                  <div class="content">
                    <!-- <span class="placeholder"> 条件</span> -->
                    <div>
                      {{
                        data.selfNodeConfig.conditionNodes[index].conditionList.length > 0
                          ? data.selfNodeConfig.conditionNodes[index].conditionList[0].schemeName
                          : ''
                      }}
                    </div>
                    <!-- {{ item.id }} -->
                  </div>
                  <div v-if="item.error" class="error_tip">
                    <i class="el-icon-warning" style="color: rgb(242, 86, 67)"></i>
                  </div>
                </div>
                <!-- 添加审批人 -->
                <AddNodeComponent v-model:child-node-p="item.childNode" :tip="'条件'" />
              </div>
            </div>
            <!-- 递归遍历 -->
            <nodeWrap v-if="item.childNode && item.childNode" v-model:node-config="item.childNode" :table-name="tableName"></nodeWrap>
            <div v-if="index === 0" class="top-left-cover-line"></div>
            <div v-if="index === 0" class="bottom-left-cover-line"></div>
            <div v-if="index === data.selfNodeConfig.conditionNodes.length - 1" class="top-right-cover-line"></div>
            <div v-if="index === data.selfNodeConfig.conditionNodes.length - 1" class="bottom-right-cover-line"></div>
          </div>
        </div>
        <!-- 添加节点按钮 -->
        <AddNodeComponent v-model:child-node-p="data.selfNodeConfig.childNode" :node-config="data.selfNodeConfig" :tip="'条件'" />
      </div>
    </div>
    <!-- 递归本体 -->
    <nodeWrap
      v-if="data.selfNodeConfig.childNode && data.selfNodeConfig.childNode"
      v-model:node-config="data.selfNodeConfig.childNode"
      :table-name="tableName"
    ></nodeWrap>
  </div>
</template>

<script lang="ts" setup>
import { reactive, onMounted } from 'vue'
import AddNodeComponent from './addNode.vue'
import { hookComponent } from '@/components/system'

const emit = defineEmits(['update:nodeConfig'])

const props = defineProps<{
  nodeConfig: any
  tableName: any
}>()

const data = reactive({
  drawer: false as boolean,
  indexTiaojian: 0 as number,
  formTiaoJian: {
    schemeName: ''
  } as any,
  selfNodeConfig: props.nodeConfig,
  selfTableName: props.tableName
})

const method = reactive({
  // 生成guid
  guid: () => {
    let guid = ''
    for (let i = 1; i <= 32; i++) {
      const num = Math.floor(Math.random() * 16.0).toString(16)
      guid += num
      if (i === 8 || i === 12 || i === 16 || i === 20) guid += '-'
    }
    return guid
  },
  // 生成guid
  guidGroup: () => {
    let guid = ''
    for (let i = 1; i <= 4; i++) {
      const n = Math.floor(Math.random() * 3.0).toString(3)
      guid += n
      if (i === 4 || i === 10 || i === 16 || i === 20) guid += '-'
    }
    return guid
  },
  // 添加条件组
  addConditionGroup: (flag: any, item: any) => {
    if (!data.formTiaoJian.schemeName) {
      hookComponent.$message({
        type: 'error',
        content: '请输入条件名称'
      })
      return
    }

    // todo 校验
    const result = false

    if (result) {
      if (flag === '1') {
        item.conditionChildrenNodes.push({
          schemeName: item.schemeName,
          conditionGroup: item.conditionGroup,
          ctrlType: '',
          colLabel: '',
          bFlowFilterSetID: 0,
          colName: '',
          compare: '',
          content: '',
          sort: 0,
          formulas: '',
          assertMode: '存在',
          tableName: '',
          guid: '',
          ifOther: '否'
        })
      }
      data.selfNodeConfig.conditionNodes[data.indexTiaojian].conditionList.push({
        conditionChildrenNodes: [],
        conditionGroup: method.guidGroup(),
        schemeName: data.formTiaoJian.schemeName
      })
    }
  },
  // 删除条件组
  removeCondition: (conditonflag: string, index: number, ind: number) => {
    if (conditonflag === 'group') {
      data.selfNodeConfig.conditionNodes[data.indexTiaojian].conditionList.splice(index, 1)
      return
    }
    data.selfNodeConfig.conditionNodes[data.indexTiaojian].conditionList[index].conditionChildrenNodes.splice(ind, 1)
  },
  handleClickCard: (index: number) => {
    console.log('点击卡片')
  },
  // 删除节点
  delNode: () => {
    // this.$emit('update:nodeConfig', this.nodeConfig.childNode)
  },
  // 添加条件
  addTerm: () => {
    data.selfNodeConfig.conditionNodes.push({
      nodeName: '条件',
      error: false,
      type: 4,
      id: 0,
      conditionList: [],
      childNode: null,
      guid: method.guid(),
      guidLuYou: data.selfNodeConfig.guid
    })

    emit('update:nodeConfig', data.selfNodeConfig)
  },
  // 删除条件
  delCardItem: (index: number) => {
    data.selfNodeConfig.conditionNodes.splice(index, 1)
    emit('update:nodeConfig', data.selfNodeConfig)

    // 如果只有一个条件，删除条件的同时，删除条件的子节点
    if (data.selfNodeConfig.conditionNodes.length === 1) {
      if (data.selfNodeConfig.childNode) {
        if (data.selfNodeConfig.conditionNodes[0].childNode) {
          method.reData(data.selfNodeConfig.conditionNodes[0].childNode, data.selfNodeConfig.childNode)
        } else {
          data.selfNodeConfig.conditionNodes[0].childNode = data.selfNodeConfig.childNode
        }
      }
      emit('update:nodeConfig', data.selfNodeConfig.conditionNodes[0].childNode)
    }
  },
  reData: (NodeData: any, addData: any) => {
    if (!NodeData.childNode) {
      NodeData.childNode = addData
    } else {
      method.reData(NodeData.childNode, addData)
    }
  }
})

onMounted(() => {
  console.log(data.selfNodeConfig)
})
</script>

<style scoped lang="less">
//弹框
.set_promoter {
  .el-drawer__header {
    margin-bottom: 0;
    padding: 0 24px;
    border-bottom: 1px solid #ebebeb;

    .title {
      height: 64px;
      line-height: 64px;
      font-size: 20px;
      color: #333;
      justify-content: flex-start;

      i {
        margin-left: 8px;
        font-size: 30px;
      }
    }
  }

  .el-drawer__body {
    overflow-y: auto;
    max-height: calc(100% - 64px);
    padding-bottom: 80px;
  }

  .drawer-content {
    padding: 24px;

    .el-radio {
      margin-bottom: 16px;

      .mark {
        color: #828282;
      }

      &.is-checked {
        .mark {
          color: #4880ff;
        }
      }

      &:last-child {
        margin-bottom: 0;
      }
    }
  }

  .drawer-content-condition {
    padding: 24px 0;

    .condition-conent-group {
      border-bottom: solid 1px #ebebeb;
      padding-bottom: 20px;

      .condition-group-title {
        padding: 0 24px;
        background: #f7f8fa;
        height: 48px;
        border-top: solid 1px #ebebeb;
        border-bottom: solid 1px #ebebeb;
        margin-bottom: 12px;
        display: flex;
        justify-content: space-between;
        align-items: center;
      }

      .condition-group-select {
        padding: 10px 24px 0px 24px;
        display: flex;
        border-bottom: 1px dashed #c2bfbf;
      }

      .remove {
        font-size: 15px;
        color: #828282;

        &:hover {
          cursor: pointer;
          color: red;
        }
      }

      .removedetail {
        font-size: 25px;
        color: #828282;

        &:hover {
          cursor: pointer;
          color: red;
        }
      }

      .mg-bot-10 {
        margin-bottom: 10px;
      }

      .conditionbtn {
        .el-button {
          border: 1px dashed #4880ff;
          width: 110px;
        }
      }
    }

    .conditionbtn {
      padding: 0 24px;
      margin-top: 26px;

      .el-button {
        border: solid 1px #4880ff;
        color: #4880ff;
        width: 123px;
      }
    }
  }

  .set_promoter_footer {
    height: 80px;
    position: absolute;
    bottom: 0;
    left: 0;
    padding: 24px;
    justify-content: flex-end;
    background: #fff;
    width: 100%;
    border-top: 1px solid #ebebeb;
    z-index: 2;
  }

  .icon-icon_explain {
    color: #bfbfbf;
  }
}
.error_tip {
  position: absolute;
  top: 36px;
  right: 0px;
  transform: translate(150%, 0px);

  i {
    font-size: 24px;
  }
}
</style>
