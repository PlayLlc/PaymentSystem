using Microsoft.EntityFrameworkCore;

using Play.Domain.Common.ValueObjects;
using Play.Globalization.Time;
using Play.Persistence.Sql;
using Play.TimeClock.Domain.Aggregates;
using Play.TimeClock.Domain.Entities;
using Play.TimeClock.Domain.Enums;
using Play.TimeClock.Domain.ValueObject;
using Play.TimeClock.Persistence.Sql.Configuration;

namespace Play.TimeClock.Persistence.Sql.Persistence;

public sealed class TimeClockDbContext : DbContext
{
    #region Static Metadata

    public const string DatabaseName = "TimeClock";

    #endregion

    #region Constructor

    public TimeClockDbContext(DbContextOptions<TimeClockDbContext> options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    #endregion

    #region Instance Members

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        ValueConverters.ConfigureCommonConverters(configurationBuilder);
        DomainValueConverters.ConfigureCommonConverters(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Enums

        TimeClockEntityConfiguration timeClockEntityConfiguration = new();

        #region Enums

        builder.Entity<TimeClockStatus>().ToTable($"{nameof(TimeClockStatus)}").HasKey(a => a.Value);
        builder.Entity<TimeClockStatus>().HasData(TimeClockStatuses.Empty.GetAll().Select(e => new TimeClockStatus(e)));

        #endregion

        #region Entities

        // Complex Type
        builder.RegisterMoneyValueObjectType();

        builder.Entity<TimePuncher>().ToTable($"{nameof(TimeClock)}s").HasKey(a => a.Id);
        builder.Entity<TimePuncher>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<TimePuncher>().PrivateProperty<TimePuncher, SimpleStringId>("_EmployeeId").ValueGeneratedOnAdd();
        builder.Entity<TimePuncher>().PrivateProperty<TimePuncher, TimeClockStatus?>("_TimeClockStatus");
        builder.Entity<TimePuncher>().PrivateProperty<TimePuncher, DateTimeUtc?>("_ClockedInAt");

        builder.Entity<TimeEntry>().ToTable("TimeEntries").HasKey(x => x.Id);
        builder.Entity<TimeEntry>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<TimeEntry>().PrivateProperty<TimeEntry, SimpleStringId>("_EmployeeId");
        builder.Entity<TimeEntry>().PrivateProperty<TimeEntry, DateTimeUtc>("_StartTime");
        builder.Entity<TimeEntry>().PrivateProperty<TimeEntry, DateTimeUtc>("_EndTime");

        timeClockEntityConfiguration.Configure(builder.Entity<Employee>());

        #endregion
    }

    #endregion
}