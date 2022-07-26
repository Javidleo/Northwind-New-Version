using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace KnowledgeManagementAPI
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IWebHostEnvironment env)
        {
            try
            {
                await _next(context);
            }
            catch (Exception)
            {
                // await HandleExceptionAsync(context, ex, env);
            }
        }

        //private static Task HandleExceptionAsync(HttpContext context, Exception exception, IWebHostEnvironment env)
        //{
        //    HttpStatusCode status;
        //    string message;
        //    var stackTrace = String.Empty;

        //    var exceptionType = exception.GetType();
        //    if (exceptionType == typeof(BadRequestException))
        //    {
        //        message = exception.Message;
        //        status = HttpStatusCode.BadRequest;
        //    }
        //    else if (exceptionType == typeof(NotFoundException))
        //    {
        //        message = exception.Message;
        //        status = HttpStatusCode.NotFound;
        //    }
        //    else
        //    {
        //        status = HttpStatusCode.InternalServerError;
        //        message = exception.Message;
        //        if (env.IsEnvironment("Development"))
        //            stackTrace = exception.StackTrace;
        //    }

        //    var result = JsonSerializer.Serialize(new { error = message, stackTrace });
        //    context.Response.ContentType = "application/json";
        //    context.Response.StatusCode = (int)status;
        //    return context.Response.WriteAsync(result);
        //}
    }


    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
