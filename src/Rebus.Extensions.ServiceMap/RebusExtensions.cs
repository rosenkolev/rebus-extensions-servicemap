using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Rebus.Bus;
using Rebus.Config;
using Rebus.Handlers;
using Rebus.Pipeline;
using Rebus.Routing.TypeBased;

namespace Rebus.Extensions.ServiceMap
{
    /// <summary>Rebus extensions.</summary>
    public static class RebusExtensions
    {
        /// <summary>Adds the default routing fallback queue.</summary>
        /// <param name="configurer">The configurer.</param>
        /// <param name="defaultQueue">The default queue.</param>
        /// <param name="mapper">A route mapper.</param>
        public static RebusConfigurer AddDefaultRouting(
            this RebusConfigurer configurer,
            string defaultQueue,
            Action<TypeBasedRouterConfigurationExtensions.TypeBasedRouterConfigurationBuilder> mapper = null) =>
            configurer.Routing(a =>
            {
                var builder = a.TypeBased().MapFallback(defaultQueue);
                mapper?.Invoke(builder);
            });

        /// <summary>Adds the command handler.</summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        public static IServiceCollection AddRebusCommand<TService, TRequest>(
            this IServiceCollection services,
            Func<TService, TRequest, CancellationToken, Task> func) =>
            services.AddTransient<IHandleMessages<TRequest>>(
                provider => new GenericCommandHandler<TService, TRequest>(
                    provider.GetRequiredService<TService>(),
                    provider.GetRequiredService<IMessageContext>(),
                    func));

        /// <summary>Adds the request handler.</summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        public static IServiceCollection AddRebusRequest<TService, TRequest, TResult>(
            this IServiceCollection services,
            Func<TService, TRequest, CancellationToken, Task<TResult>> func) =>
            services.AddTransient<IHandleMessages<TRequest>>(
                provider => new GenericRequestHandler<TService, TRequest, TResult>(
                    provider.GetRequiredService<TService>(),
                    provider.GetRequiredService<IMessageContext>(),
                    provider.GetRequiredService<IBus>(),
                    func));
    }
}
