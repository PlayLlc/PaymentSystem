using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Play.Loyalty.Domain.ValueObjects;

namespace Play.Loyalty.Persistence.Sql.Configuration;

public static class DomainValueConverters
{
    #region Instance Members

    public static void ConfigureCommonConverters(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<RewardsNumber>().HaveConversion<RewardsNumberConverter>();
    }

    #endregion

    public class RewardsNumberConverter : ValueConverter<RewardsNumber, string>
    {
        #region Constructor

        public RewardsNumberConverter() : base(x => x.Value, y => new(y))
        { }

        #endregion
    }
}