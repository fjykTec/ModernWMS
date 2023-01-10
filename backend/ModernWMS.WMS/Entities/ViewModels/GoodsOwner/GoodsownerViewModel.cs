using System.ComponentModel.DataAnnotations;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// goods owner view model
    /// </summary>
    public class GoodsownerViewModel
    {
        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public GoodsownerViewModel()
        {

        }
        #endregion

        #region Property

        /// <summary>
        /// primary key
        /// </summary>
        public int id { get; set; } = 0;

        /// <summary>
        /// goods owner's name
        /// </summary>
        [Display(Name = "goods_owner_name")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(256, ErrorMessage = "MaxLength")]
        public string goods_owner_name { get; set; } = string.Empty;

        /// <summary>
        /// city
        /// </summary>
        [Display(Name = "city")]
        [MaxLength(128, ErrorMessage = "MaxLength")]
        public string city { get; set; } = string.Empty;

        /// <summary>
        /// address
        /// </summary>
        [Display(Name = "address")]
        [MaxLength(256, ErrorMessage = "MaxLength")]
        public string address { get; set; } = string.Empty;

        /// <summary>
        /// manager
        /// </summary>
        [Display(Name = "manager")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string manager { get; set; } = string.Empty;

        /// <summary>
        /// contact tel
        /// </summary>
        [Display(Name = "contact_tel")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string contact_tel { get; set; } = string.Empty;

        /// <summary>
        /// creator
        /// </summary>
        [Display(Name = "creator")]
        [MaxLength(64, ErrorMessage = "MaxLength")]
        public string creator { get; set; } = string.Empty;

        /// <summary>
        /// create time
        /// </summary>
        [Display(Name = "create_time")]
        [DataType(DataType.DateTime, ErrorMessage = "DataType_DateTime")]
        public DateTime create_time { get; set; } = DateTime.Now;

        /// <summary>
        /// last update time
        /// </summary>
        [Display(Name = "last_update_time")]
        [DataType(DataType.DateTime, ErrorMessage = "DataType_DateTime")]
        public DateTime last_update_time { get; set; } = DateTime.Now;

        /// <summary>
        /// valid
        /// </summary>
        [Display(Name = "is_valid")]
        public bool is_valid { get; set; } = true;
        #endregion
    }
}
