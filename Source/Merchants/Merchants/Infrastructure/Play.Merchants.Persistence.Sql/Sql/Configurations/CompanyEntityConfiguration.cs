using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Play.MerchantPortal.Domain.Entities;

namespace Play.MerchantPortal.Infrastructure.Persistence.Sql.Configurations;

internal class CompanyEntityConfiguration : BaseEntityConfiguration<Company>
{
    #region Instance Members

    public override void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).IsRequired();

        base.Configure(builder);
    }

    #endregion
}