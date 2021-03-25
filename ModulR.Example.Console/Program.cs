using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModulR.Example.Console
{
    public class Program
    {
        public static void Main(string[] _)
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("config.json", false, false)
                .Build();

            services.AddTransient<ISomeNewService, SomeNewService>();

            // A
            services.AddModule<OrderModule>(configuration);

            // B
            services
                .AddTransitientModularClient<ISharedService>()
                .From<ArticleModule>();
            
            // C
            services.AddModuleRFactoryForService<string, ISharedService>((registry) => 
            {
                registry
                    .OnKey("Order").FromModule<OrderModule>()
                    .OnKey("Article").FromModule<ArticleModule>();
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
                .FromModule<OrderModule>()
                .Get<ISharedService>()
                .GetFrom();

            var result5 = provider
                .FromModule<OrderModule>()
                .Get<IOrderNewService>()
                .GetFrom();

            System.Console.WriteLine($"Result 1:  {result1}.");
            System.Console.WriteLine($"Result 2:  {result2}.");
            System.Console.WriteLine($"Result 3:  {result3}.");
            System.Console.WriteLine($"Result 4:  {result4}.");
            System.Console.WriteLine($"Result 5:  {result5}.");
        }
    }
}
