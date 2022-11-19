using Quartz;

namespace Play.Scheduling;

[DisallowConcurrentExecution]
public interface IScheduledCronJob : IJob
{
    string GetCronSchedule();

    string GetJobName();
}