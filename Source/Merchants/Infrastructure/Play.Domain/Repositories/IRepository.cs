using Play.Domain.Aggregates;
using Play.Domain.Entities;

namespace Play.Domain.Repositories;

public interface IRepository<_Aggregate, _TId> where _Aggregate : Aggregate<_TId> where _TId : IEquatable<_TId>
{
    #region Instance Members

    Task<_Aggregate?> GetByIdAsync(_TId id);
    Task SaveAsync(_Aggregate aggregate);
    Task RemoveAsync(_Aggregate id);
    _Aggregate? GetById(_TId id);
    void Save(_Aggregate aggregate);
    void Remove(_Aggregate entity);

    #endregion
}