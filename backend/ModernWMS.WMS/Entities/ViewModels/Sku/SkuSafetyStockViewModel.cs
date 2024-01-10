/*
 * date：2023-08-25
 * developer：AMo
 */
using System.ComponentModel.DataAnnotations;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// sku_safety_stock view model
    /// </summary>
    public class SkuSafetyStockViewModel
    {
        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public SkuSafetyStockViewModel()
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
        /// sku id
        /// </summary>
        [Display(Name = "sku_id")]
        public int sku_id { get; set; } = 0;

        /// <summary>
        /// warehouse's id
        /// </summary>
        [Display(Name = "warehouse_id")]
        public int warehouse_id { get; set; } = 0;

        /// <summary>
        /// warehouse_name
        /// </summary>
        [Display(Name = "warehouse_name")]
        [MaxLength(32, ErrorMessage = "MaxLength")]
        [Required(ErrorMessage = "Required")]
        public string warehouse_name { get; set; } = string.Empty;

        /// <summary>
        /// safety stock
        /// </summary>
        [Display(Name = "safety_stock_qty")]
        public int safety_stock_qty { get; set; } = 0;
        #endregion
    }
}
