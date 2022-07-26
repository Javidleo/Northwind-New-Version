using CommandHandling.MediatRAdopter;
using DataSource;
using Microsoft.Extensions.DependencyInjection;
using QueryHandling.MediatRAdopter;
using ReadModels.Common;
using ReadModels.Queries.PostQueries.GetPostList;
using UseCases.Commands.PostCommands;
using UseCases.Common;
using static UseCases.Commands.PostCommands.PostCommand;

namespace KnowledgeManagementAPI
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
            services.AddStation<PostCommand, LoggingStation<PostCommand>>();
            //services.AddStation<AddEtebarDorehCommand, LoggingStation<AddEtebarDorehCommand>>();
            services.AddScoped<Filters.UnitOfWorkFilter>();


            //EventHandling.MediatRAdopter.MediatRServiceConfiguration.WrapEventHandler<ProvinceAddedProjector, ProvinceAdded>(services);
            //services.AddEventHandlersFromAssembly<ProvinceAddedProjector>();
        }

        static void AddMessageHandlers(this IServiceCollection services)
        {
            services.AddCommandHandlersFromAssembly<PostHandler>();
            services.AddQueryHandlersFromAssembly<GetPostListQuery>();
            //services.AddEventHandlersFromAssembly(Assembly.GetAssembly(typeof(TeamDefinedReactor))
            //                                     , Assembly.GetAssembly(typeof(TeamListProjector))
            //                                     );
        }
    }
}
