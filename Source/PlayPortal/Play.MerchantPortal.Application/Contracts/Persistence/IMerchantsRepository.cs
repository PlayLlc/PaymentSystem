using Play.MerchantPortal.Domain.Entities;

namespace Play.MerchantPortal.Application.Contracts.Persistence;

public interface IMerchantsRepository : IRepository<MerchantEntity>
{
    #region Instance Members

    Task<MerchantEntity?> SelectById(long id);

    #endregion
}