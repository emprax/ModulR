using System;
using Microsoft.Extensions.DependencyInjection;

namespace ModulR.Example.Tests
{
    public abstract class ModulRTestsBase
    {
        protected IServiceProvider Create(Action<IServiceCollection> builder)
        {
            var services = new ServiceCollection();
            builder.Invoke(services);
            return services.BuildServiceProvider();
        }
    }
}
