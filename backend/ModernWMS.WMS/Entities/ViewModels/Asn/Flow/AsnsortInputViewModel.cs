/*
 * date：2022-12-30
 * developer：AMo
 */
using System;
using System.ComponentModel.DataAnnotations;
using ModernWMS.Core.Utility;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// asn sorting input viewModel
    /// </summary>
    public class AsnsortInputViewModel
    {
        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public AsnsortInputViewModel()
        {

        }
        #endregion

        #region Property

        /// <summary>
        /// asn_id
        /// </summary>
        [Display(Name = "asn_id")]
        public int asn_id { get; set; } = 0;

        /// <summary>
        /// is automatically generate series number
        /// </summary>
        public bool is_auto_num { get; set; } = false;

        /// <summary>
        /// series_number
        /// </summary>
        public string series_number { get; set; } = string.Empty;

        /// <summary>
        /// sorted_qty
        /// </summary>
        [Display(Name = "sorted_qty")]
        public int sorted_qty { get; set; } = 0;


        /// <summary>
        /// expiry_date
        /// </summary>
        public DateTime expiry_date { get; set; } = Core.Utility.UtilConvert.MinDate;

        #endregion
    }
}
