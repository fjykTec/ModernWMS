/*
 * date：2022-12-20
 * developer：AMo
 */
using System;
using System.ComponentModel.DataAnnotations;
using ModernWMS.Core.Utility;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// category viewModel
    /// </summary>
    public class CategoryViewModel
    {

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public CategoryViewModel()
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
        /// category_name
        /// </summary>
        [Display(Name = "category_name")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(32, ErrorMessage = "MaxLength")]
        public string category_name { get; set; } = string.Empty;

        /// <summary>
        /// parent_id
        /// </summary>
        [Display(Name = "parent_id")]
        public int parent_id { get; set; } = 0;

        /// <summary>
        /// creator
        /// </summary>
        [Display(Name = "creator")]
        public string creator { get; set; } = string.Empty;

        /// <summary>
        /// create_time
        /// </summary>
        [Display(Name = "create_time")]
        [DataType(DataType.DateTime, ErrorMessage = "DataType_DateTime")]
        public DateTime create_time { get; set; } = UtilConvert.MinDate;

        /// <summary>
        /// last_update_time
        /// </summary>
        [Display(Name = "last_update_time")]
        [DataType(DataType.DateTime, ErrorMessage = "DataType_DateTime")]
        public DateTime last_update_time { get; set; } = UtilConvert.MinDate;

        /// <summary>
        /// is_valid
        /// </summary>
        [Display(Name = "is_valid")]
        public bool is_valid { get; set; } = true;

        #endregion

    }
}
