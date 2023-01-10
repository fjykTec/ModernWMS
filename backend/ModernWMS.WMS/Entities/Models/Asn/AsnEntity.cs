/*
 * date：2022-12-22
 * developer：AMo
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ModernWMS.Core.Models;
using ModernWMS.Core.Utility;

namespace ModernWMS.WMS.Entities.Models
{
    /// <summary>
    /// asn  entity
    /// </summary>
    [Table("asn")]
    public class AsnEntity : BaseModel
    {

        #region Property

        /// <summary>
        /// asn_no
        /// </summary>
        public string asn_no { get; set; }  = string.Empty;

        /// <summary>
        /// asn_status
        /// </summary>
        public byte asn_status { get; set; }  = 0;

        /// <summary>
        /// spu_id
        /// </summary>
        public int spu_id { get; set; }  = 0;

        /// <summary>
        /// sku_id
        /// </summary>
        public int sku_id { get; set; }  = 0;

        /// <summary>
        /// asn_qty
        /// </summary>
        public int asn_qty { get; set; }  = 0;

        /// <summary>
        /// actual_qty
        /// </summary>
        public int actual_qty { get; set; }  = 0;

        /// <summary>
        /// sorted_qty
        /// </summary>
        public int sorted_qty { get; set; }  = 0;

        /// <summary>
        /// shortage_qty
        /// </summary>
        public int shortage_qty { get; set; }  = 0;

        /// <summary>
        /// more_qty
        /// </summary>
        public int more_qty { get; set; }  = 0;

        /// <summary>
        /// damage_qty
        /// </summary>
        public int damage_qty { get; set; }  = 0;

        /// <summary>
        /// weight
        /// </summary>
        public decimal weight { get; set; }  = 0;

        /// <summary>
        /// volume
        /// </summary>
        public decimal volume { get; set; }  = 0;

        /// <summary>
        /// supplier_id
        /// </summary>
        public int supplier_id { get; set; }  = 0;

        /// <summary>
        /// supplier_name
        /// </summary>
        public string supplier_name { get; set; }  = string.Empty;

        /// <summary>
        /// goods_owner_id
        /// </summary>
        public int goods_owner_id { get; set; }  = 0;

        /// <summary>
        /// goods_owner_name
        /// </summary>
        public string goods_owner_name { get; set; }  = string.Empty;

        /// <summary>
        /// creator
        /// </summary>
        public string creator { get; set; }  = string.Empty;

        /// <summary>
        /// create_time
        /// </summary>
        public DateTime create_time { get; set; }  = DateTime.Now;

        /// <summary>
        /// last_update_time
        /// </summary>
        public DateTime last_update_time { get; set; }  = DateTime.Now;

        /// <summary>
        /// is_valid
        /// </summary>
        public bool is_valid { get; set; } = true;

        /// <summary>
        /// tenant_id
        /// </summary>
        public long tenant_id { get; set; }  = 1;


        #endregion

    }
}
