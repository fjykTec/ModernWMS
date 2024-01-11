using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// asn summary output viewModel
    /// </summary>
    public class AnsSummaryViewModel
    {
        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public AnsSummaryViewModel()
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
        /// asn_no
        /// </summary>
        [Display(Name = "asn_no")]
        [MaxLength(32, ErrorMessage = "MaxLength")]
        public string asn_no { get; set; } = string.Empty;

        /// <summary>
        /// asn_batch
        /// </summary>
        public string asn_batch { get; set; } = string.Empty;

        /// <summary>
        /// goods_location_id
        /// </summary>
        [Display(Name = "goods_location_id")]
        public int goods_location_id { get; set; } = 0;

        /// <summary>
        /// warehouse_name
        /// </summary>
        public string warehouse_name { get; set; } = string.Empty;

        /// <summary>
        /// location_name
        /// </summary>
        public string location_name { get; set; } = string.Empty;

        /// <summary>
        /// spu_id
        /// </summary>
        [Display(Name = "spu_id")]
        [Required(ErrorMessage = "Required")]
        public int spu_id { get; set; } = 0;

        /// <summary>
        /// spu_name
        /// </summary>
        [Display(Name = "spu_name")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(200, ErrorMessage = "MaxLength")]
        public string spu_name { get; set; } = string.Empty;

        /// <summary>
        /// spu_code
        /// </summary>
        [Display(Name = "spu_code")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(32, ErrorMessage = "MaxLength")]
        public string spu_code { get; set; } = string.Empty;

        /// <summary>
        /// sku_id
        /// </summary>
        [Display(Name = "sku_id")]
        public int sku_id { get; set; } = 0;

        /// <summary>
        /// sku_name
        /// </summary>
        [Display(Name = "sku_name")]
        [MaxLength(200, ErrorMessage = "MaxLength")]
        public string sku_name { get; set; } = string.Empty;

        /// <summary>
        /// supplier_id
        /// </summary>
        [Display(Name = "supplier_id")]
        public int supplier_id { get; set; } = 0;

        /// <summary>
        /// supplier_name
        /// </summary>
        [Display(Name = "supplier_name")]
        [MaxLength(256, ErrorMessage = "MaxLength")]
        public string supplier_name { get; set; } = string.Empty;

        /// <summary>
        /// series_number
        /// </summary>
        public string series_number { get; set; } = string.Empty;

        /// <summary>
        /// goods_owner_id
        /// </summary>
        [Display(Name = "goods_owner_id")]
        public int goods_owner_id { get; set; } = 0;

        /// <summary>
        /// goods_owner_name
        /// </summary>
        [Display(Name = "goods_owner_name")]
        [MaxLength(256, ErrorMessage = "MaxLength")]
        public string goods_owner_name { get; set; } = string.Empty;


        #endregion
    }
}
