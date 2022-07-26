using Identity.DataSource;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.IOCConfig
{
    public static class DbContextConfiguration
    {
        public static void AddDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                                  options.UseSqlServer(connectionString));

            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<IUnitOfWork>(x => x.GetService<ApplicationDbContext>());

        }
    }
}
