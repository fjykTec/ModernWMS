/*
 * date：2022-12-22
 * developer：NoNo
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
    /// dispatchlist  entity
    /// </summary>
    [Table("dispatchlist")]
    public class DispatchlistEntity : BaseModel
    {

        #region Property

        /// <summary>
        /// dispatch_no
        /// </summary>
        public string dispatch_no { get; set; }  = string.Empty;

        /// <summary>
        /// dispatch_status
        /// </summary>
        public byte dispatch_status { get; set; }  = 0;

        /// <summary>
        /// customer_id
        /// </summary>
        public int customer_id { get; set; }  = 0;

        /// <summary>
        /// customer_name
        /// </summary>
        public string customer_name { get; set; }  = string.Empty;

        /// <summary>
        /// sku_id
        /// </summary>
        public int sku_id { get; set; }  = 0;

        /// <summary>
        /// qty
        /// </summary>
        public int qty { get; set; }  = 0;

        /// <summary>
        /// weight
        /// </summary>
        public decimal weight { get; set; }  = 0;

        /// <summary>
        /// volume
        /// </summary>
        public decimal volume { get; set; }  = 0;

        /// <summary>
        /// creator
        /// </summary>
        public string creator { get; set; }  = string.Empty;

        /// <summary>
        /// create_time
        /// </summary>
        public DateTime create_time { get; set; }  = UtilConvert.MinDate;

        /// <summary>
        /// damage_qty
        /// </summary>
        public int damage_qty { get; set; }  = 0;

        /// <summary>
        /// lock_qty
        /// </summary>
        public int lock_qty { get; set; }  = 0;

        /// <summary>
        /// picked_qty
        /// </summary>
        public int picked_qty { get; set; }  = 0;

        /// <summary>
        /// intrasit_qty
        /// </summary>
        public int intrasit_qty { get; set; }  = 0;

        /// <summary>
        /// package_qty
        /// </summary>
        public int package_qty { get; set; }  = 0;

        /// <summary>
        /// weighing_qty
        /// </summary>
        public int weighing_qty { get; set; }  = 0;

        /// <summary>
        /// actual_qty
        /// </summary>
        public int actual_qty { get; set; }  = 0;

        /// <summary>
        /// sign_qty
        /// </summary>
        public int sign_qty { get; set; }  = 0;

        /// <summary>
        /// package_no
        /// </summary>
        public string package_no { get; set; }  = string.Empty;

        /// <summary>
        /// package_person
        /// </summary>
        public string package_person { get; set; }  = string.Empty;

        /// <summary>
        /// package_time
        /// </summary>
        public DateTime package_time { get; set; }  = UtilConvert.MinDate;

        /// <summary>
        /// weighing_no
        /// </summary>
        public string weighing_no { get; set; }  = string.Empty;

        /// <summary>
        /// weighing_person
        /// </summary>
        public string weighing_person { get; set; }  = string.Empty;

        /// <summary>
        /// weighing_weight
        /// </summary>
        public decimal weighing_weight { get; set; }  = 0;

        /// <summary>
        /// waybill_no
        /// </summary>
        public string waybill_no { get; set; }  = string.Empty;

        /// <summary>
        /// carrier
        /// </summary>
        public string carrier { get; set; }  = string.Empty;

        /// <summary>
        /// freightfee
        /// </summary>
        public decimal freightfee { get; set; }  = 0;

        /// <summary>
        /// last_update_time
        /// </summary>
        [ConcurrencyCheck]
        public DateTime last_update_time { get; set; }  = UtilConvert.MinDate;

        /// <summary>
        /// tenant_id
        /// </summary>
        public long tenant_id { get; set; }  = 0;

        /// <summary>
        /// pick_checker_id
        /// </summary>
        public int pick_checker_id { get; set; } = 0;

        /// <summary>
        /// pick_checker
        /// </summary>
        public string pick_checker { get; set; } = string.Empty;


        #endregion


        #region detail table

        /// <summary>
        /// detail table
        /// </summary>
        public List<DispatchpicklistEntity> detailList { get; set; } = new List<DispatchpicklistEntity>(2);

        #endregion

    }
}
