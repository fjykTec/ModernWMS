/*
 * date：2023-08-30
 * developer：AMo
 */
using System.ComponentModel.DataAnnotations;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// Asnmaster ViewModel
    /// </summary>
    public class AsnmasterBothViewModel
    {

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public AsnmasterBothViewModel()
        {

        }
        #endregion

        #region Property

        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
        public int id { get; set; } = 0;

        /// <summary>
        /// asn_no
        /// </summary>
        [Display(Name = "asn_no")]
        [MaxLength(32, ErrorMessage = "MaxLength")]
        public string asn_no { get; set; } = string.Empty;

        /// <summary>
        /// asn_batch
        /// </summary>
        [Display(Name = "asn_batch")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string asn_batch { get; set; } = string.Empty;

        /// <summary>
        /// estimated_arrival_time
        /// </summary>
        [Display(Name = "estimated_arrival_time")]
        public DateTime estimated_arrival_time { get; set; } = Core.Utility.UtilConvert.MinDate;

        /// <summary>
        /// asn_status
        /// </summary>
        [Display(Name = "asn_status")]
        public byte asn_status { get; set; } = 0;

        /// <summary>
        /// weight
        /// </summary>
        [Display(Name = "weight")]
        public decimal weight { get; set; } = 0;

        /// <summary>
        /// volume
        /// </summary>
        [Display(Name = "volume")]
        public decimal volume { get; set; } = 0;

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
        /// creator
        /// </summary>
        [Display(Name = "creator")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string creator { get; set; } = string.Empty;

        /// <summary>
        /// create_time
        /// </summary>
        [Display(Name = "create_time")]
        public DateTime create_time { get; set; } = DateTime.Now;

        /// <summary>
        /// last_update_time
        /// </summary>
        [Display(Name = "last_update_time")]
        public DateTime last_update_time { get; set; } = DateTime.Now;

        #endregion

        #region details
        /// <summary>
        /// details
        /// </summary>
        public List<AsnmasterDetailViewModel> detailList { get; set; } = new List<AsnmasterDetailViewModel>();
        #endregion
    }
}
