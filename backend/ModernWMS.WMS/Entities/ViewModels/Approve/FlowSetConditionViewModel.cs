using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.WMS.Entities.ViewModels.Approve
{
    public class FlowSetConditionViewModel
    {
        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
        public int id { get; set; } = 0;

        /// <summary>
        /// menu
        /// </summary>
        public string menu { get; set; } = string.Empty;

        /// <summary>
        /// flowset id
        /// </summary>
        public int flowset_id { get; set; } = 0;

        /// <summary>
        /// node guid
        /// </summary>
        public string node_guid { get; set; } = string.Empty;

        /// <summary>
        /// logic operator
        /// </summary>
        public string logic { get; set; } = "且";

        /// <summary>
        /// (
        /// </summary>
        public string c1 { get; set; } = string.Empty;

        /// <summary>
        /// column label
        /// </summary>
        public string col_label { get; set; } = string.Empty;

        /// <summary>
        /// column name
        /// </summary>
        public string col_name { get; set; } = string.Empty;

        /// <summary>
        /// comparation
        /// </summary>
        public string compare { get; set; } = "等于";

        /// <summary>
        /// content
        /// </summary>
        public string content { get; set; } = string.Empty;

        /// <summary>
        /// )
        /// </summary>
        public string c2 { get; set; } = string.Empty;

        /// <summary>
        /// sort
        /// </summary>
        public int sort { get; set; } = 0;

        /// <summary>
        /// condition group
        /// </summary>
        public string condition_group { get; set; } = string.Empty;

        /// <summary>
        /// formulas
        /// </summary>
        public string formulas { get; set; } = string.Empty;

        /// <summary>
        /// assert mode（存在、不存在、求和、平均值、最大值、最小值）
        /// </summary>
        public string assert_mode { get; set; } = string.Empty;

        /// <summary>
        /// table name
        /// </summary>
        public string table_name { get; set; } = string.Empty;

        /// <summary>
        /// scheme name
        /// </summary>
        public string scheme_name { get; set; } = string.Empty;
    }
}