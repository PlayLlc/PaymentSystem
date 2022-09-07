using Play.MerchantPortal.Domain.Entities.PointOfSale;
using System.Linq.Expressions;

namespace Play.MerchantPortal.Application.Contracts.Persistence;

public interface IPoSRepository
{
    Task InsertNewPosConfigurationAsync(PoSConfiguration posConfiguration);

    Task<IEnumerable<PoSConfiguration>> SelectByCompanyIdAsync(long companyId);

    Task<IEnumerable<PoSConfiguration>> SelectPoSConfigurationsByMerchantIdAsync(long merchantId);

    Task<IEnumerable<PoSConfiguration>> SelectPosConfigurationsByStoreIdAsync(long storeId);

    Task<PoSConfiguration?> FindByTerminalIdAsync(long terminalId);

    Task<PoSConfiguration?> FindByIdAsync(long id);

    Task<bool> ExistsAsync(long id);

    Task UpdateGivenFieldsAsync(long id, List<(Expression<Func<PoSConfiguration, object>>, object)> updatedValues);

    Task AddCombinationConfigurationAsync(long id, Combination combination);

    Task AddCertificateConfigurationAsync(long id, CertificateConfiguration configuration);
}