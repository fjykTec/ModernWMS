export default {
  asnStatistic: {
    default: ['supplier_name', 'sku_name'],
    list: [
      {
        type: 'string',
        name: 'spu_code'
      },
      {
        type: 'string',
        name: 'spu_name'
      },
      {
        type: 'string',
        name: 'sku_code'
      },
      {
        type: 'string',
        name: 'supplier_name'
      },
      {
        type: 'string',
        name: 'sku_name'
      },
      {
        type: 'string',
        name: 'goods_owner_name'
      }
    ]
  },
  deliveryStatistic: {
    default: ['sku_code', 'warehouse_name', 'customer_name'],
    list: [
      {
        type: 'string',
        name: 'spu_code'
      },
      {
        type: 'string',
        name: 'spu_name'
      },
      {
        type: 'string',
        name: 'sku_code'
      },
      {
        type: 'string',
        name: 'sku_name'
      },
      {
        type: 'string',
        name: 'warehouse_name'
      },
      {
        type: 'string',
        name: 'customer_name'
      },
      {
        type: 'datetime',
        name: 'delivery_date_from'
      },
      {
        type: 'datetime',
        name: 'delivery_date_to'
      }
    ]
  },
  stockageStatistic: {
    default: ['sku_code', 'warehouse_name', 'location_name'],
    list: [
      {
        type: 'string',
        name: 'spu_code'
      },
      {
        type: 'string',
        name: 'spu_name'
      },
      {
        type: 'string',
        name: 'sku_code'
      },
      {
        type: 'string',
        name: 'sku_name'
      },
      {
        type: 'string',
        name: 'warehouse_name'
      },
      {
        type: 'string',
        name: 'location_name'
      },
      {
        type: 'number',
        name: 'stock_age_from'
      },
      {
        type: 'number',
        name: 'stock_age_to'
      },
      {
        type: 'datetime',
        name: 'expiry_date_from'
      },
      {
        type: 'datetime',
        name: 'expiry_date_to'
      }
    ]
  }
}
