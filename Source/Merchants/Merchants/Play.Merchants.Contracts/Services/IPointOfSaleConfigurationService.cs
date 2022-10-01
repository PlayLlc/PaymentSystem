using Play.Merchants.Contracts.DTO;
using Play.Merchants.Contracts.Messages;

namespace Play.Merchants.Contracts.Services;

public interface IPointOfSaleConfigurationService
{
    #region Instance Members

    Task CreatePosConfigurationAsync(CreatePosConfigurationDto initialConfiguration);

    Task<PointOfSaleConfigurationDto> GetTerminalConfigurationAsync(long terminalId);

    Task<PointOfSaleConfigurationDto> GetPosConfigurationAsync(Guid id);

    Task<IEnumerable<PointOfSaleConfigurationDto>> GetPosConfigurationByStoreIdAsync(long storeId);

    Task<IEnumerable<PointOfSaleConfigurationDto>> GetPosConfigurationByMerchantIdAsync(long merchantId);

    Task UpdateTerminalConfigurationAsync(Guid id, TerminalConfigurationDto terminalConfiguration);

    Task UpdateCombinationsConfigurationAsync(Guid id, IEnumerable<CombinationConfigurationDto> combinations);

    Task UpdateKernelConfigurationAsync(Guid id, KernelConfigurationDto kernelConfiguration);

    Task UpdateDisplayConfigurationAsync(Guid id, DisplayConfigurationDto displayConfiguration);

    Task UpdateProximityCouplingDeviceConfigurationAsync(Guid id, ProximityCouplingDeviceConfigurationDto proximityCouplingDeviceConfiguration);

    Task UpdateCertificateAuthorityConfigurationAsync(Guid id, CertificateAuthorityConfigurationDto certificateAuthorityConfiguration);

    #endregion
}