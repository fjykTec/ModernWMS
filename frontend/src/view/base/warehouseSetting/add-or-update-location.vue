<!-- Location Setting Operate Dialog -->
<template>
  <v-dialog v-model="isShow" :width="'30%'" transition="dialog-top-transition" :persistent="true">
    <template #default>
      <v-card>
        <v-toolbar color="white" :title="`${$t('base.warehouseSetting.locationSetting')}`"></v-toolbar>
        <v-card-text>
          <v-form ref="formRef">
            <v-select
              v-model="data.form.warehouse_id"
              :items="data.combobox.warehouse_name"
              item-title="label"
              item-value="value"
              :rules="data.rules.warehouse_name"
              :label="$t('base.warehouseSetting.warehouse_name')"
              variant="outlined"
              clearable
              @update:model-value="method.changeWarehouse"
            ></v-select>
            <v-select
              v-model="data.form.warehouse_area_id"
              :items="data.combobox.warehouse_area_name"
              item-title="label"
              item-value="value"
              :rules="data.rules.warehouse_area_name"
              :label="$t('base.warehouseSetting.area_name')"
              variant="outlined"
              clearable
              @update:model-value="method.changeWarehouseArea"
            ></v-select>
            <v-select
              v-model="data.form.warehouse_area_property"
              :items="data.combobox.area_property"
              item-title="label"
              item-value="value"
              :rules="data.rules.warehouse_area_property"
              :label="$t('base.warehouseSetting.area_property')"
              variant="outlined"
              disabled
              clearable
            ></v-select>
            <v-text-field
              v-model="data.form.location_name"
              :label="$t('base.warehouseSetting.location_name')"
              :rules="data.rules.location_name"
              variant="outlined"
              clearable
            ></v-text-field>
            <v-text-field
              v-model="data.form.location_length"
              :label="$t('base.warehouseSetting.location_length')"
              :rules="data.rules.location_length"
              variant="outlined"
              clearable
            ></v-text-field>
            <v-text-field
              v-model="data.form.location_width"
              :label="$t('base.warehouseSetting.location_width')"
              :rules="data.rules.location_width"
              variant="outlined"
              clearable
            ></v-text-field>
            <v-text-field
              v-model="data.form.location_heigth"
              :label="$t('base.warehouseSetting.location_heigth')"
              :rules="data.rules.location_heigth"
              variant="outlined"
              clearable
            ></v-text-field>
            <v-text-field
              v-model="data.form.location_volume"
              :label="$t('base.warehouseSetting.location_volume')"
              :rules="data.rules.location_volume"
              variant="outlined"
              clearable
            ></v-text-field>
            <v-text-field
              v-model="data.form.location_load"
              :label="$t('base.warehouseSetting.location_load')"
              :rules="data.rules.location_load"
              variant="outlined"
              clearable
            ></v-text-field>
            <v-text-field
              v-model="data.form.roadway_number"
              :label="$t('base.warehouseSetting.roadway_number')"
              :rules="data.rules.roadway_number"
              variant="outlined"
              clearable
            ></v-text-field>
            <v-text-field
              v-model="data.form.shelf_number"
              :label="$t('base.warehouseSetting.shelf_number')"
              :rules="data.rules.shelf_number"
              variant="outlined"
              clearable
            ></v-text-field>
            <v-text-field
              v-model="data.form.layer_number"
              :label="$t('base.warehouseSetting.layer_number')"
              :rules="data.rules.layer_number"
              variant="outlined"
              clearable
            ></v-text-field>
            <v-text-field
              v-model="data.form.tag_number"
              :label="$t('base.warehouseSetting.tag_number')"
              :rules="data.rules.tag_number"
              variant="outlined"
              clearable
            ></v-text-field>
            <v-switch
              v-model="data.form.is_valid"
              color="primary"
              :label="$t('base.warehouseSetting.is_valid')"
              :rules="data.rules.is_valid"
            ></v-switch>
          </v-form>
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="method.closeDialog">{{ $t('system.page.close') }}</v-btn>
          <v-btn color="primary" variant="text" @click="method.submit">{{ $t('system.page.submit') }}</v-btn>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>
</template>

<script lang="ts" setup>
import { reactive, computed, ref, watch } from 'vue'
import { hookComponent } from '@/components/system/index'
import { addGoodsLocation, updateGoodsLocation, getWarehouseSelect, getWarehouseAreaSelect } from '@/api/base/warehouseSetting'
import { GoodsLocationVO, AreaProperty } from '@/types/Base/Warehouse'
import i18n from '@/languages/i18n'
import { StringLength, IsDecimal } from '@/utils/dataVerification/formRule'
import { removeObjectNull } from '@/utils/common'

const formRef = ref()
const emit = defineEmits(['close', 'saveSuccess'])

const props = defineProps<{
  showDialog: boolean
  form: GoodsLocationVO
}>()

const isShow = computed(() => props.showDialog)

const dialogTitle = computed(() => {
  if (props.form.id && props.form.id > 0) {
    return 'update'
  }
  return 'add'
})

