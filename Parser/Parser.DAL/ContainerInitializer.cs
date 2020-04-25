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
            services.AddDbContext<ParserDbContext>(options =>
                options.UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=Parser;Trusted_Connection=True;MultipleActiveResultSets=true;"));
            services.AddScoped<IRepository, Repository>();
            return services;
        }

        public static async Task CreateDbIfRequired(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<ParserDbContext>>();
            try
            {
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