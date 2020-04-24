using System;
using Microsoft.Extensions.DependencyInjection;

namespace Parser.Services
{
    public static class ContainerInitializer
    {
        public static IServiceCollection Initialize(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<ICurrencyProvider, CurrencyProvider>();
            services.AddHttpClient("CurrencyClient", client =>
            {
                client.BaseAddress = new Uri("https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/");
            });
            return services;
        }
    }
}
