<template>
  <div class="approval-flow fd-nav-content">
    <div class="zoom flex">
      <div :class="'zoom-out' + (data.nowVal === 50 ? ' disabled' : '')" @click="method.zoomSize(1)"></div>
      <div class="zoomFont">{{ data.nowVal }}%</div>
      <div :class="'zoom-in' + (data.nowVal === 300 ? ' disabled' : '')" @click="method.zoomSize(2)"></div>
    </div>
    <div id="dingflow-design" class="dingflow-design">
      <div id="box-scale" class="box-scale" :style="'transform: scale(' + data.nowVal / 100 + '); transform-origin: 50% 0px 0px;'">
        <nodeWrap :node-config="data.nodeConfig" :table-name="tableName"></nodeWrap>
        <div class="end-node">
          <div class="end-node-circle"></div>
          <div class="end-node-text">流程结束</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { reactive, onMounted, nextTick } from 'vue'
import nodeWrap from './components/nodeWrap.vue'

defineProps<{
  // nodeConfig: any
  tableName: any
}>()

const data = reactive({
  tipList: [] as any,
  nowVal: 100 as number, // 缩放比例
  nodeConfig: {
    error: false,
    type: 1,
    id: 0,
    guid: 'cf1ed102-13a8-0b32-c7a3-bc8e6a70eb36',
    guidFater: '',
    nodeName: '使用部门',
    bFlowNotice: '',
    bFlowUser: '',
    form: {
      detailListBFlowDetail: [
        {
          id: 3334,
          detailList: [
            {
              id: 3634,
              bFlowID: 3279,
              bFlowDetailID: 3334,
              hrEmployeeID: 69,
              eNum: '002',
              eName: '大燕',
              valid: '是',
              guid: '70b7778b-3d3e-b923-eacf-4f81e09802b0'
            }
          ],
          bFlowID: 3279,
          approveLevel: 0,
          nodeName: '2022',
          passCondition: '1',
          mode: 1,
          origIDForMode: 0,
          nameForMode: '',
          guid: '70b7778b-3d3e-b923-eacf-4f81e09802b0',
          valid: '',
          rName: '',
          dName: '',
          modeStr: '指定人员'
        },
        {
          id: 3336,
          detailList: [
            {
              id: 3633,
              bFlowID: 3279,
              bFlowDetailID: 3336,
              hrEmployeeID: 52,
              eNum: '123',
              eName: '孟小翰',
              valid: '是',
              guid: 'dd82ee45-91d2-ce04-2270-6f75951aceb7'
            }
          ],
          bFlowID: 3279,
          approveLevel: 0,
          nodeName: '其他',
          passCondition: '1',
          mode: 1,
          origIDForMode: 0,
          nameForMode: '',
          guid: 'dd82ee45-91d2-ce04-2270-6f75951aceb7',
          valid: '',
          rName: '',
          dName: '',
          modeStr: '指定人员'
        }
      ],
      detailListBFlowNotice: [],
      detailListBFlowUser: [],
      detailListBFlowLink: [
        {
          id: 3776,
          detailListBFlowFilterScheme: [],
          bFlowID: 3279,
          beforeID: 3334,
          afterID: -1,
          beforeGUID: '70b7778b-3d3e-b923-eacf-4f81e09802b0',
          afterGUID: '',
          guid: '83706c24-0c57-4738-b308-432e4f6c231d',
          sort: 1
        },
        {
          id: 3777,
          detailListBFlowFilterScheme: [],
          bFlowID: 3279,
          beforeID: 3336,
          afterID: -1,
          beforeGUID: 'dd82ee45-91d2-ce04-2270-6f75951aceb7',
          afterGUID: '',
          guid: '9f317b42-11b3-4e35-843d-97dbad71263d',
          sort: 2
        },
        {
          id: 3778,
          detailListBFlowFilterScheme: [
            {
              id: 1116,
              ctrlType: 'string',
              bFlowID: 3279,
              bFlowFilterSetID: 127,
              conditionGroup: '1212-',
              colLabel: '编号',
              colName: 'Num',
              compare: '包含',
              content: '2022',
              sort: 0,
              formulas: '',
              assertMode: '存在',
              tableName: 'SOffer a left join BCustomer b on a.BCustomerID = b.ID',
              guid: '62aa6fd4-aaee-4e05-bccd-f0427a34d830',
              bFlowLinkID: 3778,
              schemeName: '2022',
              ifOther: '否'
            }
          ],
          bFlowID: 3279,
          beforeID: 0,
          afterID: 3334,
          beforeGUID: '',
          afterGUID: '70b7778b-3d3e-b923-eacf-4f81e09802b0',
          guid: '62aa6fd4-aaee-4e05-bccd-f0427a34d830',
          sort: 3
        },
        {
          id: 3779,
          detailListBFlowFilterScheme: [
            {
              id: 1117,
              ctrlType: null,
              bFlowID: 3279,
              bFlowFilterSetID: 0,
              conditionGroup: '2012-',
              colLabel: '',
              colName: '',
              compare: '',
              content: '',
              sort: 0,
              formulas: '',
              assertMode: '存在',
              tableName: '',
              guid: '8a7e261b-a8c8-4ca7-b1de-badae68d5b1a',
              bFlowLinkID: 3779,
              schemeName: '其他',
              ifOther: '是'
            }
          ],
          bFlowID: 3279,
          beforeID: 0,
          afterID: 3336,
          beforeGUID: '',
          afterGUID: 'dd82ee45-91d2-ce04-2270-6f75951aceb7',
          guid: '8a7e261b-a8c8-4ca7-b1de-badae68d5b1a',
          sort: 4
        }
      ],
      id: 3279,
      writer: '管理员',
      writerID: 3,
      writeDate: '2023-02-02 16:00:39',
      locker: '',
      lockerID: 0,
      lockDate: '1900-01-01 00:00:00',
      approveState: '未发起',
      bAuthorityName: 'SOffer',
      alText: '销售报价单',
      name: '其他',
      memo: '',
      valid: '是',
      origID: 0,
      fuZeRen: '大燕',
      fuZeRenID: 69,
      menuArgs: '',
      gongsi: '',
      priorityLevel: 0,
      bFlowMainID: 0
    },
    childNode: {
      nodeName: '路由',
      type: 4,
      nodeId: 'conditionID',
      childNode: null,
      form: 0,
      guid: '564d8a6a-390f-cdc6-321c-1502cbbe21fc',
      conditionNodes: [
        {
          nodeName: '条件',
          error: false,
          type: 4,
          id: 0,
          nodeId: 'conditionNode',
          conditionList: [
            {
              conditionChildrenNodes: [
                {
                  schemeName: '2022',
                  conditionGroup: '1212-',
                  bFlowFilterSetID: 127,
                  ctrlType: 'string',
                  colLabel: '编号',
                  colName: 'Num',
                  compare: '包含',
                  content: '2022',
                  sort: 0,
                  formulas: '',
                  assertMode: '存在',
                  tableName: 'SOffer a left join BCustomer b on a.BCustomerID = b.ID',
                  guid: '',
                  ifOther: '否'
                }
              ],
              schemeName: '2022',
              conditionGroup: '1212-'
            }
          ],
          childNode: {
            nodeName: '审批节点',
            error: false,
            type: 2,
            id: 0,
            nodeId: 'approvalID',
            childNode: null,
            guid: '70b7778b-3d3e-b923-eacf-4f81e09802b0',
            formJieDian: {
              id: 3334,
              detailList: [
                {
                  id: 3634,
                  bFlowID: 3279,
                  bFlowDetailID: 3334,
                  hrEmployeeID: 69,
                  eNum: '002',
                  eName: '大燕',
                  valid: '是',
                  guid: '70b7778b-3d3e-b923-eacf-4f81e09802b0'
                }
              ],
              bFlowID: 3279,
              approveLevel: 0,
              nodeName: '2022',
              passCondition: '1',
              mode: 1,
              origIDForMode: 0,
              nameForMode: '',
              guid: '70b7778b-3d3e-b923-eacf-4f81e09802b0',
              valid: '',
              rName: '',
              dName: '',
              modeStr: '指定人员'
            }
          },
          form: 0,
          to: 3334,
          guid: 'e33859ea-27f3-a5ca-7bca-f2917eb4ff78',
          guidLuYou: '564d8a6a-390f-cdc6-321c-1502cbbe21fc'
        },
        {
          nodeName: '条件',
          error: false,
          type: 4,
          id: 0,
          nodeId: 'conditionNode',
          conditionList: [
            {
              conditionChildrenNodes: [
                {
                  schemeName: '其他',
                  conditionGroup: '2012-',
                  bFlowFilterSetID: 0,
                  ctrlType: null,
                  colLabel: '',
                  colName: '',
                  compare: '',
                  content: '',
                  sort: 0,
                  formulas: '',
                  assertMode: '存在',
                  tableName: '',
                  guid: '',
                  ifOther: '是'
                }
              ],
              schemeName: '其他',
              conditionGroup: '2012-'
            }
          ],
          childNode: {
            nodeName: '审批节点',
            error: false,
            type: 2,
            id: 0,
            nodeId: 'approvalID',
            childNode: null,
            guid: 'dd82ee45-91d2-ce04-2270-6f75951aceb7',
            formJieDian: {
              id: 3336,
              detailList: [
                {
                  id: 3633,
                  bFlowID: 3279,
                  bFlowDetailID: 3336,
                  hrEmployeeID: 52,
                  eNum: '123',
                  eName: '孟小翰',
                  valid: '是',
                  guid: 'dd82ee45-91d2-ce04-2270-6f75951aceb7'
                }
              ],
              bFlowID: 3279,
              approveLevel: 0,
              nodeName: '其他',
              passCondition: '1',
              mode: 1,
              origIDForMode: 0,
              nameForMode: '',
              guid: 'dd82ee45-91d2-ce04-2270-6f75951aceb7',
              valid: '',
              rName: '',
              dName: '',
              modeStr: '指定人员'
            }
          },
          form: 0,
          to: 3336,
          guid: '75d2de0e-3533-f367-6929-5bc126213b52',
          guidLuYou: '564d8a6a-390f-cdc6-321c-1502cbbe21fc'
        }
      ]
    }
  } as any
})

