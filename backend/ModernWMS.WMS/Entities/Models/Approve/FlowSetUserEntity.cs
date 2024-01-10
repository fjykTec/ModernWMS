using ModernWMS.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.WMS.Entities.Models
{
    [Table("flowsetusers")]
    public class FlowSetUserEntity : BaseModel
    {
        #region foreign table

        /// <summary>
        /// foreign table
        /// </summary>
        [ForeignKey("flowset_id")]
        public FlowSetEntity FlowSet { get; set; }

        #endregion foreign table

        /// <summary>
        /// flowset id
        /// </summary>
        public int flowset_id { get; set; } = 0;

        /// <summary>
        /// flowset main id
        /// </summary>
        public int flowsetmain_id { get; set; } = 0;

        /// <summary>
        /// node guid
        /// </summary>
        public string node_guid { get; set; } = string.Empty;

        /// <summary>
        /// user id
        /// </summary>
        public int user_id { get; set; } = 0;
    }
}