using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModulR
{
    /// <summary>
    /// Mechanism used with the ServiceCollection extensions. Implementation is internally defined.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public interface IModuleProvider<TService> where TService : class
    {
        /// <summary>
        /// Providing a module based on its class type.
        /// </summary>
        /// <typeparam name="TModule">Type of the module.</typeparam>
        /// <returns>IServiceCollection.</returns>
        IServiceCollection From<TModule>() where TModule : class, IModule;

        /// <summary>
        /// Providing a module based on its class type and attaching the configuration.
        /// </summary>
        /// <typeparam name="TModule">Type of the module.</typeparam>
        /// <param name="configuration">The shared configuration.</param>
        /// <returns>IServiceCollection.</returns>
        IServiceCollection From<TModule>(IConfiguration configuration) where TModule : class, IModule;
    }
}
