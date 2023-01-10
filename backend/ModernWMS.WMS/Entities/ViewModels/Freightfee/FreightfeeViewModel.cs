/*
 * date：2022-12-22
 * developer：NoNo
 */
using System;
using System.ComponentModel.DataAnnotations;
using ModernWMS.Core.Utility;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// freightfee viewModel
    /// </summary>
    public class FreightfeeViewModel
    {

         #region constructor
         /// <summary>
         /// constructor
         /// </summary>
         public FreightfeeViewModel()
         {
 
         }
         #endregion
        #region Property

        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
        public int id { get; set; }  = 0;

        /// <summary>
        /// carrier
        /// </summary>
        [Display(Name = "carrier")]
        [MaxLength(256,ErrorMessage = "MaxLength")]
        [Required(ErrorMessage = "Required")]
        public string carrier { get; set; }  = string.Empty;

        /// <summary>
        /// departure_city
        /// </summary>
        [Display(Name = "departure_city")]
        [MaxLength(128,ErrorMessage = "MaxLength")]
        [Required(ErrorMessage = "Required")]
        public string departure_city { get; set; }  = string.Empty;

        /// <summary>
        /// arrival_city
        /// </summary>
        [Display(Name = "arrival_city")]
        [MaxLength(128,ErrorMessage = "MaxLength")]
        [Required(ErrorMessage = "Required")]
        public string arrival_city { get; set; }  = string.Empty;

        /// <summary>
        /// price_per_weight
        /// </summary>
        [Display(Name = "price_per_weight")]
        [Required(ErrorMessage = "Required")]
        public decimal price_per_weight { get; set; }  = 0;

        /// <summary>
        /// price_per_volume
        /// </summary>
        [Display(Name = "price_per_volume")]
        [Required(ErrorMessage = "Required")]
        public decimal price_per_volume { get; set; }  = 0;

        /// <summary>
        /// min_payment
        /// </summary>
        [Display(Name = "min_payment")]
        [Required(ErrorMessage = "Required")]
        public decimal min_payment { get; set; }  = 0;

        /// <summary>
        /// creator
        /// </summary>
        [Display(Name = "creator")]
        [MaxLength(64,ErrorMessage = "MaxLength")]
        public string creator { get; set; }  = string.Empty;

        /// <summary>
        /// create_time
        /// </summary>
        [Display(Name = "create_time")]
         [DataType(DataType.DateTime, ErrorMessage = "DataType_DateTime")]
        public DateTime create_time { get; set; }  = UtilConvert.MinDate;

        /// <summary>
        /// last_update_time
        /// </summary>
        [Display(Name = "last_update_time")]
         [DataType(DataType.DateTime, ErrorMessage = "DataType_DateTime")]
        public DateTime last_update_time { get; set; }  = UtilConvert.MinDate;

        /// <summary>
        /// is_valid
        /// </summary>
        [Display(Name = "is_valid")]
        public bool is_valid { get; set; } =true;

        /// <summary>
        /// tenant_id
        /// </summary>
        [Display(Name = "tenant_id")]
        public long tenant_id { get; set; }  = 0;


        #endregion

    }
}
