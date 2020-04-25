using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Parser.Services;
using Quartz;

namespace Parser.BackgroundService.Jobs
{
    internal class DailyParsingJob : BaseJob
    {
        public DailyParsingJob(IServiceScopeFactory serviceScopeFactory, ILogger<BaseJob> logger)
            : base(serviceScopeFactory, logger)
        {
        }

        protected override async Task ExecuteInternal(IJobExecutionContext context)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var parser = scope.ServiceProvider.GetService<ICurrencySaveService>();
            await parser.SaveByDate(DateTime.Now); //по-хорошему бы узнать что на тачке правильное время стоит, но делать я этого конечно же не буду
        }
    }
}