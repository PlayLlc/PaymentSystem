using Microsoft.EntityFrameworkCore;

using Play.Domain;

namespace Play.Persistence.Sql;

public class Repository<_> : IRepository<_> where _ : BaseEntity
{
    #region Instance Values

    protected readonly DbContext _DbContext;

    public IQueryable<_> Query => _DbContext.Set<_>();

    #endregion

    #region Constructor

    public Repository(DbContext dbContext)
    {
        _DbContext = dbContext;
    }

    #endregion

    #region Instance Members

    public _ AddEntity(_ entity)
    {
        return _DbContext.Set<_>().Add(entity).Entity;
    }

    public void DeleteEntity(_ entity)
    {
        _DbContext.Set<_>().Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        int result = await _DbContext.SaveChangesAsync();
    }

    #endregion
}