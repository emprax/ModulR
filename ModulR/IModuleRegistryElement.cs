namespace ModulR
{
    /// <summary>
    /// Element for in the collection held by the ModuleRegistry, used for the IModulRServiceProviderFactory.
    /// </summary>
    /// <typeparam name="TKey">Type of the search-key</typeparam>
    /// <typeparam name="TService"></typeparam>
    public interface IModuleRegistryElement<TKey, TService> where TService : class
    {
        /// <summary>
        /// Determine which module to use.
        /// </summary>
        /// <typeparam name="TModule">Type of the module.</typeparam>
        /// <returns>The initiating registry.</returns>
        IModuleRegistry<TKey, TService> FromModule<TModule>() where TModule : class, IModule;
    }
}
