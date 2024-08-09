/*
 * date：2023-08-30
 * developer：AMo
 */
using ModernWMS.Core.Utility;
using System.ComponentModel.DataAnnotations;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class AsnmasterDetailViewModel
    {

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public AsnmasterDetailViewModel()
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
        /// asnmaster_id
        /// </summary>
        public int asnmaster_id { get; set; } = 0;

        /// <summary>
        /// asn_status
        /// </summary>
        [Display(Name = "asn_status")]
        public byte asn_status { get; set; } = 0;

        /// <summary>
        /// spu_id
        /// </summary>
        [Display(Name = "spu_id")]
        [Required(ErrorMessage = "Required")]
        public int spu_id { get; set; } = 0;

        /// <summary>
        /// spu_code
        /// </summary>
        [Display(Name = "spu_code")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(32, ErrorMessage = "MaxLength")]
        public string spu_code { get; set; } = string.Empty;

        /// <summary>
        /// spu_name
        /// </summary>
        [Display(Name = "spu_name")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(200, ErrorMessage = "MaxLength")]
        public string spu_name { get; set; } = string.Empty;

        /// <summary>
        /// sku_id
        /// </summary>
        [Display(Name = "sku_id")]
        public int sku_id { get; set; } = 0;

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
        /// origin
        /// </summary>
        [Display(Name = "origin")]
        [MaxLength(256, ErrorMessage = "MaxLength")]
        public string origin { get; set; } = string.Empty;

        /// <summary>
        /// length_unit （0=毫米、1=厘米、2=分米、3=米）
        /// </summary>
        [Display(Name = "length_unit")]
        public byte length_unit { get; set; } = 0;

        /// <summary>
        /// volume_unit （0=立方厘米、1=立方分米、2=立方米）
        /// </summary>
        [Display(Name = "volume_unit")]
        public byte volume_unit { get; set; } = 0;

        /// <summary>
        /// weight_unit （0=毫克、1=克、2=千克）
        /// </summary>
        [Display(Name = "weight_unit")]
        public byte weight_unit { get; set; } = 0;

        /// <summary>
        /// asn_qty
        /// </summary>
        [Display(Name = "asn_qty")]
        public int asn_qty { get; set; } = 0;

        /// <summary>
        /// actual_qty
        /// </summary>
        [Display(Name = "actual_qty")]
        public int actual_qty { get; set; } = 0;

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
        /// is_valid
        /// </summary>
        [Display(Name = "is_valid")]
        public bool is_valid { get; set; } = true;

        /// <summary>
        /// expiry_date
        /// </summary>
        [Display(Name = "expiry_date")]
        public DateTime expiry_date { get; set; } = Core.Utility.UtilConvert.MinDate;

        /// <summary>
        /// price
        /// </summary>
        [Display(Name = "price")]
        public decimal price { get; set; } = 0;

        /// <summary>
        /// sorted_qty
        /// </summary>
        public int sorted_qty { get; set; } = 0;

        /// <summary>
        /// putaway_date
        /// </summary>
        public DateTime putaway_date { get; set; } = UtilConvert.MinDate;


        #endregion

    }
}
