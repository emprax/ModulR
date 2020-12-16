using Microsoft.Extensions.DependencyInjection;

namespace ModulR
{
    internal class ModuleServiceProvider : IModuleServiceProvider
    {
        private readonly IModule module;

        internal ModuleServiceProvider(IModule module) => this.module = module;

        public TService Get<TService>() where TService : class
        {
            var provider = this.module?.GetServiceProvider() ?? throw new ModulRModuleNotFoundException(nameof(this.module));
            return provider.GetService<TService>() ?? throw new ModulRServiceNotFoundException(typeof(TService).Name);
        }
    }
}
