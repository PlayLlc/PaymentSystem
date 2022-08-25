using MerchantPortal.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MerchantPortal.Infrastructure.Persistence.Configurations;

internal class StoreEntityConfiguration : BaseEntityConfiguration<StoreEntity>
{
    public override void Configure(EntityTypeBuilder<StoreEntity> builder)
    {
        base.Configure(builder);
    }
}
