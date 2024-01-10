
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace ModernWMS.Core.Models
{
    /// <summary>
    /// login input viewmodel
    /// </summary>
    public class LoginInputViewModel
    {
        /// <summary>
        /// username
        /// </summary>
        [Required(ErrorMessage ="Required")]
        [Display(Name = "user_name")]
        [MaxLength(128,ErrorMessage = "MaxLength")]
        public string user_name { get; set; } = string.Empty;

        /// <summary>
        /// password
        /// </summary>
        [Required(ErrorMessage ="Required")]
        [Display(Name = "password")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string password { get; set; } = string.Empty;
    }
}
