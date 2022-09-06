using Play.MerchantPortal.Application.Contracts.Persistence;

using MerchantPortal.Core.Entities;
using MerchantPortal.Infrastructure.Persistence.Sql;

using Microsoft.EntityFrameworkCore;

namespace MerchantPortal.Infrastructure.Persistence.Repositories;

internal class StoresRepository : Repository<StoreEntity>, IStoresRepository
{
    #region Constructor

    public StoresRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    { }

    #endregion

    #region Instance Members

    public async Task<StoreEntity?> SelectById(long id)
    {
        return await _DbContext.Stores.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public IEnumerable<StoreEntity> SelectStoresByMerchant(long merchantId)
    {
        return _DbContext.Stores.Where(x => x.MerchantId == merchantId).AsEnumerable();
    }

    #endregion
}