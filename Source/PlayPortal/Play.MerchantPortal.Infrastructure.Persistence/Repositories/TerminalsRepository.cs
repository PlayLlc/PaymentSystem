using Microsoft.EntityFrameworkCore;
using Play.MerchantPortal.Domain.Entities;
using Play.MerchantPortal.Domain.Persistence;
using Play.MerchantPortal.Infrastructure.Persistence.Sql;

namespace Play.MerchantPortal.Infrastructure.Persistence.Repositories;

internal class TerminalsRepository : Repository<Terminal>, ITerminalsRepository
{
    #region Constructor

    public TerminalsRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="OperationCanceledException"></exception>
    public async Task<Terminal?> SelectById(long terminalId)
    {
        return await _DbContext.Terminals.AsNoTracking().FirstOrDefaultAsync(x => x.Id == terminalId).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Terminal>> SelectTerminalsByStore(long storeId)
    {
        return await Task.FromResult(_DbContext.Terminals.AsNoTracking().Where(x => x.StoreId == storeId)).ConfigureAwait(false);
    }

    #endregion
}