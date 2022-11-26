using Play.TimeClock.Domain.Entities;

namespace Play.TimeClock.Domain.Services;

public interface IRetrieveMerchants
{
    #region Instance Members

    public Task<Merchant> GetByIdAsync(string id);
    public Merchant GetById(string id);

    #endregion
}