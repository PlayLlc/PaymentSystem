using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;
using Play.Inventory.Contracts.Dtos;

namespace Play.Inventory.Domain.Repositories;

public interface ICategoryRepository : IRepository<Category, SimpleStringId>
{
    #region Instance Members

    public Task<bool> DoesCategoryAlreadyExist(SimpleStringId merchantId, Name categoryName);
    public Task<IEnumerable<Category>> GetCategoriesAsync(SimpleStringId merchantId);

    #endregion
}