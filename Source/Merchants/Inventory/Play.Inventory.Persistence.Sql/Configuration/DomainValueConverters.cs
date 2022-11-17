using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Play.Domain.Common.ValueObjects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Inventory.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;

using Play.Globalization.Currency;
using Play.Globalization.Time;

using static Play.Persistence.Sql.ValueConverters;

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

        public SkuConverter() : base(x => x.Value, y => new Sku(y))
        { }

        #endregion
    }
}