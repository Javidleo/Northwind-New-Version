
using Identity.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity.IOCConfig
{
    public static class DbContextOptionsExtensions
    {

        public static IHost InitializeDb(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var identityDbInitialize = scope.ServiceProvider.GetRequiredService<IIdentityDbInitializer>();
                //identityDbInitialize.Initialize();
                identityDbInitialize.SeedData();
            }
            return host;

        }
    }
}
