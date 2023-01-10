using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ModernWMS.Core.Utility;

namespace ModernWMS.Core.JWT
{
    /// <summary>
    /// token manager
    /// </summary>
    public class TokenManager : ITokenManager
    {
        private readonly IOptions<TokenSettings> _tokenSettings;//token setting
        private readonly IHttpContextAccessor _accessor; // Inject IHttpContextAccessor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tokenSettings">token setting s</param>
        /// <param name="accessor">Inject IHttpContextAccessor</param>
        public TokenManager(IOptions<TokenSettings> tokenSettings
            , IHttpContextAccessor accessor)
        {
            this._tokenSettings = tokenSettings;
            this._accessor = accessor;
        }
        /// <summary>
        /// Method of refreshing token
        /// </summary>
        /// <returns></returns>
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }
        }
        /// <summary>
        /// Method of generating AccessToken
        /// </summary>
        /// <param name="userClaims">自定义信息</param>
        /// <returns>(token,有效分钟数)</returns>
        public (string token, int expire) GenerateToken(CurrentUser userClaims)
        {
            string token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                                                                                 issuer: _tokenSettings.Value.Issuer,
                                                                                 audience: _tokenSettings.Value.Audience,
                                                                                 claims: SetClaims(userClaims),
                                                                                 expires: DateTime.Now.AddMinutes(_tokenSettings.Value.ExpireMinute),
                                                                                 signingCredentials: new SigningCredentials(
                                                                                                                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalConsts.SigningKey)),
                                                                                                                            SecurityAlgorithms.HmacSha256)
                                                                                ));

            return (token, _tokenSettings.Value.ExpireMinute);
        }
        /// <summary>
        /// Get the current user information in the token
        /// </summary>
        /// <returns></returns>
        public CurrentUser GetCurrentUser()
        {
            if (_accessor.HttpContext == null)
            {
                return new CurrentUser();
            }
            var token = _accessor.HttpContext.Request.Headers["Authorization"].ObjToString();
            if (!token.StartsWith("Bearer"))
            {
                return new CurrentUser();
            }
            token = token.Replace("Bearer ", "");
            if (token.Length > 0)
            {
                var principal = new JwtSecurityTokenHandler().ValidateToken(token,
                                                                        new TokenValidationParameters
                                                                        {
                                                                            ValidateAudience = false,
                                                                            ValidateIssuer = false,
                                                                            ValidateIssuerSigningKey = true,
                                                                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalConsts.SigningKey)),
                                                                            ValidateLifetime = false
                                                                        },
                                                                        out var securityToken);

                if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return new CurrentUser();
                }
                var user = JsonHelper.DeserializeObject<CurrentUser>(principal.Claims.First(claim => claim.Type == ClaimValueTypes.Json).Value);
                if (user != null)
                {
                    return user;
                }
                else
                {
                    return new CurrentUser();
                }
            }
            else
            {
                return new CurrentUser();
            }

        }

        /// <summary>
        /// Get the current user information in the token
        /// </summary>
        /// <returns></returns>
        public CurrentUser GetCurrentUser(string token)
        {
            if (token.Length > 0)
            {
                var principal = new JwtSecurityTokenHandler().ValidateToken(token,
                                                                        new TokenValidationParameters
                                                                        {
                                                                            ValidateAudience = false,
                                                                            ValidateIssuer = false,
                                                                            ValidateIssuerSigningKey = true,
                                                                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalConsts.SigningKey)),
                                                                            ValidateLifetime = false
                                                                        },
                                                                        out var securityToken);

                if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return new CurrentUser();
                }
                var user = JsonHelper.DeserializeObject<CurrentUser>(principal.Claims.First(claim => claim.Type == ClaimValueTypes.Json).Value);
                if (user != null)
                {
                    return user;
                }
                else
                {
                    return new CurrentUser();
                }
            }
            else
            {
                return new CurrentUser();
            }

        }
        /// <summary>
        /// Method of refreshing token
        /// </summary>
        /// <returns></returns>
        public int GetRefreshTokenExpireMinute()
        {
            return _tokenSettings.Value.ExpireMinute + 1;
        }

        /// <summary>
        /// Setting Custom Information
        /// </summary>
        /// <param name="userClaims">Custom Information</param>
        /// <returns></returns>
        private static IEnumerable<Claim> SetClaims(CurrentUser userClaims)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()),
                new Claim(ClaimValueTypes.Json,JsonHelper.SerializeObject(userClaims), ClaimValueTypes.Json)
            };
        }

    }
}