const method = reactive({
  reErr: (nodeData: any) => {
    if (nodeData.childNode) {
      if (nodeData.childNode.type === 1) {
        // 审批人
        if (nodeData.childNode.error) {
          data.tipList.push({
            name: nodeData.childNode.nodeName,
            type: '审核人'
          })
        }
        method.reErr(nodeData.childNode)
      } else if (nodeData.childNode.type === 2) {
        if (nodeData.childNode.error) {
          data.tipList.push({
            name: nodeData.childNode.nodeName,
            type: '抄送人'
          })
        }
        method.reErr(nodeData.childNode)
      } else if (nodeData.childNode.type === 3) {
        method.reErr(nodeData.childNode.childNode)
      } else if (nodeData.childNode.type === 4) {
        method.reErr(nodeData.childNode)
        for (let i = 0; i < nodeData.childNode.conditionNodes.length; i++) {
          if (nodeData.childNode.conditionNodes[i].error) {
            data.tipList.push({
              name: nodeData.childNode.conditionNodes[i].nodeName,
              type: '条件'
            })
          }
          method.reErr(nodeData.childNode.conditionNodes[i])
        }
      }
    } else {
      nodeData.childNode = null
    }
  },
  //  给数据加preId(递归)
  handleData: (nodeData: any) => {
    if (nodeData.childNode) {
      nodeData.childNode.preId = nodeData.nodeId
      method.handleData(nodeData.childNode)
    }
    return nodeData
  },
  zoomSize: (type: number) => {
    if (type === 1) {
      if (data.nowVal === 50) {
        return
      }
      data.nowVal -= 10
    } else {
      if (data.nowVal === 300) {
        return
      }
      data.nowVal += 10
    }
  }
})

