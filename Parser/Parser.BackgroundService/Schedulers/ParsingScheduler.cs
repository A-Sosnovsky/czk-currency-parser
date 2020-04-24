using System;
using Microsoft.Extensions.DependencyInjection;
using Parser.BackgroundService.Jobs;
using Quartz;
using Quartz.Impl;

namespace Parser.BackgroundService.Schedulers
{
    internal static class ParsingScheduler
    {
        public static async void Start(IServiceProvider serviceProvider)
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = serviceProvider.GetService<JobFactory>();
            await scheduler.Start();

            var jobDetail = JobBuilder.Create<DailyParsingJob>().Build();
            
            var trigger = TriggerBuilder.Create()
                .WithIdentity("DayParsingTrigger")
                .StartNow()
                .WithSimpleSchedule(builder => builder.WithIntervalInMinutes(1).RepeatForever())
                .Build();

            var trigger2 = TriggerBuilder.Create()
                .WithIdentity("YearParsingTrigger")
                .StartNow()
                .WithSimpleSchedule(builder => builder.WithRepeatCount(1))
                .Build();

            await scheduler.ScheduleJob(jobDetail, trigger);
            // await scheduler.ScheduleJob(jobDetail, trigger2);
        }
    }
}