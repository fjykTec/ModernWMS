using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ModernWMS.Core.JWT;
using ModernWMS.Core.Models;
using ModernWMS.Core.Services;

namespace ModernWMS.Core.Controller
{
    /// <summary>
    /// account
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Base")]
    public class AccountController : BaseController
    {
        /// <summary>
        /// token manger
        /// </summary>
        private readonly ITokenManager _tokenManager;
        /// <summary>
        /// Log helper
        /// </summary>
        private readonly ILogger<AccountController> _logger;

        /// <summary>
        /// cache helper
        /// </summary>
        private readonly CacheManager _cacheManager;

        /// <summary>
        /// account service class
        /// </summary>
        private readonly IAccountService _accountService;

        /// <summary>
        /// Localizer
        /// </summary>
        private readonly IStringLocalizer _stringLocalizer;
        /// <summary>
        /// Structure
        /// </summary>
        /// <param name="logger">logger helper</param>
        /// <param name="tokenManager">token manger</param>
        /// <param name="cacheManager">cache helper</param>
        /// <param name="accountService">account service class</param>
        /// <param name="stringLocalizer">Localizer</param>
        public AccountController(ILogger<AccountController> logger
            , ITokenManager tokenManager
            , CacheManager cacheManager
            , IAccountService accountService
            , IStringLocalizer stringLocalizer
            )
        {
            this._tokenManager = tokenManager;
            this._logger = logger;
            this._cacheManager = cacheManager;
            this._accountService = accountService;
            this._stringLocalizer = stringLocalizer;
        }

        #region Login

        /// <summary>
        /// login
        /// </summary>
        /// <param name="loginAccount">user's account infomation</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("/login")]
        public async Task<ResultModel<LoginOutputViewModel>> LoginAsync(LoginInputViewModel loginAccount)
        {

            var user = await _accountService.Login(loginAccount,CurrentUser);
            if (user != null)
            {
                var result = _tokenManager.GenerateToken(
                    new CurrentUser
                    {
                        user_id = user.user_id,
                        user_name = user.user_name,
                        user_num = user.user_num,
                        user_role = user.user_role,
                        tenant_id = user.tenant_id
                    }
                    );
                string rt = this._tokenManager.GenerateRefreshToken();

                user.access_token = result.token;
                user.expire = result.expire;
                user.refresh_token = rt;

                await _cacheManager.TokenSet(user.user_id, "WebRefreshToken", rt, _tokenManager.GetRefreshTokenExpireMinute());

                return ResultModel<LoginOutputViewModel>.Success(user);
            }
            else
            {
                return ResultModel<LoginOutputViewModel>.Error(_stringLocalizer["login_failed"]);
            }
        }
        /// <summary>
        /// get a new token
        /// </summary>
        /// <param name="inPutViewModel">old access token and refreshtoken key</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("/refresh-token")]
        public async Task<ResultModel<string>> RefreshToken([FromBody] RefreshTokenInPutViewModel inPutViewModel)
        {
            var currentUser = this._tokenManager.GetCurrentUser(inPutViewModel.AccessToken);

            var flag = _cacheManager.Is_Token_Exist<string>(currentUser.user_id, "WebRefreshToken", _tokenManager.GetRefreshTokenExpireMinute());
            if (!flag)
            {
                return ResultModel<string>.Error("refreshtoken_failure");
            }
            else
            {
                var result = _tokenManager.GenerateToken(currentUser);
                return ResultModel<string>.Success(result.token);
            }

        }
        #endregion
        #region hello world
        [AllowAnonymous]
        [HttpPost("/hello-world")]
        public ResultModel<string> hello_world()
        {
            return ResultModel<string>.Success(_accountService.HelloWorld());
        }
        #endregion
    }
}
