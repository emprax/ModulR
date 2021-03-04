namespace ModulR
{
    /// <summary>
    /// The core of the module-factory, registers and provides the modules identified by key.
    /// </summary>
    /// <typeparam name="TKey">Type of the search-key.</typeparam>
    /// <typeparam name="TService">Type of the service/client.</typeparam>
    public interface IModuleRegistry<TKey, TService> where TService : class
    {
        IModuleRegistryElement<TKey, TService> OnKey(TKey key);
    }
}