onMounted(() => {
  nextTick(() => {
    window.addEventListener(
      'mousewheel',
      (event: any) => {
        if (event.ctrlKey === true || event.metaKey) {
          event.preventDefault()
        }
      },
      { passive: false }
    )

    // firefox
    window.addEventListener(
      'DOMMouseScroll',
      (event: any) => {
        if (event.ctrlKey === true || event.metaKey) {
          event.preventDefault()
        }
      },
      { passive: false }
    )

    // 通过altKey+鼠标滚轮控制nowVal的数值
    const DOM = document.getElementById('dingflow-design')

    if (DOM) {
      DOM.addEventListener('mousewheel', (e: any) => {
        if (e.ctrlKey) {
          if (e.wheelDelta > 0) {
            method.zoomSize(2)
          } else {
            method.zoomSize(1)
          }
        }
      })
    }
  })
})
</script>

<style lang="less">
.approval-flow {
  position: relative;
  .zoom {
    position: absolute;
    height: 30px;
    // width: 125px;
    top: 15px;
    right: 30px;
    z-index: 10;
    display: flex;

    .zoomFont {
      height: 30px;
      width: 45px;

      display: flex;
      justify-content: center;
      align-items: center;
    }

    .zoom-in,
    .zoom-out {
      width: 30px;
      height: 30px;
      background: #fff;
      color: #c1c1cd;
      cursor: pointer;
      background-size: 100%;
      background-repeat: no-repeat;
    }

    .zoom-out {
      background-image: url(./img/ZoomOut.png);
    }

    .zoom-out.disabled {
      opacity: 0.5;
    }

    .zoom-in {
      background-image: url(./img/ZoomIn.png);
    }

    .zoom-in.disabled {
      opacity: 0.5;
    }
  }
  .dingflow-design {
    .box-scale {
      transform: scale(1);
      display: inline-block;
      position: relative;
      // width: calc(100%);
      min-width: 100%;
      padding: 54.5px 0;
      transform-origin: 0 0 0;
    }
    .branch-wrap {
      .branch-box-wrap {
        // margin-left: -240px;
        .auto-judge {
          position: relative;
          width: 250px;
          // min-height: 100px;
          background: #fff;
          border-radius: 4px;
          cursor: pointer;

          .title-wrapper {
            position: relative;
            font-size: 12px;
            color: #333;
            text-align: center;
            height: 38px;
            line-height: 38px;
            // padding: 0 13px 0 0px;
            border-bottom: 1px solid #ebebeb;
          }
        }
      }
    }
    .node-wrap-box {
      display: inline-flex;
      flex-direction: column;
      position: relative;
      width: 200px;
      min-height: 72px;
      background: #fff;
      border-radius: 4px;
      cursor: pointer;
    }
    .node-wrap-box:after {
      pointer-events: none;
      content: '';
      position: absolute;
      top: 0;
      bottom: 0;
      left: 0;
      right: 0;
      z-index: 2;
      border-radius: 4px;
      border: 1px solid transparent;
      transition: all 0.1s cubic-bezier(0.645, 0.045, 0.355, 1);
      box-shadow: 0 2px 5px 0 rgba(0, 0, 0, 0.1);
    }
    .end-node {
      border-radius: 50%;
      font-size: 14px;
      color: rgba(25, 31, 37, 0.4);
      text-align: left;
    }
    .end-node .end-node-circle {
      width: 10px;
      height: 10px;
      margin: auto;
      border-radius: 50%;
      background: #dbdcdc;
    }
    .end-node .end-node-text {
      margin-top: 5px;
      text-align: center;
    }
  }
}
.fd-nav-content {
  width: calc(100%);
  height: calc(100%);
  position: relative;
}
.dingflow-design {
  width: calc(100%);
  height: calc(100%);
  background-color: #f5f8ff;
  overflow: auto;
  position: absolute;
}
.dingflow-design .editable-title {
  font-size: 14px;
  // padding-left: 18px;
  width: 100%;
}
.dingflow-design .ie-polyfill-container {
  display: -ms-grid;
  -ms-grid-columns: min-content;
}

