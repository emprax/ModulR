using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace ModulR
{
    internal class ModuleRegistryElement<TKey, TService> : IModuleRegistryElement<TKey, TService> where TService : class
    {
        private readonly IModuleRegistry<TKey, TService> registry;
        private readonly IDictionary<TKey, Func<IServiceProvider, TService>> dictionary;
        private readonly TKey key;

        internal ModuleRegistryElement(
            IModuleRegistry<TKey, TService> moduleDictionary,
            IDictionary<TKey, Func<IServiceProvider, TService>> dictionary,
            TKey key)
        {
            this.registry = moduleDictionary;
            this.dictionary = dictionary;
            this.key = key;
        }

        public IModuleRegistry<TKey, TService> FromModule<TModule>() where TModule : class, IModule
        {
            if (!this.dictionary.ContainsKey(key))
            {
                this.dictionary.Add(this.key, provider => 
                {
                    var module = provider.GetService<TModule>() ?? throw new ModulRModuleNotFoundException(nameof(TModule));
                    
                    return module
                        .GetServiceProvider()
                        .GetService<TService>() ?? throw new ModulRServiceNotFoundException(nameof(TService));
                });
            }

            return this.registry;
        }
    }
}
