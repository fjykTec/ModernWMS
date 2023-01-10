using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ModernWMS.Core.Middleware
{
    /// <summary>
    /// Cross domain middleware
    /// </summary>
    public class CorsMiddleware
    {
        #region parameter
        /// <summary>
        /// agent
        /// </summary>
        private readonly RequestDelegate _next;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next">Delegate in next step</param>
        public CorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        #endregion

        /// <summary>
        /// Invoke
        /// </summary>
        /// <param name="httpContext">httpContext</param>
        /// <returns></returns>
        public Task Invoke(HttpContext httpContext)
        {

            if (httpContext.Request.Method == "OPTIONS")
            {
                httpContext.Response.Headers.Add("Access-Control-Allow-Origin", httpContext.Request.Headers["Origin"]);
                httpContext.Response.Headers.Add("Access-Control-Allow-Headers", httpContext.Request.Headers["Access-Control-Request-Headers"]);
                httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "PUT,POST,GET,DELETE,OPTIONS,HEAD,PATCH");
                httpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                httpContext.Response.Headers.Add("Access-Control-Max-Age", "86400");
                httpContext.Response.StatusCode = StatusCodes.Status200OK;
                return Task.CompletedTask;
            }
            if (httpContext.Request.Headers["Origin"] != "")
            {
                httpContext.Response.Headers.Add("Access-Control-Allow-Origin", httpContext.Request.Headers["Origin"]);
            }

            httpContext.Response.Headers.Add("Access-Control-Allow-Headers", httpContext.Request.Headers["Access-Control-Request-Headers"]);
            httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "PUT,POST,GET,DELETE,OPTIONS,HEAD,PATCH");
            httpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            httpContext.Response.Headers.Add("Access-Control-Max-Age", "86400");
            return _next.Invoke(httpContext);
        }

    }
}