.dingflow-design .node-wrap {
  display: inline-flex;
  flex-direction: column;
  -webkit-box-pack: start;
  -ms-flex-pack: start;
  justify-content: flex-start;
  -webkit-box-align: center;
  -ms-flex-align: center;
  align-items: center;
  -ms-flex-wrap: wrap;
  flex-wrap: wrap;
  -webkit-box-flex: 1;
  -ms-flex-positive: 1;
  // width: 100%;
  // padding: 0 50px;
  position: relative;
}

.dingflow-design .branch-wrap,
.dingflow-design .node-wrap {
  display: inline-flex;
  width: calc(100%);
}

.dingflow-design .branch-box-wrap {
  display: flex;
  -webkit-box-orient: vertical;
  -webkit-box-direction: normal;
  -ms-flex-direction: column;
  flex-direction: column;
  -ms-flex-wrap: wrap;
  flex-wrap: wrap;
  -webkit-box-align: center;
  -ms-flex-align: center;
  align-items: center;
  min-height: 270px;
  width: calc(100%);
  -ms-flex-negative: 0;
  flex-shrink: 0;
}

.dingflow-design .branch-box {
  display: flex;
  overflow: visible;
  min-height: 180px;
  height: auto;
  border-bottom: 1px solid #070707;
  border-top: 1px solid #070707;
  position: relative;
  margin-top: 15px;
  background-color: #f5f8ff;
}

