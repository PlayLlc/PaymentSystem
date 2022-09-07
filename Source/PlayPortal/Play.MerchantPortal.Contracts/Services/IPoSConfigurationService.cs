using Play.MerchantPortal.Contracts.DTO.PointOfSale;

namespace Play.MerchantPortal.Contracts.Services;

public interface IPoSConfigurationService
{
    Task CreateNewPosConfiguratioAsync(CreatePosConfigurationDto initialConfiguration);

    Task<PoSConfigurationDto> GetTerminalPoSConfigurationAsync(long terminalId);

    Task<PoSConfigurationDto> GetPoSConfigurationAsync(long id);

    Task<IEnumerable<PoSConfigurationDto>> GetStorePoSConfigurationsAsync(long storeId);

    Task<IEnumerable<PoSConfigurationDto>> GetMerchantPoSConfigurationsAsync(long merchantId);

    Task UpdatePosTerminalConfigurationAsync(long id, TerminalConfigurationDto terminalConfiguration);

    Task AddPosCombinationConfigurationAsync(long id, CombinationDto combination);

    Task UpdatePosCombinationsConfigurationAsync(long id, IEnumerable<CombinationDto> combinations);

    Task UpdatePosKernelConfigurationAsync(long id, KernelConfigurationDto kernelConfiguration);

    Task UpdatePosDisplayConfigurationAsync(long id, DisplayConfigurationDto displayConfiguration);

    Task UpdateProximityCouplingDeviceConfigurationAsync(long id, ProximityCouplingDeviceConfigurationDto proximityCouplingDeviceConfiguration);

    Task UpdateCertificateConfigurationAsync(long id, CertificateAuthorityConfigurationDto certificateAuthorityConfiguration);

    Task AddCertificateConfigurationAsync(long id, CertificateConfigurationDto certificateConfiguration);

    Task UpdateCertificateAuthorityConfigurationAsync(long id, CertificateAuthorityConfigurationDto certificateAuthorityConfiguration);
}
