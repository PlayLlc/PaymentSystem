using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Play.TimeClock.Domain.ValueObject;

namespace Play.TimeClock.Persistence.Sql.Configuration;

public static class DomainValueConverters
{
    #region Instance Members

    public static void ConfigureCommonConverters(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<TimeClockStatus>().HaveConversion<TimeClockStatusConverter>();
    }

    #endregion

    public class TimeClockStatusConverter : ValueConverter<TimeClockStatus, string>
    {
        #region Constructor

        public TimeClockStatusConverter() : base(x => x.Value, y => new TimeClockStatus(y))
        { }

        #endregion
    }
}