namespace Play.Domain.Entities;

public interface IEntity
{
    #region Instance Members

    public abstract IDto AsDto();

    #endregion
}