﻿using Microsoft.Extensions.DependencyInjection;
using ModulR.Example.Console;
using Xunit;

namespace ModulR.Example.Tests
{
    public class BasicModulRExampleTests : ModulRTestsBase
    {
        [Fact]
        public void ShouldAddModules()
        {
            // Arrange
            var provider = this.Create((services, configuration) =>
            {
                services.AddModule<OrderModule>(configuration);
                services.AddModule<ArticleModule>();
            });

            // Act & Assert
            Assert.True(provider.GetRequiredService<OrderModule>()?.GetServiceProvider(provider)?.GetService<ISharedService>() is OrderSharedService);
            Assert.True(provider.GetRequiredService<ArticleModule>()?.GetServiceProvider(provider)?.GetService<ISharedService>() is ArticleSharedService);
        }

        [Fact]
        public void ShouldAddServiceByModule()
        {
            // Arrange
            var provider = this.Create((services, configuration) =>
            {
                services
                    .AddModule<OrderModule>(configuration)
                    .AddModularClient<ISharedService>(ServiceLifetime.Transient)
                    .From<ArticleModule>();
            });

            // Act & Assert
            Assert.True(provider.GetRequiredService<OrderModule>()?.GetServiceProvider(provider)?.GetService<ISharedService>() is OrderSharedService);
            Assert.True(provider.GetService<ArticleModule>()?.GetServiceProvider(provider)?.GetService<ISharedService>() is ArticleSharedService);
            Assert.True(provider.GetRequiredService<ISharedService>() is ArticleSharedService);
        }
    }
}
