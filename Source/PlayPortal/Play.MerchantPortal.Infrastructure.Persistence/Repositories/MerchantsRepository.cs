using Microsoft.EntityFrameworkCore;

using Play.MerchantPortal.Application.Contracts.Persistence;
using Play.MerchantPortal.Domain.Entities;
using Play.MerchantPortal.Infrastructure.Persistence.Sql;

namespace Play.MerchantPortal.Infrastructure.Persistence.Repositories;

internal class MerchantsRepository : Repository<MerchantEntity>, IMerchantsRepository
{
    #region Constructor

    public MerchantsRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="OperationCanceledException"></exception>
    public async Task<MerchantEntity?> SelectById(long id)
    {
        return await _DbContext.Merchants.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
    }

    #endregion
}