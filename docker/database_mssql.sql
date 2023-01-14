SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL Serializable
GO
BEGIN TRANSACTION
GO
create database wms
go
use wms
go
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'company' AND type = 'U')
BEGIN 
CREATE TABLE company (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_company_ID PRIMARY KEY CLUSTERED  ,
	company_name varchar(256)  NOT NULL  CONSTRAINT DF_company_company_name DEFAULT ('') ,
	city varchar(128)  NOT NULL  CONSTRAINT DF_company_city DEFAULT ('') ,
	address varchar(256)  NOT NULL  CONSTRAINT DF_company_address DEFAULT ('') ,
	manager varchar(64)  NOT NULL  CONSTRAINT DF_company_manager DEFAULT ('') ,
	contact_tel varchar(64)  NOT NULL  CONSTRAINT DF_company_contact_tel DEFAULT ('') ,
	create_time datetime  NOT NULL  CONSTRAINT DF_company_create_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_company_last_update_time DEFAULT (N'1000-01-01') ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'supplier' AND type = 'U')
BEGIN 
CREATE TABLE supplier (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_supplier_ID PRIMARY KEY CLUSTERED  ,
	supplier_name varchar(256)  NOT NULL  CONSTRAINT DF_supplier_supplier_name DEFAULT ('') ,
	city varchar(128)  NOT NULL  CONSTRAINT DF_supplier_city DEFAULT ('') ,
	address varchar(256)  NOT NULL  CONSTRAINT DF_supplier_address DEFAULT ('') ,
	email varchar(128)  NOT NULL  CONSTRAINT DF_supplier_email DEFAULT ('') ,
	manager varchar(64)  NOT NULL  CONSTRAINT DF_supplier_manager DEFAULT ('') ,
	contact_tel varchar(64)  NOT NULL  CONSTRAINT DF_supplier_contact_tel DEFAULT ('') ,
	creator varchar(64)  NOT NULL  CONSTRAINT DF_supplier_creator DEFAULT ('') ,
	create_time datetime  NOT NULL  CONSTRAINT DF_supplier_create_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_supplier_last_update_time DEFAULT (N'1000-01-01') ,
	is_valid bit  NOT NULL  CONSTRAINT DF_supplier_is_valid DEFAULT (1) ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'customer' AND type = 'U')
BEGIN 
CREATE TABLE customer (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_customer_ID PRIMARY KEY CLUSTERED  ,
	customer_name varchar(256)  NOT NULL  CONSTRAINT DF_customer_customer_name DEFAULT ('') ,
	city varchar(128)  NOT NULL  CONSTRAINT DF_customer_city DEFAULT ('') ,
	address varchar(256)  NOT NULL  CONSTRAINT DF_customer_address DEFAULT ('') ,
	email varchar(128)  NOT NULL  CONSTRAINT DF_customer_email DEFAULT ('') ,
	manager varchar(64)  NOT NULL  CONSTRAINT DF_customer_manager DEFAULT ('') ,
	contact_tel varchar(64)  NOT NULL  CONSTRAINT DF_customer_contact_tel DEFAULT ('') ,
	creator varchar(64)  NOT NULL  CONSTRAINT DF_customer_creator DEFAULT ('') ,
	create_time datetime  NOT NULL  CONSTRAINT DF_customer_create_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_customer_last_update_time DEFAULT (N'1000-01-01') ,
	is_valid bit  NOT NULL  CONSTRAINT DF_customer_is_valid DEFAULT (1) ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'warehouse' AND type = 'U')
BEGIN 
CREATE TABLE warehouse (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_warehouse_ID PRIMARY KEY CLUSTERED  ,
	warehouse_name varchar(32)  NOT NULL  CONSTRAINT DF_warehouse_warehouse_name DEFAULT ('') ,
	city varchar(128)  NOT NULL  CONSTRAINT DF_warehouse_city DEFAULT ('') ,
	address varchar(256)  NOT NULL  CONSTRAINT DF_warehouse_address DEFAULT ('') ,
	email varchar(128)  NOT NULL  CONSTRAINT DF_warehouse_email DEFAULT ('') ,
	manager varchar(64)  NOT NULL  CONSTRAINT DF_warehouse_manager DEFAULT ('') ,
	contact_tel varchar(64)  NOT NULL  CONSTRAINT DF_warehouse_contact_tel DEFAULT ('') ,
	creator varchar(64)  NOT NULL  CONSTRAINT DF_warehouse_creator DEFAULT ('') ,
	create_time datetime  NOT NULL  CONSTRAINT DF_warehouse_create_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_warehouse_last_update_time DEFAULT (N'1000-01-01') ,
	is_valid bit  NOT NULL  CONSTRAINT DF_warehouse_is_valid DEFAULT (1) ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'warehousearea' AND type = 'U')
