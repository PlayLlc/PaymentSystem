using MerchantPortal.Core.Entities;
using MerchantPortal.Infrastructure.Persistence.Mongo.MongoDBHelper;

using Play.MerchantPortal.Application.Contracts.Persistence;

namespace MerchantPortal.Infrastructure.Persistence.Repositories;

internal class PointOfSaleConfigurationRepository : IPointOfSaleConfigurationRepository
{
    #region Static Metadata

    private static readonly string _CollectionName = "MerchantPortal_Pos";

    #endregion

    #region Instance Values

    private readonly IMongoDbHelper _MongoDbHelper;

    public IEnumerable<CertificateEntity> Query => _MongoDbHelper.SelectAsync<CertificateEntity>(_CollectionName).Result;

    #endregion

    #region Constructor

    public PointOfSaleConfigurationRepository(IMongoDbHelper mongoDbHelper)
    {
        _MongoDbHelper = mongoDbHelper;
    }

    #endregion

    #region Instance Members

    public CertificateEntity AddEntity(CertificateEntity entity)
    {
        _MongoDbHelper.InsertAsync(_CollectionName, entity).Wait();

        return entity;
    }

    public void DeleteEntity(CertificateEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    #endregion
}