.dingflow-design .branch-box .col-box:before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 0;
  margin: auto;
  width: 1px;
  height: 100%;
  background-color: #070707;
}

.dingflow-design .add-branch {
  border: none;
  outline: none;
  user-select: none;
  justify-content: center;
  font-size: 12px;
  padding: 0 10px;
  height: 30px;
  line-height: 30px;
  border-radius: 15px;
  color: #4880ff;
  background: #fff;
  box-shadow: 0 2px 4px 0 rgba(0, 0, 0, 0.1);
  position: absolute;
  top: -16px;
  left: 50%;
  transform: translateX(-50%);
  transform-origin: center center;
  cursor: pointer;
  z-index: 1;
  display: inline-flex;
  align-items: center;
  -webkit-transition: all 0.3s cubic-bezier(0.645, 0.045, 0.355, 1);
  transition: all 0.3s cubic-bezier(0.645, 0.045, 0.355, 1);
}

.dingflow-design .add-branch:hover {
  transform: translateX(-50%) scale(1.1);
  box-shadow: 0 8px 16px 0 rgba(0, 0, 0, 0.1);
}

.dingflow-design .add-branch:active {
  transform: translateX(-50%);
  box-shadow: none;
}

.dingflow-design .col-box {
  display: inline-flex;
  -webkit-box-orient: vertical;
  -webkit-box-direction: normal;
  flex-direction: column;
  -webkit-box-align: center;
  align-items: center;
  position: relative;
}

.dingflow-design .condition-node {
  min-height: 220px;
}

.dingflow-design .condition-node,
.dingflow-design .condition-node-box {
  display: inline-flex;
  -webkit-box-orient: vertical;
  -webkit-box-direction: normal;
  flex-direction: column;
  -webkit-box-flex: 1;
}

