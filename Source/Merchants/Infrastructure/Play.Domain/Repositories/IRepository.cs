using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Aggregates;
using Play.Domain.Entities;

namespace Play.Domain.Repositories
{
    public interface IRepository<_Aggregate, _TId> where _Aggregate : IAggregate<_TId>
    {
        #region Instance Members

        Task<IEnumerable<_Aggregate>> GetAllAsync(Expression<Func<_Aggregate, bool>> predicate);
        Task<_Aggregate> GetByIdAsync(EntityId<_TId> id);
        Task SaveAsync(_Aggregate aggregate);
        Task RemoveAsync(EntityId<_TId> id);

        _Aggregate GetById(EntityId<_TId> id);
        void Save(_Aggregate entity);
        void Remove(EntityId<_TId> entity);

        #endregion
    }
}