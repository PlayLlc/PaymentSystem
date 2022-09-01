using MerchantPortal.Core.Entities;

namespace MerchantPortal.Application.Contracts.Persistence;

public interface IStoresRepository : IRepository<StoreEntity>
{
    Task<StoreEntity> SelectById(long id);

    IEnumerable<StoreEntity> SelectStoresByMerchant(long merchantId);
}
