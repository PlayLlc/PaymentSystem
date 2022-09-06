using Play.MerchantPortal.Application.Contracts.Persistence;
using Play.MerchantPortal.Domain.Entities;
using Play.MerchantPortal.Infrastructure.Persistence.Mongo;

namespace Play.MerchantPortal.Infrastructure.Persistence.Repositories;

internal class PointOfSaleConfigurationRepository : IPointOfSaleConfigurationRepository
{
    #region Static Metadata

    private static readonly string _CollectionName = "MerchantPortal_Pos";

    #endregion

    #region Instance Values

    private readonly IMongoDbHelper _MongoDbHelper;

    #endregion

    #region Constructor

    public PointOfSaleConfigurationRepository(IMongoDbHelper mongoDbHelper)
    {
        _MongoDbHelper = mongoDbHelper;
    }

    #endregion

    #region Instance Members

    public async Task AddEntity(CertificateEntity entity)
    {
        await _MongoDbHelper.InsertAsync(_CollectionName, entity).ConfigureAwait(false);
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