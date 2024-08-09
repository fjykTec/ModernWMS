using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ModernWMS.Core.JWT
{
    /// <summary>
    /// CurrentUser
    /// </summary>
    public class CurrentUser
    {
        /// <summary>
        /// user_id
        /// </summary>
        public int user_id { get; set; } = 1;

        /// <summary>
        /// user_num
        /// </summary>
        public string user_num { get; set; } = "admin";

        /// <summary>
        /// user_name
        /// </summary>
        public string user_name { get; set; } = "admin";

        /// <summary>
        /// user_role
        /// </summary>
        public string user_role { get; set; } = "admin";

        /// <summary>
        /// tenant
        /// </summary>
        public long tenant_id { get; set; } = 1;
    }
}