using Play.Accounts.Domain.Aggregates.Merchants;
using Play.Accounts.Domain.Repositories;

namespace Play.Accounts.Persistence.Sql.Repositories;

public class MerchantRepository : IMerchantRepository
{
    #region Instance Members

    public Task<Merchant?> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(Merchant aggregate)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(Merchant id)
    {
        throw new NotImplementedException();
    }

    public Merchant? GetById(string id)
    {
        throw new NotImplementedException();
    }

    public void Save(Merchant aggregate)
    {
        throw new NotImplementedException();
    }

    public void Remove(Merchant entity)
    {
        throw new NotImplementedException();
    }

    #endregion
}