using System;
using Microsoft.Extensions.DependencyInjection;

namespace ModulR.Extensions
{
    public static class ModulRServiceCollectionExtensions
    {
        public static IServiceCollection AddModule<TModule>(this IServiceCollection services) where TModule : class, IModule
        {
            return services.AddSingleton<TModule>();
        }

        public static IModuleProvider<TService> AddModularClient<TService, TImplementation>(this IServiceCollection services) 
            where TService : class 
            where TImplementation : class, TService
        {
            return new ModuleProvider<TService, TImplementation>(services);
        }

        public static IModuleProvider<TService> AddModularClient<TService>(this IServiceCollection services) where TService : class 
        {
            return new ModuleProvider<TService>(services);
        }

        public static IServiceCollection AddModuleRFactoryForService<TKey, TService>(this IServiceCollection services, Func<TKey, IModuleRegistry<TKey, TService>, TService> builder) where TService : class
        {
            return services
                .AddSingleton<IModuleRegistry<TKey, TService>>(provider => new ModuleRegistry<TKey, TService>(provider))
                .AddTransient<IModulRServiceProviderFactory<TKey, TService>>(provider => new ModulRServiceProviderFactory<TKey, TService>(key => 
                {
                    return builder
                        .Invoke(key, provider
                        .GetRequiredService<IModuleRegistry<TKey, TService>>());
                }));
        }
    }
}
