using Play.TimeClock.Domain.Entities;

namespace Play.TimeClock.Domain.Services;

public interface IRetrieveUsers
{
    #region Instance Members

    public Task<User> GetByIdAsync(string id);
    public User GetById(string id);

    #endregion
}