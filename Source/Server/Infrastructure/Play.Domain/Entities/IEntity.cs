namespace Play.Domain.Entities;

public interface IEntity
{
    #region Instance Members

    public IDto AsDto();

    #endregion
}