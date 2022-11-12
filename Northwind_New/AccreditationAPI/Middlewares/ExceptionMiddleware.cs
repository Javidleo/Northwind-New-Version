using API.Common;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace API.Middlewares
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
                await HandleExceptionAsync(httpContext, ex);
            }
        }


        public async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            switch (exception)
            {
                case SecurityTokenExpiredException:
                    await SetException(context, StatusCodes.Status401Unauthorized, "token expired");
                    break;
                case SecurityTokenValidationException:
                    await SetException(context, StatusCodes.Status401Unauthorized, "token is not valid");
                    break;
                case AccessViolationException:
                    await SetException(context, StatusCodes.Status401Unauthorized, exception.Message);
                    break;
                case BadHttpRequestException:
                    await SetException(context, StatusCodes.Status400BadRequest, exception.Message);
                    break;
                case KeyNotFoundException:
                    await SetException(context, StatusCodes.Status404NotFound, exception.Message);
                    break;
                case DuplicateNameException:
                    await SetException(context, StatusCodes.Status409Conflict, exception.Message);
                    break;
                case SqlException:
                    await SetException(context, StatusCodes.Status500InternalServerError, "Database Error");
                    break;
                case NotFoundException:
                    await SetException(context, StatusCodes.Status404NotFound, exception.Message);
                    break;
                case NotAcceptableException:
                    await SetException(context, StatusCodes.Status406NotAcceptable, exception.Message);
                    break;
                case ForbiddenException:
                    await SetException(context, StatusCodes.Status403Forbidden, exception.Message);
                    break;
                case MethodNotAllowedException:
                    await SetException(context, StatusCodes.Status405MethodNotAllowed, exception.Message);
                    break;
                case ConflictException:
                    await SetException(context, StatusCodes.Status409Conflict, exception.Message);
                    break;
                default:
                    await SetException(context, StatusCodes.Status500InternalServerError, exception.Message);
                    break;
            }

        }
        private static async Task SetException(HttpContext context, int code, string message)
        {
            context.Response.StatusCode = code;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(new ErrorDetail()
            {
                StatusCode = code,
                Message = message
            }.ToString());
        }
    }
}
