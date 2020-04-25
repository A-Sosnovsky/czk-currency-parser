using System;

namespace Parser.BackgroundService.Schedulers
{
    internal class JobConfiguration
    {
        public JobConfiguration(Type type, string cronExpression)
        {
            Type = type;
            CronExpression = cronExpression;
        }

        public Type Type { get; }
        public string CronExpression { get; }
    }
}