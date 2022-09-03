using MerchantPortal.Core.Entities;

namespace Play.MerchantPortal.Application.Contracts.Persistence;

public interface IMerchantsRepository : IRepository<MerchantEntity>
{
    Task<MerchantEntity?> SelectById(long id);
}
