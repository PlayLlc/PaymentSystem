﻿using MerchantPortal.Core.Entities.PointOfSale;
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

    Task<PoSConfiguration> FindById(Guid id);

    Task UpdateGivenFields(Guid id, List<(Expression<Func<PoSConfiguration, object>>, object)> updatedValues);

    Task AddCombinationConfiguration(Guid id, Combination combination);

    Task AddCertificateConfiguration(Guid id, CertificateConfiguration configuration);
}
