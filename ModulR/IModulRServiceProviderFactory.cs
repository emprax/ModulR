namespace ModulR
{
    public interface IModulRServiceProviderFactory<TKey, TService> where TService : class
    {
        TService Resolve(TKey key);
    }
}
