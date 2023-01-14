create database wms;
use wms;
CREATE TABLE IF NOT EXISTS `company` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`company_name` varchar(256) NOT NULL  DEFAULT ''  ,
	`city` varchar(128) NOT NULL  DEFAULT ''  ,
	`address` varchar(256) NOT NULL  DEFAULT ''  ,
	`manager` varchar(64) NOT NULL  DEFAULT ''  ,
	`contact_tel` varchar(64) NOT NULL  DEFAULT ''  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `supplier` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`supplier_name` varchar(256) NOT NULL  DEFAULT ''  ,
	`city` varchar(128) NOT NULL  DEFAULT ''  ,
	`address` varchar(256) NOT NULL  DEFAULT ''  ,
	`email` varchar(128) NOT NULL  DEFAULT ''  ,
	`manager` varchar(64) NOT NULL  DEFAULT ''  ,
	`contact_tel` varchar(64) NOT NULL  DEFAULT ''  ,
	`creator` varchar(64) NOT NULL  DEFAULT ''  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`is_valid` bit  NOT NULL  DEFAULT 1  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `customer` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`customer_name` varchar(256) NOT NULL  DEFAULT ''  ,
	`city` varchar(128) NOT NULL  DEFAULT ''  ,
	`address` varchar(256) NOT NULL  DEFAULT ''  ,
	`email` varchar(128) NOT NULL  DEFAULT ''  ,
	`manager` varchar(64) NOT NULL  DEFAULT ''  ,
	`contact_tel` varchar(64) NOT NULL  DEFAULT ''  ,
	`creator` varchar(64) NOT NULL  DEFAULT ''  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`is_valid` bit  NOT NULL  DEFAULT 1  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `warehouse` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`warehouse_name` varchar(32) NOT NULL  DEFAULT ''  ,
	`city` varchar(128) NOT NULL  DEFAULT ''  ,
	`address` varchar(256) NOT NULL  DEFAULT ''  ,
	`email` varchar(128) NOT NULL  DEFAULT ''  ,
	`manager` varchar(64) NOT NULL  DEFAULT ''  ,
	`contact_tel` varchar(64) NOT NULL  DEFAULT ''  ,
	`creator` varchar(64) NOT NULL  DEFAULT ''  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`is_valid` bit  NOT NULL  DEFAULT 1  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `warehousearea` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`warehouse_id` int  NOT NULL  ,
	`area_name` varchar(32) NOT NULL  DEFAULT ''  ,
	`area_property` tinyint  NOT NULL  DEFAULT 0  ,
	`parent_id` int  NOT NULL  DEFAULT 0  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`is_valid` bit  NOT NULL  DEFAULT 1  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `goodslocation` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`warehouse_id` int  NOT NULL  DEFAULT 0  ,
	`warehouse_name` varchar(32) NOT NULL  DEFAULT ''  ,
	`warehouse_area_id` int  NOT NULL  DEFAULT 0  ,
	`warehouse_area_name` varchar(32) NOT NULL  DEFAULT ''  ,
	`warehouse_area_property` tinyint  NOT NULL  DEFAULT 0  ,
	`location_name` varchar(64) NOT NULL  DEFAULT ''  ,
	`location_length` decimal(5,2)  NOT NULL  DEFAULT 0  ,
	`location_width` decimal(5,2)  NOT NULL  DEFAULT 0  ,
	`location_heigth` decimal(5,2)  NOT NULL  DEFAULT 0  ,
	`location_volume` decimal(12,2)  NOT NULL  DEFAULT 0  ,
	`location_load` decimal(8,2)  NOT NULL  DEFAULT 0  ,
	`roadway_number` varchar(10) NOT NULL  DEFAULT ''  ,
	`shelf_number` varchar(10) NOT NULL  DEFAULT ''  ,
	`layer_number` varchar(10) NOT NULL  DEFAULT ''  ,
	`tag_number` varchar(10) NOT NULL  DEFAULT ''  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`is_valid` bit  NOT NULL  DEFAULT 1  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `freightfee` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`carrier` varchar(256) NOT NULL  DEFAULT ''  ,
	`departure_city` varchar(128) NOT NULL  DEFAULT ''  ,
	`arrival_city` varchar(128) NOT NULL  DEFAULT ''  ,
	`price_per_weight` decimal(6,2)  NOT NULL  DEFAULT 0  ,
	`price_per_volume` decimal(6,2)  NOT NULL  DEFAULT 0  ,
	`min_payment` decimal(8,2)  NOT NULL  DEFAULT 0  ,
	`creator` varchar(64) NOT NULL  DEFAULT ''  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`is_valid` bit  NOT NULL  DEFAULT 1  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `category` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`category_name` varchar(32) NOT NULL  DEFAULT ''  ,
	`parent_id` int  NOT NULL  DEFAULT 0  ,
	`creator` varchar(64) NOT NULL  DEFAULT ''  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`is_valid` bit  NOT NULL  DEFAULT 1  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `spu` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`spu_code` varchar(32) NOT NULL  DEFAULT ''  ,
	`spu_name` varchar(200) NOT NULL  DEFAULT ''  ,
	`category_id` int  NOT NULL  DEFAULT 0  ,
	`spu_description` varchar(1000) NOT NULL  DEFAULT ''  ,
	`bar_code` varchar(64) NOT NULL  DEFAULT ''  ,
	`supplier_id` int  NOT NULL  DEFAULT 0  ,
	`supplier_name` varchar(256) NOT NULL  DEFAULT ''  ,
	`brand` varchar(128) NOT NULL  DEFAULT ''  ,
	`origin` varchar(256) NOT NULL  DEFAULT ''  ,
	`length_unit` tinyint  NOT NULL  DEFAULT 1  ,
	`volume_unit` tinyint  NOT NULL  DEFAULT 0  ,
	`weight_unit` tinyint  NOT NULL  DEFAULT 1  ,
	`creator` varchar(64) NOT NULL  DEFAULT ''  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`is_valid` bit  NOT NULL  DEFAULT 1  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `sku` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`spu_id` int  NOT NULL  DEFAULT 0  ,
	`sku_code` varchar(32) NOT NULL  DEFAULT ''  ,
	`sku_name` varchar(200) NOT NULL  DEFAULT ''  ,
	`weight` decimal(8,3)  NOT NULL  DEFAULT 0  ,
	`lenght` decimal(8,3)  NOT NULL  DEFAULT 0  ,
	`width` decimal(8,3)  NOT NULL  DEFAULT 0  ,
	`height` decimal(8,3)  NOT NULL  DEFAULT 0  ,
	`volume` decimal(15,3)  NOT NULL  DEFAULT 0  ,
	`unit` varchar(5) NOT NULL  DEFAULT ''  ,
	`cost` decimal(10,2)  NOT NULL  DEFAULT 0  ,
	`price` decimal(10,2)  NOT NULL  DEFAULT 0  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  
);
CREATE TABLE IF NOT EXISTS `user` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`user_num` varchar(128) NOT NULL  DEFAULT ''  ,
	`user_name` varchar(128) NOT NULL  DEFAULT ''  ,
	`contact_tel` varchar(64) NOT NULL  DEFAULT ''  ,
	`user_role` varchar(128) NOT NULL  DEFAULT ''  ,
	`sex` varchar(10) NOT NULL  DEFAULT ''  ,
	`is_valid` bit  NOT NULL  DEFAULT 1  ,
	`auth_string` varchar(64) NOT NULL  DEFAULT ''  ,
	`email` varchar(128) NOT NULL  DEFAULT ''  ,
	`creator` varchar(64) NOT NULL  DEFAULT ''  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `userrole` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`role_name` varchar(32) NOT NULL  DEFAULT ''  ,
	`is_valid` bit  NOT NULL  DEFAULT 1  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `menu` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`menu_name` varchar(32) NOT NULL  DEFAULT ''  ,
	`module` varchar(32) NOT NULL  DEFAULT ''  ,
	`vue_path` varchar(64) NOT NULL  DEFAULT ''  ,
	`vue_path_detail` varchar(64) NOT NULL  DEFAULT ''  ,
	`vue_directory` varchar(128) NOT NULL  DEFAULT ''  ,
	`sort` int  NOT NULL  DEFAULT 0  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `rolemenu` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`userrole_id` int  NOT NULL  DEFAULT 0  ,
	`menu_id` int  NOT NULL  DEFAULT 0  ,
	`authority` tinyint  NOT NULL  DEFAULT 0  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `goodsowner` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`goods_owner_name` varchar(256) NOT NULL  DEFAULT ''  ,
	`city` varchar(128) NOT NULL  DEFAULT ''  ,
	`address` varchar(256) NOT NULL  DEFAULT ''  ,
	`manager` varchar(64) NOT NULL  DEFAULT ''  ,
	`contact_tel` varchar(64) NOT NULL  DEFAULT ''  ,
	`creator` varchar(64) NOT NULL  DEFAULT ''  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`is_valid` bit  NOT NULL  DEFAULT 1  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `asn` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`asn_no` varchar(32) NOT NULL  DEFAULT ''  ,
	`asn_status` tinyint  NOT NULL  DEFAULT 0  ,
	`spu_id` int  NOT NULL  DEFAULT 0  ,
	`sku_id` int  NOT NULL  DEFAULT 0  ,
	`asn_qty` int  NOT NULL  DEFAULT 0  ,
	`actual_qty` int  NOT NULL  DEFAULT 0  ,
	`sorted_qty` int  NOT NULL  DEFAULT 0  ,
	`shortage_qty` int  NOT NULL  DEFAULT 0  ,
	`more_qty` int  NOT NULL  DEFAULT 0  ,
	`damage_qty` int  NOT NULL  DEFAULT 0  ,
	`weight` decimal(12,3)  NOT NULL  DEFAULT 0  ,
	`volume` decimal(15,3)  NOT NULL  DEFAULT 0  ,
	`supplier_id` int  NOT NULL  DEFAULT 0  ,
	`supplier_name` varchar(256) NOT NULL  DEFAULT ''  ,
	`goods_owner_id` int  NOT NULL  DEFAULT 0  ,
	`goods_owner_name` varchar(256) NOT NULL  DEFAULT ''  ,
	`creator` varchar(64) NOT NULL  DEFAULT ''  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`is_valid` bit  NOT NULL  DEFAULT 1  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `asnsort` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`asn_id` int  NOT NULL  DEFAULT 0  ,
	`sorted_qty` int  NOT NULL  DEFAULT 0  ,
	`creator` varchar(64) NOT NULL  DEFAULT ''  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`is_valid` bit  NOT NULL  DEFAULT 1  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `stock` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`sku_id` int  NOT NULL  DEFAULT 0  ,
	`goods_location_id` int  NOT NULL  DEFAULT 0  ,
	`qty` int  NOT NULL  DEFAULT 0  ,
	`goods_owner_id` int  NOT NULL  DEFAULT 0  ,
	`is_freeze` bit  NOT NULL  DEFAULT 0  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `stockmove` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`job_code` varchar(32) NOT NULL  DEFAULT ''  ,
	`move_status` tinyint  NOT NULL  DEFAULT 0  ,
	`sku_id` int  NOT NULL  DEFAULT 0  ,
	`orig_goods_location_id` int  NOT NULL  DEFAULT 0  ,
	`dest_googs_location_id` int  NOT NULL  DEFAULT 0  ,
	`qty` int  NOT NULL  DEFAULT 0  ,
	`goods_owner_id` int  NOT NULL  DEFAULT 0  ,
	`handler` varchar(64) NOT NULL  DEFAULT ''  ,
	`handle_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`creator` varchar(64) NOT NULL  DEFAULT ''  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `stocktaking` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`job_code` varchar(32) NOT NULL  DEFAULT ''  ,
	`job_status` bit  NOT NULL  DEFAULT 0  ,
	`sku_id` int  NOT NULL  DEFAULT 0  ,
	`goods_owner_id` int  NOT NULL  DEFAULT 0  ,
	`goods_location_id` int  NOT NULL  DEFAULT 0  ,
	`book_qty` int  NOT NULL  DEFAULT 0  ,
	`counted_qty` int  NOT NULL  DEFAULT 0  ,
	`difference_qty` int  NOT NULL  DEFAULT 0  ,
	`handler` varchar(64) NOT NULL  DEFAULT ''  ,
	`handle_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`creator` varchar(64) NOT NULL  DEFAULT ''  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `stockadjust` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`job_code` varchar(32) NOT NULL  DEFAULT ''  ,
	`job_type` tinyint  NOT NULL  DEFAULT 0  ,
	`sku_id` int  NOT NULL  DEFAULT 0  ,
	`goods_owner_id` int  NOT NULL  DEFAULT 0  ,
	`goods_location_id` int  NOT NULL  DEFAULT 0  ,
	`qty` int  NOT NULL  DEFAULT 0  ,
	`creator` varchar(64) NOT NULL  DEFAULT ''  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`tenant_id` bigint  NOT NULL  ,
	`is_update_stock` bit  NOT NULL  DEFAULT 0  ,
	`source_table_id` int  NOT NULL  DEFAULT 0  
);
CREATE TABLE IF NOT EXISTS `stockfreeze` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`job_code` varchar(32) NOT NULL  DEFAULT ''  ,
	`job_type` bit  NOT NULL  DEFAULT 1  ,
	`sku_id` int  NOT NULL  DEFAULT 0  ,
	`goods_owner_id` int  NOT NULL  DEFAULT 0  ,
	`goods_location_id` int  NOT NULL  DEFAULT 0  ,
	`handler` varchar(64) NOT NULL  DEFAULT ''  ,
	`handle_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `stockprocess` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`job_code` varchar(32) NOT NULL  DEFAULT ''  ,
	`job_type` bit  NOT NULL  DEFAULT 0  ,
	`process_status` bit  NOT NULL  DEFAULT 0  ,
	`processor` varchar(64) NOT NULL  DEFAULT ''  ,
	`process_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`creator` varchar(64) NOT NULL  DEFAULT ''  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `stockprocessdetail` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`stock_process_id` int  NOT NULL  DEFAULT 0  ,
	`sku_id` int  NOT NULL  DEFAULT 0  ,
	`goods_owner_id` int  NOT NULL  DEFAULT 0  ,
	`goods_location_id` int  NOT NULL  DEFAULT 0  ,
	`qty` int  NOT NULL  DEFAULT 0  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`tenant_id` bigint  NOT NULL  ,
	`is_source` bit  NOT NULL  DEFAULT 0  ,
	`is_update_stock` bit  NOT NULL  DEFAULT 0  
);
CREATE TABLE IF NOT EXISTS `dispatchlist` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`dispatch_no` varchar(32) NOT NULL  DEFAULT ''  ,
	`dispatch_status` tinyint  NOT NULL  DEFAULT 0  ,
	`customer_id` int  NOT NULL  DEFAULT 0  ,
	`customer_name` varchar(256) NOT NULL  DEFAULT ''  ,
	`sku_id` int  NOT NULL  DEFAULT 0  ,
	`qty` int  NOT NULL  DEFAULT 0  ,
	`weight` decimal(12,3)  NOT NULL  DEFAULT 0  ,
	`volume` decimal(15,3)  NOT NULL  DEFAULT 0  ,
	`creator` varchar(64) NOT NULL  DEFAULT ''  ,
	`create_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`damage_qty` int  NOT NULL  DEFAULT 0  ,
	`lock_qty` int  NOT NULL  DEFAULT 0  ,
	`picked_qty` int  NOT NULL  DEFAULT 0  ,
	`intrasit_qty` int  NOT NULL  DEFAULT 0  ,
	`package_qty` int  NOT NULL  DEFAULT 0  ,
	`weighing_qty` int  NOT NULL  DEFAULT 0  ,
	`actual_qty` int  NOT NULL  DEFAULT 0  ,
	`sign_qty` int  NOT NULL  DEFAULT 0  ,
	`package_no` varchar(32) NOT NULL  DEFAULT ''  ,
	`package_person` varchar(64) NOT NULL  DEFAULT ''  ,
	`package_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`weighing_no` varchar(32) NOT NULL  DEFAULT ''  ,
	`weighing_person` varchar(64) NOT NULL  DEFAULT ''  ,
	`weighing_weight` decimal(15,3)  NOT NULL  DEFAULT 0  ,
	`waybill_no` varchar(64) NOT NULL  DEFAULT ''  ,
	`carrier` varchar(256) NOT NULL  DEFAULT ''  ,
	`freightfee` decimal(10,2)  NOT NULL  DEFAULT 0  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  ,
	`tenant_id` bigint  NOT NULL  
);
CREATE TABLE IF NOT EXISTS `dispatchpicklist` (
	`id` int AUTO_INCREMENT  NOT NULL PRIMARY KEY  ,
	`dispatchlist_id` int  NOT NULL  DEFAULT 0  ,
	`goods_owner_id` int  NOT NULL  DEFAULT 0  ,
	`goods_location_id` int  NOT NULL  DEFAULT 0  ,
	`sku_id` int  NOT NULL  DEFAULT 0  ,
	`pick_qty` int  NOT NULL  DEFAULT 0  ,
	`picked_qty` int  NOT NULL  DEFAULT 0  ,
	`is_update_stock` bit  NOT NULL  DEFAULT 0  ,
	`last_update_time` datetime  NOT NULL  DEFAULT '1000-01-01'  
);
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
insert into `userrole` ( `role_name`, `is_valid`, `create_time`, `last_update_time`, `tenant_id`) 
values('administrator',true,'2022-12-21 10:30:00','2022-12-23 08:26:36','1');
insert into `user` (`id`, `user_num`, `user_name`, `contact_tel`, `user_role`, `sex`, `is_valid`, `auth_string`, `creator`, `create_time`, `last_update_time`, `tenant_id`, `email`) 
values('1','admin','管理员','18559851','administrator','male',true,'c4ca4238a0b923820dcc509a6f75849b','admin','1000-01-01 00:00:00','2022-12-23 10:55:37','1','');


