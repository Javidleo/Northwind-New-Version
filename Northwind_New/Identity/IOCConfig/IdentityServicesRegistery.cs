using Identity.DataSource;
using Identity.Models;
using Identity.Repositories;
using Identity.Services;
using Identity.Services.Contracts;
using Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Identity.IOCConfig
{
    public static class IdentityServicesRegistery
    {
        public static void AddIdentity(this IServiceCollection services)
        {
            //For Identity
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.SignIn.RequireConfirmedPhoneNumber = true;
                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            })
                .AddUserStore<AppUserStore>()
                .AddUserManager<AppUserManager>()
                .AddRoleStore<AppRoleStore>()
                .AddRoleManager<AppRoleManager>()
                .AddSignInManager<AppSignInManager>()
                //.AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddCustomIdentityServices();
        }
        static void AddCustomIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<IAppRoleStore, AppRoleStore>();
            services.AddScoped<RoleStore<AppRole, ApplicationDbContext, string, AppUserRole, AppRoleClaim>, AppRoleStore>();

            services.AddScoped<IAppRoleManager, AppRoleManager>();
            services.AddScoped<RoleManager<AppRole>, AppRoleManager>();

            services.AddScoped<IAppUserStore, AppUserStore>();
            services.AddScoped<UserStore<AppUser, AppRole, ApplicationDbContext, string, AppUserClaim, AppUserRole, AppUserLogin, AppUserToken, AppRoleClaim>, AppUserStore>();

            services.AddScoped<IAppUserManager, AppUserManager>();
            services.AddScoped<UserManager<AppUser>, AppUserManager>();

            services.AddScoped<IAppSignInManager, AppSignInManager>();
            services.AddScoped<SignInManager<AppUser>, AppSignInManager>();

        }








    }
}
