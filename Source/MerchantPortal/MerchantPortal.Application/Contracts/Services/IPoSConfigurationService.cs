using MerchantPortal.Application.DTO;

namespace MerchantPortal.Application.Contracts.Services;

public interface IPoSConfigurationService
{
    Task CreateNewPosConfiguration(CreatePosConfigurationDto initialConfiguration);

    Task UpdatePosTerminalConfiguration(long terminalId);

    Task UpdatePosCombinationConfiguration();

    Task UpdatePosKernelConfiguration();

    Task UpdatePosDisplayConfiguration();

    Task UpdateProximityCouplingDeviceConfiguration();

    Task UpdateCertificateConfiguration();
}
