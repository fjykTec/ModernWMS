using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.Core.Models
{
    /// <summary>
    /// global_unique_serial  entity
    /// </summary>
    [Table("global_unique_serial")]
    public class GlobalUniqueSerialEntity : BaseModel
    {
        /// <summary>
        /// table name
        /// </summary>
        public string table_name { get; set; } = string.Empty;

        /// <summary>
        ///
        /// </summary>
        public string prefix_char { get; set; } = string.Empty;

        /// <summary>
        ///
        /// </summary>
        public string reset_rule { get; set; } = string.Empty;

        /// <summary>
        ///
        /// </summary>
        public int current_no { get; set; } = 1;

        /// <summary>
        ///
        /// </summary>
        public DateTime last_update_time { get; set; } = DateTime.Now;

        /// <summary>
        ///  current user's tenant_id
        /// </summary>
        public long tenant_id { get; set; } = 1;
    }
}