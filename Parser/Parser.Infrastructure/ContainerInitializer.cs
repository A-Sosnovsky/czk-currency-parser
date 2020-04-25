using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Parser.Infrastructure
{
    public static class ContainerInitializer
    {
        public static IServiceCollection Initialize(this IServiceCollection services)
        {
            services.TryAdd(ServiceDescriptor.Singleton<ILoggerFactory, LoggerFactory>());
            services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(Logger<>)));
            return services;
        }
    }
}
