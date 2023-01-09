using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using ModernWMS.Core.Models;
using Microsoft.Extensions.Localization;

namespace ModernWMS.Core.Middleware
{
    /// <summary>
    /// Global exception middleware
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        #region parameter
        private readonly RequestDelegate next; 
        private readonly ILogger<GlobalExceptionMiddleware> logger;
        /// <summary>
        /// Localizer Service
        /// </summary>
        private readonly IStringLocalizer<MultiLanguage> _stringLocalizer;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next">Delegate in next step</param>
        /// <param name="logger">log manager</param>
        /// <param name="stringLocalizer">Localizer</param>
        public GlobalExceptionMiddleware(RequestDelegate next,
                  ILogger<GlobalExceptionMiddleware> logger,
                  IStringLocalizer<MultiLanguage> stringLocalizer
                                         )
        {
            this.next = next;
            this.logger = logger;
            this._stringLocalizer = stringLocalizer;
        }
        #endregion

        /// <summary>
        /// invoke
        /// </summary>
        /// <param name="context">httpcontext</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                await WriteExceptionAsync(context, ex);
            }
        }
        /// <summary>
        /// Write Log
        /// </summary>
        /// <param name="context">httpcontext</param>
        /// <param name="e">error messasge</param>
        /// <returns></returns>
        private async Task WriteExceptionAsync(HttpContext context, Exception e)
        {
            if (e != null)
            {
                var response = context.Response;
                var message = e.InnerException == null ? e.Message : e.InnerException.Message;
                response.ContentType = "application/json";

                var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (string.IsNullOrEmpty(ip))
                {
                    ip = context.Connection.RemoteIpAddress.ToString();
                }
                logger.LogError($"\r\n\r\nIP:{ip},Exception：{e.Message}\r\nStackTrace：{e.StackTrace}");

                string result = Utility.JsonHelper.SerializeObject(ResultModel<object>.Error(_stringLocalizer["operation_failed"]));
                await context.Response.WriteAsync(result).ConfigureAwait(false);
            }
            else
            {
                var code = context.Response.StatusCode;
                switch (code)
                {
                    case 200:
                        return;
                    case 204:
                        return;
                    case 401:
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(Utility.JsonHelper.SerializeObject(ResultModel<object>.Error("Invalid Token"))).ConfigureAwait(false);
                        break;
                    default:
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(Utility.JsonHelper.SerializeObject(ResultModel<object>.Error("Unknown Error"))).ConfigureAwait(false);
                        break;
                }
            }
        }

    }
}
