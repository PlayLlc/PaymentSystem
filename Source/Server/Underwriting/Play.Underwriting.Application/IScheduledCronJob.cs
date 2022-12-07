using Quartz;

namespace Play.Underwriting.Application;

[DisallowConcurrentExecution]
public interface IScheduledCronJob : IJob
{
    #region Instance Members

    string GetCronSchedule();

    string GetJobName();

    #endregion
}