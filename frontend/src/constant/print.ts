export const PRINT_MENU = [{
  vue_path: 'commodityManagement',
  i18nName: 'base.commodityManagement',
  children: [{
    tab_page: 'print_page_main',
    form: {
      detailList: {
        name: 'print_data_main',
        columns: ['sku_code', 'sku_name', 'category_name', 'spu_description', 'supplier_name', 'bar_code', 'weight', 'lenght', 'width', 'height', 'volume', 'cost', 'price']
      }
    }
  }, {
    tab_page: 'print_page_detail',
    form: {
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
      detailList: {
        name: 'print_data_detail',
        columns: ['sku_code', 'sku_name', 'unit', 'weight', 'width', 'height', 'volume', 'cost', 'price']
      }
    }
  }]
},
{
  vue_path: 'customer',
  i18nName: 'base.customer',
  children: [{
    tab_page: 'print_page_main',
    form: {
      detailList: {
        name: 'print_data_main',
        columns: ['customer_name', 'city', 'address', 'manager', 'email', 'contact_tel']
      }

    }
  }, {
    tab_page: 'print_page_detail',
    form: {
      id: 0,
      customer_name: '',
      city: '',
      address: '',
      manager: '',
      email: '',
      contact_tel: '',
    }
  }]
},

]