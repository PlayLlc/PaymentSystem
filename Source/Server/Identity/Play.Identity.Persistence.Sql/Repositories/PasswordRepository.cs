using Microsoft.EntityFrameworkCore;

using Play.Identity.Domain.Entities;
using Play.Identity.Domain.Repositories;
using Play.Persistence.Sql;

namespace Play.Identity.Persistence.Sql.Repositories;

public class PasswordRepository : IPasswordRepository
{
    #region Instance Values

    private readonly DbSet<Password> _Set;

    #endregion

    #region Constructor

    public PasswordRepository(DbContext context)
    {
        _Set = context.Set<Password>();
        context.ChangeTracker.LazyLoadingEnabled = false;
    }

    #endregion

    #region Instance Members

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task<IEnumerable<Password>> GetByIdAsync(string userId)
    {
        try
        {
            return await _Set.Where(a => a.Id == userId).ToArrayAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // logging
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(PasswordRepository)} encountered an exception retrieving {nameof(Password)} with the User ID: [{userId}]", ex);
        }
    }

    #endregion
}