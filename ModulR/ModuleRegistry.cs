using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ModulR
{
    internal class ModuleRegistry<TKey, TService> : IModuleRegistry<TKey, TService> where TService : class
    {
        private readonly ConcurrentDictionary<TKey, Func<IServiceProvider, TService>> dictionary;
        private readonly IServiceProvider provider;

        internal ModuleRegistry(IServiceProvider provider)
        {
            this.dictionary = new ConcurrentDictionary<TKey, Func<IServiceProvider, TService>>();
            this.provider = provider;
        }

        public IModuleRegistryElement<TKey, TService> OnKey(TKey key) => new ModuleRegistryElement<TKey, TService>(this, this.dictionary, key);

        public TService Provide(TKey key)
        {
            return (this.dictionary.TryGetValue(key, out var invoker) && invoker != null)
                ? invoker.Invoke(this.provider) ?? throw new ModulRServiceNotFoundException(nameof(TService))
                : throw new KeyNotFoundException($"Could not find registered provider for key '{key}'.");
        }
    }
}
