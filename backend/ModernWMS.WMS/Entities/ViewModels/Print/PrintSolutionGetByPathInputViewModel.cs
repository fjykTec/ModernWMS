using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// PrintSolutionGetByPathInputViewModel
    /// </summary>
    public class PrintSolutionGetByPathInputViewModel
    {
        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public PrintSolutionGetByPathInputViewModel()
        {
        }

        #endregion constructor

        #region Property

        /// <summary>
        /// vue_path
        /// </summary>
        [Display(Name = "vue_path")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string vue_path { get; set; } = string.Empty;

        /// <summary>
        /// tab_page
        /// </summary>
        [Display(Name = "tab_page")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string tab_page { get; set; } = string.Empty;
        #endregion Property
    }
}
