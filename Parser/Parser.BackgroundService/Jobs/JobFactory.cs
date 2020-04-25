using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;

namespace Parser.BackgroundService.Jobs
{
    internal class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _container;

        public JobFactory(IServiceProvider container)
        {
            _container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                if (_container.GetService(bundle.JobDetail.JobType) is IJob job)
                {
                    return job;
                }

                var exception = new Exception($"Can't cast {bundle.JobDetail.JobType} to {typeof(IJob)}");
                _container.GetRequiredService<ILogger<JobFactory>>().LogCritical(exception, "Unable to schedule job");
                throw exception;
            }
            catch (Exception exception)
            {
                _container.GetRequiredService<ILogger<JobFactory>>()
                    .LogError(exception, $"Can't get {bundle.JobDetail.JobType} through DI");
                throw;
            }
        }

        public void ReturnJob(IJob job)
        {
            (job as IDisposable)?.Dispose();
        }
    }
}