using System.ComponentModel.DataAnnotations;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// safety stock qty put view model
    /// </summary>
    public class SkuSafetyStockPutViewModel
    {

        /// <summary>
        /// sku id
        /// </summary>
        [Display(Name = "sku_id")]
        [Required(ErrorMessage = "Required")]
        public int sku_id { get; set; } = 0;

        /// <summary>
        /// safety stock qty list
        /// </summary>
        public List<SkuSafetyStockViewModel> detailList { get; set; } = new List<SkuSafetyStockViewModel>();
    }
}
