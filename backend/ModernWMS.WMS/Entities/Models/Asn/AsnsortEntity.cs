/*
 * date：2022-12-30
 * developer：AMo
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
    /// asnsort  entity
    /// </summary>
    [Table("asnsort")]
    public class AsnsortEntity : BaseModel
    {
        #region Property

        /// <summary>
        /// asn_id
        /// </summary>
        public int asn_id { get; set; } = 0;

        /// <summary>
        /// sorted_qty
        /// </summary>
        public int sorted_qty { get; set; } = 0;

        /// <summary>
        /// series_number
        /// </summary>
        public string series_number { get; set; } = string.Empty;

        /// <summary>
        /// putaway qty
        /// </summary>
        public int putaway_qty { get; set; } = 0;

        /// <summary>
        /// creator
        /// </summary>
        public string creator { get; set; } = string.Empty;

        /// <summary>
        /// create_time
        /// </summary>
        public DateTime create_time { get; set; } = DateTime.Now;

        /// <summary>
        /// last_update_time
        /// </summary>
        public DateTime last_update_time { get; set; } = DateTime.Now;

        /// <summary>
        /// is_valid
        /// </summary>
        public bool is_valid { get; set; } = true;

        /// <summary>
        /// tenant_id
        /// </summary>
        public long tenant_id { get; set; } = 1;

        #endregion
    }
}
