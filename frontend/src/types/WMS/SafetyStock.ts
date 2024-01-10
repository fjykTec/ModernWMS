export interface SafetyStockVo {
  warehouse_name: string
  spu_code: string
  spu_name: string
  sku_code: string
  sku_name: string
  sku_id: number
  qty: number
  qty_available: number
  qty_locked: number
  qty_frozen: number
  safety_stock_qty: number
}
