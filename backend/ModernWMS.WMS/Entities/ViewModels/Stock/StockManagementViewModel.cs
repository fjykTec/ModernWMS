using ModernWMS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// StockManagementViewModel
    /// </summary>
    public class StockManagementViewModel
    {
        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public StockManagementViewModel()
        {
        }

        #endregion constructor

        #region Property

        /// <summary>
        /// spu_code
        /// </summary>
        public string spu_code { get; set; } = string.Empty;

        /// <summary>
        /// spu_name
        /// </summary>
        public string spu_name { get; set; } = string.Empty;

        /// <summary>
        /// sku_code
        /// </summary>
        public string sku_code { get; set; } = string.Empty;

        /// <summary>
        /// sku_id
        /// </summary>
        public int sku_id { get; set; } = 0;

        /// <summary>
        /// quantity
        /// </summary>
        public int qty { get; set; } = 0;

        /// <summary>
        /// quantity available
        /// </summary>
        public int qty_available { get; set; } = 0;

        /// <summary>
        /// quantity locked
        /// </summary>
        public int qty_locked { get; set; } = 0;

        /// <summary>
        /// quantity frozen
        /// </summary>
        public int qty_frozen { get; set; } = 0;

        /// <summary>
        /// asn qty
        /// </summary>
        public int qty_asn { get; set; } = 0;

        /// <summary>
        /// qty to be unloaded
        /// </summary>
        public int qty_to_unload { get; set; } = 0;

        /// <summary>
        ///  qty to be sorted
        /// </summary>
        public int qty_to_sort { get; set; } = 0;

        /// <summary>
        /// qty sorted
        /// </summary>
        public int qty_sorted { get; set; } = 0;

        /// <summary>
        /// shortage qty
        /// </summary>
        public int shortage_qty { get; set; } = 0;


        /// <summary>
        /// expiry_date
        /// </summary>
        public DateTime expiry_date { get; set; } = UtilConvert.MinDate;

        #endregion Property
    }
}