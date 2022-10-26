using Microsoft.EntityFrameworkCore;

using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Repositories;

namespace Play.Accounts.Persistence.Sql.Repositories;

public class PasswordRepository : IPasswordRepository
{
    #region Instance Values

    private readonly DbSet<Password> _Set;

    #endregion

    #region Constructor

    public PasswordRepository(DbContext context)
    {
        _Set = context.Set<Password>();
    }

    #endregion

    #region Instance Members

    /// <exception cref="OperationCanceledException"></exception>
    public async Task<IEnumerable<Password>> GetByIdAsync(string userId)
    {
        return await _Set.Where(a => a.Id == userId).ToArrayAsync().ConfigureAwait(false);
    }

    #endregion
}