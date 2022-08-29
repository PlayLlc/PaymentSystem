using MerchantPortal.Core.Entities;

namespace MerchantPortal.Application.Contracts.Persistence;

public interface IStoresRepository : IRepository<StoreEntity>
{
    IEnumerable<StoreEntity> SelectStoresByMerchant(long merchantId);
}
