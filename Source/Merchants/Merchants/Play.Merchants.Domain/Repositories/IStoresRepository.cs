using Play.Domain;
using Play.Merchants.Domain.Entities;

namespace Play.Merchants.Domain.Repositories;

public interface IStoresRepository : IRepository<Store>
{
    #region Instance Members

    Task<Store?> SelectById(long id);

    Task<IEnumerable<Store>> SelectStoresByMerchant(long merchantId);

    #endregion
}