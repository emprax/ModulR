using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModulR
{
    public static class ModulRServiceCollectionExtensions
    {
        /// <summary>
        /// Simply adding the module as singleton dependency.
        /// </summary>
        /// <typeparam name="TModule">Type of the module.</typeparam>
        /// <param name="services">IServiceCollection.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddModule<TModule>(this IServiceCollection services) where TModule : class, IModule
        {
            return services.AddSingleton<TModule>();
        }

        /// <summary>
        /// Add a module with configuration being set. Note that when leaving the configuration to be null, a handful of processes will possibly throw exceptions.
        /// </summary>
        /// <typeparam name="TModule">Type of the module.</typeparam>
        /// <param name="configuration">The IConfiguration</param>
        /// <param name="services">IServiceCollection.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddModule<TModule>(this IServiceCollection services, IConfiguration configuration) where TModule : class, IModule
        {
            return services.AddSingleton(provider =>
            {
                var constructor = typeof(TModule)?
                    .GetConstructors()?
                    .FirstOrDefault();

                var parameters = constructor?
                    .GetParameters()?
                    .Select(p => provider.GetService(p?.ParameterType))?
                    .ToArray();

                var instance = constructor.Invoke(parameters) as TModule;
                return instance?.WithConfiguration(configuration) as TModule;
            });
        }

        /// <summary>
        /// Creating a module directly for a client implementation.
        /// </summary>
        /// <typeparam name="TService">Type of the client abstraction/interface.</typeparam>
        /// <typeparam name="TImplementation">Type of the concrete implementation of the client.</typeparam>
        /// <param name="services">IServiceCollection.</param>
        /// <returns>IModuleProvider.</returns>
        public static IModuleProvider<TService> AddModularClient<TService, TImplementation>(this IServiceCollection services) 
            where TService : class 
            where TImplementation : class, TService
        {
            return new ModuleProvider<TService, TImplementation>(services);
        }

        /// <summary>
        /// Creating a module directly for a client.
        /// </summary>
        /// <typeparam name="TService">Type of the client.</typeparam>
        /// <param name="services">IServiceCollection.</param>
        /// <returns>IModuleProvider.</returns>
        public static IModuleProvider<TService> AddModularClient<TService>(this IServiceCollection services) where TService : class 
        {
            return new ModuleProvider<TService>(services);
        }

        /// <summary>
        /// Creating a module-factory (registered transient) and the module-registry (registered singleton).
        /// </summary>
        /// <typeparam name="TKey">Type of the search-key.</typeparam>
        /// <typeparam name="TService">Type of the client.</typeparam>
        /// <param name="services">IServiceCollection.</param>
        /// <param name="builder">The building block for setting up the factory.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddModuleRFactoryForService<TKey, TService>(this IServiceCollection services, Action<IModuleRegistry<TKey, TService>> builder) where TService : class
        {
            return services
                .AddSingleton(provider =>
                {
                    var registry = new ModuleRegistry<TKey, TService>(provider);
                    builder.Invoke(registry);
                    return registry;
                })
                .AddTransient<IModulRServiceProviderFactory<TKey, TService>>(provider => new ModulRServiceProviderFactory<TKey, TService>(key => 
                {
                    return provider
                        .GetRequiredService<ModuleRegistry<TKey, TService>>()
                        .Provide(key);
                }));
        }
    }
}
