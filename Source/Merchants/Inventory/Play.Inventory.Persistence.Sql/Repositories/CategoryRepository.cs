using Play.Domain.Common.ValueObjects;
using Play.Inventory.Domain.Repositories;
using Play.Inventory.Domain;
using Play.Persistence.Sql;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Play.Inventory.Persistence.Sql.Repositories;

public class CategoryRepository : Repository<Category, SimpleStringId>, ICategoryRepository
{
    #region Constructor

    public CategoryRepository(DbContext dbContext, ILogger<CategoryRepository> logger) : base(dbContext, logger)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public override async Task<Category?> GetByIdAsync(SimpleStringId id)
    {
        try
        {
            return await _DbSet.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(ItemRepository)} encountered an exception attempting to invoke {nameof(GetByIdAsync)} for the {nameof(id)}: [{id}];", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public override Category? GetById(SimpleStringId id)
    {
        try
        {
            return _DbSet.AsNoTracking().FirstOrDefault(a => a.Id == id);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(ItemRepository)} encountered an exception attempting to invoke {nameof(GetByIdAsync)} for the {nameof(id)}: [{id}];", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task<bool> DoesCategoryAlreadyExist(string merchantId, Name categoryName)
    {
        try
        {
            return await _DbSet
                .AnyAsync(a => (EF.Property<string>(a, $"_MerchantId") == merchantId) && (EF.Property<string>(a, $"_Name") == categoryName.Value))
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(CategoryRepository)} encountered an exception trying to determine {nameof(DoesCategoryAlreadyExist)} for the specified Merchant",
                ex);
        }
    }

    #endregion
}