using Play.MerchantPortal.Application.Contracts.Persistence;
using Play.MerchantPortal.Domain.Entities.PointOfSale;
using Play.MerchantPortal.Infrastructure.Persistence.Mongo.Mongo.Helpers;
using System.Linq.Expressions;

namespace Play.MerchantPortal.Infrastructure.Persistence.Mongo;

internal class PoSRepository : IPoSRepository
{
    private readonly IMongoDbHelper _MongoDbHelper;
    private const string _collectionName = "PosConfigurations";

    public PoSRepository(IMongoDbHelper mongoDbHelper)
    {
        _MongoDbHelper = mongoDbHelper;
    }

    public async Task InsertNewPosConfigurationAsync(PoSConfiguration posConfiguration)
    {
        await _MongoDbHelper.InsertAsync<PoSConfiguration>(_collectionName, posConfiguration);
    }

    public async Task<IEnumerable<PoSConfiguration>> SelectByCompanyIdAsync(long companyId)
    {
        return await _MongoDbHelper.SelectAsync<PoSConfiguration>(_collectionName, filter => filter.CompanyId == companyId);
    }

    public async Task<IEnumerable<PoSConfiguration>> SelectPoSConfigurationsByMerchantIdAsync(long merchantId)
    {
        return await _MongoDbHelper.SelectAsync<PoSConfiguration>(_collectionName, filter => filter.MerchantId == merchantId);
    }

    public async Task<IEnumerable<PoSConfiguration>> SelectPosConfigurationsByStoreIdAsync(long storeId)
    {
        return await _MongoDbHelper.SelectAsync<PoSConfiguration>(_collectionName, filter => filter.StoreId == storeId);
    }

    public async Task<PoSConfiguration?> FindByTerminalIdAsync(long terminalId)
    {
        var result = await _MongoDbHelper.SelectAsync<PoSConfiguration>(_collectionName, filter => filter.TerminalId == terminalId);

        return result.SingleOrDefault();
    }

    public async Task<PoSConfiguration?> FindByIdAsync(long id)
    {
        return (await _MongoDbHelper.SelectAsync<PoSConfiguration>(_collectionName, filter => filter.Id == id)).SingleOrDefault();
    }

    public async Task<bool> ExistsAsync(long id)
    {
        return (await _MongoDbHelper.CountAsync<PoSConfiguration>(_collectionName, filter => filter.Id == id)) > 0;
    }

    public async Task UpdateGivenFieldsAsync(long id, List<(Expression<Func<PoSConfiguration, object>>, object)> updatedValues)
    {
        await _MongoDbHelper.FindOneAndUpdateAsync<PoSConfiguration>(
            _collectionName,
            filter => filter.Id == id,
            updatedValues.Select(x => new UpdateFieldConfig<PoSConfiguration> { Field = x.Item1!, Value = x.Item2 }).ToArray());
    }

    public async Task AddCombinationConfigurationAsync(long id, Combination combination)
    {
        await _MongoDbHelper.AddToSetAsync<PoSConfiguration, Combination>(
            _collectionName,
            filter => filter.Id == id,
            new AddFieldConfig<PoSConfiguration, Combination>
            {
                Field = combination,
                FieldDefinition = x => x.Combinations
            });
    }

    public async Task AddCertificateConfigurationAsync(long id, CertificateConfiguration configuration)
    {
        await _MongoDbHelper.AddToSetAsync<PoSConfiguration, CertificateConfiguration>(
            _collectionName,
            fiter => fiter.Id == id,
            new AddFieldConfig<PoSConfiguration, CertificateConfiguration>
            {
                Field = configuration,
                FieldDefinition = x => x.CertificateAuthorityConfiguration.Certificates
            });
    }
}