BEGIN 
CREATE TABLE warehousearea (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_warehousearea_ID PRIMARY KEY CLUSTERED  ,
	warehouse_id int  NOT NULL  ,
	area_name varchar(32)  NOT NULL  CONSTRAINT DF_warehousearea_area_name DEFAULT ('') ,
	area_property tinyint  NOT NULL  CONSTRAINT DF_warehousearea_area_property DEFAULT (0) ,
	parent_id int  NOT NULL  CONSTRAINT DF_warehousearea_parent_id DEFAULT (0) ,
	create_time datetime  NOT NULL  CONSTRAINT DF_warehousearea_create_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_warehousearea_last_update_time DEFAULT (N'1000-01-01') ,
	is_valid bit  NOT NULL  CONSTRAINT DF_warehousearea_is_valid DEFAULT (1) ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'goodslocation' AND type = 'U')
BEGIN 
CREATE TABLE goodslocation (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_goodslocation_ID PRIMARY KEY CLUSTERED  ,
	warehouse_id int  NOT NULL  CONSTRAINT DF_goodslocation_warehouse_id DEFAULT (0) ,
	warehouse_name varchar(32)  NOT NULL  CONSTRAINT DF_goodslocation_warehouse_name DEFAULT ('') ,
	warehouse_area_id int  NOT NULL  CONSTRAINT DF_goodslocation_warehouse_area_id DEFAULT (0) ,
	warehouse_area_name varchar(32)  NOT NULL  CONSTRAINT DF_goodslocation_warehouse_area_name DEFAULT ('') ,
	warehouse_area_property tinyint  NOT NULL  CONSTRAINT DF_goodslocation_warehouse_area_property DEFAULT (0) ,
	location_name varchar(64)  NOT NULL  CONSTRAINT DF_goodslocation_location_name DEFAULT ('') ,
	location_length decimal(5,2)  NOT NULL  CONSTRAINT DF_goodslocation_location_length DEFAULT (0) ,
	location_width decimal(5,2)  NOT NULL  CONSTRAINT DF_goodslocation_location_width DEFAULT (0) ,
	location_heigth decimal(5,2)  NOT NULL  CONSTRAINT DF_goodslocation_location_heigth DEFAULT (0) ,
	location_volume decimal(12,2)  NOT NULL  CONSTRAINT DF_goodslocation_location_volume DEFAULT (0) ,
	location_load decimal(8,2)  NOT NULL  CONSTRAINT DF_goodslocation_location_load DEFAULT (0) ,
	roadway_number varchar(10)  NOT NULL  CONSTRAINT DF_goodslocation_roadway_number DEFAULT ('') ,
	shelf_number varchar(10)  NOT NULL  CONSTRAINT DF_goodslocation_shelf_number DEFAULT ('') ,
	layer_number varchar(10)  NOT NULL  CONSTRAINT DF_goodslocation_layer_number DEFAULT ('') ,
	tag_number varchar(10)  NOT NULL  CONSTRAINT DF_goodslocation_tag_number DEFAULT ('') ,
	create_time datetime  NOT NULL  CONSTRAINT DF_goodslocation_create_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_goodslocation_last_update_time DEFAULT (N'1000-01-01') ,
	is_valid bit  NOT NULL  CONSTRAINT DF_goodslocation_is_valid DEFAULT (1) ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'freightfee' AND type = 'U')
BEGIN 
CREATE TABLE freightfee (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_freightfee_ID PRIMARY KEY CLUSTERED  ,
	carrier varchar(256)  NOT NULL  CONSTRAINT DF_freightfee_carrier DEFAULT ('') ,
	departure_city varchar(128)  NOT NULL  CONSTRAINT DF_freightfee_departure_city DEFAULT ('') ,
	arrival_city varchar(128)  NOT NULL  CONSTRAINT DF_freightfee_arrival_city DEFAULT ('') ,
	price_per_weight decimal(6,2)  NOT NULL  CONSTRAINT DF_freightfee_price_per_weight DEFAULT (0) ,
	price_per_volume decimal(6,2)  NOT NULL  CONSTRAINT DF_freightfee_price_per_volume DEFAULT (0) ,
	min_payment decimal(8,2)  NOT NULL  CONSTRAINT DF_freightfee_min_payment DEFAULT (0) ,
	creator varchar(64)  NOT NULL  CONSTRAINT DF_freightfee_creator DEFAULT ('') ,
	create_time datetime  NOT NULL  CONSTRAINT DF_freightfee_create_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_freightfee_last_update_time DEFAULT (N'1000-01-01') ,
	is_valid bit  NOT NULL  CONSTRAINT DF_freightfee_is_valid DEFAULT (1) ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'category' AND type = 'U')
