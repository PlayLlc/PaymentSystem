using Play.MerchantPortal.Contracts.DTO.PointOfSale;

namespace Play.MerchantPortal.Contracts.Services;

public interface IPosConfigurationService
{
    Task CreateNewPosConfiguratioAsync(CreatePosConfigurationDto initialConfiguration);

    Task<PosConfigurationDto> GetTerminalPoSConfigurationAsync(long terminalId);

    Task<PosConfigurationDto> GetPoSConfigurationAsync(Guid id);

    Task<IEnumerable<PosConfigurationDto>> GetStorePoSConfigurationsAsync(long storeId);

    Task<IEnumerable<PosConfigurationDto>> GetMerchantPoSConfigurationsAsync(long merchantId);

    Task UpdateTerminalConfigurationAsync(Guid id, TerminalConfigurationDto terminalConfiguration);

    Task UpdateCombinationsConfigurationAsync(Guid id, IEnumerable<CombinationConfigurationDto> combinations);

    Task UpdateKernelConfigurationAsync(Guid id, KernelConfigurationDto kernelConfiguration);

    Task UpdateDisplayConfigurationAsync(Guid id, DisplayConfigurationDto displayConfiguration);

    Task UpdateProximityCouplingDeviceConfigurationAsync(Guid id, ProximityCouplingDeviceConfigurationDto proximityCouplingDeviceConfiguration);

    Task UpdateCertificateAuthorityConfigurationAsync(Guid id, CertificateAuthorityConfigurationDto certificateAuthorityConfiguration);
}
