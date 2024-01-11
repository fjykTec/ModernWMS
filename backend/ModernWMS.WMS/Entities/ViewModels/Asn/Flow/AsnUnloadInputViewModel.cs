/*
 * date：2023-09-01
 * developer：AMo
 */
using System.ComponentModel.DataAnnotations;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// Unload
    /// </summary>
    public class AsnUnloadInputViewModel
    {

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public AsnUnloadInputViewModel()
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
        /// unload_time
        /// </summary>
        [Display(Name = "unload_time")]
        public DateTime unload_time { get; set; } = Core.Utility.UtilConvert.MinDate;

        /// <summary>
        /// unload_person_id
        /// </summary>
        [Display(Name = "unload_person_id")]
        public int unload_person_id { get; set; } = 0;

        /// <summary>
        /// unload_person
        /// </summary>
        [Display(Name = "unload_person")]
        public string unload_person { get; set; } = string.Empty;
        #endregion
    }
}
