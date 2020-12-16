using System;

namespace ModulR
{
    public class ModulRModuleNotFoundException : Exception
    {
        public ModulRModuleNotFoundException(string name) : base($"[ModulR-Exception] Could not find the requested module '{name}'.") { }
    }
}
