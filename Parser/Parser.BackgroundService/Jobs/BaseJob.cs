using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Parser.BackgroundService.Jobs
{
    internal abstract class BaseJob : IJob
    {
        protected readonly IServiceScopeFactory ServiceScopeFactory;
        private readonly ILogger<BaseJob> _logger;
        protected BaseJob(IServiceScopeFactory serviceScopeFactory, ILogger<BaseJob> logger)
        {
            ServiceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected abstract Task ExecuteInternal(IJobExecutionContext context);

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await ExecuteInternal(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Job execution error. Job name: {this.GetType().Name}");
                throw;
            }
        }
    }
}