using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Play.Inventory.Domain.ValueObjects;

namespace Play.Inventory.Persistence.Sql.Configuration;

public static class DomainValueConverters
{
    #region Instance Members

    public static void ConfigureCommonConverters(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Sku>().HaveConversion<SkuConverter>();
    }

    #endregion

    public class SkuConverter : ValueConverter<Sku, string>
    {
        #region Constructor

        public SkuConverter() : base(x => x.Value, y => new(y))
        { }

        #endregion
    }
}