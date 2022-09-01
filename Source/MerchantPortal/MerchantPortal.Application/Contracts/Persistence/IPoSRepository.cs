using MerchantPortal.Core.Entities.PointOfSale;
using System.Linq.Expressions;

namespace MerchantPortal.Application.Contracts.Persistence;

//name maybe accordingly with async
public interface IPoSRepository
{
    Task InsertPosConfigurationHeader(PosConfigurationHeader posConfigurationHeader);

    Task<IEnumerable<PoSConfiguration>> SelectByCompanyId(long companyId);

    Task<IEnumerable<PoSConfiguration>> SelectPoSConfigurationsByMerchantId(long merchantId);

    Task<IEnumerable<PoSConfiguration>> SelectPosConfigurationsByStoreId(long storeId);

    Task<PoSConfiguration> FindByTerminalId(long terminalId);

    Task<PoSConfiguration> FindById(long id);

    Task<bool> Exists(long id);

    Task UpdateGivenFields(long id, List<(Expression<Func<PoSConfiguration, object>>, object)> updatedValues);

    Task AddCombinationConfiguration(long id, Combination combination);

    Task AddCertificateConfiguration(long id, CertificateConfiguration configuration);
}
