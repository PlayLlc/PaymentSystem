using Play.MerchantPortal.Contracts.DTO;

namespace Play.MerchantPortal.Contracts.Services;

public interface ITerminalConfigurationService
{
    Task<IEnumerable<TerminalDto>> GetStoreTerminalsAsync(long storeId);

    Task<TerminalDto> GetTerminalAsync(long id);

    Task<long> InsertTerminalAsync(TerminalDto terminalDto);

    Task UpdateTerminalAsync(TerminalDto terminalDto);

    Task DeleteTerminalAsync(long id);

}
