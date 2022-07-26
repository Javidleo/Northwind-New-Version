using Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Identity.IOCConfig
{
    public static class JWTAuthenticationConfiguration
    {
        public static void ConfigureJWTAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,  // tolerance for the expiration date .Default is 5 min
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["BearerTokens:Issuer"],
                    ValidAudience = Configuration["BearerTokens:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["BearerTokens:Key"])),
                    TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["BearerTokens:EncryptKey"]))
                };
                options.Events = new JwtBearerEvents
                {
                    // //////زمانی فراخوانی می‌شود که اعتبارسنج‌های تنظیمی فوق، با شکست مواجه شوند
                    // OnAuthenticationFailed = context =>
                    //{
                    //    //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                    //    //logger.LogError("Authentication failed.", context.Exception);
                    //     return Task.CompletedTask;
                    //   // throw new SecurityTokenValidationException();
                    //},
                    //پس از کامل شدن اعتبارسنجی توکن دریافتی از سمت کاربر فراخوانی می‌شود
                    OnTokenValidated = context =>
                    {
                        var tokenValidatorService = context.HttpContext.RequestServices.GetRequiredService<TokenValidatorService>();
                        return tokenValidatorService.ValidateAsync(context);
                    },
                    /////برای حالتی است که توکن دریافتی، توسط هدر مخصوص Bearer به سمت سرور ارسال نمی‌شود
                    // OnMessageReceived = context =>
                    //{
                    //    return Task.CompletedTask;
                    //},
                    // ////یک سری دیگر از خطاهای اعتبارسنجی را پیش از ارسال آن‌ها به فراخوان در اختیار ما قرار می‌دهد
                    OnChallenge = context =>
                    {
                        //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                        //logger.LogError("OnChallenge error", context.Error, context.ErrorDescription);
                        //return Task.CompletedTask;

                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        // Ensure we always have an error and error description.
                        if (string.IsNullOrEmpty(context.Error))
                            context.Error = "invalid_token";
                        if (string.IsNullOrEmpty(context.ErrorDescription))
                            context.ErrorDescription = "This request requires a valid JWT access token to be provided";

                        // Add some extra context for expired tokens.
                        if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
                            context.Response.Headers.Add("x-token-expired", authenticationException.Expires.ToString("o"));
                            context.ErrorDescription = $"The token expired on {authenticationException.Expires.ToString("o")}";
                        }

                        return context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            error = context.Error,
                            error_description = context.ErrorDescription
                        }));
                    }
                };
            })
            //.AddGoogle("google", opt =>
            //{
            //    var googleAuth = Configuration.GetSection("Authentication:Google");
            //    opt.ClientId = googleAuth["ClientId"];
            //    opt.ClientSecret = googleAuth["ClientSecret"];
            //    opt.SignInScheme = IdentityConstants.ExternalScheme;
            //})
            ;
        }
    }
}
