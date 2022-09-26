using Play.Merchants.Contracts.DTO;

namespace Play.Merchants.Contracts.Services;

public interface ITerminalConfigurationService
{
    #region Instance Members

    Task<IEnumerable<TerminalDto>> GetStoreTerminalsAsync(long storeId);

    Task<TerminalDto> GetTerminalAsync(long id);

    Task<long> InsertTerminalAsync(TerminalDto terminalDto);

    Task UpdateTerminalAsync(TerminalDto terminalDto);

    Task DeleteTerminalAsync(long id);

    #endregion
}