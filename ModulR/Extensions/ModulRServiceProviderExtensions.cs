using System;
using Microsoft.Extensions.DependencyInjection;

namespace ModulR
{
    public static class ModulRServiceProviderExtensions
    {
        public static IModuleServiceProvider FromModule<TModule>(this IServiceProvider provider) where TModule : class, IModule
        {
            return new ModuleServiceProvider(provider.GetService<TModule>());
        }
    }
}
