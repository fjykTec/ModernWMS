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
  }
}
