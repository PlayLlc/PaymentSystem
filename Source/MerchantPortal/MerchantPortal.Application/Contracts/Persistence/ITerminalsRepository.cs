using MerchantPortal.Core.Entities;

namespace MerchantPortal.Application.Contracts.Persistence;

public interface ITerminalsRepository : IRepository<TerminalEntity>
{
    IEnumerable<TerminalEntity> SelectTerminalsByStore(long storeId);
}