BEGIN 
CREATE TABLE category (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_category_ID PRIMARY KEY CLUSTERED  ,
	category_name varchar(32)  NOT NULL  CONSTRAINT DF_category_category_name DEFAULT ('') ,
	parent_id int  NOT NULL  CONSTRAINT DF_category_parent_id DEFAULT (0) ,
	creator varchar(64)  NOT NULL  CONSTRAINT DF_category_creator DEFAULT ('') ,
	create_time datetime  NOT NULL  CONSTRAINT DF_category_create_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_category_last_update_time DEFAULT (N'1000-01-01') ,
	is_valid bit  NOT NULL  CONSTRAINT DF_category_is_valid DEFAULT (1) ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'spu' AND type = 'U')
BEGIN 
CREATE TABLE spu (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_spu_ID PRIMARY KEY CLUSTERED  ,
	spu_code varchar(32)  NOT NULL  CONSTRAINT DF_spu_spu_code DEFAULT ('') ,
	spu_name varchar(200)  NOT NULL  CONSTRAINT DF_spu_spu_name DEFAULT ('') ,
	category_id int  NOT NULL  CONSTRAINT DF_spu_category_id DEFAULT (0) ,
	spu_description varchar(1000)  NOT NULL  CONSTRAINT DF_spu_spu_description DEFAULT ('') ,
	bar_code varchar(64)  NOT NULL  CONSTRAINT DF_spu_bar_code DEFAULT ('') ,
	supplier_id int  NOT NULL  CONSTRAINT DF_spu_supplier_id DEFAULT (0) ,
	supplier_name varchar(256)  NOT NULL  CONSTRAINT DF_spu_supplier_name DEFAULT ('') ,
	brand varchar(128)  NOT NULL  CONSTRAINT DF_spu_brand DEFAULT ('') ,
	origin varchar(256)  NOT NULL  CONSTRAINT DF_spu_origin DEFAULT ('') ,
	length_unit tinyint  NOT NULL  CONSTRAINT DF_spu_length_unit DEFAULT (1) ,
	volume_unit tinyint  NOT NULL  CONSTRAINT DF_spu_volume_unit DEFAULT (0) ,
	weight_unit tinyint  NOT NULL  CONSTRAINT DF_spu_weight_unit DEFAULT (1) ,
	creator varchar(64)  NOT NULL  CONSTRAINT DF_spu_creator DEFAULT ('') ,
	create_time datetime  NOT NULL  CONSTRAINT DF_spu_create_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_spu_last_update_time DEFAULT (N'1000-01-01') ,
	is_valid bit  NOT NULL  CONSTRAINT DF_spu_is_valid DEFAULT (1) ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'sku' AND type = 'U')
BEGIN 
CREATE TABLE sku (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_sku_ID PRIMARY KEY CLUSTERED  ,
	spu_id int  NOT NULL  CONSTRAINT DF_sku_spu_id DEFAULT (0) ,
	sku_code varchar(32)  NOT NULL  CONSTRAINT DF_sku_sku_code DEFAULT ('') ,
	sku_name varchar(200)  NOT NULL  CONSTRAINT DF_sku_sku_name DEFAULT ('') ,
	weight decimal(8,3)  NOT NULL  CONSTRAINT DF_sku_weight DEFAULT (0) ,
	lenght decimal(8,3)  NOT NULL  CONSTRAINT DF_sku_lenght DEFAULT (0) ,
	width decimal(8,3)  NOT NULL  CONSTRAINT DF_sku_width DEFAULT (0) ,
	height decimal(8,3)  NOT NULL  CONSTRAINT DF_sku_height DEFAULT (0) ,
	volume decimal(15,3)  NOT NULL  CONSTRAINT DF_sku_volume DEFAULT (0) ,
	unit varchar(5)  NOT NULL  CONSTRAINT DF_sku_unit DEFAULT ('') ,
	cost decimal(10,2)  NOT NULL  CONSTRAINT DF_sku_cost DEFAULT (0) ,
	price decimal(10,2)  NOT NULL  CONSTRAINT DF_sku_price DEFAULT (0) ,
	create_time datetime  NOT NULL  CONSTRAINT DF_sku_create_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_sku_last_update_time DEFAULT (N'1000-01-01') 
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'user' AND type = 'U')
BEGIN 
CREATE TABLE user (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_user_ID PRIMARY KEY CLUSTERED  ,
	user_num varchar(128)  NOT NULL  CONSTRAINT DF_user_user_num DEFAULT ('') ,
	user_name varchar(128)  NOT NULL  CONSTRAINT DF_user_user_name DEFAULT ('') ,
	contact_tel varchar(64)  NOT NULL  CONSTRAINT DF_user_contact_tel DEFAULT ('') ,
	user_role varchar(128)  NOT NULL  CONSTRAINT DF_user_user_role DEFAULT ('') ,
	sex varchar(10)  NOT NULL  CONSTRAINT DF_user_sex DEFAULT ('') ,
	is_valid bit  NOT NULL  CONSTRAINT DF_user_is_valid DEFAULT (1) ,
	auth_string varchar(64)  NOT NULL  CONSTRAINT DF_user_auth_string DEFAULT ('') ,
	email varchar(128)  NOT NULL  CONSTRAINT DF_user_email DEFAULT ('') ,
	creator varchar(64)  NOT NULL  CONSTRAINT DF_user_creator DEFAULT ('') ,
	create_time datetime  NOT NULL  CONSTRAINT DF_user_create_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_user_last_update_time DEFAULT (N'1000-01-01') ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'userrole' AND type = 'U')
