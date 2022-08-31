using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Application.Contracts.Services;

namespace MerchantPortal.Application.Services.Terminals;

internal class TerminalConfigurationService : ITerminalConfigurationService
{
    private readonly ITerminalsRepository _TerminalsRepository;
    private readonly IPoSRepository _posRepository;

    public TerminalConfigurationService(ITerminalsRepository terminalsRepository, IPoSRepository posRepository)
    {
        _TerminalsRepository = terminalsRepository;
        _posRepository = posRepository;
    }


}
