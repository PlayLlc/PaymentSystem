using MerchantPortal.Application.DTO.PointOfSale;

namespace MerchantPortal.Application.Contracts.Services;

public interface IDefaultPoSConfigurationService
{
    ValueTask<PoSConfigurationDto> GetDefaultPoSConfiguration();

    Task UpdateDefaultPoSConfiguration(PoSConfigurationDto posConfiguration);
}
