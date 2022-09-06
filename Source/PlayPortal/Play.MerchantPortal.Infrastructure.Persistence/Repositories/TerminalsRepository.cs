using MerchantPortal.Core.Entities;
using MerchantPortal.Infrastructure.Persistence.Sql;

using Microsoft.EntityFrameworkCore;

using Play.MerchantPortal.Application.Contracts.Persistence;

namespace MerchantPortal.Infrastructure.Persistence.Repositories;

internal class TerminalsRepository : Repository<TerminalEntity>, ITerminalsRepository
{
    #region Constructor

    public TerminalsRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    { }

    #endregion

    #region Instance Members

    public async Task<TerminalEntity?> SelectById(long terminalId)
    {
        return await _DbContext.Terminals.AsNoTracking().FirstOrDefaultAsync(x => x.Id == terminalId);
    }

    public IEnumerable<TerminalEntity> SelectTerminalsByStore(long storeId)
    {
        return _DbContext.Terminals.AsNoTracking().Where(x => x.StoreId == storeId);
    }

    #endregion
}