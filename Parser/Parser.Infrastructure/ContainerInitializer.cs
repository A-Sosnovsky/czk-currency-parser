using Microsoft.Extensions.DependencyInjection;

namespace Parser.Infrastructure
{
    public static class ContainerInitializer
    {
        public static IServiceCollection Initialize(this IServiceCollection services)
        {
            services.AddScoped<IConfigurationProvider, EnvironmentVariablesConfigurationProvider>();
            return services;
        }
    }
}
