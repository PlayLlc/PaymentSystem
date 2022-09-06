using Play.MerchantPortal.Domain.Entities;

namespace Play.MerchantPortal.Application.Contracts.Persistence;

public interface IStoresRepository : IRepository<StoreEntity>
{
    #region Instance Members

    Task<StoreEntity?> SelectById(long id);

    IEnumerable<StoreEntity> SelectStoresByMerchant(long merchantId);

    #endregion
}