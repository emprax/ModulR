using Microsoft.Extensions.DependencyInjection;
using ModulR.Example.Console;
using Xunit;

namespace ModulR.Example.Tests
{
    public class ModuleFactoryModulRExampleTests : ModulRTestsBase
    {
        [Fact]
        public void ShouldVerifyFactoryUsage()
        {
            // Arrange
            var factory = base
                .Create((services, configuration) =>
                {
                    services
                        .AddModule<ArticleModule>()
                        .AddModule<OrderModule>(configuration)
                        .AddModuleRFactoryForService<string, ISharedService>(builder =>
                        {
                            builder.OnKey("TEST1").FromModule<ArticleModule>()
                                   .OnKey("TEST2").FromModule<OrderModule>();
                        });
                })
                .GetRequiredService<IModulRServiceProviderFactory<string, ISharedService>>();

            // Act & Assert
            Assert.Contains("Article", factory.Resolve("TEST1").GetFrom());
            Assert.Contains("Order", factory.Resolve("TEST2").GetFrom());
        }
    }
}
