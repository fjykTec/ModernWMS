/*
 * date：2022-12-21
 * developer：NoNo
 */
using System;
using System.ComponentModel.DataAnnotations;
using ModernWMS.Core.Utility;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// spu viewModel
    /// </summary>
    public class SpuViewModel
    {

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public SpuViewModel()
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
        /// creator
        /// </summary>
        [Display(Name = "creator")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string creator { get; set; } = string.Empty;

        /// <summary>
        /// create_time
        /// </summary>
        [Display(Name = "create_time")]
        [DataType(DataType.DateTime, ErrorMessage = "DataType_DateTime")]
        public DateTime create_time { get; set; } = DateTime.Now;

        /// <summary>
        /// last_update_time
        /// </summary>
        [Display(Name = "last_update_time")]
        [DataType(DataType.DateTime, ErrorMessage = "DataType_DateTime")]
        public DateTime last_update_time { get; set; } = DateTime.Now;

        /// <summary>
        /// is_valid
        /// </summary>
        [Display(Name = "is_valid")]
        public bool is_valid { get; set; } = true;

        #endregion

    }
}
