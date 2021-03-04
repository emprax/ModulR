using System;

namespace ModulR
{
    /// <summary>
    /// ModulRServiceNotFoundException for indicating that a requested service could not be retrieved from the module. The message is pre-defined and only requires needs the service-name.
    /// </summary>
    public class ModulRServiceNotFoundException : Exception
    {
        public ModulRServiceNotFoundException(string name) : base($"[ModulR-Exception] Could not find the required service of type '{name}' from the module.") { }
    }
}
