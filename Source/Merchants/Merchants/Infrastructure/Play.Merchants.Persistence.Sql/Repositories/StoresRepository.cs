using Microsoft.EntityFrameworkCore;

using Play.Merchants.Domain.Entities;
using Play.Merchants.Domain.Repositories;
using Play.Merchants.Persistence.Sql.Sql;

namespace Play.Merchants.Persistence.Sql.Repositories;

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
        return await ((MerchantPortalDbContext) _DbContext).Stores.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Store>> SelectStoresByMerchant(long merchantId)
    {
        return await Task.FromResult(((MerchantPortalDbContext) _DbContext).Stores.Where(x => x.MerchantId == merchantId).AsEnumerable()).ConfigureAwait(false);
    }

    #endregion
}