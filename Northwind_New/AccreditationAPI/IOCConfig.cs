using API.Filters;
using Application.Common;
using Application.Test.Command;
using Application.Test.Query;
using CommandHandling.MediatRAdopter;
using DataAccess;
using DataSource;
using Microsoft.Extensions.DependencyInjection;
using QueryHandling.MediatRAdopter;

namespace API
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
            services.AddStation<TestCommand, LoggingStation<TestCommand>>();
            //services.AddStation<AddEtebarDorehCommand, LoggingStation<AddEtebarDorehCommand>>();
            services.AddScoped<UnitOfWorkFilter>();


            //EventHandling.MediatRAdopter.MediatRServiceConfiguration.WrapEventHandler<ProvinceAddedProjector, ProvinceAdded>(services);
            //services.AddEventHandlersFromAssembly<ProvinceAddedProjector>();
        }

        static void AddMessageHandlers(this IServiceCollection services)
        {
            services.AddCommandHandlersFromAssembly<TestCommandHandler>();
            services.AddQueryHandlersFromAssembly<TestQueryHandler>();
            //services.AddEventHandlersFromAssembly(Assembly.GetAssembly(typeof(TeamDefinedReactor))
            //                                     , Assembly.GetAssembly(typeof(TeamListProjector))
            //                                     );
        }
    }
}
