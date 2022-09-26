using Play.Domain.Entitities;
using Play.Merchants.Domain.Repositories;

namespace Play.Merchants.Persistence.Sql.Sql
{
    internal class Repository<_> : IRepository<_> where _ : BaseEntity
    {
        #region Instance Values

        protected readonly MerchantPortalDbContext _DbContext;

        public IQueryable<_> Query => _DbContext.Set<_>();

        #endregion

        #region Constructor

        internal Repository(MerchantPortalDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        #endregion

        #region Instance Members

        public _ AddEntity(_ entity)
        {
            return _DbContext.Set<_>().Add(entity).Entity;
        }

        public void DeleteEntity(_ entity)
        {
            _DbContext.Set<_>().Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            var result = await _DbContext.SaveChangesAsync();
        }

        #endregion
    }
}