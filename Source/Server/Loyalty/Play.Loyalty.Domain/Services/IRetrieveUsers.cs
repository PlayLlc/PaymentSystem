using Play.Loyalty.Domain.Entitieses;

namespace Play.Loyalty.Domain.Services;

public interface IRetrieveUsers
    #region Instance Membersrs

    public Task<User> GetByIdAsync(string id);
    public User GetById(string id

    #endregion

}