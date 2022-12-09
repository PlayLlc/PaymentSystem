using Play.Payroll.Domain.Aggregates;
using Play.Payroll.Domain.Entities;

namespace Play.Payroll.Domain.Services;

public interface IRetrieveUsers
{
    #region Instance Members

    public Task<User> GetByIdAsync(string id);
    public User GetById(string id);

    #endregion
}