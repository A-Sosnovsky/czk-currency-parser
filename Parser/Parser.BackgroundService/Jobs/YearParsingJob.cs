using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Parser.Services;
using Quartz;

namespace Parser.BackgroundService.Jobs
{
    internal class YearParsingJob : BaseJob
    {
        public YearParsingJob(IServiceScopeFactory serviceScopeFactory, ILogger<BaseJob> logger) : base(
            serviceScopeFactory, logger)
        {
        }

        protected override async Task ExecuteInternal(IJobExecutionContext context)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var parser = scope.ServiceProvider.GetService<ICurrencySaveService>();
            await parser.SaveByYear(2017);
            await parser.SaveByYear(2018);
        }
    }
}