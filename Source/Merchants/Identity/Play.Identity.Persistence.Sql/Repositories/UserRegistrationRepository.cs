using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Play.Domain.Common.ValueObjects;
using Play.Identity.Domain.Aggregates;
using Play.Identity.Domain.Repositories;
using Play.Identity.Persistence.Sql.Persistence;
using Play.Persistence.Sql;

namespace Play.Identity.Persistence.Sql.Repositories;

public class UserRegistrationRepository : Repository<UserRegistration, SimpleStringId>, IUserRegistrationRepository
{
    #region Instance Values

    private readonly DbSet<UserRegistration> _Set;

    #endregion

    #region Constructor

    public UserRegistrationRepository(UserIdentityDbContext dbContext, ILogger<UserRegistrationRepository> logger) : base(dbContext, logger)
    {
        _Set = dbContext.Set<UserRegistration>();
    }

    #endregion

    #region Instance Members

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        try
        {
            return await _Set.AnyAsync(a => a.GetEmail() == email).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // logging
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(UserRegistrationRepository)} encountered an exception determining if the email: [{email}] is unique", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task<UserRegistration?> GetByEmailAsync(string email)
    {
        try
        {
            return await _Set.FirstOrDefaultAsync(a => a.GetEmail() == email);
        }
        catch (Exception ex)
        {
            // logging
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(UserRegistrationRepository)} encountered an exception retrieving the {nameof(UserRegistration)} with the Email: [{email}]", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public override UserRegistration? GetById(SimpleStringId id)
    {
        try
        {
            return _DbSet.AsNoTracking()
                .Include("_Address")
                .Include("_Contact")
                .Include("_PersonalDetail")
                .Include("_EmailConfirmation")
                .Include("_SmsConfirmation")
                .FirstOrDefault(a => a.Id == id);
        }
        catch (Exception ex)
        {
            // logging
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(UserRegistrationRepository)} encountered an exception retrieving {nameof(UserRegistration)} with the Identifier: [{id}]", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public override async Task<UserRegistration?> GetByIdAsync(SimpleStringId id)
    {
        try
        {
            return await _DbSet.Include("_Address")
                .Include("_Contact")
                .Include("_PersonalDetail")
                .Include("_EmailConfirmation")
                .Include("_SmsConfirmation")
                .FirstOrDefaultAsync(a => a.Id == id)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // logging
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(UserRegistrationRepository)} encountered an exception retrieving {nameof(UserRegistration)} with the Identifier: [{id}]", ex);
        }
    }

    #endregion
}