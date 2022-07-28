using Application;
using Application.Contracts;
using Application.Tests.Command;
using Application.Tests.Query;
using CommandHandling.MediatRAdopter;
using DataAccess;
using DataAccess.Repositories;
using DataSource;
using Microsoft.Extensions.DependencyInjection;
using QueryHandling.MediatRAdopter;

namespace AccreditationAPI
{
    public static class IOCConfig
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            //context
            services.AddScoped<ReadAndWriteDbContext>();
            services.AddScoped<IWriteDbContext>(x => x.GetService<ReadAndWriteDbContext>());
            services.AddScoped<IReadDbContext>(x => x.GetService<ReadAndWriteDbContext>());
            services.AddScoped<IUnitOfWork>(x => x.GetService<ReadAndWriteDbContext>());

            //Repositories
            // commands
            services.AddMessageHandlers();
            services.AddStation<CreateTestCommand, LoggingStation<CreateTestCommand>>();
            //services.AddStation<AddEtebarDorehCommand, LoggingStation<AddEtebarDorehCommand>>();
            services.AddScoped<Filters.UnitOfWorkFilter>();
            services.AddTransient<ITestRepository, TestRepository>();


            //EventHandling.MediatRAdopter.MediatRServiceConfiguration.WrapEventHandler<ProvinceAddedProjector, ProvinceAdded>(services);
            //services.AddEventHandlersFromAssembly<ProvinceAddedProjector>();
        }

        static void AddMessageHandlers(this IServiceCollection services)
        {
            services.AddCommandHandlersFromAssembly<TestHandler>();
            services.AddQueryHandlersFromAssembly<TestQueryHandler>();
            //services.AddEventHandlersFromAssembly(Assembly.GetAssembly(typeof(TeamDefinedReactor))
            //                                     , Assembly.GetAssembly(typeof(TeamListProjector))
            //                                     );
        }
    }
}
