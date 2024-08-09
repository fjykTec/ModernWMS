export interface StockLocationVO {
  warehouse_name: string
    location_name: string
    spu_code: string
    spu_name: string
    sku_id: number
    sku_code: string
    sku_name: string
    qty: number
    qty_available: number
    qty_locked: number
    qty_frozen: number
}

export interface StockVO {
    spu_code: string
    spu_name: string
    sku_code: string
    sku_id: number
    qty: number
    qty_available: number
    qty_locked: number
    qty_frozen: number
    qty_asn: number
    qty_to_unload: number
    qty_to_sort: number
    qty_sorted: number
    shortage_qty: number
}
