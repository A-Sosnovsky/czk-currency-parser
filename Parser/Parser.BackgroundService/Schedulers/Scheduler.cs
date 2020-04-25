using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Parser.BackgroundService.Jobs;
using Quartz;
using Quartz.Impl;

namespace Parser.BackgroundService.Schedulers
{
    internal static class Scheduler
    {
        private const int DefaultStartIntervalSeconds = 60;
        private const string JobNameTemplate = "Job.{0}";
        private const string ConfigurationKeyPrefix = "Schedule";

        public static async void Start(IServiceProvider serviceProvider)
        {
            var configurationProvider = serviceProvider.GetRequiredService<IConfiguration>();
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

        private static IEnumerable<JobConfiguration> GetJobsToRun(IConfiguration configuration)
        {
            var type = typeof(IJob);
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsAbstract)
                .Select(p => new JobConfiguration(p, GetJobScheduleExpression(p, configuration)));
        }

        private static string GetJobScheduleExpression(MemberInfo type, IConfiguration configuration)
        {
            var key = string.Format(JobNameTemplate, type.Name);
            var value = configuration.GetSection(ConfigurationKeyPrefix)?[key];

            if (string.IsNullOrWhiteSpace(value) || !CronExpression.IsValidExpression(value))
            {
                return null;
            }

            return value;
        }
    }
}