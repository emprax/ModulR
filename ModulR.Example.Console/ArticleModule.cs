using Microsoft.Extensions.DependencyInjection;

namespace ModulR.Example.Console
{
    public class ArticleModule : Module
    {
        protected override void Configure(IServiceCollection services)
        {
            services.AddTransient<ISharedService, ArticleSharedService>();
        }
    }
}
