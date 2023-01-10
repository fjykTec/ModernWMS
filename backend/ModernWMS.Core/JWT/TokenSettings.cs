
namespace ModernWMS.Core.JWT
{
    /// <summary>
    /// token settings
    /// </summary>
    public class TokenSettings
    {
        /// <summary>
        /// Audience
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// Issuer
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// SigningKey
        /// </summary>
        public string SigningKey { get; set; }
        /// <summary>
        ///  Expire
        /// </summary>
        public int ExpireMinute { get; set; }
    }
}
