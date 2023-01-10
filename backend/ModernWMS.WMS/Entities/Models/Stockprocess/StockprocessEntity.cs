/*
 * date：2022-12-23
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
    /// stockprocess  entity
    /// </summary>
    [Table("stockprocess")]
    public class StockprocessEntity : BaseModel
    {

        #region Property

        /// <summary>
        /// job_code
        /// </summary>
        public string job_code { get; set; }  = string.Empty;

        /// <summary>
        /// job_type
        /// </summary>
        public bool job_type { get; set; } = false;

        /// <summary>
        /// process_status
        /// </summary>
        public bool process_status { get; set; } = false;

        /// <summary>
        /// processor
        /// </summary>
        public string processor { get; set; }  = string.Empty;

        /// <summary>
        /// process_time
        /// </summary>
        public DateTime process_time { get; set; }  = UtilConvert.MinDate;

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
        /// tenant_id
        /// </summary>
        public long tenant_id { get; set; }  = 0;


        #endregion


        #region detail table

         /// <summary>
         /// detail table
         /// </summary>
         public List<StockprocessdetailEntity> detailList { get; set; } = new List<StockprocessdetailEntity>(2);

        #endregion

    }
}
