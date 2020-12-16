using Microsoft.Extensions.DependencyInjection;
using ModulR.Example.Console;
using ModulR.Extensions;
using Xunit;

namespace ModulR.Example.Tests
{
    public class FromModuleModulRExampleTests : ModulRTestsBase
    {
        [Fact]
        public void ShouldThrowModulRModuleNotFoundExceptionWhenModuleCannotBeFound()
        {
            // Act & Assert
            Assert.Throws<ModulRModuleNotFoundException>(() => new ServiceCollection()
                .BuildServiceProvider()
                .FromModule<OrderModule>()
                .Get<ISharedService>());
        }

        [Fact]
        public void ShouldThrowModulRServiceNotFoundExceptionWhenModuleCannotBeFound()
        {
            // Act & Assert
            Assert.Throws<ModulRServiceNotFoundException>(() => new ServiceCollection()
                .AddModule<OrderModule>()
                .BuildServiceProvider()
                .FromModule<OrderModule>()
                .Get<IDummyService>());
        }

        [Fact]
        public void ShouldProvideServiceFromModule()
        {
            // Act & Assert
            Assert.NotNull(new ServiceCollection()
                .AddModule<OrderModule>()
                .BuildServiceProvider()
                .FromModule<OrderModule>()
                .Get<ISharedService>());
        }
    }
}
