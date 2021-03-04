using System;
using Microsoft.Extensions.DependencyInjection;

namespace ModulR
{
    public static class ModulRServiceProviderExtensions
    {
        /// <summary>
        /// Extension to provide a module specific service-provider.
        /// </summary>
        /// <typeparam name="TModule">Type of the module.</typeparam>
        /// <param name="provider">The original service-provider to get the requested module.</param>
        /// <returns>IModuleServiceProvider.</returns>
        public static IModuleServiceProvider FromModule<TModule>(this IServiceProvider provider) where TModule : class, IModule
        {
            return new ModuleServiceProvider(provider.GetService<TModule>());
        }
    }
}
