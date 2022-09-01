using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Core.Entities.PointOfSale;
using MerchantPortal.Infrastructure.Persistence.Mongo.MongoDBHelper;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace MerchantPortal.Infrastructure.Persistence.Repositories;

internal class PoSRepository : IPoSRepository
{
    private readonly IMongoDBHelper _mongoDBHelper;
    private const string _collectionName = "PoSConfigurations";

    public PoSRepository(IMongoDBHelper mongoDBHelper)
    {
        _mongoDBHelper = mongoDBHelper;
    }

    public async Task InsertPosConfigurationHeader(PosConfigurationHeader posConfigurationHeader)
    {
        await _mongoDBHelper.InsertAsync<PosConfigurationHeader>(_collectionName, posConfigurationHeader);
    }

    public async Task<PoSConfiguration> FindByTerminalId(long terminalId)
    {
        var result = await _mongoDBHelper.SelectAsync<PoSConfiguration>(_collectionName, filter => filter.TerminalId == terminalId);

        return result.SingleOrDefault();
    }

    public async Task<PoSConfiguration> FindById(long id)
    {
        return (await _mongoDBHelper.SelectAsync<PoSConfiguration>(_collectionName, filter => filter.Id == id)).SingleOrDefault();
    }

    public async Task<IEnumerable<PoSConfiguration>> SelectByCompanyId(long companyId)
    {
        return await _mongoDBHelper.SelectAsync<PoSConfiguration>(_collectionName, filter => filter.CompanyId == companyId);
    }

    public async Task<IEnumerable<PoSConfiguration>> SelectPoSConfigurationsByMerchantId(long merchantId)
    {
        return await _mongoDBHelper.SelectAsync<PoSConfiguration>(_collectionName, filter => filter.MerchantId == merchantId);
    }

    public async Task<IEnumerable<PoSConfiguration>> SelectPosConfigurationsByStoreId(long storeId)
    {
        return await _mongoDBHelper.SelectAsync<PoSConfiguration>(_collectionName, filter => filter.StoreId == storeId);
    }

    public async Task UpdateGivenFields(long id, List<(Expression<Func<PoSConfiguration, object>>, object)> updatedValues)
    {
        await _mongoDBHelper.FindBeforeUpdateAsync<PoSConfiguration>(
            _collectionName,
            filter => filter.Id == id,
            updatedValues.Select(x => new UpdateFieldConfig<PoSConfiguration> { Field = x.Item1, Value = x.Item2 }).ToArray());
    }

    public async Task AddCombinationConfiguration(long id, Combination combination)
    {
        await _mongoDBHelper.AddToSetAsync<PoSConfiguration, Combination>(
            _collectionName,
            filter => filter.Id == id,
            new AddFieldConfig<PoSConfiguration, Combination>
            {
                Field = combination,
                FieldDefinition = x => x.Combinations
            });
    }

    public async Task AddCertificateConfiguration(long id, CertificateConfiguration configuration)
    {
        await _mongoDBHelper.AddToSetAsync<PoSConfiguration, CertificateConfiguration>(
            _collectionName,
            fiter => fiter.Id == id,
            new AddFieldConfig<PoSConfiguration, CertificateConfiguration>
            {
                Field = configuration,
                FieldDefinition = x => x.CertificateAuthorityConfiguration.Certificates
            });
    }
}
