using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// AsnPrintSeriesNumberViewModel
    /// </summary>
    public class AsnPrintSeriesNumberViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public AsnPrintSeriesNumberViewModel()
        {
            
        }


        /// <summary>
        /// asn_id
        /// </summary>
        public int asn_id { get; set; } = 0;

        /// <summary>
        /// asnmaster_id
        /// </summary>
        public int asnmaster_id { get; set; } = 0;

        /// <summary>
        /// sku_id
        /// </summary>
        public int sku_id { get; set; } = 0;

        /// <summary>
        /// asn_no
        /// </summary>
        public string asn_no { get; set; } = string.Empty;

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
        /// sku_name
        /// </summary>
        public string sku_name { get; set; } = string.Empty;

        /// <summary>
        /// series_number
        /// </summary>
        public string series_number { get; set; } = string.Empty;

    }
}
