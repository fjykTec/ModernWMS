using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// ImportViewModel
    /// </summary>
    public class DispatchlistImportViewModel
    {
        /// <summary>
        /// import_group
        /// </summary>
        public int import_group { get; set; } = 0;

        /// <summary>
        /// customer_name
        /// </summary>
        public string customer_name { get; set; } = string.Empty;

        /// <summary>
        /// sku_name
        /// </summary>
        public string sku_name { get; set; } = string.Empty;

        /// <summary>
        /// spu_code
        /// </summary>
        public string spu_code { get; set; } = string.Empty;

        /// <summary>
        /// sku_code
        /// </summary>
        public string sku_code { get; set; } = string.Empty;

        /// <summary>
        /// qty
        /// </summary>
        public int qty { get; set; } = 0;
    }
}
