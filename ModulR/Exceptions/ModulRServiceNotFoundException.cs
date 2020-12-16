using System;

namespace ModulR
{
    public class ModulRServiceNotFoundException : Exception
    {
        public ModulRServiceNotFoundException(string name) : base($"[ModulR-Exception] Could not find the required service of type '{name}' from the module.") { }
    }
}
