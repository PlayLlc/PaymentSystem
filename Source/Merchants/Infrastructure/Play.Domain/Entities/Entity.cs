﻿namespace Play.Domain.Entities;

public abstract class Entity<_TId>
{
    #region Instance Members

    public abstract EntityId<_TId> GetId();
    public abstract IDto AsDto();

    #endregion
}