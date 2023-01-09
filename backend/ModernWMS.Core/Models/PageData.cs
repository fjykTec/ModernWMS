using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.Core.Models
{
    /// <summary>
    /// PageData
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageData<T>
    {
        /// <summary>
        /// data
        /// </summary>
        public List<T> Rows { get; set; } = new List<T>(2);
        /// <summary>
        /// total rows
        /// </summary>
        public int Totals { get; set; } = 0;       
    }
}
