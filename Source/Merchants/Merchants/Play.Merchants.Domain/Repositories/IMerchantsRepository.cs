using Play.Domain;
using Play.Merchants.Domain.Entities;

namespace Play.Merchants.Domain.Repositories;

public interface IMerchantsRepository : IRepository<Merchant>
{
    #region Instance Members

    Task<Merchant?> SelectById(long id);

    #endregion
}