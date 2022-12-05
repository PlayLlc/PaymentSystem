using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Play.Domain.Common.ValueObjects;
using Play.Payroll.Domain.ValueObject;

namespace Play.Payroll.Persistence.Sql.Configuration;

public static class DomainValueConverters
{
    #region Instance Members

    public static void ConfigureCommonConverters(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<CompensationType>().HaveConversion<CompensationTypeConverter>();
        configurationBuilder.Properties<DayOfTheMonth>().HaveConversion<DayOfTheMonthConverter>();
        configurationBuilder.Properties<DayOfTheWeek>().HaveConversion<DayOfTheWeekConverter>();
        configurationBuilder.Properties<PaydayRecurrence>().HaveConversion<PaydayRecurrenceConverter>();
        configurationBuilder.Properties<TimeEntryType>().HaveConversion<TimeEntryTypeConverter>();
        configurationBuilder.Properties<RoutingNumber>().HaveConversion<RoutingNumberConverter>();
        configurationBuilder.Properties<AccountNumber>().HaveConversion<AccountNumberConverter>();
    }

    #endregion

    public class CompensationTypeConverter : ValueConverter<CompensationType, string>
    {
        #region Constructor

        public CompensationTypeConverter() : base(x => x.Value, y => new CompensationType(y))
        { }

        #endregion
    }

    public class DayOfTheWeekConverter : ValueConverter<DayOfTheWeek, byte>
    {
        #region Constructor

        public DayOfTheWeekConverter() : base(x => x.Value, y => new DayOfTheWeek(y))
        { }

        #endregion
    }

    public class DayOfTheMonthConverter : ValueConverter<DayOfTheMonth, byte>
    {
        #region Constructor

        public DayOfTheMonthConverter() : base(x => x.Value, y => new DayOfTheMonth(y))
        { }

        #endregion
    }

    public class PaydayRecurrenceConverter : ValueConverter<PaydayRecurrence, string>
    {
        #region Constructor

        public PaydayRecurrenceConverter() : base(x => x.Value, y => new PaydayRecurrence(y))
        { }

        #endregion
    }

    public class TimeEntryTypeConverter : ValueConverter<TimeEntryType, string>
    {
        #region Constructor

        public TimeEntryTypeConverter() : base(x => x.Value, y => new TimeEntryType(y))
        { }

        #endregion
    }

    public class RoutingNumberConverter : ValueConverter<RoutingNumber, string>
    {
        #region Constructor

        public RoutingNumberConverter() : base(x => x.Value, y => new RoutingNumber(y))
        { }

        #endregion
    }

    public class AccountNumberConverter : ValueConverter<AccountNumber, string>
    {
        #region Constructor

        public AccountNumberConverter() : base(x => x.Value, y => new AccountNumber(y))
        { }

        #endregion
    }
}