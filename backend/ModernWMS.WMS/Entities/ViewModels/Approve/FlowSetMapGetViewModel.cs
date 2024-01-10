using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ModernWMS.WMS.Entities.Models;
using ModernWMS.WMS.Entities.ViewModels.Approve;

namespace ModernWMS.WMS.Entities.ViewModels
{
    public class FlowSetMapGetViewModel
    {
        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public FlowSetMapGetViewModel()
        {
        }

        #endregion constructor

        #region Property

        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
        public int id { get; set; } = 0;

        /// <summary>
        /// flowset main id
        /// </summary>
        [Display(Name = "flowsetmain_id")]
        public int flowsetmain_id { get; set; } = 0;

        /// <summary>
        /// menu
        /// </summary>
        [Display(Name = "menu")]
        public string menu { get; set; } = string.Empty;

        /// <summary>
        /// origin
        /// </summary>
        [Display(Name = "is_origin")]
        public bool is_origin { get; set; } = false;

        /// <summary>
        /// end
        /// </summary>
        [Display(Name = "is_end")]
        public bool is_end { get; set; } = false;

        /// <summary>
        /// this node guid
        /// </summary>
        [Display(Name = "node_guid")]
        public string node_guid { get; set; } = string.Empty;

        /// <summary>
        /// this node name
        /// </summary>
        [Display(Name = "node_name")]
        public string node_name { get; set; } = string.Empty;

        /// <summary>
        /// previous node guid
        /// </summary>
        [Display(Name = "prev_node_guid")]
        public string prev_node_guid { get; set; } = string.Empty;

        /// <summary>
        /// approve user list
        /// </summary>
        public List<FlowSetUserViewModel> user_list { get; set; } = new List<FlowSetUserViewModel>();

        /// <summary>
        /// filter condition list
        /// </summary>
        public List<FlowSetConditionViewModel> filter_list { get; set; } = new List<FlowSetConditionViewModel> { };

        /// <summary>
        /// children list
        /// </summary>
        public List<FlowSetMapGetViewModel> children { get; set; } = new List<FlowSetMapGetViewModel> { };

        #endregion Property
    }
}