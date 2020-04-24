using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Parser.Services;
using Quartz;

namespace Parser.BackgroundService.Jobs
{
    internal class YearParsingJob : BaseJob
    {
        public YearParsingJob(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        protected override async Task ExecuteInternal(IJobExecutionContext context)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var parser = scope.ServiceProvider.GetService<ICurrencyService>();
            
            await parser.ParseByYear(2017);
            await parser.ParseByYear(2018);
        }
    }
}