BEGIN 
CREATE TABLE userrole (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_userrole_ID PRIMARY KEY CLUSTERED  ,
	role_name varchar(32)  NOT NULL  CONSTRAINT DF_userrole_role_name DEFAULT ('') ,
	is_valid bit  NOT NULL  CONSTRAINT DF_userrole_is_valid DEFAULT (1) ,
	create_time datetime  NOT NULL  CONSTRAINT DF_userrole_create_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_userrole_last_update_time DEFAULT (N'1000-01-01') ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'menu' AND type = 'U')
BEGIN 
CREATE TABLE menu (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_menu_ID PRIMARY KEY CLUSTERED  ,
	menu_name varchar(32)  NOT NULL  CONSTRAINT DF_menu_menu_name DEFAULT ('') ,
	module varchar(32)  NOT NULL  CONSTRAINT DF_menu_module DEFAULT ('') ,
	vue_path varchar(64)  NOT NULL  CONSTRAINT DF_menu_vue_path DEFAULT ('') ,
	vue_path_detail varchar(64)  NOT NULL  CONSTRAINT DF_menu_vue_path_detail DEFAULT ('') ,
	vue_directory varchar(128)  NOT NULL  CONSTRAINT DF_menu_vue_directory DEFAULT ('') ,
	sort int  NOT NULL  CONSTRAINT DF_menu_sort DEFAULT (0) ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'rolemenu' AND type = 'U')
BEGIN 
CREATE TABLE rolemenu (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_rolemenu_ID PRIMARY KEY CLUSTERED  ,
	userrole_id int  NOT NULL  CONSTRAINT DF_rolemenu_userrole_id DEFAULT (0) ,
	menu_id int  NOT NULL  CONSTRAINT DF_rolemenu_menu_id DEFAULT (0) ,
	authority tinyint  NOT NULL  CONSTRAINT DF_rolemenu_authority DEFAULT (0) ,
	create_time datetime  NOT NULL  CONSTRAINT DF_rolemenu_create_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_rolemenu_last_update_time DEFAULT (N'1000-01-01') ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'goodsowner' AND type = 'U')
BEGIN 
CREATE TABLE goodsowner (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_goodsowner_ID PRIMARY KEY CLUSTERED  ,
	goods_owner_name varchar(256)  NOT NULL  CONSTRAINT DF_goodsowner_goods_owner_name DEFAULT ('') ,
	city varchar(128)  NOT NULL  CONSTRAINT DF_goodsowner_city DEFAULT ('') ,
	address varchar(256)  NOT NULL  CONSTRAINT DF_goodsowner_address DEFAULT ('') ,
	manager varchar(64)  NOT NULL  CONSTRAINT DF_goodsowner_manager DEFAULT ('') ,
	contact_tel varchar(64)  NOT NULL  CONSTRAINT DF_goodsowner_contact_tel DEFAULT ('') ,
	creator varchar(64)  NOT NULL  CONSTRAINT DF_goodsowner_creator DEFAULT ('') ,
	create_time datetime  NOT NULL  CONSTRAINT DF_goodsowner_create_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_goodsowner_last_update_time DEFAULT (N'1000-01-01') ,
	is_valid bit  NOT NULL  CONSTRAINT DF_goodsowner_is_valid DEFAULT (1) ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'asn' AND type = 'U')
