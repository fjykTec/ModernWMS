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
    /// goodslocation  entity
    /// </summary>
    [Table("goodslocation")]
    public class GoodslocationEntity : BaseModel
    {

        #region Property

        /// <summary>
        /// warehouse_id
        /// </summary>
        public int warehouse_id { get; set; }  = 0;

        /// <summary>
        /// warehouse_name
        /// </summary>
        public string warehouse_name { get; set; }  = string.Empty;

        /// <summary>
        /// warehouse_area_name
        /// </summary>
        public string warehouse_area_name { get; set; }  = string.Empty;

        /// <summary>
        /// warehouse_area_property
        /// </summary>
        public byte warehouse_area_property { get; set; }  = 0;

        /// <summary>
        /// location_name
        /// </summary>
        public string location_name { get; set; }  = string.Empty;

        /// <summary>
        /// location_length
        /// </summary>
        public decimal location_length { get; set; }  = 0;

        /// <summary>
        /// location_width
        /// </summary>
        public decimal location_width { get; set; }  = 0;

        /// <summary>
        /// location_heigth
        /// </summary>
        public decimal location_heigth { get; set; }  = 0;

        /// <summary>
        /// location_volume
        /// </summary>
        public decimal location_volume { get; set; }  = 0;

        /// <summary>
        /// location_load
        /// </summary>
        public decimal location_load { get; set; }  = 0;

        /// <summary>
        /// roadway_number
        /// </summary>
        public string roadway_number { get; set; }  = string.Empty;

        /// <summary>
        /// shelf_number
        /// </summary>
        public string shelf_number { get; set; }  = string.Empty;

        /// <summary>
        /// layer_number
        /// </summary>
        public string layer_number { get; set; }  = string.Empty;

        /// <summary>
        /// tag_number
        /// </summary>
        public string tag_number { get; set; }  = string.Empty;

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

        /// <summary>
        /// warehouse_area_id
        /// </summary>
        public int warehouse_area_id { get; set; }  = 0;


        #endregion

    }
}
