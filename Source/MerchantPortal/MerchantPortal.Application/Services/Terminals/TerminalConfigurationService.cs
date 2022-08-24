using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Application.Contracts.Services;

namespace MerchantPortal.Application.Services.Terminals;

internal class TerminalConfigurationService : ITerminalConfigurationService
{
    private readonly ITerminalsRepository _TerminalsRepository;

    public TerminalConfigurationService(ITerminalsRepository terminalsRepository)
    {
        _TerminalsRepository = terminalsRepository;
    }
}
