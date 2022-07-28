using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace AccreditationAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        //  private readonly ILoggerManager _logger;

        public ExceptionMiddleware(RequestDelegate next) //, ILoggerManager logger
        {
            //  _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // _logger.LogError($"Something went wrong: {ex}");
                await CustomException.Middleware.ExceptionMiddleware.HandleExceptionAsync(httpContext, ex);
            }
        }

    }
}
