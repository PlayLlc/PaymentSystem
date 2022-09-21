using Play.Infrastructure.Domain.Entities;

namespace Play.MerchantPortal.Domain.Persistence;

public interface IRepository
{
    #region Instance Members

    Task SaveChangesAsync();

    #endregion
}

public interface IRepository<_> : IRepository where _ : BaseEntity
{
    #region Instance Values

    IQueryable<_> Query { get; }

    #endregion

    #region Instance Members

    _ AddEntity(_ entity);
    void DeleteEntity(_ entity);

    #endregion
}