using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.Core.DynamicSearch
{
    /// <summary>
    /// DynamicSearch Operators
    /// </summary>
    public enum Operators
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Equal =
        /// </summary>
        Equal = 1,
        /// <summary>
        /// GreaterThan 
        /// </summary>
        GreaterThan = 2,
        /// <summary>
        /// GreaterThanOrEqual 
        /// </summary>
        GreaterThanOrEqual = 3,
        /// <summary>
        /// LessThan
        /// </summary>
        LessThan = 4,
        /// <summary>
        /// LessThanOrEqual
        /// </summary>
        LessThanOrEqual = 5,
        /// <summary>
        /// Contains
        /// </summary>
        Contains = 6
    }
    /// <summary>
    /// Condition
    /// </summary>
    public enum Condition
    {
        /// <summary>
        /// OR
        /// </summary>
        OrElse = 1,
        /// <summary>
        /// AND
        /// </summary>
        AndAlso = 2
    }
    /// <summary>
    /// select item combox
    /// </summary>
    public class ComboxItem
    {
        /// <summary>
        /// value
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// text
        /// </summary>
        public string text { get; set; }
    }
}
