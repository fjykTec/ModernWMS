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
    /// sku viewModel
    /// </summary>
    public class SkuViewModel
    {

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public SkuViewModel()
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
        /// spu_id
        /// </summary>
        [Display(Name = "spu_id")]
        public int spu_id { get; set; } = 0;

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
        #endregion

        #region sku safety stock
        /// <summary>
        /// sku safety sotck
        /// </summary>
        public List<SkuSafetyStockViewModel> detailList { get; set; } = new List<SkuSafetyStockViewModel>();

        #endregion
    }
}
