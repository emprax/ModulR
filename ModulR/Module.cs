using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModulR
{
    public abstract class Module : IModule
    {
        private readonly IServiceCollection services;
        private IServiceProvider provider;
        
        protected Module() => this.services = new ServiceCollection();

        public IConfiguration Configuration { get; private set; }

        protected abstract void Configure(IServiceCollection services);

        public IServiceProvider GetServiceProvider()
        {
            if (this.provider is null)
            {
                this.Configure(this.services);
                this.provider = this.services.BuildServiceProvider();
            }

            return this.provider;
        }

        public IModule WithConfiguration(IConfiguration configuration)
        {
            this.Configuration = configuration;
            return this;
        }
    }
}
