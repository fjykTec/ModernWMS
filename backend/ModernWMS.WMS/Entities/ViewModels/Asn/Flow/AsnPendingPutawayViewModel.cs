/*
 * date：2023-09-05
 * developer：AMo
 */
using System.ComponentModel.DataAnnotations;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// pending putwaay viewModel
    /// </summary>
    public class AsnPendingPutawayViewModel
    {
        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public AsnPendingPutawayViewModel()
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
        /// goods_owner_id
        /// </summary>
        [Display(Name = "goods_owner_id")]
        public int goods_owner_id { get; set; } = 0;

        /// <summary>
        /// goods_owner_name
        /// </summary>
        [Display(Name = "goods_owner_name")]
        public string goods_owner_name { get; set; } = string.Empty;

        /// <summary>
        /// series_number
        /// </summary>
        [Display(Name = "series_number")]
        public string series_number { get; set; } = string.Empty;

        /// <summary>
        /// sorted_qty
        /// </summary>
        [Display(Name = "sorted_qty")]
        public int sorted_qty { get; set; } = 0;

        #endregion
    }
}
