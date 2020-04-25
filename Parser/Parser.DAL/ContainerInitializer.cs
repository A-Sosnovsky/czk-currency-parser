using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Parser.DAL.Context;

namespace Parser.DAL
{
    public static class ContainerInitializer
    {
        public static IServiceCollection Initialize(this IServiceCollection services)
        {
            services.AddDbContext<ParserDbContext>(options => options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Parser;Trusted_Connection=True;;MultipleActiveResultSets=true;"));
            services.AddScoped<IRepository, Repository>();
            return services;
        }

        public static async Task CreateDbIfRequired(IServiceProvider serviceProvider)
        {
            try
            {
                var context = serviceProvider.GetService<ParserDbContext>();
                await context.Database.MigrateAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
