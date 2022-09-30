using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Play.Merchants.Domain.Entities;
using Play.Persistence.Sql;

namespace Play.Merchants.Persistence.Sql.Sql.Configurations;

internal class MerchantEntityConfiguration : BaseEntityConfiguration<Merchant>
{
    #region Instance Members

    public override void Configure(EntityTypeBuilder<Merchant> builder)
    {
        builder.Property(x => x.AcquirerId).HasMaxLength(11).IsRequired();

        builder.Property(x => x.MerchantIdentifier).HasMaxLength(15).IsRequired();

        builder.Property(x => x.MerchantCategoryCode).IsRequired();

        builder.Property(x => x.Name).HasMaxLength(250).IsRequired();

        builder.Property(x => x.StreetAddress).HasMaxLength(250).IsRequired();

        builder.Property(x => x.City).HasMaxLength(50).IsRequired();

        builder.Property(x => x.ZipCode).HasMaxLength(5).IsRequired();

        builder.Property(x => x.State).HasMaxLength(50).IsRequired();

        builder.Property(x => x.Country).HasMaxLength(50).IsRequired();

        //builder.HasOne<CompanyEntity>();

        builder.HasMany<Store>();

        base.Configure(builder);
    }

    #endregion
}