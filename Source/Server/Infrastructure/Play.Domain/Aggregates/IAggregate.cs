namespace Play.Domain.Aggregates;

public interface IAggregate
{
    #region Instance Members

    public IDto AsDto();

    #endregion
}