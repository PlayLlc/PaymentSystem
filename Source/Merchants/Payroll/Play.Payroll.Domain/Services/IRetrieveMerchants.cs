using Play.Payroll.Domain.Entities;

namespace Play.Loyalty.Domain.Services;

public interface IRetrieveMerchants
{
    #region Instance Members

    public Task<Merchant> GetByIdAsync(string id);
    public Merchant GetById(string id);

    #endregion
}