.dingflow-design .condition-node-box {
  padding-top: 30px;
  padding-right: 50px;
  padding-left: 50px;
  -webkit-box-pack: center;
  justify-content: center;
  -webkit-box-align: center;
  align-items: center;
  flex-grow: 1;
  position: relative;
}

.dingflow-design .condition-node-box:before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  margin: auto;
  width: 1px;
  height: 100%;
  background-color: #070707;
}

.dingflow-design .auto-judge:after {
  pointer-events: none;
  content: '';
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  z-index: 2;
  border-radius: 4px;
  border: 1px solid transparent;
  transition: all 0.1s cubic-bezier(0.645, 0.045, 0.355, 1);
  box-shadow: 0 2px 5px 0 rgba(0, 0, 0, 0.1);
}

.dingflow-design .auto-judge.active:after,
.dingflow-design .auto-judge:active:after,
.dingflow-design .auto-judge:hover:after {
  border: 1px solid #4880ff;
  box-shadow: 0 0 6px 0 rgba(50, 150, 250, 0.3);
}

.dingflow-design .auto-judge.active .close,
.dingflow-design .auto-judge:active .close,
.dingflow-design .auto-judge:hover .close {
  display: block;
}

.dingflow-design .auto-judge.error:after {
  border: 1px solid #f25643;
  box-shadow: 0 2px 5px 0 rgba(0, 0, 0, 0.1);
}

.dingflow-design .auto-judge .title-wrapper .editable-title {
  display: inline-block;
  max-width: 250px;
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
  font-size: 16px;
  background-color: #d88b0a;
  color: white;
}

.dingflow-design .auto-judge .title-wrapper .priority-title {
  display: inline-block;
  float: right;
  margin-right: 35px;
  color: #828282;
}

.dingflow-design .auto-judge .title-wrapper .priority-button {
  display: inline-block;
  height: 28px;
  float: right;
  margin-right: 10px;
  border: 1px solid rgba(0, 0, 0, 0.15);
  border-radius: 4px;
  color: rgba(0, 0, 0, 0.65);
}

.dingflow-design .auto-judge .title-wrapper .priority-button div {
  display: inline-block;
}

.dingflow-design .auto-judge .title-wrapper .priority-button .priority-content {
  padding: 0 8px;
  font-size: 12px;
  line-height: 28px;
  border-left: 1px solid rgba(0, 0, 0, 0.15);
  border-right: 1px solid rgba(0, 0, 0, 0.15);
}

.dingflow-design .auto-judge .placeholder {
  color: #bfbfbf;
}

.dingflow-design .auto-judge .close {
  display: none;
  position: absolute;
  right: 22px;
  top: 8px;
  width: 20px;
  height: 20px;
  font-size: 14px;
  color: rgba(0, 0, 0, 0.25);
  border-radius: 50%;
  text-align: center;
  line-height: 20px;
  z-index: 2;
}

.dingflow-design .auto-judge .content {
  // height: calc(100% - 39px);
  // display: flex;
  // align-items: center;
  // max-width: calc(100% - 36px);
  // padding: 0 18px;
  // overflow: hidden;
  // text-overflow: ellipsis;
  // font-size: 14px;
  position: relative;
  font-size: 14px;
  padding: 20px 18px;
  /* padding-right: 30px */
  text-align: center;
}

.dingflow-design .auto-judge .sort-left,
.dingflow-design .auto-judge .sort-right {
  position: absolute;
  top: 0;
  bottom: 0;
  display: none;
  z-index: 1;
  color: rgba(0, 0, 0, 0.25);
}

.dingflow-design .auto-judge .sort-left {
  left: 4px;
  top: 33px;
  color: rgba(0, 0, 0, 0.5);
}

.dingflow-design .auto-judge .sort-right {
  right: 26px;
  top: 33px;
}

.dingflow-design .auto-judge:hover .sort-left,
.dingflow-design .auto-judge:hover .sort-right {
  display: flex;
  align-items: center;
}

