using System;
using Microsoft.Extensions.Configuration;

namespace ModulR
{
    /// <summary>
    /// IModule interface, fundamental ModulR building block. Recommented to utilize the Module abstract class.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Configuration that is shared with the module.
        /// </summary>
        IConfiguration Configuration { get; }

        /// <summary>
        /// Used primarly by the internal ModulR DI mechanisms to attach the shared configuration.
        /// </summary>
        /// <param name="configuration">The shared configuration.</param>
        /// <returns>This module.</returns>
        IModule WithConfiguration(IConfiguration configuration);

        /// <summary>
        /// Creates the IServiceProvider regarding the registered dependencies within this module.
        /// </summary>
        /// <param name="supplimentaryProvider">The service-provider from the main DI container.</param>
        /// <returns>IServiceProvider.</returns>
        IServiceProvider GetServiceProvider(IServiceProvider supplimentaryProvider);
    }
}
