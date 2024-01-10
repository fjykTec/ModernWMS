using ModernWMS.Core.Models;
using ModernWMS.WMS.Entities.Models.Approve;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.WMS.Entities.Models
{
    [Table("flowset")]
    public class FlowSetEntity : BaseModel
    {
        #region foreign table

        /// <summary>
        /// foreign table
        /// </summary>
        [ForeignKey("flowsetmain_id")]
        public FlowSetMainEntity FlowMainSet { get; set; }

        #endregion foreign table

        /// <summary>
        /// flowset main id
        /// </summary>
        public int flowsetmain_id { get; set; } = 0;

        /// <summary>
        /// origin
        /// </summary>
        public bool is_origin { get; set; } = false;

        /// <summary>
        /// end
        /// </summary>
        public bool is_end { get; set; } = false;

        /// <summary>
        /// this node guid
        /// </summary>
        public string node_guid { get; set; } = string.Empty;

        /// <summary>
        /// this node name
        /// </summary>
        public string node_name { get; set; } = string.Empty;

        /// <summary>
        /// previous node guid
        /// </summary>
        public string prev_node_guid { get; set; } = string.Empty;

        /// <summary>
        /// approve user list
        /// </summary>
        public List<FlowSetUserEntity> user_list { get; set; } = new List<FlowSetUserEntity>();

        /// <summary>
        /// condition list
        /// </summary>
        public List<FlowSetFilterEntity> filter_list { get; set; } = new List<FlowSetFilterEntity>();
    }
}