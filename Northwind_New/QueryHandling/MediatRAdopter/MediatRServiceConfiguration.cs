using MediatR;
using Microsoft.Extensions.DependencyInjection;
using QueryHandling.Abstractions;
using QueryHandling.MediatRAdopter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QueryHandling.MediatRAdopter
{
    public static class MediatRServiceConfiguration
    {
        public static bool IsQueryHandler(this Type @class)
            => @class.IsClass && @class.GetInterfaceOfType(typeof(IHandleQuery<,>)).Any();

        static void WrapQueryHandlerByType(Type handler, Type query, Type viewModel, IServiceCollection services)
        => typeof(MediatRServiceConfiguration)
            .GetMethod(nameof(WrapQueryHandler), BindingFlags.Static | BindingFlags.NonPublic)
            .MakeGenericMethod(handler, query, viewModel)
            .Invoke(null, new object[] { services });

        static void WrapQueryHandler<THandler, TQuery, TViewModel>(IServiceCollection services)
            where TQuery : Query<TViewModel>
            where THandler : class, IHandleQuery<TQuery, TViewModel>
            where TViewModel : IAmAViewModel
            => services.AddScoped(serviceProvider =>
            {
                var ourHandler = ActivatorUtilities.CreateInstance<THandler>(serviceProvider);
                return new MediatRQueryHandlerAdopter<TQuery, TViewModel>(ourHandler) as
                IRequestHandler<MediatRQueryEnvelope<TQuery, TViewModel>, TViewModel>;
            }
                );
        public static IServiceCollection AddQueryHandlersFromAssembly<T>(this IServiceCollection services)
            => AddQueryHandlersFromAssembly(services, Assembly.GetAssembly(typeof(T)));

        public static IServiceCollection AddQueryHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            foreach (var (HandlerType, HandleMethodSignatures) in assembly.GetTypesOfHandlersAndQueries())
            {
                foreach (var signature in HandleMethodSignatures)
                {
                    WrapQueryHandlerByType(HandlerType, signature.Query, signature.ViewModel, services);
                }
            }
            return services.AddMediatR(assembly)
            .AddScoped<IQueryBus, MediatRQueryBusAdopter>();
        }

        public static IEnumerable<(Type HandlerType, IEnumerable<(Type Query, Type ViewModel)> HandleMethodSignatures)> GetTypesOfHandlersAndQueries(this Assembly assembly)
        => assembly.GetTypes().Where(IsQueryHandler)
            .Select(@class =>
                (HandlerType: @class,
                    HandleMethodSignatures: @class.GetInterfaceOfType(typeof(IHandleQuery<,>))
                        .Select(i => (i.GetGenericArguments()[0], i.GetGenericArguments()[1])))
            );
    }
}
