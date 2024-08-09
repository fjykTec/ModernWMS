/*
 * date：2022-12-27
 * developer：NoNo
 */

using System;
using System.ComponentModel.DataAnnotations;
using ModernWMS.Core.Utility;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// stockmove viewModel
    /// </summary>
    public class StockmoveViewModel
    {
        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public StockmoveViewModel()
        {
        }

        #endregion constructor

        #region Property

        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
        public int id { get; set; } = 0;

        /// <summary>
        /// job_code
        /// </summary>
        [Display(Name = "job_code")]
        [MaxLength(32, ErrorMessage = "MaxLength")]
        public string job_code { get; set; } = string.Empty;

        /// <summary>
        /// move_status
        /// </summary>
        [Display(Name = "move_status")]
        public byte move_status { get; set; } = 0;

        /// <summary>
        /// sku_id
        /// </summary>
        [Display(Name = "sku_id")]
        public int sku_id { get; set; } = 0;

        /// <summary>
        /// orig_goods_location_id
        /// </summary>
        [Display(Name = "orig_goods_location_id")]
        public int orig_goods_location_id { get; set; } = 0;

        /// <summary>
        /// dest_googs_location_id
        /// </summary>
        [Display(Name = "dest_googs_location_id")]
        public int dest_googs_location_id { get; set; } = 0;

        /// <summary>
        /// qty
        /// </summary>
        [Display(Name = "qty")]
        public int qty { get; set; } = 0;

        /// <summary>
        /// goods_owner_id
        /// </summary>
        [Display(Name = "goods_owner_id")]
        public int goods_owner_id { get; set; } = 0;

        /// <summary>
        /// handler
        /// </summary>
        [Display(Name = "handler")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string handler { get; set; } = string.Empty;

        /// <summary>
        /// handle_time
        /// </summary>
        [Display(Name = "handle_time")]
        [DataType(DataType.DateTime, ErrorMessage = "DataType_DateTime")]
        public DateTime handle_time { get; set; } = UtilConvert.MinDate;

        /// <summary>
        /// creator
        /// </summary>
        [Display(Name = "creator")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string creator { get; set; } = string.Empty;

        /// <summary>
        /// create_time
        /// </summary>
        [Display(Name = "create_time")]
        [DataType(DataType.DateTime, ErrorMessage = "DataType_DateTime")]
        public DateTime create_time { get; set; } = UtilConvert.MinDate;

        /// <summary>
        /// last_update_time
        /// </summary>
        [Display(Name = "last_update_time")]
        [DataType(DataType.DateTime, ErrorMessage = "DataType_DateTime")]
        public DateTime last_update_time { get; set; } = UtilConvert.MinDate;

        /// <summary>
        /// tenant_id
        /// </summary>
        [Display(Name = "tenant_id")]
        public long tenant_id { get; set; } = 0;

        /// <summary>
        /// origin warehouse
        /// </summary>
        public string orig_goods_warehouse { get; set; } = string.Empty;

        /// <summary>
        /// origin location_name
        /// </summary>
        public string orig_goods_location_name { get; set; } = string.Empty;

        /// <summary>
        /// destination warehouse
        /// </summary>
        public string dest_googs_warehouse { get; set; } = string.Empty;

        /// <summary>
        /// destination location_name
        /// </summary>
        public string dest_googs_location_name { get; set; } = string.Empty;

        /// <summary>
        /// spu_code
        /// </summary>
        public string spu_code { get; set; } = string.Empty;

        /// <summary>
        /// spu_name
        /// </summary>
        public string spu_name { get; set; } = string.Empty;

        /// <summary>
        /// sku_code
        /// </summary>
        public string sku_code { get; set; } = string.Empty;

        /// <summary>
        /// sku_name
        /// </summary>
        public string sku_name { get; set; } = string.Empty;

        /// <summary>
        /// series_number
        /// </summary>
        [Display(Name = "series_number")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
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


        #endregion Property
    }
}