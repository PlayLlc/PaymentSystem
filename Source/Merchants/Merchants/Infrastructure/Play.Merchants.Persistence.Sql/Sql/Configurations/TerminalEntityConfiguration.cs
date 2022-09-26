using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Play.Merchants.Domain.Entities;
using Play.Persistence.Sql;

namespace Play.Merchants.Persistence.Sql.Sql.Configurations;

internal class TerminalEntityConfiguration : BaseEntityConfiguration<Terminal>
{
    #region Instance Members

    public override void Configure(EntityTypeBuilder<Terminal> builder)
    {
        builder.HasOne<Store>();

        base.Configure(builder);
    }

    #endregion
}