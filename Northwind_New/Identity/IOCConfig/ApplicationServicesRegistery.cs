using Identity.Repositories;
using Identity.Services;
using Identity.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;


namespace Identity.IOCConfig
{
    public static class ApplicationServicesRegistery
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<SecurityService>();
            services.AddTransient<TokenFactoryService>();
            services.AddTransient<TokenStoreService>();
            services.AddTransient<TokenValidatorService>();
            services.AddTransient<AuthService>();
            services.AddScoped<IIdentityDbInitializer, IdentityDbInitializer>();
            services.AddTransient<UserService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<EmailService>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            services.AddRepositories();
        }

        static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<UserJWTTokenRepository>();
            services.AddTransient<UserSmsTokenRepository>();
        }
    }
}
