using MerchantPortal.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MerchantPortal.Infrastructure.Persistence.Configurations
{
    internal class MerchantEntityConfiguration: BaseEntityConfiguration<MerchantEntity>
    {
        public override void Configure(EntityTypeBuilder<MerchantEntity> builder)
        {
            builder.Property(x => x.AcquirerId)
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(x => x.MerchantIdentifier)
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(x => x.MerchantCategoryCode)
                .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(x => x.StreetAddress)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(x => x.City)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.ZipCode)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.State)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Country)
                .HasMaxLength(50)
                .IsRequired();

            //builder.HasOne<CompanyEntity>();

            builder.HasMany<StoreEntity>();

            base.Configure(builder);
        }
    }
}
