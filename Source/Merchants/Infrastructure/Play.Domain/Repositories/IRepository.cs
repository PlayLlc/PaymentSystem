using Play.Domain.Aggregates;

namespace Play.Domain.Repositories;

public interface IRepository<_Aggregate, in _TId> where _Aggregate : Aggregate<_TId> where _TId : IEquatable<_TId>
{
    #region Instance Members

    public Task<_Aggregate?> GetByIdAsync(_TId id);
    public _Aggregate? GetById(_TId id);
    public Task SaveAsync(_Aggregate aggregate);
    public Task RemoveAsync(_Aggregate id);
    public void Save(_Aggregate aggregate);
    public void Remove(_Aggregate entity);

    #endregion
}