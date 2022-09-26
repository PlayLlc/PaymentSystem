using System.Linq.Expressions;

using Play.Merchants.Domain.Entities.PointOfSale;

namespace Play.Merchants.Domain.Repositories;

public interface IPointOfSaleRepository
{
    #region Instance Members

    Task InsertNewPosConfigurationAsync(PointOfSaleConfiguration posConfiguration);

    Task<IEnumerable<PointOfSaleConfiguration>> SelectByCompanyIdAsync(long companyId);

    Task<IEnumerable<PointOfSaleConfiguration>> SelectPoSConfigurationsByMerchantIdAsync(long merchantId);

    Task<IEnumerable<PointOfSaleConfiguration>> SelectPosConfigurationsByStoreIdAsync(long storeId);

    Task<PointOfSaleConfiguration?> FindByTerminalIdAsync(long terminalId);

    Task<PointOfSaleConfiguration?> FindByIdAsync(Guid id);

    Task<bool> ExistsAsync(Guid id);

    Task UpdateGivenFieldsAsync(Guid id, List<(Expression<Func<PointOfSaleConfiguration, object>>, object)> updatedValues);

    Task AddCombinationConfigurationAsync(Guid id, CombinationConfiguration combination);

    Task AddCertificateConfigurationAsync(Guid id, CertificateConfiguration configuration);

    #endregion
}