using System;

namespace ModulR
{
    public class ModulRServiceProviderFactory<TKey, TService> : IModulRServiceProviderFactory<TKey, TService> where TService : class
    {
        private readonly Func<TKey, TService> factory;

        public ModulRServiceProviderFactory(Func<TKey, TService> factory) => this.factory = factory;

        public TService Resolve(TKey key) => this.factory.Invoke(key);
    }
}
