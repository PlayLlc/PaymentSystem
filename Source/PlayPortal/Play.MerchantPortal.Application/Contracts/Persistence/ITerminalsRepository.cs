using MerchantPortal.Core.Entities;

namespace Play.MerchantPortal.Application.Contracts.Persistence;

public interface ITerminalsRepository : IRepository<TerminalEntity>
{
    Task<TerminalEntity> SelectById(long terminalId);

    IEnumerable<TerminalEntity> SelectTerminalsByStore(long storeId);
}
