using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModulR
{
    internal class ModuleProvider<TService, TImplementation> : IModuleProvider<TService>
        where TService : class
        where TImplementation : class, TService
    {
        private readonly IServiceCollection services;

        internal ModuleProvider(IServiceCollection services) => this.services = services;

        public IServiceCollection From<TModule>() where TModule : class, IModule => this.From<TModule>(null);

        public IServiceCollection From<TModule>(IConfiguration configuration) where TModule : class, IModule
        {
            var module = this.services.BuildServiceProvider().GetService<TModule>();
            if (module is null)
            {
                this.services.AddModule<TModule>(configuration);
            }

            return this.services.AddTransient<TService, TImplementation>(provider =>
            {
                var otherModule = provider.GetService<TModule>() ?? throw new ModulRModuleNotFoundException(nameof(TModule));
                
                return otherModule
                    .GetServiceProvider(provider)
                    .GetServices<TService>()?
                    .FirstOrDefault(x => x is TImplementation) as TImplementation ?? throw new ModulRServiceNotFoundException(nameof(TImplementation));
            });
        }
    }

    internal class ModuleProvider<TService> : IModuleProvider<TService> where TService : class
    {
        private readonly IServiceCollection services;

        internal ModuleProvider(IServiceCollection services) => this.services = services;

        public IServiceCollection From<TModule>() where TModule : class, IModule => this.From<TModule>(null);

        public IServiceCollection From<TModule>(IConfiguration configuration) where TModule : class, IModule
        {
            var module = this.services.BuildServiceProvider().GetService<TModule>();
            if (module is null && configuration is null)
            {
                this.services.AddModule<TModule>(configuration);
            }

            return this.services.AddTransient(provider =>
            {
                var otherModule = provider.GetRequiredService<TModule>() ?? throw new ModulRModuleNotFoundException(nameof(TModule));

                return otherModule
                    .GetServiceProvider(provider)
                    .GetService<TService>() ?? throw new ModulRServiceNotFoundException(nameof(TService));
            });
        }
    }
}
