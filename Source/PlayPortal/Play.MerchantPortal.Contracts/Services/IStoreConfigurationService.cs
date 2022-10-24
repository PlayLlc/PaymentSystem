using Play.MerchantPortal.Contracts.DTO;

namespace Play.MerchantPortal.Contracts.Services;

public interface IStoreConfigurationService
{
    Task<IEnumerable<StoreDto>> GetMerchantStoresAsync(long merchantId);

    Task<StoreDto> GetStoreAsync(long id);

    Task<long> InsertStoreAsync(StoreDto storeDto);

    Task UpdateStoreAsync(StoreDto storeDto);

    Task DeleteStoreAsync(long id);
}
