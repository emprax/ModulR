using System;
using Microsoft.Extensions.DependencyInjection;

namespace ModulR
{
    internal class ModuleServiceProvider : IModuleServiceProvider
    {
        private readonly IModule module;
        private readonly IServiceProvider supplimentaryProvider;

        internal ModuleServiceProvider(IModule module, IServiceProvider supplimentaryProvider)
        {
            this.module = module;
            this.supplimentaryProvider = supplimentaryProvider;
        }

        public TService Get<TService>() where TService : class
        {
            var provider = this.module?.GetServiceProvider(this.supplimentaryProvider) ?? throw new ModulRModuleNotFoundException(nameof(this.module));
            return provider.GetService<TService>() ?? throw new ModulRServiceNotFoundException(typeof(TService).Name);
        }
    }
}
