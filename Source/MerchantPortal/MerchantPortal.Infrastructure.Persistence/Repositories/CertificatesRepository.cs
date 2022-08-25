using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Core.Entities;
using MerchantPortal.Infrastructure.Persistence.Mongo.MongoDBHelper;

namespace MerchantPortal.Infrastructure.Persistence.Repositories;

internal class CertificatesRepository : ICertificatesRepository
{
    private readonly IMongoDBHelper _MongoDBHelper;
    private static readonly string _CollectionName = "MerchantPortal_Certificates";

    public CertificatesRepository(IMongoDBHelper mongoDBHelper)
    {
        _MongoDBHelper = mongoDBHelper;
    }

    public IQueryable<CertificateEntity> Query => 
        _MongoDBHelper
        .SelectAsync<CertificateEntity>(_CollectionName)
        .Result
        .AsQueryable();

    public CertificateEntity AddEntity(CertificateEntity entity)
    {
        _MongoDBHelper.InsertAsync(_CollectionName, entity).Wait();

        return entity;
    }

    public void DeleteEntity(CertificateEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task SaveChanges()
    {
        throw new NotImplementedException();
    }
}
