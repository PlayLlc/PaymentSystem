using Microsoft.EntityFrameworkCore;

using Play.Merchants.Domain.Entities;
using Play.Merchants.Domain.Repositories;
using Play.Merchants.Persistence.Sql.Sql;

namespace Play.Merchants.Persistence.Sql.Repositories;

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