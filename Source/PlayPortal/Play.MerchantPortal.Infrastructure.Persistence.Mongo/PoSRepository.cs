using Play.MerchantPortal.Domain.Entities.PointOfSale;
using Play.MerchantPortal.Domain.Persistence;
using Play.MerchantPortal.Infrastructure.Persistence.Mongo.Mongo.Helpers;
using System.Linq.Expressions;

namespace Play.MerchantPortal.Infrastructure.Persistence.Mongo;

internal class PosRepository : IPosRepository
{
    private readonly IMongoDbHelper _MongoDbHelper;
    private const string _collectionName = "PosConfigurations";

    public PosRepository(IMongoDbHelper mongoDbHelper)
    {
        _MongoDbHelper = mongoDbHelper;
    }

    public async Task InsertNewPosConfigurationAsync(PosConfiguration posConfiguration)
    {
        await _MongoDbHelper.InsertAsync<PosConfiguration>(_collectionName, posConfiguration);
    }

    public async Task<IEnumerable<PosConfiguration>> SelectByCompanyIdAsync(long companyId)
    {
        return await _MongoDbHelper.SelectAsync<PosConfiguration>(_collectionName, filter => filter.CompanyId == companyId);
    }

    public async Task<IEnumerable<PosConfiguration>> SelectPoSConfigurationsByMerchantIdAsync(long merchantId)
    {
        return await _MongoDbHelper.SelectAsync<PosConfiguration>(_collectionName, filter => filter.MerchantId == merchantId);
    }

    public async Task<IEnumerable<PosConfiguration>> SelectPosConfigurationsByStoreIdAsync(long storeId)
    {
        return await _MongoDbHelper.SelectAsync<PosConfiguration>(_collectionName, filter => filter.StoreId == storeId);
    }

    public async Task<PosConfiguration?> FindByTerminalIdAsync(long terminalId)
    {
        var result = await _MongoDbHelper.SelectAsync<PosConfiguration>(_collectionName, filter => filter.TerminalId == terminalId);

        return result.SingleOrDefault();
    }

    public async Task<PosConfiguration?> FindByIdAsync(Guid id)
    {
        return (await _MongoDbHelper.SelectAsync<PosConfiguration>(_collectionName, filter => filter.Id == id)).SingleOrDefault();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        var count = (await _MongoDbHelper.CountAsync<PosConfiguration>(_collectionName, filter => filter.Id == id));
        return count > 0;
    }

    public async Task UpdateGivenFieldsAsync(Guid id, List<(Expression<Func<PosConfiguration, object>>, object)> updatedValues)
    {
        await _MongoDbHelper.FindOneAndUpdateAsync<PosConfiguration>(
            _collectionName,
            filter => filter.Id == id,
            updatedValues.Select(x => new UpdateFieldConfig<PosConfiguration> { Field = x.Item1!, Value = x.Item2 }).ToArray());
    }

    public async Task AddCombinationConfigurationAsync(Guid id, CombinationConfiguration combination)
    {
        await _MongoDbHelper.AddToSetAsync<PosConfiguration, CombinationConfiguration>(
            _collectionName,
            filter => filter.Id == id,
            new AddFieldConfig<PosConfiguration, CombinationConfiguration>
            {
                Field = combination,
                FieldDefinition = x => x.Combinations
            });
    }

    public async Task AddCertificateConfigurationAsync(Guid id, CertificateConfiguration configuration)
    {
        await _MongoDbHelper.AddToSetAsync<PosConfiguration, CertificateConfiguration>(
            _collectionName,
            fiter => fiter.Id == id,
            new AddFieldConfig<PosConfiguration, CertificateConfiguration>
            {
                Field = configuration,
                FieldDefinition = x => x.CertificateAuthorityConfiguration.Certificates
            });
    }
}
