using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Parser.Services.Report;

[assembly: InternalsVisibleTo("Parser.Services.Tests")]
namespace Parser.Services
{
    public static class ContainerInitializer
    {
        public static IServiceCollection Initialize(this IServiceCollection services)
        {
            services.AddScoped<ICurrencySaveService, CurrencySaveService>();
            services.AddScoped<ICurrencyProvider, CurrencyProvider>();
            services.AddScoped<ICurrencyReportService, CurrencyReportService>();
            services.AddHttpClient("CurrencyClient", client =>
            {
                client.BaseAddress = new Uri("https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/");
            });
            return services;
        }
    }
}
