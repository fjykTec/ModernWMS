/*
 * date：2022-12-21
 * developer：AMo
 */
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// spu with sku viewModel
    /// </summary>
    public class SkuDetailViewModel
    {
        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public SkuDetailViewModel()
        {

        }
        #endregion

        #region Property

        /// <summary>
        /// spu_id
        /// </summary>
        [Display(Name = "spu_id")]
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
        /// category_id
        /// </summary>
        [Display(Name = "category_id")]
        public int category_id { get; set; } = 0;

        /// <summary>
        /// category_name
        /// </summary>
        [Display(Name = "category_name")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(32, ErrorMessage = "MaxLength")]
        public string category_name { get; set; } = string.Empty;

        /// <summary>
        /// spu_description
        /// </summary>
        [Display(Name = "spu_description")]
        [MaxLength(1000, ErrorMessage = "MaxLength")]
        public string spu_description { get; set; } = string.Empty;

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
        /// brand
        /// </summary>
        [Display(Name = "brand")]
        [MaxLength(128, ErrorMessage = "MaxLength")]
        public string brand { get; set; } = string.Empty;

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
        /// sku_id
        /// </summary>
        [Display(Name = "sku_id")]
        public int sku_id { get; set; } = 0;

        /// <summary>
        /// sku_code
        /// </summary>
        [Display(Name = "sku_code")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(32, ErrorMessage = "MaxLength")]
        public string sku_code { get; set; } = string.Empty;

        /// <summary>
        /// sku_name
        /// </summary>
        [Display(Name = "sku_name")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(200, ErrorMessage = "MaxLength")]
        public string sku_name { get; set; } = string.Empty;

        /// <summary>
        /// bar_code
        /// </summary>
        [Display(Name = "bar_code")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string bar_code { get; set; } = string.Empty;

        /// <summary>
        /// weight
        /// </summary>
        [Display(Name = "weight")]
        public decimal weight { get; set; } = 0;

        /// <summary>
        /// lenght
        /// </summary>
        [Display(Name = "lenght")]
        public decimal lenght { get; set; } = 0;

        /// <summary>
        /// width
        /// </summary>
        [Display(Name = "width")]
        public decimal width { get; set; } = 0;

        /// <summary>
        /// height
        /// </summary>
        [Display(Name = "height")]
        public decimal height { get; set; } = 0;

        /// <summary>
        /// volume
        /// </summary>
        [Display(Name = "volume")]
        public decimal volume { get; set; } = 0;

        /// <summary>
        /// unit
        /// </summary>
        [Display(Name = "unit")]
        [MaxLength(5, ErrorMessage = "MaxLength")]
        public string unit { get; set; } = string.Empty;

        /// <summary>
        /// cost
        /// </summary>
        [Display(Name = "cost")]
        public decimal cost { get; set; } = 0;

        /// <summary>
        /// price
        /// </summary>
        [Display(Name = "price")]
        public decimal price { get; set; } = 0;
        #endregion
    }
}
