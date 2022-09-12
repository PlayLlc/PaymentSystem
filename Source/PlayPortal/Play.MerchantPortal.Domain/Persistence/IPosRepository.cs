using Play.MerchantPortal.Domain.Entities.PointOfSale;
using System.Linq.Expressions;

namespace Play.MerchantPortal.Domain.Persistence;

public interface IPosRepository
{
    Task InsertNewPosConfigurationAsync(PosConfiguration posConfiguration);

    Task<IEnumerable<PosConfiguration>> SelectByCompanyIdAsync(long companyId);

    Task<IEnumerable<PosConfiguration>> SelectPoSConfigurationsByMerchantIdAsync(long merchantId);

    Task<IEnumerable<PosConfiguration>> SelectPosConfigurationsByStoreIdAsync(long storeId);

    Task<PosConfiguration?> FindByTerminalIdAsync(long terminalId);

    Task<PosConfiguration?> FindByIdAsync(Guid id);

    Task<bool> ExistsAsync(Guid id);

    Task UpdateGivenFieldsAsync(Guid id, List<(Expression<Func<PosConfiguration, object>>, object)> updatedValues);

    Task AddCombinationConfigurationAsync(Guid id, CombinationConfiguration combination);

    Task AddCertificateConfigurationAsync(Guid id, CertificateConfiguration configuration);
}