﻿using System.Linq.Expressions;

using Play.Merchants.Domain.Entities.PointOfSale;
using Play.Merchants.Domain.Repositories;
using Play.Persistence.Mongo.Helpers;

namespace Play.Merchants.Persistence.Mongo;

internal class PointOfSaleRepository : IPointOfSaleRepository
{
    #region Static Metadata

    private const string _collectionName = "PosConfigurations";

    #endregion

    #region Instance Values

    private readonly IMongoDbHelper _MongoDbHelper;

    #endregion

    #region Constructor

    public PointOfSaleRepository(IMongoDbHelper mongoDbHelper)
    {
        _MongoDbHelper = mongoDbHelper;
    }

    #endregion

    #region Instance Members

    public async Task InsertNewPosConfigurationAsync(PointOfSaleConfiguration posConfiguration)
    {
        await _MongoDbHelper.InsertAsync<PointOfSaleConfiguration>(_collectionName, posConfiguration);
    }

    public async Task<IEnumerable<PointOfSaleConfiguration>> SelectByCompanyIdAsync(long companyId)
    {
        return await _MongoDbHelper.SelectAsync<PointOfSaleConfiguration>(_collectionName, filter => filter.CompanyId == companyId);
    }

    public async Task<IEnumerable<PointOfSaleConfiguration>> SelectPoSConfigurationsByMerchantIdAsync(long merchantId)
    {
        return await _MongoDbHelper.SelectAsync<PointOfSaleConfiguration>(_collectionName, filter => filter.MerchantId == merchantId);
    }

    public async Task<IEnumerable<PointOfSaleConfiguration>> SelectPosConfigurationsByStoreIdAsync(long storeId)
    {
        return await _MongoDbHelper.SelectAsync<PointOfSaleConfiguration>(_collectionName, filter => filter.StoreId == storeId);
    }

    public async Task<PointOfSaleConfiguration?> FindByTerminalIdAsync(long terminalId)
    {
        var result = await _MongoDbHelper.SelectAsync<PointOfSaleConfiguration>(_collectionName, filter => filter.TerminalId == terminalId);

        return result.SingleOrDefault();
    }

    public async Task<PointOfSaleConfiguration?> FindByIdAsync(Guid id)
    {
        return (await _MongoDbHelper.SelectAsync<PointOfSaleConfiguration>(_collectionName, filter => filter.Id == id)).SingleOrDefault();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        var count = await _MongoDbHelper.CountAsync<PointOfSaleConfiguration>(_collectionName, filter => filter.Id == id);

        return count > 0;
    }

    public async Task UpdateGivenFieldsAsync(Guid id, List<(Expression<Func<PointOfSaleConfiguration, object>>, object)> updatedValues)
    {
        await _MongoDbHelper.FindOneAndUpdateAsync<PointOfSaleConfiguration>(_collectionName, filter => filter.Id == id,
            updatedValues.Select(x => new UpdateFieldConfig<PointOfSaleConfiguration> {Field = x.Item1!, Value = x.Item2}).ToArray());
    }

    public async Task AddCombinationConfigurationAsync(Guid id, CombinationConfiguration combination)
    {
        await _MongoDbHelper.AddToSetAsync<PointOfSaleConfiguration, CombinationConfiguration>(_collectionName, filter => filter.Id == id,
            new AddFieldConfig<PointOfSaleConfiguration, CombinationConfiguration> {Field = combination, FieldDefinition = x => x.Combinations});
    }

    public async Task AddCertificateConfigurationAsync(Guid id, CertificateConfiguration configuration)
    {
        await _MongoDbHelper.AddToSetAsync<PointOfSaleConfiguration, CertificateConfiguration>(_collectionName, fiter => fiter.Id == id,
            new AddFieldConfig<PointOfSaleConfiguration, CertificateConfiguration>
            {
                Field = configuration, FieldDefinition = x => x.CertificateAuthorityConfiguration.Certificates
            });
    }

    #endregion
}