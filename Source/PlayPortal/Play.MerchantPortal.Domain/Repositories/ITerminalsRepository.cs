using Play.MerchantPortal.Domain.Entities;

namespace Play.MerchantPortal.Domain.Persistence;

public interface ITerminalsRepository : IRepository<TerminalEntity>
{
    #region Instance Members

    Task<TerminalEntity?> SelectById(long terminalId);

    Task<IEnumerable<TerminalEntity>> SelectTerminalsByStore(long storeId);

    #endregion
}