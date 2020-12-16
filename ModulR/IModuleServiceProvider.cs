namespace ModulR
{
    public interface IModuleServiceProvider
    {
        TService Get<TService>() where TService : class;
    }
}
