using Play.Domain.Common.ValueObjects;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Services;

namespace Play.Inventory.Application.Services;

public class UserRetriever : IRetrieveUsers
{
    #region Instance Members

    private SimpleStringId GetId()
    {
        return new SimpleStringId("=U]Pd:\\W($Q5[XEBi9us");
    }

    private User GetMockUser()
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public User GetById(string id)
    {
        throw new NotImplementedException();
    }

    #endregion
}