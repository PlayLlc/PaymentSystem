using MerchantPortal.Application.DTO;

namespace MerchantPortal.Application.Contracts.Services;

public interface ITerminalConfigurationService
{
    Task<IEnumerable<TerminalDto>> GetStoreTerminalsAsync(long storeId);

    Task<TerminalDto> GetTerminalAsync(long id);

    Task<long> InsertTerminalAsync(TerminalDto terminalDto);

    Task UpdateTerminalAsync(long id, TerminalDto terminalDto);

    Task DeleteTerminalAsync(long id);

}
