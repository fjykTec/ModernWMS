using System.ComponentModel.DataAnnotations;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// goods owner view model
    /// </summary>
    public class GoodsownerImportViewModel
    {
        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public GoodsownerImportViewModel()
        {

        }
        #endregion

        #region Property
         
        /// <summary>
        /// goods owner's name
        /// </summary>
        [Display(Name = "goods_owner_name")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(256, ErrorMessage = "MaxLength")]
        public string goods_owner_name { get; set; } = string.Empty;

        /// <summary>
        /// city
        /// </summary>
        [Display(Name = "city")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(128, ErrorMessage = "MaxLength")]
        public string city { get; set; } = string.Empty;

        /// <summary>
        /// address
        /// </summary>
        [Display(Name = "address")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(256, ErrorMessage = "MaxLength")]
        public string address { get; set; } = string.Empty;

        /// <summary>
        /// manager
        /// </summary>
        [Display(Name = "manager")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string manager { get; set; } = string.Empty;

        /// <summary>
        /// contact tel
        /// </summary>
        [Display(Name = "contact_tel")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string contact_tel { get; set; } = string.Empty;

        /// <summary>
        /// _XID
        /// </summary>
        public string _XID { get; set; } = string.Empty;

        /// <summary>
        /// error message
        /// </summary>
        public string errorMsg { get; set; } = string.Empty;
        #endregion
    }
}
