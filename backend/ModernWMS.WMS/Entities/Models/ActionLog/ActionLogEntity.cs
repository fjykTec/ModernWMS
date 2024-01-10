/*
 * date：2023-08-22
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
    /// action_log  entity
    /// </summary>
    [Table("action_log")]
    public class ActionLogEntity : BaseModel
    {
        #region Property

        /// <summary>
        /// vue_path
        /// </summary>
        public string vue_path { get; set; } = string.Empty;

        /// <summary>
        /// user_name
        /// </summary>
        public string user_name { get; set; } = string.Empty;

        /// <summary>
        /// action_content
        /// </summary>
        public string action_content { get; set; } = string.Empty;

        /// <summary>
        /// action_time
        /// </summary>
        public DateTime action_time { get; set; } = UtilConvert.MinDate;

        /// <summary>
        /// tenant_id
        /// </summary>
        public long tenant_id { get; set; } = 1;

        #endregion Property
    }
}