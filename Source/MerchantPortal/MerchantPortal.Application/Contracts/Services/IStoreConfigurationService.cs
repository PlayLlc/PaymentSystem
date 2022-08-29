using MerchantPortal.Application.DTO;

namespace MerchantPortal.Application.Contracts.Services;

public interface IStoreConfigurationService
{
    Task<IEnumerable<StoreHeaderDto>> GetMerchantStoresAsync(long merchantId);

    Task<StoreDto> GetStoreAsync(long id);

    Task<long> InsertStoreAsync(StoreDto storeDto);

    Task UpdateStoreAsync(long id, StoreDto storeDto);

    Task DeleteStoreAsync(long id);
}
