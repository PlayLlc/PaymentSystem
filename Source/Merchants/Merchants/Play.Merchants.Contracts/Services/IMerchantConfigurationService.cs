using Play.Merchants.Contracts.DTO;

namespace Play.Merchants.Contracts.Services;

public interface IMerchantConfigurationService
{
    #region Instance Members

    Task<MerchantDto> GetMerchantAsync(long id);

    Task<long> InsertMerchantAsync(MerchantDto merchant);

    Task UpdateMerchantAsync(MerchantDto merchant);

    #endregion
}