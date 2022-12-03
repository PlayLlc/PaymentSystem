using System.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Quartz;

namespace Play.Underwriting.Application;

public static class JobConfigurationExtensions
{
    #region Instance Members

    public static void RegisterSchedulingConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(quartz =>
        {
            quartz.UseDefaultThreadPool();
            quartz.UseMicrosoftDependencyInjectionJobFactory();
            quartz.RegisterJobs(configuration);
        });

        services.AddQuartzHostedService(options =>
        {
            //when shutting down we want jobs to complete gracefully.
            options.WaitForJobsToComplete = true;
        });
    }

    private static void RegisterJobs(this IServiceCollectionQuartzConfigurator quartzConfig, IConfiguration config)
    {
        Type[] jobTypes = GetJobTypes();

        foreach (Type t in jobTypes)
        {
            string jobName = t.Name;

            string configKey = $"Quartz:Jobs:{jobName}";
            string? cronSchedule = config[configKey];

            if (string.IsNullOrEmpty(cronSchedule))
                throw new ConfigurationErrorsException("No schedule configuration found, cannot configure and schedule current job");

            JobKey jobKey = new JobKey(jobName);

            quartzConfig.AddJob(t, jobKey, options =>
            {
                options.WithIdentity(jobKey);
            });

            quartzConfig.AddTrigger(options => options.ForJob(jobKey).WithIdentity(jobName + "-trigger").StartNow().WithCronSchedule(cronSchedule));
        }
    }

    private static Type[] GetJobTypes()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(t => typeof(IScheduledCronJob).IsAssignableFrom(t) & t.IsClass)
            .ToArray();
    }

    #endregion
}