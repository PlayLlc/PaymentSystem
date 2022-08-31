using MerchantPortal.Application.DTO;
using MerchantPortal.Application.DTO.PointOfSale;

namespace MerchantPortal.Application.Contracts.Services;

public interface IPoSConfigurationService
{
    Task CreateNewPosConfiguration(CreatePosConfigurationDto initialConfiguration);

    Task<PoSConfigurationDto> GetTerminalPoSConfiguration(long terminalId);

    Task<PoSConfigurationDto> GetPoSConfiguration(Guid id);

    Task UpdatePosTerminalConfiguration(Guid id, TerminalConfigurationDto terminalConfiguration);

    Task AddPosCombinationConfiguration(Guid id, CombinationDto combination);

    Task UpdatePosKernelConfiguration(Guid id, KernelConfigurationDto kernelConfiguration);

    Task UpdatePosDisplayConfiguration(Guid id, DisplayConfigurationDto displayConfiguration);

    Task UpdateProximityCouplingDeviceConfiguration(Guid id, ProximityCouplingDeviceConfigurationDto proximityCouplingDeviceConfiguration);

    Task UpdateCertificateConfiguration(Guid id, CertificateAuthorityConfigurationDto certificateAuthorityConfiguration);

    Task AddCertificateConfiguration(Guid id, CertificateConfigurationDto certificateConfiguration);
}
