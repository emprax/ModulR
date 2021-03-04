namespace ModulR
{
    /// <summary>
    /// A module specific service-provider. Contains logic to handle module and service provisioning.
    /// Could throw:
    ///  - ModulRModuleNotFoundException: When module not found.
    ///  - ModulRServiceNotFoundException: When service not found in module.
    /// </summary>
    public interface IModuleServiceProvider
    {
        /// <summary>
        /// Providing the requested service from the module.
        /// </summary>
        /// <typeparam name="TService">Type of the service to retrieve from the module.</typeparam>
        /// <returns>The requested service.</returns>
        TService Get<TService>() where TService : class;
    }
}
