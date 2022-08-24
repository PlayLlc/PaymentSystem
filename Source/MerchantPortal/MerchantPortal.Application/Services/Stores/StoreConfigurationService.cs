using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Application.Contracts.Services;

namespace MerchantPortal.Application.Services.Stores;

internal class StoreConfigurationService : IStoreConfigurationService
{
    private readonly IStoresRepository _StoresRepository;

    public StoreConfigurationService(IStoresRepository storesRepository)
    {
        _StoresRepository = storesRepository;
    }
}
