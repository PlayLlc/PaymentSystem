using Play.MerchantPortal.Domain.Entities;

namespace Play.MerchantPortal.Domain.Persistence;

public interface IStoresRepository : IRepository<Store>
{
    #region Instance Members

    Task<Store?> SelectById(long id);

    Task<IEnumerable<Store>> SelectStoresByMerchant(long merchantId);

    #endregion
}