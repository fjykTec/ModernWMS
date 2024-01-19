<template>
  <v-dialog v-model="isShow" width="70%" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('router.sideBar.commodityManagement')}`"></v-toolbar>
        <v-card-text>
          <v-row>
            <v-col cols="3">
              <div class="mainForm" :style="{ height: mainFormHeight }">
                <v-form ref="formRef">
                  <v-text-field
                    v-model="data.form.spu_code"
                    :rules="data.rules.spu_code"
                    :label="$t('base.commodityManagement.spu_code')"
                    variant="outlined"
                    density="compact"
                    clearable
                    class="mb-4"
                  ></v-text-field>
                  <v-text-field
                    v-model="data.form.spu_name"
                    :rules="data.rules.spu_name"
                    :label="$t('base.commodityManagement.spu_name')"
                    variant="outlined"
                    density="compact"
                    clearable
                    class="mb-4"
                  ></v-text-field>
                  <v-select
                    v-model="data.form.category_name"
                    :items="data.combobox.category_name"
                    item-title="label"
                    item-value="label"
                    :rules="data.rules.category_name"
                    :label="$t('base.commodityManagement.category_name')"
                    variant="outlined"
                    density="compact"
                    class="mb-4"
                    clearable
                    @update:model-value="method.categoryNameChange"
                  ></v-select>
                  <v-text-field
                    v-model="data.form.spu_description"
                    :rules="data.rules.spu_description"
                    :label="$t('base.commodityManagement.spu_description')"
                    variant="outlined"
                    density="compact"
                    clearable
                    class="mb-4"
                  ></v-text-field>
                  <!-- <v-text-field
                    v-model="data.form.bar_code"
                    :rules="data.rules.bar_code"
                    :label="$t('base.commodityManagement.bar_code')"
                    variant="outlined"
                    density="compact"
                    clearable
                    class="mb-4"
                  ></v-text-field> -->
                  <v-select
                    v-model="data.form.supplier_name"
                    :items="data.combobox.supplier_name"
                    :rules="data.rules.supplier_name"
                    item-title="label"
                    item-value="label"
                    :label="$t('base.commodityManagement.supplier_name')"
                    variant="outlined"
                    density="compact"
                    class="mb-4"
                    clearable
                    @update:model-value="method.supplierNameChange"
                  ></v-select>
                  <v-text-field
                    v-model="data.form.brand"
                    :rules="data.rules.brand"
                    :label="$t('base.commodityManagement.brand')"
                    variant="outlined"
                    density="compact"
                    clearable
                    class="mb-4"
                  ></v-text-field>
                  <v-select
                    v-model="data.form.length_unit"
                    :items="data.combobox.length_unit"
                    item-title="label"
                    item-value="value"
                    :rules="data.rules.length_unit"
                    :label="$t('base.commodityManagement.length_unit')"
                    variant="outlined"
                    density="compact"
                    class="mb-4"
                    clearable
                  ></v-select>
                  <v-select
                    v-model="data.form.volume_unit"
                    :items="data.combobox.volume_unit"
                    item-title="label"
                    item-value="value"
                    :rules="data.rules.volume_unit"
                    :label="$t('base.commodityManagement.volume_unit')"
                    variant="outlined"
                    density="compact"
                    class="mb-4"
                    clearable
                  ></v-select>
                  <v-select
                    v-model="data.form.weight_unit"
                    :items="data.combobox.weight_unit"
                    item-title="label"
                    item-value="value"
                    :rules="data.rules.weight_unit"
                    :label="$t('base.commodityManagement.weight_unit')"
                    variant="outlined"
                    density="compact"
                    class="mb-4"
                    clearable
                  ></v-select>
                </v-form>
              </div>
            </v-col>
            <v-col cols="9">
              <div class="dataTable">
                <div class="toolbar">
                  <tooltip-btn
                    icon="mdi-plus"
                    :tooltip-text="$t('system.page.insertOneRow')"
                    size="x-small"
                    @click="method.insertOneRow"
                  ></tooltip-btn>
                  <tooltip-btn
                    icon="mdi-export-variant"
                    :tooltip-text="$t('system.page.export')"
                    size="x-small"
                    @click="method.exportTable"
                  ></tooltip-btn>
                </div>
                <vxe-table
                  ref="xTable"
                  keep-source
                  :column-config="{ minWidth: '100px' }"
                  :data="data.form.detailList"
                  :height="SYSTEM_HEIGHT.SELECT_TABLE"
                  align="center"
                  :edit-rules="data.validRules"
                  :edit-config="{ trigger: 'click', mode: 'cell' }"
                  :mouse-config="{ selected: true }"
                  :keyboard-config="{ isArrow: true, isDel: true, isEnter: true, isTab: true, isEdit: true, isChecked: true }"
                >
                  <template #empty>
                    {{ i18n.global.t('system.page.noData') }}
                  </template>
                  <vxe-column type="seq" width="60"></vxe-column>
                  <vxe-column field="sku_code" :title="$t('base.commodityManagement.sku_code')" :edit-render="{ autofocus: '.vxe-input--inner' }">
                    <template #edit="{ row }">
                      <vxe-input v-model="row.sku_code" type="text"></vxe-input>
                    </template>
                  </vxe-column>
                  <vxe-column field="sku_name" :title="$t('base.commodityManagement.sku_name')" :edit-render="{ autofocus: '.vxe-input--inner' }">
                    <template #edit="{ row }">
                      <vxe-input v-model="row.sku_name" type="text"></vxe-input>
                    </template>
                  </vxe-column>
                  <!-- <vxe-column field="supplier_name" :title="$t('base.commodityManagement.supplier_name')"></vxe-column> -->
                  <!-- <vxe-column field="brand" :title="$t('base.commodityManagement.brand')"></vxe-column> -->
                  <vxe-column field="unit" :title="$t('base.commodityManagement.unit')" :edit-render="{ autofocus: '.vxe-input--inner' }">
                    <template #edit="{ row }">
                      <vxe-input v-model="row.unit" type="text"></vxe-input>
                    </template>
                  </vxe-column>
                  <vxe-column field="bar_code" :title="$t('base.commodityManagement.bar_code')" :edit-render="{ autofocus: '.vxe-input--inner' }">
                    <template #edit="{ row }">
                      <vxe-input v-model="row.bar_code" type="text"></vxe-input>
                    </template>
                  </vxe-column>
                  <vxe-column field="weight" :title="$t('base.commodityManagement.weight')" :edit-render="{ autofocus: '.vxe-input--inner' }">
                    <template #edit="{ row }">
                      <vxe-input v-model="row.weight" type="text"></vxe-input>
                    </template>
                  </vxe-column>
                  <vxe-column field="lenght" :title="$t('base.commodityManagement.lenght')" :edit-render="{ autofocus: '.vxe-input--inner' }">
                    <template #edit="{ row }">
                      <vxe-input v-model="row.lenght" type="text"></vxe-input>
                    </template>
                  </vxe-column>
                  <vxe-column field="width" :title="$t('base.commodityManagement.width')" :edit-render="{ autofocus: '.vxe-input--inner' }">
                    <template #edit="{ row }">
                      <vxe-input v-model="row.width" type="text"></vxe-input>
                    </template>
                  </vxe-column>
                  <vxe-column field="height" :title="$t('base.commodityManagement.height')" :edit-render="{ autofocus: '.vxe-input--inner' }">
                    <template #edit="{ row }">
                      <vxe-input v-model="row.height" type="text"></vxe-input>
                    </template>
                  </vxe-column>
                  <vxe-column field="volume" :title="$t('base.commodityManagement.volume')"> </vxe-column>
                  <vxe-column field="cost" :title="$t('base.commodityManagement.cost')" :edit-render="{ autofocus: '.vxe-input--inner' }">
                    <template #edit="{ row }">
                      <vxe-input v-model="row.cost" type="text"></vxe-input>
                    </template>
                  </vxe-column>
                  <vxe-column field="price" :title="$t('base.commodityManagement.price')" :edit-render="{ autofocus: '.vxe-input--inner' }">
                    <template #edit="{ row }">
                      <vxe-input v-model="row.price" type="text"></vxe-input>
                    </template>
                  </vxe-column>
                  <vxe-column field="operate" :title="$t('system.page.operate')" width="100" :resizable="false" show-overflow>
                    <template #default="{ row }">
                      <tooltip-btn
                        :flat="true"
                        icon="mdi-delete-outline"
                        :tooltip-text="$t('system.page.delete')"
                        :icon-color="errorColor"
                        @click="method.deleteRow(row)"
                      ></tooltip-btn>
                    </template>
                  </vxe-column>
                </vxe-table>
              </div>
            </v-col>
          </v-row>
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="method.closeDialog">{{ $t('system.page.close') }}</v-btn>
          <v-btn color="primary" variant="text" @click="method.submit">{{ $t('system.page.submit') }}</v-btn>
          <v-btn color="primary" variant="text" @click="method.print">{{ $t('system.page.print') }}</v-btn>
        </v-card-actions>
      </v-card>
      <hprintDialog ref="hprintDialogRef" :form="data.form" :tab-page="'print_page_detail'" />
    </template>
  </v-dialog>
</template>

<script lang="ts" setup>
import { reactive, computed, ref, watch } from 'vue'
import { CommodityVO, CommodityDetailVO } from '@/types/Base/CommodityManagement'
import i18n from '@/languages/i18n'
import { hookComponent } from '@/components/system/index'
import { addSpu, updateSpu } from '@/api/base/commodityManagementSetting'
import { getCategoryAll } from '@/api/base/commodityCategorySetting'
import { getSupplierAll } from '@/api/base/supplier'
import { CategoryVO } from '@/types/Base/CommodityCategorySetting'
import { SupplierVO } from '@/types/Base/Supplier'
import { computedSelectTableSearchHeight, SYSTEM_HEIGHT, errorColor } from '@/constant/style'
import tooltipBtn from '@/components/tooltip-btn.vue'
import { removeArrayNull, removeObjectNull } from '@/utils/common'
import { StringLength } from '@/utils/dataVerification/formRule'
import { isDecimal } from '@/utils/dataVerification/tableRule'
import { exportData } from '@/utils/exportTable'
import hprintDialog from '@/components/hiprint/hiprintFast.vue'

const formRef = ref()
const emit = defineEmits(['close', 'saveSuccess'])
const xTable = ref()
const hprintDialogRef = ref()
const props = defineProps<{
  showDialog: boolean
  form: CommodityVO
}>()

const isShow = computed(() => props.showDialog)

const dialogTitle = computed(() => {
  if (props.form.id && props.form.id > 0) {
    return 'update'
  }
  return 'add'
})

const data = reactive({
  form: ref<CommodityVO>({
    id: 0,
    spu_code: '',
    spu_name: '',
    category_id: 0,
    category_name: '',
    spu_description: '',
    supplier_id: 0,
    supplier_name: '',
    brand: '',
    origin: '',
    length_unit: 1,
    volume_unit: 0,
    weight_unit: 1,
    detailList: []
  }),
  tableData: [],
  rules: {
    spu_code: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.spu_code') }!`,
      (val: string) => StringLength(val, 0, 32) === '' || StringLength(val, 0, 32)
    ],
    spu_name: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.spu_name') }!`,
      (val: string) => StringLength(val, 0, 200) === '' || StringLength(val, 0, 200)
    ],
    spu_description: [(val: string) => StringLength(val, 0, 1000) === '' || StringLength(val, 0, 1000)],
    // bar_code: [(val: string) => StringLength(val, 0, 64) === '' || StringLength(val, 0, 64)],
    brand: [(val: string) => StringLength(val, 0, 128) === '' || StringLength(val, 0, 128)],
    category_name: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.category_name') }!`
    ],
    length_unit: [
      (val: number) => [0, 1, 2, 3].includes(val) || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.length_unit') }!`
    ],
    volume_unit: [
      (val: number) => [0, 1, 2].includes(val) || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.volume_unit') }!`
    ],
    weight_unit: [
      (val: number) => [0, 1, 2].includes(val) || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.weight_unit') }!`
    ],
    supplier_name: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.supplier_name') }!`
    ]
  },
  validRules: ref<any>({
    bar_code: [
      {
        type: 'string',
        min: 0,
        max: 64,
        message: `${ i18n.global.t('system.checkText.lengthValid') }${ 0 }-${ 64 }`,
        trigger: 'change'
      }
    ],
    sku_code: [
      { required: true, message: `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.sku_code') }` },
      {
        type: 'string',
        min: 0,
        max: 32,
        message: `${ i18n.global.t('system.checkText.lengthValid') }${ 0 }-${ 32 }`,
        trigger: 'change'
      }
    ],
    sku_name: [
      { required: true, message: `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.sku_name') }` },
      {
        type: 'string',
        min: 0,
        max: 200,
        message: `${ i18n.global.t('system.checkText.lengthValid') }${ 0 }-${ 200 }`,
        trigger: 'change'
      }
    ],
    unit: [
      { required: true, message: `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.commodityManagement.unit') }` },
      {
        type: 'string',
        min: 0,
        max: 5,
        message: `${ i18n.global.t('system.checkText.lengthValid') }${ 0 }-${ 5 }`,
        trigger: 'change'
      }
    ],
    weight: [
      {
        validator: isDecimal,
        validNumerical: 'nonNegative',
        length: 8,
        decimalLength: 3,
        trigger: 'change'
      }
    ],
    lenght: [
      {
        validator: isDecimal,
        validNumerical: 'nonNegative',
        length: 8,
        decimalLength: 3,
        trigger: 'change'
      }
    ],
    width: [
      {
        validator: isDecimal,
        validNumerical: 'nonNegative',
        length: 8,
        decimalLength: 3,
        trigger: 'change'
      }
    ],
    height: [
      {
        validator: isDecimal,
        validNumerical: 'nonNegative',
        length: 8,
        decimalLength: 3,
        trigger: 'change'
      }
    ],
    volume: [
      // {
      //   validator: isDecimal,
      //   validNumerical: 'nonNegative',
      //   length: 8,
      //   decimalLength: 3,
      //   trigger: 'change'
      // }
    ],
    cost: [
      {
        validator: isDecimal,
        validNumerical: 'nonNegative',
        length: 10,
        decimalLength: 2,
        trigger: 'change'
      }
    ],
    price: [
      {
        validator: isDecimal,
        validNumerical: 'nonNegative',
        length: 10,
        decimalLength: 2,
        trigger: 'change'
      }
    ]
  }),
  combobox: ref<{
    length_unit: {
      label: string
      value: number
    }[]
    volume_unit: {
      label: string
      value: number
    }[]
    weight_unit: {
      label: string
      value: number
    }[]
    category_name: {
      label: string
      value: number
    }[]
    supplier_name: {
      label: string
      value: number
    }[]
  }>({
    length_unit: [
      {
        label: 'mm',
        value: 0
      },
      {
        label: 'cm',
        value: 1
      },
      {
        label: 'dm',
        value: 2
      },
      {
        label: 'm',
        value: 3
      }
    ],
    volume_unit: [
      {
        label: 'cm³',
        value: 0
      },
      {
        label: 'dm³',
        value: 1
      },
      {
        label: 'm³',
        value: 2
      }
    ],
    weight_unit: [
      {
        label: 'mg',
        value: 0
      },
      {
        label: 'g',
        value: 1
      },
      {
        label: 'kg',
        value: 2
      }
    ],
    category_name: [],
    supplier_name: []
  })
})

