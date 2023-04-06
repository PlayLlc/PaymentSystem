using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Play.Domain.Common.ValueObjects;
using Play.Persistence.Sql;
using Play.Registration.Domain.Aggregates.MerchantRegistration;
using Play.Registration.Domain.Aggregates.UserRegistration;
using Play.Registration.Persistence.Sql.Persistence;

namespace Play.Registration.Persistence.Sql.Repositories;

public class MerchantRegistrationRepository : Repository<MerchantRegistration, SimpleStringId>
{
    #region Instance Values

    private readonly DbSet<MerchantRegistration> _Set;

    #endregion

    #region Constructor

    public MerchantRegistrationRepository(RegistrationDbContext dbContext, ILogger<MerchantRegistrationRepository> logger) : base(dbContext, logger)
    {
        _Set = dbContext.Set<MerchantRegistration>();
    }

    #endregion

    #region Instance Members

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public override MerchantRegistration? GetById(SimpleStringId id)
    {
        try
        {
            return _DbSet.AsNoTracking().Include("_Address").Include("_BusinessInfo").FirstOrDefault(a => a.Id == id);
        }
        catch (Exception ex)
        {
            // logging
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(MerchantRegistrationRepository)} encountered an exception retrieving {nameof(UserRegistration)} with the Identifier: [{id}]", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public override async Task<MerchantRegistration?> GetByIdAsync(SimpleStringId id)
    {
        try
        {
            return await _DbSet.Include("_Address").Include("_BusinessInfo").FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // logging
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(MerchantRegistrationRepository)} encountered an exception retrieving {nameof(UserRegistration)} with the Identifier: [{id}]", ex);
        }
    }

    #endregion
}