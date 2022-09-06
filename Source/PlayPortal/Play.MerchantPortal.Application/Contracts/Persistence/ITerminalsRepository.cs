using Play.MerchantPortal.Domain.Entities;

namespace Play.MerchantPortal.Application.Contracts.Persistence;

public interface ITerminalsRepository : IRepository<TerminalEntity>
{
    #region Instance Members

    Task<TerminalEntity?> SelectById(long terminalId);

    Task<IEnumerable<TerminalEntity>> SelectTerminalsByStore(long storeId);

    #endregion
}