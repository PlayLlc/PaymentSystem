using Play.Merchants.Contracts.Messages.PointOfSale;

namespace Play.Merchants.Contracts.Services;

public interface IPointOfSaleConfigurationService
{
    #region Instance Members

    Task CreateNewPosConfiguratioAsync(CreatePosConfigurationDto initialConfiguration);

    Task<PointOfSaleConfigurationDto> GetTerminalPoSConfigurationAsync(long terminalId);

    Task<PointOfSaleConfigurationDto> GetPoSConfigurationAsync(Guid id);

    Task<IEnumerable<PointOfSaleConfigurationDto>> GetStorePoSConfigurationsAsync(long storeId);

    Task<IEnumerable<PointOfSaleConfigurationDto>> GetMerchantPoSConfigurationsAsync(long merchantId);

    Task UpdateTerminalConfigurationAsync(Guid id, TerminalConfigurationDto terminalConfiguration);

    Task UpdateCombinationsConfigurationAsync(Guid id, IEnumerable<CombinationConfigurationDto> combinations);

    Task UpdateKernelConfigurationAsync(Guid id, KernelConfigurationDto kernelConfiguration);

    Task UpdateDisplayConfigurationAsync(Guid id, DisplayConfigurationDto displayConfiguration);

    Task UpdateProximityCouplingDeviceConfigurationAsync(Guid id, ProximityCouplingDeviceConfigurationDto proximityCouplingDeviceConfiguration);

    Task UpdateCertificateAuthorityConfigurationAsync(Guid id, CertificateAuthorityConfigurationDto certificateAuthorityConfiguration);

    #endregion
}