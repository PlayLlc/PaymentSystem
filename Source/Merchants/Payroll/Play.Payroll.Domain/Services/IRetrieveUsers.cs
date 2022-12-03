using Play.Payroll.Domain.Entities;

namespace Play.Loyalty.Domain.Services;

public interface IRetrieveUsers
{
    #region Instance Members

    public Task<User> GetByIdAsync(string id);
    public User GetById(string id);

    #endregion
}