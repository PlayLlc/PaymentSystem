using Play.MerchantPortal.Domain.Entities;

namespace Play.MerchantPortal.Domain.Persistence;

public interface ITerminalsRepository : IRepository<Terminal>
{
    #region Instance Members

    Task<Terminal?> SelectById(long terminalId);

    Task<IEnumerable<Terminal>> SelectTerminalsByStore(long storeId);

    #endregion
}