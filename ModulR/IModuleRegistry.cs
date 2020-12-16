namespace ModulR
{
    public interface IModuleRegistry<TKey, TService> where TService : class
    {
        IModuleRegistryElement<TKey, TService> OnKey(TKey key);

        TService Provide(TKey key);
    }
}
