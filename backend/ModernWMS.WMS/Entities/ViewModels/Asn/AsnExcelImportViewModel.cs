using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// asn excel import view model
    /// </summary>
    public class AsnExcelImportViewModel
    {
        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public AsnExcelImportViewModel()
        {

        }
        #endregion

        #region Property

        /// <summary>
        /// asn_no
        /// </summary>
        [Display(Name = "asn_no")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(32, ErrorMessage = "MaxLength")]
        public string asn_no { get; set; } = string.Empty;


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
        [Required(ErrorMessage = "Required")]
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
        [Required(ErrorMessage = "Required")]
        [MaxLength(256, ErrorMessage = "MaxLength")]
        public string supplier_name { get; set; } = string.Empty;

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

        /// <summary>
        /// 
        /// </summary>
        public string _X_ROW_KEY { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string error_msg { get; set; } = string.Empty;
        #endregion
    }
}
