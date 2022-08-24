using MerchantPortal.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MerchantPortal.Infrastructure.Persistence.Configurations;

internal class CompanyEntityConfiguration : BaseEntityConfiguration<CompanyEntity>
{
    public override void Configure(EntityTypeBuilder<CompanyEntity> builder)
    {
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).IsRequired();

        base.Configure(builder);
    }
}
