using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// what user input when changing password
    /// </summary>
    public class UserChangePwdViewModel
    {
        /// <summary>
        /// user's id
        /// </summary>
        public int id { get; set; }  = 0;

        /// <summary>
        /// old password
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [Display(Name = "old password")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string old_password { get; set; }   = string.Empty;

        /// <summary>
        /// new password
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [Display(Name = "new password")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string new_password { get; set; } = string.Empty;
    }
}
