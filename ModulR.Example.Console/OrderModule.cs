using Microsoft.Extensions.DependencyInjection;

namespace ModulR.Example.Console
{
    public class OrderModule : Module
    {
        protected override void Configure(IServiceCollection services)
        {
            services
                .AddTransient<ISharedService>(_ => new OrderSharedService(base.Configuration["Data:ModuleParameter"]))
                .AddTransient<IOrderNewService, OrderNewService>();
        }
    }
}
