using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Play.MerchantPortal.Domain.Entities;

namespace Play.MerchantPortal.Infrastructure.Persistence.Sql.Configurations;

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