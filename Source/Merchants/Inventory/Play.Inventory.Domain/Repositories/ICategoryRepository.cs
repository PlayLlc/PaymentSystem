using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;

namespace Play.Inventory.Domain.Repositories;

public interface ICategoryRepository : IRepository<Category, SimpleStringId>
{
    #region Instance Members

    public Task<bool> DoesCategoryAlreadyExist(string merchantId, Name categoryName);

    #endregion
}