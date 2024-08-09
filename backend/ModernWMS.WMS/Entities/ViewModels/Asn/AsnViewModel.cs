/*
 * date：2022-12-22
 * developer：AMo
 */
using System;
using System.ComponentModel.DataAnnotations;
using ModernWMS.Core.Utility;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// asn viewModel
    /// </summary>
    public class AsnViewModel
    {

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public AsnViewModel()
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
        /// estimated_arrival_time
        /// </summary>
        public DateTime estimated_arrival_time { get; set; } = Core.Utility.UtilConvert.MinDate;

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
        /// arrival_time
        /// </summary>
        public DateTime arrival_time { get; set; } = Core.Utility.UtilConvert.MinDate;

        /// <summary>
        /// unload_time
        /// </summary>
        public DateTime unload_time { get; set; } = Core.Utility.UtilConvert.MinDate;

        /// <summary>
        /// unload_person_id
        /// </summary>
        public int unload_person_id { get; set; } = 0;

        /// <summary>
        /// unload_person
        /// </summary>
        public string unload_person { get; set; } = string.Empty;

        /// <summary>
        /// sorted_qty
        /// </summary>
        [Display(Name = "sorted_qty")]
        public int sorted_qty { get; set; } = 0;

        /// <summary>
        /// shortage_qty
        /// </summary>
        [Display(Name = "shortage_qty")]
        public int shortage_qty { get; set; } = 0;

        /// <summary>
        /// more_qty
        /// </summary>
        [Display(Name = "more_qty")]
        public int more_qty { get; set; } = 0;

        /// <summary>
        /// damage_qty
        /// </summary>
        [Display(Name = "damage_qty")]
        public int damage_qty { get; set; } = 0;

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

        /// <summary>
        /// is_valid
        /// </summary>
        [Display(Name = "is_valid")]
        public bool is_valid { get; set; } = true;


        /// <summary>
        /// expiry_date
        /// </summary>
        public DateTime expiry_date { get; set; } = Core.Utility.UtilConvert.MinDate;


        /// <summary>
        /// price
        /// </summary>
        [Display(Name = "price")]
        public decimal price { get; set; } = 0;

        #endregion

    }
}
