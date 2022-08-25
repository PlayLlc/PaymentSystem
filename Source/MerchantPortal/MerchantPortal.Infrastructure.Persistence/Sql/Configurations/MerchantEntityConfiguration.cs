using MerchantPortal.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MerchantPortal.Infrastructure.Persistence.Configurations
{
    internal class MerchantEntityConfiguration: BaseEntityConfiguration<MerchantEntity>
    {
        public override void Configure(EntityTypeBuilder<MerchantEntity> builder)
        {
            //builder.Property(x => x.AcquirerId).IsRequired();
            //builder.Property(x => x.MerchantIdentifier).IsRequired();
            //builder.Property(x => x.MerchantCategoryCode).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.StreetAddress).IsRequired();
            builder.Property(x => x.City).IsRequired();
            builder.Property(x => x.ZipCode).IsRequired();
            builder.Property(x => x.State).IsRequired();
            builder.Property(x => x.Country).IsRequired();

            builder.HasOne<CompanyEntity>();
            builder.HasMany<StoreEntity>();

            base.Configure(builder);
        }
    }
}
