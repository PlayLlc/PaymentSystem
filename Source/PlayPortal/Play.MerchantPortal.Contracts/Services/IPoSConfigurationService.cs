using Play.MerchantPortal.Contracts.DTO.PointOfSale;

namespace Play.MerchantPortal.Contracts.Services;

public interface IPoSConfigurationService
{
    Task CreateNewPosConfiguratioAsync(CreatePosConfigurationDto initialConfiguration);

    Task<PoSConfigurationDto> GetTerminalPoSConfigurationAsync(long terminalId);

    Task<PoSConfigurationDto> GetPoSConfigurationAsync(long id);

    Task<IEnumerable<PoSConfigurationDto>> GetStorePoSConfigurationsAsync(long storeId);

    Task<IEnumerable<PoSConfigurationDto>> GetMerchantPoSConfigurationsAsync(long merchantId);

    Task UpdateTerminalConfigurationAsync(long id, TerminalConfigurationDto terminalConfiguration);

    Task UpdateCombinationsConfigurationAsync(long id, IEnumerable<CombinationDto> combinations);

    Task UpdateKernelConfigurationAsync(long id, KernelConfigurationDto kernelConfiguration);

    Task UpdateDisplayConfigurationAsync(long id, DisplayConfigurationDto displayConfiguration);

    Task UpdateProximityCouplingDeviceConfigurationAsync(long id, ProximityCouplingDeviceConfigurationDto proximityCouplingDeviceConfiguration);

    Task UpdateCertificateConfigurationAsync(long id, CertificateAuthorityConfigurationDto certificateAuthorityConfiguration);

    Task UpdateCertificateAuthorityConfigurationAsync(long id, CertificateAuthorityConfigurationDto certificateAuthorityConfiguration);
}
