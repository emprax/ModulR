﻿using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModulR
{
    /// <summary>
    /// Abstract default implementation of the IModule interface. This class is the recommended to be utilized instead of directly implementing the IModule interface.
    /// </summary>
    public abstract class Module : IModule
    {
        private readonly IServiceCollection services;
        private readonly ConcurrentDictionary<int, ServiceDescriptor> core;
        private IServiceProvider provider;
        
        protected Module()
        {
            this.core = new ConcurrentDictionary<int, ServiceDescriptor>();
            this.services = new ModulRServiceCollection(core);
        }

        /// <summary>
        /// Configuration that is shared with the module.
        /// </summary>
        public IConfiguration Configuration { get; private set; }

        /// <summary>
        /// The to be implemented module definition for registering services.
        /// </summary>
        /// <param name="services">The custom service-collection for this module.</param>
        protected abstract void Configure(IServiceCollection services);

        /// <summary>
        /// Creates the IServiceProvider regarding the registered dependencies within this module.
        /// </summary>
        /// <param name="supplimentaryProvider">The service-provider from the main DI container.</param>
        /// <returns>IServiceProvider.</returns>
        public IServiceProvider GetServiceProvider(IServiceProvider supplimentaryProvider)
        {
            if (this.provider is null)
            {
                this.Configure(this.services);
                this.provider = new ModulRServiceProvider(supplimentaryProvider, this.core);
            }

            return this.provider;
        }

        /// <summary>
        /// Used primarly by the internal ModulR DI mechanisms to attach the shared configuration.
        /// </summary>
        /// <param name="configuration">The shared configuration.</param>
        /// <returns>This module.</returns>
        public IModule WithConfiguration(IConfiguration configuration)
        {
            this.Configuration = configuration;
            return this;
        }
    }
}
