/*
 * date：2022-12-22
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
    /// freightfee  entity
    /// </summary>
    [Table("freightfee")]
    public class FreightfeeEntity : BaseModel
    {

        #region Property

        /// <summary>
        /// carrier
        /// </summary>
        public string carrier { get; set; }  = string.Empty;

        /// <summary>
        /// departure_city
        /// </summary>
        public string departure_city { get; set; }  = string.Empty;

        /// <summary>
        /// arrival_city
        /// </summary>
        public string arrival_city { get; set; }  = string.Empty;

        /// <summary>
        /// price_per_weight
        /// </summary>
        public decimal price_per_weight { get; set; }  = 0;

        /// <summary>
        /// price_per_volume
        /// </summary>
        public decimal price_per_volume { get; set; }  = 0;

        /// <summary>
        /// min_payment
        /// </summary>
        public decimal min_payment { get; set; }  = 0;

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
