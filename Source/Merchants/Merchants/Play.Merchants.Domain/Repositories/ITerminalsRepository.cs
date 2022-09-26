using Play.Merchants.Domain.Entities;

namespace Play.Merchants.Domain.Repositories;

public interface ITerminalsRepository : IRepository<Terminal>
{
    #region Instance Members

    Task<Terminal?> SelectById(long terminalId);

    Task<IEnumerable<Terminal>> SelectTerminalsByStore(long storeId);

    #endregion
}