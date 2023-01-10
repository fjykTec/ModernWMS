using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.WMS.Entities.ViewModels
{
    public class DispatchlistConfirmPickDetailViewModel
    {
        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public DispatchlistConfirmPickDetailViewModel()
        {

        }
        #endregion
        #region Property
        /// <summary>
        /// stock_id        
        /// </summary>
        public int stock_id { get; set; } = 0;

        /// <summary>
        /// dispatchlist_id
        /// </summary>
        public int dispatchlist_id { get; set; } = 0;

        /// <summary>
        /// goods_owner_id
        /// </summary>
        public int goods_owner_id { get; set; } = 0;

        /// <summary>
        /// goods_owner_name
        /// </summary>
        public string goods_owner_name { get; set; } = string.Empty;

        /// <summary>
        /// goods_location_id
        /// </summary>
        public int goods_location_id { get; set; } = 0;

        /// <summary>
        /// warehouse_name
        /// </summary>
        public string warehouse_name { get; set; } = string.Empty;

        /// <summary>
        /// location_name
        /// </summary>
        public string location_name { get; set; } = string.Empty;

        /// <summary>
        /// warehouse_area_name
        /// </summary>
        public string warehouse_area_name { get; set; } = string.Empty;


        /// <summary>
        /// quantity available
        /// </summary>
        public int qty_available { get; set; } = 0;

        /// <summary>
        /// pick_qty
        /// </summary>
        public int pick_qty { get; set; } = 0;

        #endregion
    }
}
