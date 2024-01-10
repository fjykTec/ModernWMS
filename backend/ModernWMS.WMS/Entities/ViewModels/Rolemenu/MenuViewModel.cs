/*
 * date：2022-12-20
 * developer：AMo
 */
using System.ComponentModel.DataAnnotations;

namespace ModernWMS.WMS.Entities.ViewModels
{
    /// <summary>
    /// menu viewModel
    /// </summary>
    public class MenuViewModel
    {

        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; } = 0;

        /// <summary>
        /// menu_name
        /// </summary>
        public string menu_name { get; set; } = string.Empty;

        /// <summary>
        /// module
        /// </summary>
        public string module { get; set; } = string.Empty;

        /// <summary>
        /// vue_path
        /// </summary>
        public string vue_path { get; set; } = string.Empty;

        /// <summary>
        /// vue_path_detail
        /// </summary>
        public string vue_path_detail { get; set; } = string.Empty;

        /// <summary>
        /// vue_directory
        /// </summary>
        public string vue_directory { get; set; } = string.Empty;

        /// <summary>
        /// sort
        /// </summary>
        public int sort { get; set; } = 0;

        /// <summary>
        /// menu's actions
        /// </summary>
        public List<string> menu_actions { get; set; } = new List<string>();
    }
}
