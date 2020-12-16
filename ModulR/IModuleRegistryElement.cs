namespace ModulR
{
    public interface IModuleRegistryElement<TKey, TService> where TService : class
    {
        IModuleRegistry<TKey, TService> FromModule<TModule>() where TModule : class, IModule;
    }
}
