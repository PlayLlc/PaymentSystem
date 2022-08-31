using MerchantPortal.Application.Contracts.Services;
using MerchantPortal.Application.DTO.PointOfSale;

namespace MerchantPortal.Application.Services.PoS;

public class DefaultPoSConfigurationService : IDefaultPoSConfigurationService
{
    public ValueTask<PoSConfigurationDto> GetDefaultPoSConfiguration()
    {
        throw new NotImplementedException();
    }

    public Task UpdateDefaultPoSConfiguration(PoSConfigurationDto posConfiguration)
    {
        throw new NotImplementedException();
    }
}
