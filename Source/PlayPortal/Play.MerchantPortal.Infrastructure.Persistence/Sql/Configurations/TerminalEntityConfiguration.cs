using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Play.MerchantPortal.Domain.Entities;

namespace Play.MerchantPortal.Infrastructure.Persistence.Sql.Configurations;

internal class TerminalEntityConfiguration : BaseEntityConfiguration<TerminalEntity>
{
    #region Instance Members

    public override void Configure(EntityTypeBuilder<TerminalEntity> builder)
    {
        builder.HasOne<StoreEntity>();

        base.Configure(builder);
    }

    #endregion
}