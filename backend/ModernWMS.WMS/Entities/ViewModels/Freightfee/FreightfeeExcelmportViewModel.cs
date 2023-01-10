using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ModernWMS.WMS.Entities.ViewModels
{
    public class FreightfeeExcelmportViewModel
    {
        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public FreightfeeExcelmportViewModel()
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
        /// carrier
        /// </summary>
        [Display(Name = "carrier")]
        [MaxLength(256, ErrorMessage = "MaxLength")]
        [Required(ErrorMessage = "Required")]
        public string carrier { get; set; } = string.Empty;

        /// <summary>
        /// departure_city
        /// </summary>
        [Display(Name = "departure_city")]
        [MaxLength(128, ErrorMessage = "MaxLength")]
        [Required(ErrorMessage = "Required")]
        public string departure_city { get; set; } = string.Empty;

        /// <summary>
        /// arrival_city
        /// </summary>
        [Display(Name = "arrival_city")]
        [MaxLength(128, ErrorMessage = "MaxLength")]
        [Required(ErrorMessage = "Required")]
        public string arrival_city { get; set; } = string.Empty;

        /// <summary>
        /// price_per_weight
        /// </summary>
        [Display(Name = "price_per_weight")]
        [Required(ErrorMessage = "Required")]
        public decimal price_per_weight { get; set; } = 0;

        /// <summary>
        /// price_per_volume
        /// </summary>
        [Display(Name = "price_per_volume")]
        [Required(ErrorMessage = "Required")]
        public decimal price_per_volume { get; set; } = 0;

        /// <summary>
        /// min_payment
        /// </summary>
        [Display(Name = "min_payment")]
        [Required(ErrorMessage = "Required")]
        public decimal min_payment { get; set; } = 0;
        #endregion
    }
}
