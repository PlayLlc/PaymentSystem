﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Play.Domain.Entities;

namespace Play.Persistence.Sql;

public static class EntityTypeBuilderExtensions
{
    #region Instance Members

    /// <summary>
    ///     Configures a relationship where the
    ///     <typeparam name="_TRelatedEntity" />
    ///     related entity type does not reference the entity and renames the shadow foreign key that gets created by default
    /// </summary>
    public static void HasOne<_TEntity, _TRelatedEntity, _ForeignKey>(this EntityTypeBuilder<_TEntity> builder, string navigationName, string fkColumnName)
        where _TEntity : class, IEntity where _TRelatedEntity : class, IEntity
    {
        builder.HasOne<_TRelatedEntity>(navigationName);
        builder.Property<_ForeignKey>($"{navigationName}Id").HasColumnName(fkColumnName);
    }

    /// <summary>
    ///     Configures a relationship where the
    ///     <typeparam name="_TRelatedEntity" />
    ///     related entity type does not reference the entity and renames the shadow
    ///     foreign key that gets created by default
    /// </summary>
    public static void HasMany<_TEntity, _TRelatedEntity, _ForeignKey>(this EntityTypeBuilder<_TEntity> builder, string navigationName, string fkColumnName)
        where _TEntity : class, IEntity where _TRelatedEntity : class, IEntity
    {
        builder.HasMany<_TRelatedEntity>(navigationName);
        builder.Property<_ForeignKey>($"{navigationName}Id").HasColumnName(fkColumnName);
    }

    #endregion
}