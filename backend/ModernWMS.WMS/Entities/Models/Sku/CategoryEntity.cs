/*
 * date：2022-12-20
 * developer：AMo
 */
using ModernWMS.Core.Models;
using ModernWMS.Core.Utility;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModernWMS.WMS.Entities.Models
{
    /// <summary>
    /// category  entity
    /// </summary>
    [Table("category")]
    public class CategoryEntity : BaseModel
    {

        #region Property

        /// <summary>
        /// category_name
        /// </summary>
        public string category_name { get; set; } = string.Empty;

        /// <summary>
        /// parent_id
        /// </summary>
        public int parent_id { get; set; } = 0;

        /// <summary>
        /// creator
        /// </summary>
        public string creator { get; set; } = string.Empty;

        /// <summary>
        /// create_time
        /// </summary>
        public DateTime create_time { get; set; } = UtilConvert.MinDate;

        /// <summary>
        /// last_update_time
        /// </summary>
        public DateTime last_update_time { get; set; } = UtilConvert.MinDate;

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