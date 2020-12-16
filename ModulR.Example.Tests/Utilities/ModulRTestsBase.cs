using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModulR.Example.Tests
{
    public abstract class ModulRTestsBase
    {
        protected IServiceProvider Create(Action<IServiceCollection, IConfiguration> builder)
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("config.json", false, false)
                .Build();

            builder.Invoke(services, configuration);
            return services.BuildServiceProvider();
        }
    }
}
