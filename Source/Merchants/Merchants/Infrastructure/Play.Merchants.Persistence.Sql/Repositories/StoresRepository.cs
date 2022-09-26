using Microsoft.EntityFrameworkCore;
using Play.MerchantPortal.Domain.Entities;
using Play.MerchantPortal.Domain.Persistence;
using Play.MerchantPortal.Infrastructure.Persistence.Sql;

namespace Play.MerchantPortal.Infrastructure.Persistence.Repositories;

internal class StoresRepository : Repository<Store>, IStoresRepository
{
    #region Constructor

    public StoresRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="OperationCanceledException"></exception>
    public async Task<Store?> SelectById(long id)
    {
        return await _DbContext.Stores.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Store>> SelectStoresByMerchant(long merchantId)
    {
        return await Task.FromResult(_DbContext.Stores.Where(x => x.MerchantId == merchantId).AsEnumerable()).ConfigureAwait(false);
    }

    #endregion
}