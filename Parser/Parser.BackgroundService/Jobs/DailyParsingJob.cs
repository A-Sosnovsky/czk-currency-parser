using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Parser.Services;
using Quartz;

namespace Parser.BackgroundService.Jobs
{
    internal class DailyParsingJob : BaseJob
    {
        public DailyParsingJob(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        protected override async Task ExecuteInternal(IJobExecutionContext context)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var parser = scope.ServiceProvider.GetService<ICurrencyService>();
            await parser.ParseByDate(DateTime.Now);
        }
    }
}