/*
 * date：2022-12-21
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
    /// warehouse  entity
    /// </summary>
    [Table("warehouse")]
    public class WarehouseEntity : BaseModel
    {

        #region Property

        /// <summary>
        /// warehouse_name
        /// </summary>
        public string warehouse_name { get; set; }  = string.Empty;

        /// <summary>
        /// city
        /// </summary>
        public string city { get; set; }  = string.Empty;

        /// <summary>
        /// address
        /// </summary>
        public string address { get; set; }  = string.Empty;

        /// <summary>
        /// email
        /// </summary>
        public string email { get; set; }  = string.Empty;

        /// <summary>
        /// manager
        /// </summary>
        public string manager { get; set; }  = string.Empty;

        /// <summary>
        /// contact_tel
        /// </summary>
        public string contact_tel { get; set; }  = string.Empty;

        /// <summary>
        /// creator
        /// </summary>
        public string creator { get; set; }  = string.Empty;

        /// <summary>
        /// create_time
        /// </summary>
        public DateTime create_time { get; set; }  = UtilConvert.MinDate;

        /// <summary>
        /// last_update_time
        /// </summary>
        public DateTime last_update_time { get; set; }  = UtilConvert.MinDate;

        /// <summary>
        /// is_valid
        /// </summary>
        public bool is_valid { get; set; } = false;

        /// <summary>
        /// tenant_id
        /// </summary>
        public long tenant_id { get; set; }  = 0;


        #endregion

    }
}
