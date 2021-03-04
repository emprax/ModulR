using System;

namespace ModulR
{
    /// <summary>
    /// ModulRModuleNotFoundException for indicating that a requested ModulR module could not be found. The message is pre-defined and only requires needs the module-name.
    /// </summary>
    public class ModulRModuleNotFoundException : Exception
    {
        public ModulRModuleNotFoundException(string name) : base($"[ModulR-Exception] Could not find the requested module '{name}'.") { }
    }
}
