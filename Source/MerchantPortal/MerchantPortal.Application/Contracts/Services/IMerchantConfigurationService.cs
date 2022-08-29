using MerchantPortal.Application.DTO;

namespace MerchantPortal.Application.Contracts.Services;

public interface IMerchantConfigurationService
{
    Task<MerchantDto> GetMerchantAsync(long id);

    Task<long> InsertMerchantAsync(MerchantDto merchant);

    Task UpdateMerchantAsync(long id, MerchantDto merchant);
}
