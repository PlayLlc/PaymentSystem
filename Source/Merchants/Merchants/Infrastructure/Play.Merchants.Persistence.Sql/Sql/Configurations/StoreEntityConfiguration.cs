using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Play.MerchantPortal.Domain.Entities;

namespace Play.MerchantPortal.Infrastructure.Persistence.Sql.Configurations;

internal class StoreEntityConfiguration : BaseEntityConfiguration<Store>
{
    #region Instance Members

    public override void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.HasOne<Merchant>();
        builder.HasMany<Terminal>();

        base.Configure(builder);
    }

    #endregion
}