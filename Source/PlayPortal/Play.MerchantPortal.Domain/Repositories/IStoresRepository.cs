using Play.MerchantPortal.Domain.Entities;

namespace Play.MerchantPortal.Domain.Persistence;

public interface IStoresRepository : IRepository<StoreEntity>
{
    #region Instance Members

    Task<StoreEntity?> SelectById(long id);

    Task<IEnumerable<StoreEntity>> SelectStoresByMerchant(long merchantId);

    #endregion
}