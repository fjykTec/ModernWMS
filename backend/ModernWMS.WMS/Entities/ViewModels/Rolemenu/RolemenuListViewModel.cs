/*
 * date：2022-12-20
 * developer：AMo
 */
using ModernWMS.Core.Utility;
using System.ComponentModel.DataAnnotations;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// rolemenu list viewModel
    /// </summary>
    public class RolemenuListViewModel
    {
        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public RolemenuListViewModel()
        {

        }
        #endregion

        #region Property

        /// <summary>
        /// userrole_id
        /// </summary>
        public int userrole_id { get; set; } = 0;

        /// <summary>
        /// role_name
        /// </summary>
        public string role_name { get; set; } = string.Empty;


        /// <summary>
        /// role valid
        /// </summary>
        public bool is_valid { get; set; } = true;


        /// <summary>
        /// create_time
        /// </summary>
        public DateTime create_time { get; set; } = UtilConvert.MinDate;

        /// <summary>
        /// last_update_time
        /// </summary>
        public DateTime last_update_time { get; set; } = UtilConvert.MinDate;

        #endregion
    }
}
