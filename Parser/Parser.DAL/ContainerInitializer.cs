using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Parser.DAL.Context;

namespace Parser.DAL
{
    public static class ContainerInitializer
    {
        public static IServiceCollection Initialize(this IServiceCollection services)
        {
            services.AddDbContext<ParserDbContext>();
            services.AddScoped<IRepository, Repository>();
            return services;
        }

        public static async Task InitDb(IServiceProvider serviceProvider, string connectionString)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<ParserDbContext>>();
            try
            {
                ParserDbContext.SetConnectionString(connectionString);
                var context = serviceProvider.GetRequiredService<ParserDbContext>();
                await context.Database.MigrateAsync();
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Unable to migrate databese");
                throw;
            }
        }
    }
}