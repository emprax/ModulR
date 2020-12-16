using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModulR
{
    public interface IModuleProvider<TService> where TService : class
    {
        IServiceCollection From<TModule>() where TModule : class, IModule;

        IServiceCollection From<TModule>(IConfiguration configuration) where TModule : class, IModule;
    }
}
