using Play.MerchantPortal.Domain.Entities;

namespace Play.MerchantPortal.Domain.Persistence;

public interface IMerchantsRepository : IRepository<Merchant>
{
    #region Instance Members

    Task<Merchant?> SelectById(long id);

    #endregion
}