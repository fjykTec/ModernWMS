using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.Core.DynamicSearch
{
    /// <summary>
    /// SearchObject
    /// </summary>
    public class SearchObject
    {
        /// <summary>
        /// sort
        /// </summary>
        public int Sort { get; set; } = 0;

        /// <summary>
        /// label
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// type
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// operator
        /// </summary>
        public Operators Operator { get; set; } = Operators.Equal;

        /// <summary>
        /// text
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// value
        /// </summary>
        public object Value { get; set; } = new object();

        /// <summary>
        /// select item combox list
        /// </summary>
        public List<ComboxItem> comboxItem { get; set; } = new List<ComboxItem>();

    }
}
