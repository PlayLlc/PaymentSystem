using MerchantPortal.Application.DTO;

namespace MerchantPortal.Application.Contracts.Services;

public interface IMerchantConfigurationService
{
    Task<MerchantDto> GetMerchantAsync(long id);

    Task InsertMerchantAsync(MerchantDto merchant);
}
