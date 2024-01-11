/*
 * date：2023-09-11
 * developer：NoNo
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ModernWMS.Core.Models;
using ModernWMS.Core.Utility;

namespace ModernWMS.WMS.Entities.Models
{
    /// <summary>
    /// user_defined_print_solution  entity
    /// </summary>
    [Table("user_defined_print_solution")]
    public class PrintSolutionEntity : BaseModel
    {
        #region Property

        /// <summary>
        /// vue_path
        /// </summary>
        public string vue_path { get; set; } = string.Empty;

        /// <summary>
        /// tab_page
        /// </summary>
        public string tab_page { get; set; } = string.Empty;

        /// <summary>
        /// solution_name
        /// </summary>
        public string solution_name { get; set; } = string.Empty;

        /// <summary>
        /// config_json
        /// </summary>
        public string config_json { get; set; }

        /// <summary>
        /// report_length
        /// </summary>
        public decimal report_length { get; set; } = 0;

        /// <summary>
        /// report_width
        /// </summary>
        public decimal report_width { get; set; } = 0;

        /// <summary>
        /// report_direction
        /// </summary>
        public string report_direction { get; set; } = string.Empty;

        /// <summary>
        /// last updated time
        /// </summary>
        public DateTime last_update_time { get; set; } = UtilConvert.MinDate;

        /// <summary>
        /// tenant_id
        /// </summary>
        public long tenant_id { get; set; } = 0;

        #endregion Property
    }
}