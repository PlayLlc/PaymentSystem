using Play.Loyalty.Domain.Entitieses;

namespace Play.Loyalty.Domain.Services;

public interface IRetrieveMerchants
    #region Instance Membersrs

    public Task<Merchant> GetByIdAsync(string id);
    public Merchant GetById(string id

    #endregion

}