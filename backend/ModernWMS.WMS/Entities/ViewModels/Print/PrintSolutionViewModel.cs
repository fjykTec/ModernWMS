/*
 * date：2023-09-11
 * developer：NoNo
 */

using System;
using System.ComponentModel.DataAnnotations;
using ModernWMS.Core.Utility;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// user_defined_print_solution viewModel
    /// </summary>
    public class PrintSolutionViewModel
    {
        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public PrintSolutionViewModel()
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
        /// vue_path
        /// </summary>
        [Display(Name = "vue_path")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string vue_path { get; set; } = string.Empty;

        /// <summary>
        /// tab_page
        /// </summary>
        [Display(Name = "tab_page")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string tab_page { get; set; } = string.Empty;

        /// <summary>
        /// solution_name
        /// </summary>
        [Display(Name = "solution_name")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string solution_name { get; set; } = string.Empty;

        /// <summary>
        /// config_json
        /// </summary>
        [Display(Name = "config_json")]
        public string config_json { get; set; }

        /// <summary>
        /// report_length
        /// </summary>
        [Display(Name = "report_length")]
        public decimal report_length { get; set; } = 0;

        /// <summary>
        /// report_width
        /// </summary>
        [Display(Name = "report_width")]
        public decimal report_width { get; set; } = 0;

        /// <summary>
        /// report_direction
        /// </summary>
        [Display(Name = "report_direction")]
        [MaxLength(2, ErrorMessage = "MaxLength")]
        public string report_direction { get; set; } = string.Empty;

        /// <summary>
        /// tenant_id
        /// </summary>
        [Display(Name = "tenant_id")]
        public long tenant_id { get; set; } = 0;

        #endregion Property
    }
}