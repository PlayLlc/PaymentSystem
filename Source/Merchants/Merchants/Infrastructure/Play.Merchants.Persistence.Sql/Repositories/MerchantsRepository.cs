using Microsoft.EntityFrameworkCore;

using Play.Merchants.Domain.Entities;
using Play.Merchants.Domain.Repositories;
using Play.Merchants.Persistence.Sql.Sql;
using Play.Persistence.Sql;

namespace Play.Merchants.Persistence.Sql.Repositories;

internal class MerchantsRepository : Repository<Merchant>, IMerchantsRepository
{
    #region Constructor

    public MerchantsRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="OperationCanceledException"></exception>
    public async Task<Merchant?> SelectById(long id)
    {
        return await ((MerchantPortalDbContext) _DbContext).Merchants.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
    }

    #endregion
}