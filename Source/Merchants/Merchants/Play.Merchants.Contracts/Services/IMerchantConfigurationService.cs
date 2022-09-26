using Play.MerchantPortal.Contracts.DTO;

namespace Play.MerchantPortal.Contracts.Services;

public interface IMerchantConfigurationService
{
    Task<MerchantDto> GetMerchantAsync(long id);

    Task<long> InsertMerchantAsync(MerchantDto merchant);

    Task UpdateMerchantAsync(MerchantDto merchant);
}
