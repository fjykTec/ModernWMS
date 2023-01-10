/*
 * date：2022-12-21
 * developer：NoNo
 */

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// spu with sku viewModel
    /// </summary>
    public class SpuBothViewModel : SpuViewModel
    {
        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public SpuBothViewModel()
        {

        }
        #endregion

        #region details
        /// <summary>
        /// sku
        /// </summary>
        public List<SkuViewModel> detailList { get; set; } = new List<SkuViewModel>();
        #endregion
    }
}
