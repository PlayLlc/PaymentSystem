using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Play.Infrastructure.Domain.Entities;

namespace Play.MerchantPortal.Infrastructure.Persistence.Sql.Configurations;

internal class BaseEntityConfiguration<_Entity> : IEntityTypeConfiguration<_Entity> where _Entity : BaseEntity
{
    #region Instance Members

    public virtual void Configure(EntityTypeBuilder<_Entity> builder)
    {
        builder.HasKey(x => x.Id);
    }

    #endregion
}