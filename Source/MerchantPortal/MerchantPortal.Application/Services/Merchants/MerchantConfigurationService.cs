using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Application.Contracts.Services;

namespace MerchantPortal.Application.Services.Merchants;

internal class MerchantConfigurationService : IMerchantConfigurationService
{
    private readonly IMerchantsRepository _MerchantsRepository;

    public MerchantConfigurationService(IMerchantsRepository merchantsRepository)
    {
        _MerchantsRepository = merchantsRepository;
    }
}
