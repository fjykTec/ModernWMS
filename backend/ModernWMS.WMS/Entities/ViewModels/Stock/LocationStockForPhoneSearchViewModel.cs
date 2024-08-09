/*
 * date：2023-9-3
 * developer：NoNo
 */

using ModernWMS.Core.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// get stock infomation by phone api input viewmodel
    /// </summary>
    public class LocationStockForPhoneSearchViewModel
    {
        /// <summary>
        /// sku_id
        /// </summary>
        public int sku_id { get; set; } = 0;

        /// <summary>
        /// goods_location_id
        /// </summary>
        public int goods_location_id { get; set; } = 0;

        /// <summary>
        /// warehouse_id
        /// </summary>
        public int warehouse_id { get; set; } = 0;

        /// <summary>
        /// spu name
        /// </summary>
        public string spu_name { get; set; } = string.Empty;

        /// <summary>
        /// location name
        /// </summary>
        public string location_name { get; set; } = string.Empty;

        /// <summary>
        /// expiry_date
        /// </summary>
        public DateTime expiry_date { get; set; } = UtilConvert.MinDate;

        /// <summary>
        /// series_number
        /// </summary>
        public string series_number { get; set; } = string.Empty;
    }
}