using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// StockprocessImportViewModel
    /// </summary>
    public class StockprocessImportViewModel
    {
        /// <summary>
        /// import_group
        /// </summary>
        public int import_group { get; set; } = 0;

        /// <summary>
        /// spu_code
        /// </summary>
        public string spu_code { get; set; } = string.Empty;

        /// <summary>
        /// sku_name
        /// </summary>
        public string sku_name { get; set; } = string.Empty;

        /// <summary>
        /// sku_code
        /// </summary>
        public string sku_code { get; set; } = string.Empty;

        /// <summary>
        /// location_name
        /// </summary>
        public string location_name { get; set; } = string.Empty;

        /// <summary>
        /// goods_owner_name
        /// </summary>
        public string goods_owner_name { get; set; } = string.Empty;

        /// <summary>
        /// qty
        /// </summary>
        public int qty { get; set; } = 0;

        /// <summary>
        /// is origin
        /// </summary>
        public bool is_ori { get; set; } = false;
    }
}
