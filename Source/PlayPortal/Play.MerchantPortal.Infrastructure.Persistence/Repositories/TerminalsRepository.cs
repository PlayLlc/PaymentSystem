using Microsoft.EntityFrameworkCore;

using Play.MerchantPortal.Application.Contracts.Persistence;
using Play.MerchantPortal.Domain.Entities;
using Play.MerchantPortal.Infrastructure.Persistence.Sql;

namespace Play.MerchantPortal.Infrastructure.Persistence.Repositories;

internal class TerminalsRepository : Repository<TerminalEntity>, ITerminalsRepository
{
    #region Constructor

    public TerminalsRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="OperationCanceledException"></exception>
    public async Task<TerminalEntity?> SelectById(long terminalId)
    {
        return await _DbContext.Terminals.AsNoTracking().FirstOrDefaultAsync(x => x.Id == terminalId).ConfigureAwait(false);
    }

    public IEnumerable<TerminalEntity> SelectTerminalsByStore(long storeId)
    {
        return _DbContext.Terminals.AsNoTracking().Where(x => x.StoreId == storeId);
    }

    #endregion
}