using Play.Merchants.Contracts.DTO;

namespace Play.Merchants.Contracts.Services;

public interface IStoreConfigurationService
{
    #region Instance Members

    Task<IEnumerable<StoreDto>> GetMerchantStoresAsync(long merchantId);

    Task<StoreDto> GetStoreAsync(long id);

    Task<long> InsertStoreAsync(StoreDto storeDto);

    Task UpdateStoreAsync(StoreDto storeDto);

    Task DeleteStoreAsync(long id);

    #endregion
}