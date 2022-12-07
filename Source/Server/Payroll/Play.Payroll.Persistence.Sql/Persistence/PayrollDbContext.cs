using Microsoft.EntityFrameworkCore;

using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Globalization.Time;
using Play.Payroll.Contracts.Enums;
using Play.Payroll.Domain.Aggregates;
using Play.Payroll.Domain.Entities;
using Play.Payroll.Domain.ValueObject;
using Play.Payroll.Persistence.Sql.Configuration;
using Play.Persistence.Sql;

namespace Play.Payroll.Persistence.Sql.Persistence;

public sealed class PayrollDbContext : DbContext
{
    #region Static Metadata

    public const string DatabaseName = "Payroll";

    #endregion

    #region Constructor

    public PayrollDbContext(DbContextOptions<PayrollDbContext> options) : base(options)
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

        PayrollEntityConfiguration payrollEntityConfiguration = new();

        #region Enums

        builder.Entity<CompensationType>().ToTable($"{nameof(CompensationTypes)}").HasKey(a => a.Value);
        builder.Entity<CompensationType>().HasData(CompensationTypes.Empty.GetAll().Select(e => new CompensationType(e)));

        builder.Entity<PaydayRecurrence>().ToTable($"{nameof(PaydayRecurrences)}").HasKey(a => a.Value);
        builder.Entity<PaydayRecurrence>().HasData(PaydayRecurrences.Empty.GetAll().Select(e => new PaydayRecurrence(e)));

        builder.Entity<TimeEntryType>().ToTable($"{nameof(TimeEntryTypes)}").HasKey(a => a.Value);
        builder.Entity<TimeEntryType>().HasData(TimeEntryTypes.Empty.GetAll().Select(e => new TimeEntryType(e)));

        #region Entities

        // Complex Type
        builder.RegisterMoneyValueObjectType();

        builder.Entity<PaydaySchedule>().ToTable($"{nameof(PaydaySchedule)}s").HasKey(a => a.Id);
        builder.Entity<PaydaySchedule>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<PaydaySchedule>().PrivateProperty<PaydaySchedule, PaydayRecurrence>("_PaydayRecurrence");
        builder.Entity<PaydaySchedule>().PrivateProperty<PaydaySchedule, DayOfTheWeek?>("_WeeklyPayday");
        builder.Entity<PaydaySchedule>().PrivateProperty<PaydaySchedule, DayOfTheMonth?>("_MonthlyPayday");
        builder.Entity<PaydaySchedule>().PrivateProperty<PaydaySchedule, DayOfTheMonth?>("_SecondMonthlyPayday");

        builder.Entity<Compensation>().ToTable($"{nameof(Compensation)}s").HasKey(a => a.Id);
        builder.Entity<Compensation>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Compensation>().PrivateProperty<Compensation, CompensationType>("_CompensationType");
        builder.Entity<Compensation>().MoneyValueObjectProperty("_CompensationRate");

        builder.Entity<CheckingAccount>().ToTable($"{nameof(CheckingAccount)}s").HasKey(a => a.Id);
        builder.Entity<CheckingAccount>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<CheckingAccount>().PrivateProperty<CheckingAccount, RoutingNumber>("_RoutingNumber");
        builder.Entity<CheckingAccount>().PrivateProperty<CheckingAccount, AccountNumber>("_AccountNumber");

        builder.Entity<DirectDeposit>().ToTable($"{nameof(DirectDeposit)}s").HasKey(a => a.Id);
        builder.Entity<DirectDeposit>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<DirectDeposit>().HasOne<DirectDeposit, CheckingAccount, SimpleStringId>("_CheckingAccount", "CheckingAccountId");
        builder.Entity<DirectDeposit>().PrivateProperty<DirectDeposit, SimpleStringId>("_EmployeeId");

        builder.Entity<Employee>().ToTable($"{nameof(Employee)}s").HasKey(x => x.Id);
        builder.Entity<Employee>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Employee>().PrivateProperty<Employee, SimpleStringId>("_UserId");
        builder.Entity<Employee>().HasOne<Employee, Compensation, SimpleStringId>("_Compensation", "Compensation");
        builder.Entity<Employee>().HasOne<Employee, DirectDeposit, SimpleStringId>("_DirectDeposit", "DirectDeposit");
        builder.Entity<Employee>().HasMany<Employee, TimeEntry, SimpleStringId>("_TimeEntries", "TimeEntryId");
        builder.Entity<Employee>().HasMany<Employee, Paycheck, SimpleStringId>("_Paychecks", "PaycheckId");

        builder.Entity<Paycheck>().ToTable($"{nameof(Paycheck)}s").HasKey(x => x.Id);
        builder.Entity<Paycheck>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Paycheck>().PrivateProperty<Paycheck, SimpleStringId>("_EmployeeId");
        builder.Entity<Paycheck>().MoneyValueObjectProperty("_Amount");
        builder.Entity<Paycheck>().PrivateProperty<Paycheck, DateTimeUtc>("_DateIssued");
        builder.Entity<Paycheck>().HasOne<Paycheck, TimeSheet, SimpleStringId>("_TimeSheet", "TimeSheetId");
        builder.Entity<Paycheck>().HasOne<Paycheck, PayPeriod, SimpleStringId>("_PayPeriod", "PayPeriod");
        builder.Entity<Paycheck>().PrivateProperty<Paycheck, bool>("_HasBeenDistributed");

        builder.Entity<PayPeriod>().ToTable($"{nameof(PayPeriod)}s").HasKey(x => x.Id);
        builder.Entity<PayPeriod>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Paycheck>().PrivateProperty<Paycheck, DateTimeUtc>("_Start");
        builder.Entity<Paycheck>().PrivateProperty<Paycheck, DateTimeUtc>("_End");

        builder.Entity<TimeEntry>().ToTable($"TimeEntries").HasKey(x => x.Id);
        builder.Entity<TimeEntry>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<TimeEntry>().PrivateProperty<TimeEntry, SimpleStringId>("_EmployeeId");
        builder.Entity<TimeEntry>().PrivateProperty<TimeEntry, DateTimeUtc>("_Start");
        builder.Entity<TimeEntry>().PrivateProperty<TimeEntry, DateTimeUtc>("_End");
        builder.Entity<TimeEntry>().PrivateProperty<TimeEntry, TimeEntryType>("_TimeEntryType");

        builder.Entity<TimeSheet>().ToTable($"{nameof(TimeSheet)}s").HasKey(x => x.Id);
        builder.Entity<TimeSheet>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<TimeSheet>().PrivateProperty<TimeSheet, SimpleStringId>("_EmployeeId");
        builder.Entity<TimeSheet>().HasMany<TimeSheet, TimeEntry, SimpleStringId>("_TimeEntries", "TimeEntryId");
        builder.Entity<TimeSheet>().HasOne<TimeSheet, PayPeriod, SimpleStringId>("_PayPeriod", "PayPeriodId");

        payrollEntityConfiguration.Configure(builder.Entity<Employer>());

        #endregion
    }

    #endregion

    #endregion
}