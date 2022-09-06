using Play.MerchantPortal.Domain.Entities;

namespace Play.MerchantPortal.Application.Contracts.Persistence;

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