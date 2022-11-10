namespace Play.Domain.Entities;

public abstract class Entity<_TId> : IEntity where _TId : IEquatable<_TId>
{
    #region Instance Members

    public abstract _TId GetId();

    public abstract IDto AsDto();

    #endregion
}