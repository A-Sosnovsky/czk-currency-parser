using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Parser.BackgroundService.Jobs
{
    internal abstract class BaseJob : IJob
    {
        protected readonly IServiceScopeFactory ServiceScopeFactory;

        protected BaseJob(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
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
                Console.WriteLine(e);
                throw;
            }
        }
    }
}