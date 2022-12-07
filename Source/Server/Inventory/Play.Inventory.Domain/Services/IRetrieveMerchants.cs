using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Services;

public interface IRetrieveMerchants
{
    #region Instance Members

    public Task<Merchant> GetByIdAsync(string id);
    public Merchant GetById(string id);

    #endregion
}