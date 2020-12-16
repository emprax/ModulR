using System;
using Microsoft.Extensions.Configuration;

namespace ModulR
{
    public interface IModule
    {
        IConfiguration Configuration { get; }

        IModule WithConfiguration(IConfiguration configuration);

        IServiceProvider GetServiceProvider();
    }
}
