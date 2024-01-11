using ModernWMS.Core.Models;
using ModernWMS.Core.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.WMS.Entities.Models.Approve
{
    [Table("flowsetmain")]
    public class FlowSetMainEntity : BaseModel
    {
        /// <summary>
        /// menu
        /// </summary>
        public string menu { get; set; } = string.Empty;

        /// <summary>
        /// flowset name
        /// </summary>
        public string flow_name { get; set; } = string.Empty;

        /// <summary>
        /// creator
        /// </summary>
        public string creator { get; set; } = string.Empty;

        /// <summary>
        /// create_time
        /// </summary>
        public DateTime create_time { get; set; } = UtilConvert.MinDate;

        /// <summary>
        /// tenant_id
        /// </summary>
        public long tenant_id { get; set; } = 1;

        /// <summary>
        /// flowset list
        /// </summary>
        public List<FlowSetEntity> flowset_list { get; set; } = new List<FlowSetEntity>();
    }
}