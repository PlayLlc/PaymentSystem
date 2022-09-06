using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Play.MerchantPortal.Domain.Entities;

namespace Play.MerchantPortal.Infrastructure.Persistence.Sql.Configurations;

internal class StoreEntityConfiguration : BaseEntityConfiguration<StoreEntity>
{
    #region Instance Members

    public override void Configure(EntityTypeBuilder<StoreEntity> builder)
    {
        builder.HasOne<MerchantEntity>();
        builder.HasMany<TerminalEntity>();

        base.Configure(builder);
    }

    #endregion
}