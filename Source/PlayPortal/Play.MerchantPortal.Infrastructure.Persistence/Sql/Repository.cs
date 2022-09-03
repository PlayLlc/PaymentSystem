using MerchantPortal.Core.Entities;
using Play.MerchantPortal.Application.Contracts.Persistence;

namespace MerchantPortal.Infrastructure.Persistence.Sql
{
    internal class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly MerchantPortalDbContext _dbContext;

        internal Repository(MerchantPortalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Query => _dbContext.Set<T>();

        public T AddEntity(T entity)
        {
            return _dbContext.Set<T>().Add(entity).Entity;
        }

        public void DeleteEntity(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            var result = await _dbContext.SaveChangesAsync();
        }
    }
}
