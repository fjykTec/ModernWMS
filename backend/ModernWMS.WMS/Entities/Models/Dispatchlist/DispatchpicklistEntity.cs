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
    /// dispatchpicklist  entity
    /// </summary>
    [Table("dispatchpicklist")]
    public class DispatchpicklistEntity : BaseModel
    {
        #region foreign table

        /// <summary>
        /// foreign table
        /// </summary>
        [ForeignKey("dispatchlist_id")]
        public DispatchlistEntity Dispatchlist { get; set; }

        #endregion foreign table

        #region Property

        /// <summary>
        /// dispatchlist_id
        /// </summary>
        public int dispatchlist_id { get; set; } = 0;

        /// <summary>
        /// goods_owner_id
        /// </summary>
        public int goods_owner_id { get; set; } = 0;

        /// <summary>
        /// goods_location_id
        /// </summary>
        public int goods_location_id { get; set; } = 0;

        /// <summary>
        /// sku_id
        /// </summary>
        public int sku_id { get; set; } = 0;

        /// <summary>
        /// pick_qty
        /// </summary>
        public int pick_qty { get; set; } = 0;

        /// <summary>
        /// picked_qty
        /// </summary>
        public int picked_qty { get; set; } = 0;

        /// <summary>
        /// is_update_stock
        /// </summary>
        public bool is_update_stock { get; set; } = false;

        /// <summary>
        /// last_update_time
        /// </summary>
        public DateTime last_update_time { get; set; } = UtilConvert.MinDate;

        /// <summary>
        /// series_number
        /// </summary>
        public string series_number { get; set; } = string.Empty;

        /// <summary>
        /// picker_id
        /// </summary>
        public int picker_id { get; set; } = 0;

        /// <summary>
        /// picker
        /// </summary>
        public string picker { get; set; } = string.Empty;

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


        #endregion Property
    }
}