BEGIN 
CREATE TABLE asn (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_asn_ID PRIMARY KEY CLUSTERED  ,
	asn_no varchar(32)  NOT NULL  CONSTRAINT DF_asn_asn_no DEFAULT ('') ,
	asn_status tinyint  NOT NULL  CONSTRAINT DF_asn_asn_status DEFAULT (0) ,
	spu_id int  NOT NULL  CONSTRAINT DF_asn_spu_id DEFAULT (0) ,
	sku_id int  NOT NULL  CONSTRAINT DF_asn_sku_id DEFAULT (0) ,
	asn_qty int  NOT NULL  CONSTRAINT DF_asn_asn_qty DEFAULT (0) ,
	actual_qty int  NOT NULL  CONSTRAINT DF_asn_actual_qty DEFAULT (0) ,
	sorted_qty int  NOT NULL  CONSTRAINT DF_asn_sorted_qty DEFAULT (0) ,
	shortage_qty int  NOT NULL  CONSTRAINT DF_asn_shortage_qty DEFAULT (0) ,
	more_qty int  NOT NULL  CONSTRAINT DF_asn_more_qty DEFAULT (0) ,
	damage_qty int  NOT NULL  CONSTRAINT DF_asn_damage_qty DEFAULT (0) ,
	weight decimal(12,3)  NOT NULL  CONSTRAINT DF_asn_weight DEFAULT (0) ,
	volume decimal(15,3)  NOT NULL  CONSTRAINT DF_asn_volume DEFAULT (0) ,
	supplier_id int  NOT NULL  CONSTRAINT DF_asn_supplier_id DEFAULT (0) ,
	supplier_name varchar(256)  NOT NULL  CONSTRAINT DF_asn_supplier_name DEFAULT ('') ,
	goods_owner_id int  NOT NULL  CONSTRAINT DF_asn_goods_owner_id DEFAULT (0) ,
	goods_owner_name varchar(256)  NOT NULL  CONSTRAINT DF_asn_goods_owner_name DEFAULT ('') ,
	creator varchar(64)  NOT NULL  CONSTRAINT DF_asn_creator DEFAULT ('') ,
	create_time datetime  NOT NULL  CONSTRAINT DF_asn_create_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_asn_last_update_time DEFAULT (N'1000-01-01') ,
	is_valid bit  NOT NULL  CONSTRAINT DF_asn_is_valid DEFAULT (1) ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'asnsort' AND type = 'U')
BEGIN 
CREATE TABLE asnsort (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_asnsort_ID PRIMARY KEY CLUSTERED  ,
	asn_id int  NOT NULL  CONSTRAINT DF_asnsort_asn_id DEFAULT (0) ,
	sorted_qty int  NOT NULL  CONSTRAINT DF_asnsort_sorted_qty DEFAULT (0) ,
	creator varchar(64)  NOT NULL  CONSTRAINT DF_asnsort_creator DEFAULT ('') ,
	create_time datetime  NOT NULL  CONSTRAINT DF_asnsort_create_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_asnsort_last_update_time DEFAULT (N'1000-01-01') ,
	is_valid bit  NOT NULL  CONSTRAINT DF_asnsort_is_valid DEFAULT (1) ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'stock' AND type = 'U')
BEGIN 
CREATE TABLE stock (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_stock_ID PRIMARY KEY CLUSTERED  ,
	sku_id int  NOT NULL  CONSTRAINT DF_stock_sku_id DEFAULT (0) ,
	goods_location_id int  NOT NULL  CONSTRAINT DF_stock_goods_location_id DEFAULT (0) ,
	qty int  NOT NULL  CONSTRAINT DF_stock_qty DEFAULT (0) ,
	goods_owner_id int  NOT NULL  CONSTRAINT DF_stock_goods_owner_id DEFAULT (0) ,
	is_freeze bit  NOT NULL  CONSTRAINT DF_stock_is_freeze DEFAULT (0) ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_stock_last_update_time DEFAULT (N'1000-01-01') ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'stockmove' AND type = 'U')
BEGIN 
CREATE TABLE stockmove (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_stockmove_ID PRIMARY KEY CLUSTERED  ,
	job_code varchar(32)  NOT NULL  CONSTRAINT DF_stockmove_job_code DEFAULT ('') ,
	move_status tinyint  NOT NULL  CONSTRAINT DF_stockmove_move_status DEFAULT (0) ,
	sku_id int  NOT NULL  CONSTRAINT DF_stockmove_sku_id DEFAULT (0) ,
	orig_goods_location_id int  NOT NULL  CONSTRAINT DF_stockmove_orig_goods_location_id DEFAULT (0) ,
	dest_googs_location_id int  NOT NULL  CONSTRAINT DF_stockmove_dest_googs_location_id DEFAULT (0) ,
	qty int  NOT NULL  CONSTRAINT DF_stockmove_qty DEFAULT (0) ,
	goods_owner_id int  NOT NULL  CONSTRAINT DF_stockmove_goods_owner_id DEFAULT (0) ,
	handler varchar(64)  NOT NULL  CONSTRAINT DF_stockmove_handler DEFAULT ('') ,
	handle_time datetime  NOT NULL  CONSTRAINT DF_stockmove_handle_time DEFAULT (N'1000-01-01') ,
	creator varchar(64)  NOT NULL  CONSTRAINT DF_stockmove_creator DEFAULT ('') ,
	create_time datetime  NOT NULL  CONSTRAINT DF_stockmove_create_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_stockmove_last_update_time DEFAULT (N'1000-01-01') ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'stocktaking' AND type = 'U')