.dingflow-design .add-node-btn {
  user-select: none;
  width: 240px;
  padding: 30px 0;
  display: flex;
  -webkit-box-pack: center;
  justify-content: center;
  flex-shrink: 0;
  -webkit-box-flex: 1;
  flex-grow: 1;
  //  margin-left: -55px;
}

.dingflow-design .add-node-btn .btn {
  outline: none;
  box-shadow: 0 2px 4px 0 rgba(0, 0, 0, 0.1);
  width: 30px;
  height: 30px;
  background: #4880ff;
  border-radius: 50%;
  position: relative;
  border: none;
  line-height: 30px;
  -webkit-transition: all 0.3s cubic-bezier(0.645, 0.045, 0.355, 1);
  transition: all 0.3s cubic-bezier(0.645, 0.045, 0.355, 1);
  //  margin-left: 36px;
}

.dingflow-design .add-node-btn .btn .iconfont {
  color: #fff;
  font-size: 16px;
}

.dingflow-design .add-node-btn .btn:hover {
  transform: scale(1.3);
  box-shadow: 0 13px 27px 0 rgba(0, 0, 0, 0.1);
}

.dingflow-design .add-node-btn .btn:active {
  transform: none;
  background: #1e83e9;
  box-shadow: 0 2px 4px 0 rgba(0, 0, 0, 0.1);
}

.top-left-cover-line {
  left: -1px;
}

.top-left-cover-line,
.top-right-cover-line {
  position: absolute;
  height: 3px;
  width: 50%;
  background-color: #f5f8ff;
  top: -2px;
}

.top-right-cover-line {
  right: -1px;
}

.bottom-left-cover-line {
  left: -1px;
}

.bottom-left-cover-line,
.bottom-right-cover-line {
  position: absolute;
  height: 3px;
  width: 50%;
  background-color: #f5f8ff;
  bottom: -2px;
}

.bottom-right-cover-line {
  right: -1px;
}

.node-wrap-box.active:after,
.node-wrap-box:active:after,
.node-wrap-box:hover:after {
  border: 1px solid #3296fa;
  box-shadow: 0 0 6px 0 rgba(50, 150, 250, 0.3);
}

.node-wrap-box.active .close,
.node-wrap-box:active .close,
.node-wrap-box:hover .close {
  display: block;
}

.node-wrap-box.error:after {
  border: 1px solid #f25643;
  box-shadow: 0 2px 5px 0 rgba(0, 0, 0, 0.1);
}

.node-wrap-box .title {
  position: relative;
  display: flex;
  align-items: center;
  // padding-left: 16px;
  //  padding-right: 30px;
  //  width: 100%;
  height: 40px;
  line-height: 40px;
  font-size: 16px;
  color: #fff;
  text-align: center;
  background: #0687a5;
  border-radius: 4px 4px 0 0;
}

.node-wrap-box .title .iconfont {
  font-size: 12px;
  margin-right: 5px;
}

.node-wrap-box .placeholder {
  color: #bfbfbf;
}

.node-wrap-box .close {
  display: none;
  position: absolute;
  right: 10px;
  top: 50%;
  transform: translateY(-50%);
  width: 20px;
  height: 20px;
  font-size: 14px;
  color: #fff;
  border-radius: 50%;
  text-align: center;
  line-height: 20px;
}

.node-wrap-box .content {
  position: relative;
  font-size: 14px;
  padding: 20px 18px;
  /* padding-right: 30px */
  text-align: center;
}

.node-wrap-box .content .text {
  overflow: hidden;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
}

.node-wrap-box .content .arrow {
  position: absolute;
  right: 10px;
  top: 50%;
  transform: translateY(-50%);
  width: 20px;
  height: 14px;
  font-size: 14px;
  color: #979797;
}

.start-node.node-wrap-box .content .text {
  display: block;
  white-space: nowrap;
}
</style>
