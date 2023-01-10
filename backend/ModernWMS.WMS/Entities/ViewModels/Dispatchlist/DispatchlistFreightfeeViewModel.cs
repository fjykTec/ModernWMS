using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// DispatchlistFreightfeeViewModel
    /// </summary>
    public class DispatchlistFreightfeeViewModel
    {
        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public DispatchlistFreightfeeViewModel()
        {

        }
        #endregion
        #region Property
        [Display(Name = "id")]
        public int id { get; set; } = 0;
        /// <summary>
        /// dispatch_no
        /// </summary>
        [Display(Name = "dispatch_no")]
        [MaxLength(32, ErrorMessage = "MaxLength")]
        public string dispatch_no { get; set; } = string.Empty;

        /// <summary>
        /// dispatch_status
        /// </summary>
        [Display(Name = "dispatch_status")]
        public byte dispatch_status { get; set; } = 0;

        /// <summary>
        /// freightfee_id
        /// </summary>
        [Display(Name = "freightfee_id")]
        public int freightfee_id { get; set; } = 0;

        /// <summary>
        /// carrier
        /// </summary>
        [Display(Name = "carrier")]
        [MaxLength(256, ErrorMessage = "MaxLength")]
        [Required(ErrorMessage = "Required")]
        public string carrier { get; set; } = string.Empty;

        /// <summary>
        /// waybill_no
        /// </summary>
        public string waybill_no { get; set; } = string.Empty;
        #endregion
    }
}
