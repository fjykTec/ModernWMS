using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// what should be inputed when canceling order opration 
    /// </summary>
    public class CancelOrderOprationViewModel
    {
        #region constructor
        /// <summary>
        /// CancleOrderOprationViewModel
        /// </summary>
        public CancelOrderOprationViewModel()
        {

        }
        #endregion
        #region Property
        /// <summary>
        /// dispatch_no
        /// </summary>
        public string dispatch_no { get; set; } = string.Empty;

        /// <summary>
        /// dispatch_status
        /// </summary>
        public int dispatch_status { get; set; } = 0;

        #endregion
    }
}