const data = reactive({
  form: ref<GoodsLocationVO>({
    id: 0,
    warehouse_name: '',
    warehouse_area_name: '',
    location_name: '',
    location_length: 0,
    location_width: 0,
    location_heigth: 0,
    location_volume: 0,
    location_load: 0,
    roadway_number: '',
    shelf_number: '',
    layer_number: '',
    tag_number: '',
    is_valid: true
  }),
  rules: {
    warehouse_name: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.warehouseSetting.warehouse_name') }!`,
      (val: string) => StringLength(val, 0, 32) === '' || StringLength(val, 0, 32)
    ],
    warehouse_area_name: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.warehouseSetting.area_name') }!`,
      (val: string) => StringLength(val, 0, 32) === '' || StringLength(val, 0, 32)
    ],
    warehouse_area_property: [],
    location_name: [
      (val: string) => !!val || `${ i18n.global.t('system.checkText.mustInput') }${ i18n.global.t('base.warehouseSetting.location_name') }!`,
      (val: string) => StringLength(val, 0, 64) === '' || StringLength(val, 0, 64)
    ],
    location_length: [(val: number) => IsDecimal(val, 'nonNegative', 5, 2) === '' || IsDecimal(val, 'nonNegative', 5, 2)],
    location_width: [(val: number) => IsDecimal(val, 'nonNegative', 5, 2) === '' || IsDecimal(val, 'nonNegative', 5, 2)],
    location_heigth: [(val: number) => IsDecimal(val, 'nonNegative', 5, 2) === '' || IsDecimal(val, 'nonNegative', 5, 2)],
    location_volume: [(val: number) => IsDecimal(val, 'nonNegative', 12, 2) === '' || IsDecimal(val, 'nonNegative', 12, 2)],
    location_load: [(val: number) => IsDecimal(val, 'nonNegative', 8, 2) === '' || IsDecimal(val, 'nonNegative', 8, 2)],
    roadway_number: [(val: string) => StringLength(val, 0, 10) === '' || StringLength(val, 0, 10)],
    shelf_number: [(val: string) => StringLength(val, 0, 10) === '' || StringLength(val, 0, 10)],
    layer_number: [(val: string) => StringLength(val, 0, 10) === '' || StringLength(val, 0, 10)],
    tag_number: [(val: string) => StringLength(val, 0, 10) === '' || StringLength(val, 0, 10)],
    is_valid: []
  },
  combobox: ref<{
    warehouse_name: {
      label: string
      value: number
    }[]
    warehouse_area_name: {
      label: string
      value: number
      meta: any
    }[]
    area_property: {
      label: string
      value: AreaProperty
    }[]
  }>({
    warehouse_name: [],
    warehouse_area_name: [],
    area_property: [
      {
        label: i18n.global.t('base.warehouseSetting.picking_area'),
        value: AreaProperty.picking_area
      },
      {
        label: i18n.global.t('base.warehouseSetting.stocking_area'),
        value: AreaProperty.stocking_area
      },
      {
        label: i18n.global.t('base.warehouseSetting.receiving_area'),
        value: AreaProperty.receiving_area
      },
      {
        label: i18n.global.t('base.warehouseSetting.return_area'),
        value: AreaProperty.return_area
      },
      {
        label: i18n.global.t('base.warehouseSetting.defective_area'),
        value: AreaProperty.defective_area
      },
      {
        label: i18n.global.t('base.warehouseSetting.inventory_area'),
        value: AreaProperty.inventory_area
      }
    ]
  })
})

const method = reactive({
  changeWarehouse: (warehouseID: any) => {
    data.combobox.warehouse_area_name = []

    if (!warehouseID) {
      method.initWarehouseArea()
      return
    }

    // Find the ID corresponding value
    const warehouse = data.combobox.warehouse_name.find((item) => item.value === warehouseID)
    if (warehouse) {
      data.form.warehouse_name = warehouse.label
      data.form.warehouse_id = warehouseID

      // Clear messages of warehouse area when warehouse changed.
      method.initWarehouseArea()
      method.getWarehouseAreaSelect(warehouseID)
    }
  },
  changeWarehouseArea: (warehouseAreaID: any) => {
    if (!warehouseAreaID) {
      method.initWarehouseArea()
      return
    }

    // Find the ID corresponding value
    const warehouse = data.combobox.warehouse_area_name.find((item) => item.value === warehouseAreaID)
    if (warehouse) {
      data.form.warehouse_area_name = warehouse.label
      data.form.warehouse_area_id = warehouseAreaID
      data.form.warehouse_area_property = warehouse.meta
    }
  },
  // Clear messages of warehouse area when warehouse changed.
  initWarehouseArea() {
    delete data.form.warehouse_area_id
    data.form.warehouse_area_name = ''
    delete data.form.warehouse_area_property
  },
  getWarehouseSelect: async () => {
    data.combobox.warehouse_name = []
    const { data: res } = await getWarehouseSelect()
    if (!res.isSuccess) {
      return
    }
    for (const item of res.data) {
      data.combobox.warehouse_name.push({
        label: item.name,
        // WarehouseID is a numeric type
        value: Number(item.value)
      })
    }
  },
  // Get warehouse area select when warehouse changed.
  getWarehouseAreaSelect: async (warehouseID: number) => {
    data.combobox.warehouse_area_name = []
    const { data: res } = await getWarehouseAreaSelect(warehouseID)
    if (!res.isSuccess) {
      return
    }
    for (const item of res.data) {
      data.combobox.warehouse_area_name.push({
        label: item.area_name,
        // ID: WarehouseAreaID
        value: item.id,
        // Custom Property
        meta: item.area_property
      })
    }
  },
  closeDialog: () => {
    emit('close')
  },
  initForm: () => {
    data.form = props.form
  },
  submit: async () => {
    const { valid } = await formRef.value.validate()
    if (valid) {
      const form = removeObjectNull(data.form)
      const { data: res } = dialogTitle.value === 'add' ? await addGoodsLocation(form) : await updateGoodsLocation(form)
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
  }
})

watch(
  () => isShow.value,
  (val) => {
    if (val) {
      method.initForm()
      method.getWarehouseSelect()
    }
  }
)
</script>

<style scoped lang="less">
.v-form {
  div {
    margin-bottom: 7px;
  }
}
</style>
