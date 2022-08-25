using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Core.Entities;

namespace MerchantPortal.Infrastructure.Persistence.Sql
{
    internal class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly MerchantPortalDbContext _DbContext;

        internal Repository(MerchantPortalDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public IQueryable<T> Query => _DbContext.Set<T>();

        public T AddEntity(T entity)
        {
            return _DbContext.Set<T>().Add(entity).Entity;
        }

        public void DeleteEntity(T entity)
        {
            _DbContext.Set<T>().Remove(entity);
        }

        public async Task SaveChanges()
        {
            var result = await _DbContext.SaveChangesAsync();
        }
    }
}
