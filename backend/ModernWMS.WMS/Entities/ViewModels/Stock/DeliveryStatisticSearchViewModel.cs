/*
 * date：2023-9-8
 * developer：NoNo
 */

using ModernWMS.Core.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.WMS.Entities.ViewModels.Stock
{
    /// <summary>
    /// delivery data statistic input viewModel
    /// </summary>
    public class DeliveryStatisticSearchViewModel
    {
        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public DeliveryStatisticSearchViewModel()
        {
        }

        #endregion constructor

        #region Property

        /// <summary>
        /// current page number
        /// </summary>
        public int pageIndex { get; set; } = 1;

        /// <summary>
        /// rows per page
        /// </summary>
        public int pageSize { get; set; } = 20;

        /// <summary>
        /// spu_code
        /// </summary>
        [Display(Name = "spu_code")]
        [MaxLength(32, ErrorMessage = "MaxLength")]
        public string spu_code { get; set; } = string.Empty;

        /// <summary>
        /// spu_name
        /// </summary>
        [Display(Name = "spu_name")]
        [MaxLength(200, ErrorMessage = "MaxLength")]
        public string spu_name { get; set; } = string.Empty;

        /// <summary>
        /// sku_code
        /// </summary>
        [Display(Name = "sku_code")]
        [MaxLength(32, ErrorMessage = "MaxLength")]
        public string sku_code { get; set; } = string.Empty;

        /// <summary>
        /// sku_name
        /// </summary>
        [Display(Name = "sku_name")]
        [MaxLength(200, ErrorMessage = "MaxLength")]
        public string sku_name { get; set; } = string.Empty;

        /// <summary>
        /// warehouse_name
        /// </summary>
        [Display(Name = "warehouse_name")]
        [MaxLength(32, ErrorMessage = "MaxLength")]
        public string warehouse_name { get; set; } = string.Empty;

        /// <summary>
        /// customer_name
        /// </summary>
        [Display(Name = "customer_name")]
        [MaxLength(256, ErrorMessage = "MaxLength")]
        public string customer_name { get; set; } = string.Empty;

        /// <summary>
        /// Delivery date from
        /// </summary>
        [Display(Name = "delivery_date_from")]
        [DataType(DataType.DateTime, ErrorMessage = "DataType_DateTime")]
        public DateTime delivery_date_from { get; set; } = UtilConvert.MinDate;

        /// <summary>
        /// Delivery date to
        /// </summary>
        [Display(Name = "delivery_date_to")]
        [DataType(DataType.DateTime, ErrorMessage = "DataType_DateTime")]
        public DateTime delivery_date_to { get; set; } = UtilConvert.MinDate;

        /// <summary>
        /// goods_owner_name
        /// </summary>
        [Display(Name = "goods_owner_name")]
        public string goods_owner_name { get; set; } = string.Empty;

        #endregion Property
    }
}