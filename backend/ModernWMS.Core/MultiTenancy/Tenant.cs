namespace ModernWMS.Core.MultiTenancy
{
    /// <summary>
    /// Tenant Class
    /// </summary>
    public class Tenant
    {
        /// <summary>
        /// tenant's id
        /// </summary>
        public byte tenant_id { get; set; } = 1;
        /// <summary>
        /// tenant's name
        /// </summary>
        public string tenant_name { get; set; } = "default";
    }
}
