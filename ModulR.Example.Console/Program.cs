using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModulR.Extensions;

namespace ModulR.Example.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("config.json", false, false)
                .Build();

            // A
            services.AddModule<OrderModule>(configuration);

            // B
            services
                .AddModularClient<ISharedService>()
                .From<ArticleModule>();
            
            // C
            services.AddModuleRFactoryForService<string, ISharedService>((key, registry) => 
            {
                return registry
                    .OnKey("Order").FromModule<OrderModule>()
                    .OnKey("Article").FromModule<ArticleModule>()
                    .Provide(key);
            });

            // GO!
            var provider = services.BuildServiceProvider();

            var result1 = provider
                .GetRequiredService<IModulRServiceProviderFactory<string, ISharedService>>()
                .Resolve("Order")
                .GetFrom();

            var result2 = provider
                .GetRequiredService<IModulRServiceProviderFactory<string, ISharedService>>()
                .Resolve("Article")
                .GetFrom();

            var result3 = provider
                .GetRequiredService<ISharedService>()
                .GetFrom();

            var result4 = provider
                .FromModule<ArticleModule>()
                .Get<ISharedService>()
                .GetFrom();

            System.Console.WriteLine($"Result 1:  {result1}.");
            System.Console.WriteLine($"Result 1:  {result2}.");
            System.Console.WriteLine($"Result 1:  {result3}.");
            System.Console.WriteLine($"Result 1:  {result4}.");
        }
    }
}
