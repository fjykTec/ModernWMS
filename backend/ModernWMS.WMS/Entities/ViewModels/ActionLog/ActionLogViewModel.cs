/*
 * date：2023-08-22
 * developer：NoNo
 */

using System;
using System.ComponentModel.DataAnnotations;
using ModernWMS.Core.Utility;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// action_log viewModel
    /// </summary>
    public class ActionLogViewModel
    {
        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public ActionLogViewModel()
        {
        }

        #endregion constructor

        #region Property

        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
        public int id { get; set; } = 0;

        /// <summary>
        /// menu path
        /// </summary>
        [Display(Name = "vue_path")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string vue_path { get; set; } = string.Empty;

        /// <summary>
        /// user_name
        /// </summary>
        [Display(Name = "user_name")]
        [MaxLength(128, ErrorMessage = "MaxLength")]
        public string user_name { get; set; } = string.Empty;

        /// <summary>
        /// action_content
        /// </summary>
        [Display(Name = "action_content")]
        [MaxLength(2000, ErrorMessage = "MaxLength")]
        public string action_content { get; set; } = string.Empty;

        /// <summary>
        /// action_time
        /// </summary>
        [Display(Name = "action_time")]
        [DataType(DataType.DateTime, ErrorMessage = "DataType_DateTime")]
        public DateTime action_time { get; set; } = UtilConvert.MinDate;

        #endregion Property
    }
}