namespace ModernWMS.Core.MultiTenancy
{
    /// <summary>
    /// TenantProvider
    /// </summary>
    public interface ITenantProvider
    {
        /// <summary>
        /// Get Current User's TenantID
        /// </summary>
        /// <returns></returns>
        byte GetCurrentTenantID();
    }
}