const method = reactive({
  // When the commodity type changes, it is mainly used to assign the ID
  categoryNameChange: (val: string) => {
    if (!val) {
      data.form.category_id = 0
    } else {
      data.form.category_id = data.combobox.category_name.filter((item) => item.label === val)[0].value
    }
  },
  supplierNameChange: (val: string) => {
    if (!val) {
      data.form.supplier_id = 0
    } else {
      data.form.supplier_id = data.combobox.supplier_name.filter((item) => item.label === val)[0].value
    }
  },
  // Get the options required by the drop-down box
  getCombobox: async () => {
    data.combobox.category_name = []
    data.combobox.supplier_name = []
    const { data: res } = await getCategoryAll()
    if (!res.isSuccess) {
      hookComponent.$message({
        type: 'error',
        content: res.errorMessage
      })
      return
    }
    data.combobox.category_name = res.data
      .filter((item: CategoryVO) => item.is_valid)
      .map((item: CategoryVO) => ({ value: item.id, label: item.category_name }))
    // Get supplier information
    const { data: supplierRes } = await getSupplierAll()
    if (!supplierRes.isSuccess) {
      hookComponent.$message({
        type: 'error',
        content: supplierRes.errorMessage
      })
      return
    }
    data.combobox.supplier_name = supplierRes.data
      .filter((item: SupplierVO) => item.is_valid)
      .map((item: SupplierVO) => ({ value: item.id, label: item.supplier_name }))
  },
  closeDialog: () => {
    emit('close')
  },
  insertOneRow: () => {
    xTable.value.insertAt(-1)
  },
  // Export table
  exportTable: () => {
    const $table = xTable.value
    exportData({
      table: $table,
      filename: i18n.global.t('router.sideBar.commodityManagement'),
      columnFilterMethod({ column }: any) {
        return !['checkbox'].includes(column?.type) && !['operate'].includes(column?.field)
      }
    })
  },
  submit: async () => {
    const $table = xTable.value
    const errMap = await $table.validate(true)
    const { valid } = await formRef.value.validate()
    if (valid && !errMap) {
      if ($table.getTableData().fullData.length === 0) {
        hookComponent.$message({
          type: 'error',
          content: i18n.global.t('system.tips.detailLengthIsZero')
        })
        return
      }
      let form = { ...data.form }
      const insertRecords = $table.getInsertRecords()
      form.detailList = []
      // Processing detailed data
      if (dialogTitle.value === 'add') {
        form.detailList = [...insertRecords]
      } else {
        const updateRecords = $table.getUpdateRecords()
        const removeRecords = $table.getRemoveRecords()
        form.detailList = [...insertRecords, ...updateRecords]
        for (const item of removeRecords) {
          item.id = item.id > 0 ? 0 - item.id : item.id
          form.detailList.push(item)
        }
      }

      form = removeObjectNull(form)
      form.detailList = removeArrayNull(form.detailList)
      const { data: res } = dialogTitle.value === 'add' ? await addSpu(form) : await updateSpu(form)
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
    } else {
      hookComponent.$message({
        type: 'error',
        content: i18n.global.t('system.checkText.checkFormFail')
      })
    }
  },
  print() {
    const ref = hprintDialogRef.value
    ref.method.handleOpen()
  },
  editRow: (row: CommodityDetailVO) => {
    const $table = xTable.value
    $table.setEditRow(row)
  },
  deleteRow: (row: CommodityDetailVO) => {
    const $table = xTable.value
    hookComponent.$dialog({
      content: i18n.global.t('system.tips.beforeDeleteDetailMessage'),
      handleConfirm: async () => {
        $table.remove(row)
      }
    })
  }
})

const mainFormHeight = computed(() => computedSelectTableSearchHeight({ hasPager: false, hasToolBar: true }))

watch(
  () => isShow.value,
  (val) => {
    if (val) {
      method.getCombobox()
      data.form = props.form
    }
  }
)
</script>

<style scoped lang="less">
.mainForm {
  background-color: #f9f9f9;
  border-radius: 5px;
  padding: 20px;
  box-sizing: border-box;
  overflow: auto;
}

.toolbar {
  height: 40px;
}
</style>
