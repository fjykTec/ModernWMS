/*
 * date：2022-12-20
 * developer：NoNo
 */
using ModernWMS.Core.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// user import by excel cols viewmodel
    /// </summary>
    public class UserExcelImportViewModel
    {
        /// <summary>
        /// user's number
        /// </summary>
        [Display(Name = "user_num")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(128, ErrorMessage = "MaxLength")]

        public string user_num { get; set; } = string.Empty;

        /// <summary>
        /// user's name
        /// </summary>
        [Display(Name = "user_name")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(128, ErrorMessage = "MaxLength")]
        public string user_name { get; set; } = string.Empty;

        /// <summary>
        /// contact
        /// </summary>
        [Display(Name = "contact_tel")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string contact_tel { get; set; } = string.Empty;

        /// <summary>
        /// user's role
        /// </summary>
        [Display(Name = "user_role")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(128, ErrorMessage = "MaxLength")]
        public string user_role { get; set; } = string.Empty;

        /// <summary>
        /// sex
        /// </summary>
        [Display(Name = "sex")]
        [MaxLength(10, ErrorMessage = "MaxLength")]
        public string sex { get; set; } = string.Empty;

        /// <summary>
        /// is_valid
        /// </summary>
        [Display(Name = "is_valid")]
        public bool is_valid { get; set; } = false;

        /// <summary>
        /// creator
        /// </summary>
        [Display(Name = "creator")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string creator { get; set; } = string.Empty;

        /// <summary>
        /// tenant
        /// </summary>
        [Display(Name = "tenant")]
        public long tenant_id { get; set; }  =  0;
    }
}
