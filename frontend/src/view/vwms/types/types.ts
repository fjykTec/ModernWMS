export interface warehouseInfoType {
  address:string,
  city:string,
  contact_tel:string,
  create_time:string,
  creator:string,
  email:string | null,
  id:number,
  manager:string,
  is_valid:boolean,
  last_update_time:string,
  tenant_id:number,
  warehouse_name:string
}
export interface factoryInfoType {
  area_name:string,
  area_property:number,
  create_time:string,
  id:number,
  is_valid:boolean,
  last_update_time:string,
  warehouse_id:number,
  warehouse_name:string,
  parent_id:number,
  tenant_id:number,
}
export interface shelfItemInfoType {
  id:number,
  warehouse_name:string,
  warehouse_area_property:number,
  layer_number:string,
  location_heigth:number,
  location_length:number,
  location_load:number,
  location_name:string,
  location_volume:number,
  location_width:number,
  roadway_number:string,
  shelf_number:string,
  tag_number:string,
  warehouse_area_id:number,
  warehouse_area_name:string,
  warehouse_id:number,
  create_time:string,
  last_update_time:string,
  is_valid:boolean,
  tenant_id:number
}
export interface productInfoType {
  goods_location_id:number,
  goods_owner_name:string,
  location_name:string,
  qty:number,
  qty_available:number,
  qty_frozen:number,
  qty_locked:number,
  series_number:string | null,
  sku_code:string,
  sku_id:number,
  sku_name:string,
  spu_code:string,
  spu_name:string,
  warehouse_name:string
}

// 整理传递给unity的数据
export type productDataType = productInfoType
export interface shelfItemDataType extends shelfItemInfoType {
  product:productDataType[]
}
export interface shelfDataType {
  shelf_name:string,
  layer:number,
  column:number,
  shelfItems:shelfItemDataType[],
}
export interface factoryDataType extends factoryInfoType {
  shelves:shelfDataType[]
}
export interface formatDataType {
  area:factoryDataType[]
}

// 展示数据
export interface warehouseShowDataType extends warehouseInfoType{
  factory_number:number,
  warehouse_product_number:number,
  type:string
}

export interface factoryShowDataType {
  area_name:string,
  area_property:number,
  shelf_number:number,
  shelf_item_number:number,
  // 剩余库位数量
  surplus_number:number,
  // 已存货物数
  factory_product_number:number,
  type:string
}

export interface shelfGridDataType {
  shelf_name:string,
  layer:number,
  column:number,
  products:number[]
}
export interface shelfShowDataType {
  shelf_name:string,
  layer:number,
  column:number,
  // 闲置数
  surplus_number:number,
  // 已存商品数
  shelf_product_number:number,
  type:string
}

export interface shelfItemShowDataType extends shelfItemInfoType{
  product:productInfoType[],
  type:string
}