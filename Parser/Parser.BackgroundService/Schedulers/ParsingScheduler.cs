using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Parser.BackgroundService.Jobs;
using Parser.Infrastructure;
using Quartz;
using Quartz.Impl;

namespace Parser.BackgroundService.Schedulers
{
    internal static class ParsingScheduler
    {
        private const int DefaultStartIntervalSeconds = 60;
        private const string JobNameTemplate = "Job.{0}";

        public static async void Start(IServiceProvider serviceProvider)
        {
            var configurationProvider = serviceProvider.GetRequiredService<IConfigurationProvider>();

            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = serviceProvider.GetRequiredService<JobFactory>();
            foreach (var job in GetJobsToRun(configurationProvider))
            {
                var jobDetail = JobBuilder.Create(job.Type).Build();

                var schedule = !string.IsNullOrWhiteSpace(job.CronExpression)
                    ? (IScheduleBuilder) CronScheduleBuilder.CronSchedule(job.CronExpression)
                    : SimpleScheduleBuilder.Create().WithIntervalInSeconds(DefaultStartIntervalSeconds).RepeatForever();

                var trigger = TriggerBuilder.Create()
                    .WithIdentity(job.Type.FullName)
                    .WithSchedule(schedule)
                    .StartNow()
                    .Build();

                await scheduler.ScheduleJob(jobDetail, trigger);
            }

            await scheduler.Start();
        }

        private static IEnumerable<JobConfiguration> GetJobsToRun(IConfigurationProvider configurationProvider)
        {
            var type = typeof(IJob);
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsAbstract)
                .Select(p => new JobConfiguration(p, GetJobScheduleExpression(p, configurationProvider)));
        }

        private static string GetJobScheduleExpression(MemberInfo type, IConfigurationProvider configurationProvider)
        {
            var key = string.Format(JobNameTemplate, type.Name);
            var value = configurationProvider.GetConfigurationValue(key);
            if (string.IsNullOrWhiteSpace(value) || !CronExpression.IsValidExpression(value))
            {
                return null;
            }

            return value;
        }
    }
}