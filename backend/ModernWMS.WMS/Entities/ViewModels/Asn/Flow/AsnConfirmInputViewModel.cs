/*
 * date：2023-09-01
 * developer：AMo
 */
using System.ComponentModel.DataAnnotations;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// asn flow confirm input viewModel
    /// </summary>
    public class AsnConfirmInputViewModel
    {

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public AsnConfirmInputViewModel()
        {

        }
        #endregion

        #region Property

        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
        public int id { get; set; } = 0;

        /// <summary>
        /// arrival_time
        /// </summary>
        [Display(Name = "arrival_time")]
        public DateTime arrival_time { get; set; } = Core.Utility.UtilConvert.MinDate;
        #endregion
    }
}
