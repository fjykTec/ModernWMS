namespace ModernWMS.Core.Utility
{
    /// <summary>
    /// global constant
    /// </summary>
    public static class GlobalConsts
    {

        /// <summary>
        /// is Swagger enable
        /// </summary>
        public static bool IsEnabledSwagger = true;

        /// <summary>
        /// Is RequestResponseMiddleware enable
        /// </summary>
        public static bool IsRequestResponseMiddleware = true;

        /// <summary>
        /// token cipher
        /// </summary>
        public const string SigningKey = "ModernWMS_SigningKey";

        /// <summary>
        /// Password will expire every 30 days from last password change.
        /// </summary>
        public static int PasswordExpireDays = 30;


    }
}
