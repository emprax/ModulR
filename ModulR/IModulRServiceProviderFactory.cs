namespace ModulR
{
    /// <summary>
    /// Provider factory for retrieving specific implementations of services/clients from particular modules.
    /// </summary>
    /// <typeparam name="TKey">Type of the search-key.</typeparam>
    /// <typeparam name="TService">Type of the service/client.</typeparam>
    public interface IModulRServiceProviderFactory<TKey, TService> where TService : class
    {
        /// <summary>
        /// Resolves the service/client by means of providing the key to the right module identification.
        /// </summary>
        /// <param name="key">Type of the search-key.</param>
        /// <returns>The specific service/client.</returns>
        TService Resolve(TKey key);
    }
}