BEGIN 
CREATE TABLE stocktaking (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_stocktaking_ID PRIMARY KEY CLUSTERED  ,
	job_code varchar(32)  NOT NULL  CONSTRAINT DF_stocktaking_job_code DEFAULT ('') ,
	job_status bit  NOT NULL  CONSTRAINT DF_stocktaking_job_status DEFAULT (0) ,
	sku_id int  NOT NULL  CONSTRAINT DF_stocktaking_sku_id DEFAULT (0) ,
	goods_owner_id int  NOT NULL  CONSTRAINT DF_stocktaking_goods_owner_id DEFAULT (0) ,
	goods_location_id int  NOT NULL  CONSTRAINT DF_stocktaking_goods_location_id DEFAULT (0) ,
	book_qty int  NOT NULL  CONSTRAINT DF_stocktaking_book_qty DEFAULT (0) ,
	counted_qty int  NOT NULL  CONSTRAINT DF_stocktaking_counted_qty DEFAULT (0) ,
	difference_qty int  NOT NULL  CONSTRAINT DF_stocktaking_difference_qty DEFAULT (0) ,
	handler varchar(64)  NOT NULL  CONSTRAINT DF_stocktaking_handler DEFAULT ('') ,
	handle_time datetime  NOT NULL  CONSTRAINT DF_stocktaking_handle_time DEFAULT (N'1000-01-01') ,
	creator varchar(64)  NOT NULL  CONSTRAINT DF_stocktaking_creator DEFAULT ('') ,
	create_time datetime  NOT NULL  CONSTRAINT DF_stocktaking_create_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_stocktaking_last_update_time DEFAULT (N'1000-01-01') ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'stockadjust' AND type = 'U')
BEGIN 
CREATE TABLE stockadjust (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_stockadjust_ID PRIMARY KEY CLUSTERED  ,
	job_code varchar(32)  NOT NULL  CONSTRAINT DF_stockadjust_job_code DEFAULT ('') ,
	job_type tinyint  NOT NULL  CONSTRAINT DF_stockadjust_job_type DEFAULT (0) ,
	sku_id int  NOT NULL  CONSTRAINT DF_stockadjust_sku_id DEFAULT (0) ,
	goods_owner_id int  NOT NULL  CONSTRAINT DF_stockadjust_goods_owner_id DEFAULT (0) ,
	goods_location_id int  NOT NULL  CONSTRAINT DF_stockadjust_goods_location_id DEFAULT (0) ,
	qty int  NOT NULL  CONSTRAINT DF_stockadjust_qty DEFAULT (0) ,
	creator varchar(64)  NOT NULL  CONSTRAINT DF_stockadjust_creator DEFAULT ('') ,
	create_time datetime  NOT NULL  CONSTRAINT DF_stockadjust_create_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_stockadjust_last_update_time DEFAULT (N'1000-01-01') ,
	tenant_id bigint  NOT NULL  ,
	is_update_stock bit  NOT NULL  CONSTRAINT DF_stockadjust_is_update_stock DEFAULT (0) ,
	source_table_id int  NOT NULL  CONSTRAINT DF_stockadjust_source_table_id DEFAULT (0) 
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'stockfreeze' AND type = 'U')
BEGIN 
CREATE TABLE stockfreeze (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_stockfreeze_ID PRIMARY KEY CLUSTERED  ,
	job_code varchar(32)  NOT NULL  CONSTRAINT DF_stockfreeze_job_code DEFAULT ('') ,
	job_type bit  NOT NULL  CONSTRAINT DF_stockfreeze_job_type DEFAULT (1) ,
	sku_id int  NOT NULL  CONSTRAINT DF_stockfreeze_sku_id DEFAULT (0) ,
	goods_owner_id int  NOT NULL  CONSTRAINT DF_stockfreeze_goods_owner_id DEFAULT (0) ,
	goods_location_id int  NOT NULL  CONSTRAINT DF_stockfreeze_goods_location_id DEFAULT (0) ,
	handler varchar(64)  NOT NULL  CONSTRAINT DF_stockfreeze_handler DEFAULT ('') ,
	handle_time datetime  NOT NULL  CONSTRAINT DF_stockfreeze_handle_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_stockfreeze_last_update_time DEFAULT (N'1000-01-01') ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'stockprocess' AND type = 'U')
BEGIN 
CREATE TABLE stockprocess (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_stockprocess_ID PRIMARY KEY CLUSTERED  ,
	job_code varchar(32)  NOT NULL  CONSTRAINT DF_stockprocess_job_code DEFAULT ('') ,
	job_type bit  NOT NULL  CONSTRAINT DF_stockprocess_job_type DEFAULT (0) ,
	process_status bit  NOT NULL  CONSTRAINT DF_stockprocess_process_status DEFAULT (0) ,
	processor varchar(64)  NOT NULL  CONSTRAINT DF_stockprocess_processor DEFAULT ('') ,
	process_time datetime  NOT NULL  CONSTRAINT DF_stockprocess_process_time DEFAULT (N'1000-01-01') ,
	creator varchar(64)  NOT NULL  CONSTRAINT DF_stockprocess_creator DEFAULT ('') ,
	create_time datetime  NOT NULL  CONSTRAINT DF_stockprocess_create_time DEFAULT (N'1000-01-01') ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_stockprocess_last_update_time DEFAULT (N'1000-01-01') ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'stockprocessdetail' AND type = 'U')
