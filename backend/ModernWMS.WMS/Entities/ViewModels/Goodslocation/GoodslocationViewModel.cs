/*
 * date：2022-12-21
 * developer：NoNo
 */
using System;
using System.ComponentModel.DataAnnotations;
using ModernWMS.Core.Utility;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// goodslocation viewModel
    /// </summary>
    public class GoodslocationViewModel
    {

         #region constructor
         /// <summary>
         /// constructor
         /// </summary>
         public GoodslocationViewModel()
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
        /// warehouse_id
        /// </summary>
        [Display(Name = "warehouse_id")]
        public int warehouse_id { get; set; }  = 0;

        /// <summary>
        /// warehouse_name
        /// </summary>
        [Display(Name = "warehouse_name")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(32,ErrorMessage = "MaxLength")]
        public string warehouse_name { get; set; }  = string.Empty;

        /// <summary>
        /// warehouse_area_name
        /// </summary>
        [Display(Name = "warehouse_area_name")]
        [MaxLength(32,ErrorMessage = "MaxLength")]
        [Required(ErrorMessage = "Required")]
        public string warehouse_area_name { get; set; }  = string.Empty;

        /// <summary>
        /// warehouse_area_property
        /// </summary>
        [Display(Name = "warehouse_area_property")]
        [Required(ErrorMessage = "Required")]
        public byte warehouse_area_property { get; set; }  = 0;

        /// <summary>
        /// location_name
        /// </summary>
        [Display(Name = "location_name")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(64,ErrorMessage = "MaxLength")]
        public string location_name { get; set; }  = string.Empty;

        /// <summary>
        /// location_length
        /// </summary>
        [Display(Name = "location_length")]
        public decimal location_length { get; set; }  = 0;

        /// <summary>
        /// location_width
        /// </summary>
        [Display(Name = "location_width")]
        public decimal location_width { get; set; }  = 0;

        /// <summary>
        /// location_heigth
        /// </summary>
        [Display(Name = "location_heigth")]
        public decimal location_heigth { get; set; }  = 0;

        /// <summary>
        /// location_volume
        /// </summary>
        [Display(Name = "location_volume")]
        public decimal location_volume { get; set; }  = 0;

        /// <summary>
        /// location_load
        /// </summary>
        [Display(Name = "location_load")]
        public decimal location_load { get; set; }  = 0;

        /// <summary>
        /// roadway_number
        /// </summary>
        [Display(Name = "roadway_number")]
        [MaxLength(10,ErrorMessage = "MaxLength")]
        public string roadway_number { get; set; }  = string.Empty;

        /// <summary>
        /// shelf_number
        /// </summary>
        [Display(Name = "shelf_number")]
        [MaxLength(10,ErrorMessage = "MaxLength")]
        public string shelf_number { get; set; }  = string.Empty;

        /// <summary>
        /// layer_number
        /// </summary>
        [Display(Name = "layer_number")]
        [MaxLength(10,ErrorMessage = "MaxLength")]
        public string layer_number { get; set; }  = string.Empty;

        /// <summary>
        /// tag_number
        /// </summary>
        [Display(Name = "tag_number")]
        [MaxLength(10,ErrorMessage = "MaxLength")]
        public string tag_number { get; set; }  = string.Empty;

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

        /// <summary>
        /// warehouse_area_id
        /// </summary>
        [Display(Name = "warehouse_area_id")]
        public int warehouse_area_id { get; set; }  = 0;


        #endregion

    }
}
