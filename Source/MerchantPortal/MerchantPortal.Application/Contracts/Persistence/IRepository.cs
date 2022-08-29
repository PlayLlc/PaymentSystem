using MerchantPortal.Core.Entities;

namespace MerchantPortal.Application.Contracts.Persistence;

public interface IRepository
{
    Task SaveChangesAsync();
}

public interface IRepository<T> : IRepository
    where T : BaseEntity
{
    IQueryable<T> Query { get; }
    T AddEntity(T entity);
    void DeleteEntity(T entity);

}
