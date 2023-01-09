/*
 * date：2022-12-30
 * developer：AMo
 */
using System;
using System.ComponentModel.DataAnnotations;
using ModernWMS.Core.Utility;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// stocktaking confirm counted_qty viewModel
    /// </summary>
    public class StocktakingConfirmViewModel
    {
        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public StocktakingConfirmViewModel()
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
        /// counted_qty
        /// </summary>
        [Display(Name = "counted_qty")]
        public int counted_qty { get; set; } = 0;

        #endregion
    }
}
