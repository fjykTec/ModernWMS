
namespace ModernWMS.Core.JWT
{
    /// <summary>
    /// token manager interface
    /// </summary>
    public interface ITokenManager
    {
        /// <summary>
        /// Method of generating AccessToken
        /// </summary>
        /// <param name="userClaims">userClaims</param>
        /// <returns></returns>
        (string token, int expire) GenerateToken(CurrentUser userClaims);
        /// <summary>
        /// Method of refreshing token
        /// </summary>
        /// <returns></returns>
        string GenerateRefreshToken();
        /// <summary>
        /// Get the minutes of refreshed token invalidation
        /// </summary>
        /// <returns></returns>
        int GetRefreshTokenExpireMinute();
        /// <summary>
        /// Get the current user information in the token
        /// </summary>
        /// <returns></returns>
        CurrentUser GetCurrentUser();
        /// <summary>
        /// Get the current user information in the token
        /// </summary>
        /// <returns></returns>
        CurrentUser GetCurrentUser(string token);
    }
}
