using System;

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

        public TService Get<TService>() where TService : class => this.Get(typeof(TService)) as TService;

        public object Get(Type service)
        {
            var provider = this.module?.GetServiceProvider(this.supplimentaryProvider) ?? throw new ModulRModuleNotFoundException(nameof(this.module));
            return provider.GetService(service) ?? throw new ModulRServiceNotFoundException(service.Name);
        }
    }
}
