using MerchantPortal.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MerchantPortal.Infrastructure.Persistence.Configurations;

internal class TerminalEntityConfiguration : BaseEntityConfiguration<TerminalEntity>
{
    public override void Configure(EntityTypeBuilder<TerminalEntity> builder)
    {
        builder.HasOne<MerchantEntity>();
        builder.HasOne<StoreEntity>();

        base.Configure(builder);
    }
}
