using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.Entities;

namespace Play.Persistence.Sql;

public class BaseEntityConfiguration<_Aggregate, _TId> : IEntityTypeConfiguration<_Aggregate> where _Aggregate : Aggregate<_TId>
{
    #region Instance Members

    public virtual void Configure(EntityTypeBuilder<_Aggregate> builder)
    {
        builder.HasKey(x => x.Id);
    }

    #endregion
}