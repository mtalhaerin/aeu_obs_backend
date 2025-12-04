using Microsoft.Extensions.DependencyInjection;

namespace Core.Utilities.Ioc
{
    public static class ServiceTool
    {
        public static IServiceProvider serviceProvider { get; private set; }
        public static IServiceCollection Create(IServiceCollection services)
        {
            serviceProvider = services.BuildServiceProvider();
            return services;
        }
    }
}
