/*
 * date：2022-12-30
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
    /// stocktaking  entity
    /// </summary>
    [Table("stocktaking")]
    public class StocktakingEntity : BaseModel
    {

        #region Property

        /// <summary>
        /// job_code
        /// </summary>
        public string job_code { get; set; }  = string.Empty;

        /// <summary>
        /// job_status
        /// </summary>
        public bool job_status { get; set; } = false;

        /// <summary>
        /// sku_id
        /// </summary>
        public int sku_id { get; set; }  = 0;

        /// <summary>
        /// goods_owner_id
        /// </summary>
        public int goods_owner_id { get; set; }  = 0;

        /// <summary>
        /// goods_location_id
        /// </summary>
        public int goods_location_id { get; set; }  = 0;

        /// <summary>
        /// series_number
        /// </summary>
        public string series_number { get; set; } = string.Empty;

        /// <summary>
        /// expiry_date
        /// </summary>
        public DateTime expiry_date { get; set; } = UtilConvert.MinDate;

        /// <summary>
        /// price
        /// </summary>
        public decimal price { get; set; } = 0;

        /// <summary>
        /// putaway_date
        /// </summary>
        public DateTime putaway_date { get; set; } = UtilConvert.MinDate;

        /// <summary>
        /// book_qty
        /// </summary>
        public int book_qty { get; set; }  = 0;

        /// <summary>
        /// counted_qty
        /// </summary>
        public int counted_qty { get; set; }  = 0;

        /// <summary>
        /// difference_qty
        /// </summary>
        public int difference_qty { get; set; }  = 0;

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
        /// tenant_id
        /// </summary>
        public long tenant_id { get; set; }  = 1;

        /// <summary>
        /// handler
        /// </summary>
        public string handler { get; set; } = string.Empty;

        /// <summary>
        /// handle_time
        /// </summary>
        public DateTime handle_time { get; set; } = UtilConvert.MinDate;

        #endregion

    }
}
