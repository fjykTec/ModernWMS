using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ModernWMS.Core.Models;

namespace ModernWMS.Core.Middleware
{
    /// <summary>
    /// Request response middleware
    /// </summary>
    public class RequestResponseMiddleware
    {
        #region parameter
        /// <summary>
        /// Delegate
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// log manager
        /// </summary>
        private readonly ILogger<RequestResponseMiddleware> _logger;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next">Delegate</param>
        /// <param name="logger">log manager</param>
        public RequestResponseMiddleware(RequestDelegate next
            , ILogger<RequestResponseMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        #endregion

        /// <summary>
        /// Invoke
        /// </summary>
        /// <param name="context">httpcontext</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (ModernWMS.Core.Utility.GlobalConsts.IsRequestResponseMiddleware)
            {
                string requestInfo = "", responseInfo = "";
                var originalBodyStream = context.Response.Body;
                var stopwach = Stopwatch.StartNew();
                try
                {

                    requestInfo = await FormatRequest(context.Request);

                    using (var responseBody = new MemoryStream())
                    {
                        context.Response.Body = responseBody;

                        await _next(context);
                        stopwach.Stop();

                        responseInfo = await FormatResponse(context.Response);

                        await responseBody.CopyToAsync(originalBodyStream);
                    }


                    var logMsg = $@"request information: {requestInfo} ;time spent: {stopwach.ElapsedMilliseconds}ms";
                    _logger.LogInformation(logMsg);

                }
                catch (Exception ex)
                {
                    stopwach.Stop();
                    if (ex != null)
                    {
                        var logMsg = $@"request information: {requestInfo}{Environment.NewLine}exception: {ex.ToString()}{Environment.NewLine}time spent: {stopwach.ElapsedMilliseconds}ms";

                        _logger.LogError(logMsg);
                        _logger.LogError(ex.ToString());

                        string result = Utility.JsonHelper.SerializeObject(ResultModel<object>.Error(ex.Message));
                        var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(result));
                        await originalBodyStream.WriteAsync(bytes, 0, bytes.Length);

                    }
                }


            }
            else
            {
                await _next(context);
            }
           

        }
        /// <summary>
        /// format request
        /// </summary>
        /// <param name="request">request</param>
        /// <returns></returns>
        private async Task<string> FormatRequest(HttpRequest request)
        {
            HttpRequestRewindExtensions.EnableBuffering(request);
            var body = request.Body;

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;

            return $" {request.Method} {request.Scheme}://{request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }
        /// <summary>
        /// format response
        /// </summary>
        /// <param name="response">response</param>
        /// <returns></returns>
        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return $"{response.StatusCode}: {text}";
        }

    }
}
