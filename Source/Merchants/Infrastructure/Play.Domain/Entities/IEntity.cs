namespace Play.Domain.Entities;

public interface IEntity<_TId>
{
    #region Instance Values

    public EntityId<_TId> Id { get; }

    #endregion
}