<template>
  <div class="add-node-btn-box">
    <div class="add-node-btn">
      <v-btn icon="mdi-plus" size="small"></v-btn>
      <!-- <el-popover v-model="data.visible" placement="right-start" popper-class="add-node-popover">
        <div v-if="tip === '审批节点' || tip === '条件'" class="add-node-popover-body">
          <div v-if="tip === '审批节点' || tip === '条件'" class="add-node-popover-item approver" @click="method.addType(2)">
            <div class="item-wrapper">
              <img src="../img/ShenPiRen.png" alt="" class="img-style" />
            </div>
            <p>审批人</p>
          </div>
          <div v-if="tip === '审批节点'" class="add-node-popover-item condition" @click="method.addType(4)">
            <div class="item-wrapper">
              <img src="../img/TiaoJian.png" alt="" class="img-style" />
            </div>
            <p>条件分支</p>
          </div>
        </div>
        <template #reference>
          <button class="btn" type="button">
            <i style="color: #fff" class="el-icon-plus"></i>
          </button>
        </template>
      </el-popover> -->
    </div>
  </div>
</template>

<script lang="ts" setup>
import { reactive } from 'vue'

const emit = defineEmits(['update:childNodeP'])

const props = defineProps<{
  childNodeP: any
  tip: any
}>()

const data = reactive({
  visible: false as boolean,
  parentObj: {} as any,
  selfChildNode: props.childNodeP as any,
  selfTip: props.tip as any
})

const method = reactive({
  guid() {
    let guid = ''
    for (let i = 1; i <= 32; i++) {
      const n = Math.floor(Math.random() * 16.0).toString(16)
      guid += n
      if (i === 8 || i === 12 || i === 16 || i === 20) guid += '-'
    }
    return guid
  },
  addType(type: number) {
    data.visible = false
    let NodeData: any
    if (type !== 4) {
      if (type === 2) {
        NodeData = {
          nodeName: '审批节点',
          error: false,
          type: 2,
          id: 0,
          nodeId: 'approvalID',
          childNode: data.selfChildNode,
          guid: this.guid(),
          // guidFater: this.nodeConfig.guid,
          formJieDian: {
            id: 0,
            detailList: [],
            bFlowID: 0,
            approveLevel: 0,
            nodeName: '',
            passCondition: '1',
            mode: 0,
            origIDForMode: 0,
            nameForMode: '',
            guid: 0,
            valid: '',
            rName: '',
            dName: '',
            modeStr: ''
          }
        }
      }
    } else {
      const guidLuYou: string = method.guid()

      NodeData = {
        nodeName: '路由',
        type: 4,
        nodeId: 'conditionID',
        childNode: data.selfChildNode,
        guid: guidLuYou,
        conditionNodes: [
          {
            nodeName: '条件',
            error: false,
            type: 4,
            id: 0,
            nodeId: 'conditionNode',
            conditionList: [],
            childNode: null,
            guid: this.guid(),
            guidLuYou
          },
          {
            nodeName: '条件',
            error: false,
            type: 4,
            id: 0,
            nodeId: 'conditionNode',
            conditionList: [],
            childNode: null,
            guid: this.guid(),
            guidLuYou
          }
        ]
      }
    }
    emit('update:childNodeP', NodeData)
  }
})
</script>

<style lang="less" scoped>
.add-node-btn-box {
  width: 240px;
  display: inline-flex;
  flex-shrink: 0;
  position: relative;
  &:first-child {
    margin-left: 16px;
  }
  &:before {
    content: '';
    position: absolute;
    top: 1px;
    left: 0px;
    right: 0;
    bottom: 0;
    z-index: -1;
    margin: auto;
    width: 1px;
    // height: 100%;
    background-color: #070707;
  }
}
.img-style {
  width: 36px;
}
.add-node-popover {
  padding: 14px 26px;
  .add-node-popover-body {
    display: flex;
    .add-node-popover-item {
      margin-right: 20px;
      text-align: center;
      cursor: pointer;
      &:last-child {
        margin-right: 0;
      }
      i {
        font-size: 36px;
      }
      p {
        color: #333;
        margin-top: 4px;
      }
    }
    .approver {
      i {
        color: #e6a23c;
      }
    }
    .condition {
      i {
        color: #67c23a;
      }
    }
    .notifier {
      i {
        color: #4880ff;
      }
    }
  }
}
</style>
