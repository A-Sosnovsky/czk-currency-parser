using Microsoft.Extensions.DependencyInjection;
using Parser.BackgroundService.Jobs;

namespace Parser.BackgroundService
{
    public static class ContainerInitializer
    {
        public static IServiceCollection Initialize(this IServiceCollection services)
        {
            services.AddTransient<JobFactory>();
            services.AddScoped<DailyParsingJob>();
            return services;
        }
    }
}