/*
 * date：2022-12-30
 * developer：AMo
 */
using System;
using System.ComponentModel.DataAnnotations;
using ModernWMS.Core.Utility;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// stocktaking viewModel
    /// </summary>
    public class StocktakingViewModel : StocktakingBasicViewModel
    {

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public StocktakingViewModel()
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
        /// job_code
        /// </summary>
        [Display(Name = "job_code")]
        [MaxLength(32, ErrorMessage = "MaxLength")]
        public string job_code { get; set; } = string.Empty;

        /// <summary>
        /// job_status
        /// </summary>
        [Display(Name = "job_status")]
        public bool job_status { get; set; } = false;

        /// <summary>
        /// adjust_status
        /// </summary>
        public bool adjust_status { get; set; } = false;

        /// <summary>
        /// counted_qty
        /// </summary>
        [Display(Name = "counted_qty")]
        public int counted_qty { get; set; } = 0;

        /// <summary>
        /// difference_qty
        /// </summary>
        [Display(Name = "difference_qty")]
        public int difference_qty { get; set; } = 0;

        /// <summary>
        /// creator
        /// </summary>
        [Display(Name = "creator")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string creator { get; set; } = string.Empty;

        /// <summary>
        /// create_time
        /// </summary>
        [Display(Name = "create_time")]
        public DateTime create_time { get; set; } = UtilConvert.MinDate;

        /// <summary>
        /// handler
        /// </summary>
        [Display(Name = "handler")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string handler { get; set; } = string.Empty;

        /// <summary>
        /// handle_time
        /// </summary>
        [Display(Name = "handle_time")]
        public DateTime handle_time { get; set; } = UtilConvert.MinDate;

        /// <summary>
        /// last_update_time
        /// </summary>
        [Display(Name = "last_update_time")]
        public DateTime last_update_time { get; set; } = UtilConvert.MinDate;

        #endregion

    }
}
