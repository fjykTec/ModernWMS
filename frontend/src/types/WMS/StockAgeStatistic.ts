export interface StockAgeStatisticVo {
  warehouse_name: string,
  location_name: string,
  spu_code: string,
  spu_name: string,
  sku_id: number,
  sku_code: string,
  sku_name: string,
  qty: number,
  goods_owner_name: string,
  series_number: string,
  goods_location_id: number,
  expiry_date: string,
  price: number,
  putaway_date: string,
  stock_age: number
}
