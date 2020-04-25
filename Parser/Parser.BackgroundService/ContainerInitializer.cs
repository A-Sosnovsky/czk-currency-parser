using Microsoft.Extensions.DependencyInjection;
using Parser.BackgroundService.Jobs;

namespace Parser.BackgroundService
{
    public static class ContainerInitializer
    {
        public static IServiceCollection Initialize(this IServiceCollection services)
        {
            services.AddSingleton<JobFactory>();
            services.AddScoped<DailyParsingJob>();
            services.AddScoped<YearParsingJob>();
            return services;
        }
    }
}