/*
 * date：2022-12-22
 * developer：AMo
 */
using System.ComponentModel.DataAnnotations;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// asn bulk modify goods owner input view model
    /// </summary>
    public class AsnBulkModifyGoodsOwnerViewModel
    {
        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public AsnBulkModifyGoodsOwnerViewModel()
        {

        }
        #endregion

        #region Property

        /// <summary>
        /// goods_owner_id
        /// </summary>
        [Display(Name = "goods_owner_id")]
        [Required(ErrorMessage = "Required")]
        public int goods_owner_id { get; set; } = 0;

        /// <summary>
        /// goods_owner_name
        /// </summary>
        [Display(Name = "goods_owner_name")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(256, ErrorMessage = "MaxLength")]
        public string goods_owner_name { get; set; } = string.Empty;

        /// <summary>
        /// selected asn id
        /// </summary>
        public List<int> idList { get; set; } = new List<int>();

        #endregion
    }
}
