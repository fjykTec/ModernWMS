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
    public class StockAgeSearchViewModel
    {
        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public StockAgeSearchViewModel()
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
        /// location_name
        /// </summary>
        [Display(Name = "location_name")]
        [MaxLength(32, ErrorMessage = "MaxLength")]
        public string location_name { get; set; } = string.Empty;

        /// <summary>
        /// stock age date from
        /// </summary>
        public int stock_age_from { get; set; } = 0;

        /// <summary>
        /// stock age date to
        /// </summary>
        public int stock_age_to { get; set; } = 0;


        /// <summary>
        ///expiry date from
        /// </summary>
        [Display(Name = "expiry_date_from")]
        [DataType(DataType.DateTime, ErrorMessage = "DataType_DateTime")]
        public DateTime expiry_date_from { get; set; } = UtilConvert.MinDate;

        /// <summary>
        /// expiry date to
        /// </summary>
        [Display(Name = "expiry_date_to")]
        [DataType(DataType.DateTime, ErrorMessage = "DataType_DateTime")]
        public DateTime expiry_date_to { get; set; } = UtilConvert.MinDate;
        #endregion Property
    }
}