BEGIN 
CREATE TABLE stockprocessdetail (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_stockprocessdetail_ID PRIMARY KEY CLUSTERED  ,
	stock_process_id int  NOT NULL  CONSTRAINT DF_stockprocessdetail_stock_process_id DEFAULT (0) ,
	sku_id int  NOT NULL  CONSTRAINT DF_stockprocessdetail_sku_id DEFAULT (0) ,
	goods_owner_id int  NOT NULL  CONSTRAINT DF_stockprocessdetail_goods_owner_id DEFAULT (0) ,
	goods_location_id int  NOT NULL  CONSTRAINT DF_stockprocessdetail_goods_location_id DEFAULT (0) ,
	qty int  NOT NULL  CONSTRAINT DF_stockprocessdetail_qty DEFAULT (0) ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_stockprocessdetail_last_update_time DEFAULT (N'1000-01-01') ,
	tenant_id bigint  NOT NULL  ,
	is_source bit  NOT NULL  CONSTRAINT DF_stockprocessdetail_is_source DEFAULT (0) ,
	is_update_stock bit  NOT NULL  CONSTRAINT DF_stockprocessdetail_is_update_stock DEFAULT (0) 
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'dispatchlist' AND type = 'U')
BEGIN 
CREATE TABLE dispatchlist (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_dispatchlist_ID PRIMARY KEY CLUSTERED  ,
	dispatch_no varchar(32)  NOT NULL  CONSTRAINT DF_dispatchlist_dispatch_no DEFAULT ('') ,
	dispatch_status tinyint  NOT NULL  CONSTRAINT DF_dispatchlist_dispatch_status DEFAULT (0) ,
	customer_id int  NOT NULL  CONSTRAINT DF_dispatchlist_customer_id DEFAULT (0) ,
	customer_name varchar(256)  NOT NULL  CONSTRAINT DF_dispatchlist_customer_name DEFAULT ('') ,
	sku_id int  NOT NULL  CONSTRAINT DF_dispatchlist_sku_id DEFAULT (0) ,
	qty int  NOT NULL  CONSTRAINT DF_dispatchlist_qty DEFAULT (0) ,
	weight decimal(12,3)  NOT NULL  CONSTRAINT DF_dispatchlist_weight DEFAULT (0) ,
	volume decimal(15,3)  NOT NULL  CONSTRAINT DF_dispatchlist_volume DEFAULT (0) ,
	creator varchar(64)  NOT NULL  CONSTRAINT DF_dispatchlist_creator DEFAULT ('') ,
	create_time datetime  NOT NULL  CONSTRAINT DF_dispatchlist_create_time DEFAULT (N'1000-01-01') ,
	damage_qty int  NOT NULL  CONSTRAINT DF_dispatchlist_damage_qty DEFAULT (0) ,
	lock_qty int  NOT NULL  CONSTRAINT DF_dispatchlist_lock_qty DEFAULT (0) ,
	picked_qty int  NOT NULL  CONSTRAINT DF_dispatchlist_picked_qty DEFAULT (0) ,
	intrasit_qty int  NOT NULL  CONSTRAINT DF_dispatchlist_intrasit_qty DEFAULT (0) ,
	package_qty int  NOT NULL  CONSTRAINT DF_dispatchlist_package_qty DEFAULT (0) ,
	weighing_qty int  NOT NULL  CONSTRAINT DF_dispatchlist_weighing_qty DEFAULT (0) ,
	actual_qty int  NOT NULL  CONSTRAINT DF_dispatchlist_actual_qty DEFAULT (0) ,
	sign_qty int  NOT NULL  CONSTRAINT DF_dispatchlist_sign_qty DEFAULT (0) ,
	package_no varchar(32)  NOT NULL  CONSTRAINT DF_dispatchlist_package_no DEFAULT ('') ,
	package_person varchar(64)  NOT NULL  CONSTRAINT DF_dispatchlist_package_person DEFAULT ('') ,
	package_time datetime  NOT NULL  CONSTRAINT DF_dispatchlist_package_time DEFAULT (N'1000-01-01') ,
	weighing_no varchar(32)  NOT NULL  CONSTRAINT DF_dispatchlist_weighing_no DEFAULT ('') ,
	weighing_person varchar(64)  NOT NULL  CONSTRAINT DF_dispatchlist_weighing_person DEFAULT ('') ,
	weighing_weight decimal(15,3)  NOT NULL  CONSTRAINT DF_dispatchlist_weighing_weight DEFAULT (0) ,
	waybill_no varchar(64)  NOT NULL  CONSTRAINT DF_dispatchlist_waybill_no DEFAULT ('') ,
	carrier varchar(256)  NOT NULL  CONSTRAINT DF_dispatchlist_carrier DEFAULT ('') ,
	freightfee decimal(10,2)  NOT NULL  CONSTRAINT DF_dispatchlist_freightfee DEFAULT (0) ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_dispatchlist_last_update_time DEFAULT (N'1000-01-01') ,
	tenant_id bigint  NOT NULL  
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO 
IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'dispatchpicklist' AND type = 'U')
BEGIN 
CREATE TABLE dispatchpicklist (
	id int IDENTITY(1,1)  NOT NULL CONSTRAINT PK_dispatchpicklist_ID PRIMARY KEY CLUSTERED  ,
	dispatchlist_id int  NOT NULL  CONSTRAINT DF_dispatchpicklist_dispatchlist_id DEFAULT (0) ,
	goods_owner_id int  NOT NULL  CONSTRAINT DF_dispatchpicklist_goods_owner_id DEFAULT (0) ,
	goods_location_id int  NOT NULL  CONSTRAINT DF_dispatchpicklist_goods_location_id DEFAULT (0) ,
	sku_id int  NOT NULL  CONSTRAINT DF_dispatchpicklist_sku_id DEFAULT (0) ,
	pick_qty int  NOT NULL  CONSTRAINT DF_dispatchpicklist_pick_qty DEFAULT (0) ,
	picked_qty int  NOT NULL  CONSTRAINT DF_dispatchpicklist_picked_qty DEFAULT (0) ,
	is_update_stock bit  NOT NULL  CONSTRAINT DF_dispatchpicklist_is_update_stock DEFAULT (0) ,
	last_update_time datetime  NOT NULL  CONSTRAINT DF_dispatchpicklist_last_update_time DEFAULT (N'1000-01-01') 
) 
END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
insert into menu(menu_name, module, vue_path, vue_directory, tenant_id, sort)
VALUES
('companySetting', 'baseModule', 'companySetting', 'base/companySetting', 1, 1),
('userRoleSetting', 'baseModule', 'userRoleSetting', 'base/userRoleSetting', 1, 2),
('roleMenu', 'baseModule', 'roleMenu', 'base/roleMenu', 1, 3),
('userManagement', 'baseModule', 'userManagement', 'base/userManagement', 1, 4),
('commodityCategorySetting', 'baseModule', 'commodityCategorySetting', 'base/commodityCategorySetting', 1, 5),
('commodityManagement', 'baseModule', 'commodityManagement', 'base/commodityManagement', 1, 6),
('supplier', 'baseModule', 'supplier', 'base/supplier', 1, 7),
('warehouseSetting', 'baseModule', 'warehouseSetting', 'base/warehouseSetting', 1, 8),
('ownerOfCargo', 'baseModule', 'ownerOfCargo', 'base/ownerOfCargo', 1, 9),
('freightSetting', 'baseModule', 'freightSetting', 'base/freightSetting', 1, 10),
('customer', 'baseModule', 'customer', 'base/customer', 1, 11),
('stockAsn', '', 'stockAsn', 'wms/stockAsn', 1, 2),
('stockManagement', '', 'stockManagement', 'wms/stockManagement', 1, 3),
('warehouseProcessing', 'warehouseWorkingModule', 'warehouseProcessing', 'warehouseWorking/warehouseProcessing', 1, 4),
('warehouseMove', 'warehouseWorkingModule', 'warehouseMove', 'warehouseWorking/warehouseMove', 1, 5),
('warehouseFreeze', 'warehouseWorkingModule', 'warehouseFreeze', 'warehouseWorking/warehouseFreeze', 1, 6),
('warehouseAdjust', 'warehouseWorkingModule', 'warehouseAdjust', 'warehouseWorking/warehouseAdjust', 1, 7),
('warehouseTaking', 'warehouseWorkingModule', 'warehouseTaking', 'warehouseWorking/warehouseTaking', 1, 8),
('deliveryManagement', '', 'deliveryManagement', 'deliveryManagement/deliveryManagement', 1, 5)
GO
insert into userrole ( role_name, is_valid, create_time, last_update_time, tenant_id) 
values('administrator',true,'2022-12-21 10:30:00','2022-12-23 08:26:36','1');
insert into user (id, user_num, user_name, contact_tel, user_role, sex, is_valid, auth_string, creator, create_time, last_update_time, tenant_id, email) 
values('1','admin','管理员','18559851','administrator','male',true,'c4ca4238a0b923820dcc509a6f75849b','admin','1000-01-01 00:00:00','2022-12-23 10:55:37','1','');

GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
COMMIT TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
DECLARE @Success AS BIT
SET @Success = 1
SET NOEXEC OFF
IF(@Success = 1) PRINT 'The database update succeeded'
ELSE BEGIN
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
    PRINT 'The database update failed'
END
GO

