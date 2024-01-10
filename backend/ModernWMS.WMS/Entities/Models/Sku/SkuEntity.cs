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
    /// sku  entity
    /// </summary>
    [Table("sku")]
    public class SkuEntity : BaseModel
    {
        #region navigational properties
        /// <summary>
        /// navigational properties
        /// </summary>
        [ForeignKey("spu_id")]
        public SpuEntity Spu { get; set; }

        #endregion

        #region Property

        /// <summary>
        /// spu_id
        /// </summary>
        public int spu_id { get; set; } = 0;

        /// <summary>
        /// sku_code
        /// </summary>
        public string sku_code { get; set; } = string.Empty;

        /// <summary>
        /// sku_name
        /// </summary>
        public string sku_name { get; set; } = string.Empty;

        /// <summary>
        /// bar_code
        /// </summary>
        public string bar_code { get; set; } = string.Empty;

        /// <summary>
        /// weight
        /// </summary>
        public decimal weight { get; set; } = 0;

        /// <summary>
        /// lenght
        /// </summary>
        public decimal lenght { get; set; } = 0;

        /// <summary>
        /// width
        /// </summary>
        public decimal width { get; set; } = 0;

        /// <summary>
        /// height
        /// </summary>
        public decimal height { get; set; } = 0;

        /// <summary>
        /// volume
        /// </summary>
        public decimal volume { get; set; } = 0;

        /// <summary>
        /// unit
        /// </summary>
        public string unit { get; set; } = string.Empty;

        /// <summary>
        /// cost
        /// </summary>
        public decimal cost { get; set; } = 0;

        /// <summary>
        /// price
        /// </summary>
        public decimal price { get; set; } = 0;

        /// <summary>
        /// create_time
        /// </summary>
        public DateTime create_time { get; set; } = UtilConvert.MinDate;

        /// <summary>
        /// last_update_time
        /// </summary>
        public DateTime last_update_time { get; set; } = UtilConvert.MinDate;


        #endregion

        #region Sku Safety Stock

        /// <summary>
        /// Sku Safety Stock
        /// </summary>
        public List<SkuSafetyStockEntity> detailList { get; set; } = new List<SkuSafetyStockEntity>();

        #endregion
    }
}