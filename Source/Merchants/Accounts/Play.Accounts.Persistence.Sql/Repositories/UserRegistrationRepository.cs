using Microsoft.EntityFrameworkCore;

using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Repositories;
using Play.Accounts.Persistence.Sql.Persistence;
using Play.Persistence.Sql;

namespace Play.Accounts.Persistence.Sql.Repositories;

public class UserRegistrationRepository : Repository<UserRegistration, string>, IUserRegistrationRepository
{
    #region Constructor

    public UserRegistrationRepository(UserIdentityDbContext dbContext) : base(dbContext)
    { }

    #endregion

    #region Instance Members

    /// <exception cref="OperationCanceledException"></exception>
    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        return await _DbSet.AnyAsync(a => a.GetEmail() == email).ConfigureAwait(false);
    }

    /// <exception cref="OperationCanceledException"></exception>
    public async Task<UserRegistration?> GetByEmailAsync(string email)
    {
        return await _Set.FirstOrDefaultAsync(a => a.GetEmail() == email);
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public override UserRegistration? GetById(string id)
    {
        try
        {
            return _DbSet.AsNoTracking()
                .Include("_Address")
                .Include("_Contact")
                .Include("_PersonalDetail")
                .Include("_EmailConfirmation")
                .Include("_SmsConfirmation")
                .FirstOrDefault(a => a.GetId().Equals(id));
        }
        catch (Exception ex)
        {
            // logging
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(UserRegistrationRepository)} encountered an exception retrieving {nameof(UserRegistration)} with the Identifier: [{id}]", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public override async Task<UserRegistration?> GetByIdAsync(string id)
    {
        try
        {
            return await _DbSet.Include("_Address")
                .Include("_Contact")
                .Include("_PersonalDetail")
                .Include("_EmailConfirmation")
                .Include("_SmsConfirmation")
                .FirstOrDefaultAsync(a => a.GetId()!.Equals(id))
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