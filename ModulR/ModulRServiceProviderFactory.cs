using System;

namespace ModulR
{
    internal class ModulRServiceProviderFactory<TKey, TService> : IModulRServiceProviderFactory<TKey, TService> where TService : class
    {
        private readonly Func<TKey, TService> factory;

        internal ModulRServiceProviderFactory(Func<TKey, TService> factory) => this.factory = factory;

        public TService Resolve(TKey key) => this.factory.Invoke(key);
    }
}
