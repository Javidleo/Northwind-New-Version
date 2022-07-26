using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.IOCConfig
{
    public static class IdentityIOCConfiguration
    {
        //private readonly IConfiguration Configuration;
        //private readonly IServiceCollection Services;

        //public IdentityIOCConfiguration(IConfiguration configuration, IServiceCollection services)
        //{
        //    Configuration = configuration;
        //    Services = services;
        //}
        public static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext(configuration.GetConnectionString("KnowledgeManagementDBConnection"));
            services.AddIdentity();
            services.AddIdentitySettings(configuration);
            services.ConfigureJWTAuthentication(configuration);
            services.AddApplicationServices();
        }
    }
}
