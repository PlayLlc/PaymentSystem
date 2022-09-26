using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Play.Domain;

namespace Play.Merchants.Persistence.Sql.Sql.Configurations;

public class BaseEntityConfiguration<_Entity> : IEntityTypeConfiguration<_Entity> where _Entity : BaseEntity
{
    #region Instance Members

    public virtual void Configure(EntityTypeBuilder<_Entity> builder)
    {
        builder.HasKey(x => x.Id);
    }

    #endregion
}