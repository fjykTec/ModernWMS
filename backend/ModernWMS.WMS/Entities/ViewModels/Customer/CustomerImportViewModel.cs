/*
 * date：2023-01-04
 * developer：AMo
 */
using System.ComponentModel.DataAnnotations;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// customer import view model
    /// </summary>
    public class CustomerImportViewModel
    {
        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public CustomerImportViewModel()
        {

        }
        #endregion

        #region Property
         
        /// <summary>
        /// customer's name
        /// </summary>
        [Display(Name = "customer_name")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(256, ErrorMessage = "MaxLength")]
        public string customer_name { get; set; } = string.Empty;

        /// <summary>
        /// city
        /// </summary>
        [Display(Name = "city")]
        [MaxLength(128, ErrorMessage = "MaxLength")]
        public string city { get; set; } = string.Empty;

        /// <summary>
        /// address
        /// </summary>
        [Display(Name = "address")]
        [MaxLength(256, ErrorMessage = "MaxLength")]
        public string address { get; set; } = string.Empty;

        /// <summary>
        /// manager
        /// </summary>
        [Display(Name = "manager")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string manager { get; set; } = string.Empty;

        /// <summary>
        /// email
        /// </summary>
        [Display(Name = "email")]
        [MaxLength(128, ErrorMessage = "MaxLength")]
        public string email { get; set; } = string.Empty;

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
