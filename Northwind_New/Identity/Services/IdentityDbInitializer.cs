using Identity.Common;
using Identity.DataSource;
using Identity.Models;
using Identity.Services.Contracts;
using Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Identity.Services
{
    public class IdentityDbInitializer : IIdentityDbInitializer
    {
        private readonly IServiceScopeFactory ScopeFactory;
        private readonly IOptionsSnapshot<SiteSettings> AdminUserSeedOptions;
        private readonly IAppUserManager UserManager;
        private readonly IAppRoleManager RoleManager;

        public IdentityDbInitializer(IServiceScopeFactory scopeFactory
                , IOptionsSnapshot<SiteSettings> adminUserSeedOptions
                , IAppUserManager userManager
                , IAppRoleManager roleManager)
        {
            ScopeFactory = scopeFactory;
            AdminUserSeedOptions = adminUserSeedOptions;
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public void Initialize()
        {
            using var scope = ScopeFactory.CreateScope();
            var services = scope.ServiceProvider;
            using var context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            //try
            //{
            //    context.Database.mi;
            //}
            //catch
            //{

            //}

            //_scopeFactory.RunScopedService<ApplicationDbContext>(context =>
            //{
            //    if (_adminUserSeedOptions.Value.ActiveDatabase == ActiveDatabase.InMemoryDatabase)
            //    {
            //        context.Database.EnsureCreated();
            //    }
            //    else
            //    {
            //        context.Database.Migrate();
            //    }
            //});
        }

        public void SeedData()
        {
            //using (var scope = ScopeFactory.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    var identityDbSeedData = services.GetRequiredService<IdentityDbInitializer>();

            var result = SeedDatabaseWithAdminUserAsync().Result;
            if (result == IdentityResult.Failed())
            {
                throw new InvalidOperationException(result.DumpErrors());
            }
            //}
        }

        public async Task<IdentityResult> SeedDatabaseWithAdminUserAsync()
        {
            var adminUserSeed = AdminUserSeedOptions.Value.AdminUserSeed;
            //adminUserSeed.CheckArgumentIsNull(nameof(adminUserSeed));

            var name = adminUserSeed.Username;
            var password = adminUserSeed.Password;
            var email = adminUserSeed.Email;
            var roleName = adminUserSeed.RoleName;

            //  var thisMethodName = nameof(SeedDatabaseWithAdminUserAsync);

            var adminUser = await UserManager.FindByNameAsync(name);
            if (adminUser != null)
            {
                //  _logger.LogInformation($"{thisMethodName}: adminUser already exists.");
                return IdentityResult.Success;
            }

            //Create the `Admin` Role if it does not exist
            var adminRole = await RoleManager.FindByNameAsync(roleName);
            if (adminRole == null)
            {
                adminRole = new AppRole(roleName);
                var adminRoleResult = await RoleManager.CreateAsync(adminRole);
                if (adminRoleResult == IdentityResult.Failed())
                {
                    //  _logger.LogError($"{thisMethodName}: adminRole CreateAsync failed. {adminRoleResult.DumpErrors()}");
                    return IdentityResult.Failed();
                }
            }
            else
            {
                //  _logger.LogInformation($"{thisMethodName}: adminRole already exists.");
            }

            adminUser = new AppUser
            {
                UserName = name,
                Email = email,
                EmailConfirmed = true,
                // IsEmailPublic = true,
                LockoutEnabled = true
            };
            var adminUserResult = await UserManager.CreateAsync(adminUser, password);
            if (adminUserResult == IdentityResult.Failed())
            {
                // _logger.LogError($"{thisMethodName}: adminUser CreateAsync failed. {adminUserResult.DumpErrors()}");
                return IdentityResult.Failed();
            }

            var setLockoutResult = await UserManager.SetLockoutEnabledAsync(adminUser, enabled: false);
            if (setLockoutResult == IdentityResult.Failed())
            {
                // _logger.LogError($"{thisMethodName}: adminUser SetLockoutEnabledAsync failed. {setLockoutResult.DumpErrors()}");
                return IdentityResult.Failed();
            }

            // var fromDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            var fromDate = DateTime.Now.Date;
            var addToRoleResult = await UserManager.AddUserRole(adminUser.Id, adminRole.Id, fromDate, null);
            if (addToRoleResult == IdentityResult.Failed())
            {
                // _logger.LogError($"{thisMethodName}: adminUser AddToRoleAsync failed. {addToRoleResult.DumpErrors()}");
                return IdentityResult.Failed();
            }

            return IdentityResult.Success;
        }
    }
}
