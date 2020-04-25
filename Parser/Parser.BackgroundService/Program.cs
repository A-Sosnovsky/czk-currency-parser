using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Parser.BackgroundService.Schedulers;

namespace Parser.BackgroundService
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
            {
                var e = eventArgs.ExceptionObject as Exception;
                Console.WriteLine("Application has been stopped. UnhandledException: " + e);
            };
            
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            Scheduler.Start(serviceProvider);
            await DAL.ContainerInitializer.CreateDbIfRequired(serviceProvider);
            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureServices(collection =>
                {
                    DAL.ContainerInitializer.Initialize(collection);
                    Services.ContainerInitializer.Initialize(collection);
                    Infrastructure.ContainerInitializer.Initialize(collection);
                    ContainerInitializer.Initialize(collection);
                })
                .ConfigureLogging(builder => builder.AddConsole());
    }
}