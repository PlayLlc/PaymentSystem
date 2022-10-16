using Play.Accounts.Domain.Aggregates.MerchantRegistration;
using Play.Accounts.Domain.Repositories;

namespace Play.Accounts.Persistence.Sql.Repositories;

public class MerchantRegistrationRepository : IMerchantRegistrationRepository
{
    #region Instance Members

    public Task<MerchantRegistration?> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(MerchantRegistration aggregate)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(MerchantRegistration id)
    {
        throw new NotImplementedException();
    }

    public MerchantRegistration? GetById(string id)
    {
        throw new NotImplementedException();
    }

    public void Save(MerchantRegistration aggregate)
    {
        throw new NotImplementedException();
    }

    public void Remove(MerchantRegistration entity)
    {
        throw new NotImplementedException();
    }

    #endregion
}