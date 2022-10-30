using AutoMapper;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Repositories;
using Play.Accounts.Domain.ValueObjects;
using Play.Accounts.Persistence.Sql.Entities;
using Play.Accounts.Persistence.Sql.Persistence;
using Play.Domain.Exceptions;
using Play.Persistence.Sql;

using System.Data;

namespace Play.Accounts.Persistence.Sql.Repositories;

public class UserRepository : IUserRepository
{
    #region Instance Values

    private readonly IMapper _Mapper;

    private readonly UserManager<UserIdentity> _UserManager;
    private readonly DbSet<UserIdentity> _Users;
    private readonly UserIdentityDbContext _Context;

    #endregion

    #region Constructor

    public UserRepository(IMapper mapper, UserManager<UserIdentity> userManager, UserIdentityDbContext context)
    {
        context.ChangeTracker.LazyLoadingEnabled = false;
        _Mapper = mapper;
        _UserManager = userManager;
        _Context = context;
        _Users = context.Users;
    }

    #endregion

    #region Instance Members

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task UpdateUserRoles(string userId, params UserRole[] roles)
    {
        try
        {
            User? user = await GetByIdAsync(userId).ConfigureAwait(false);

            if (user is null)
                throw new NotFoundException(typeof(User));

            UserIdentity userIdentity = _Mapper.Map<UserIdentity>(user);

            await _UserManager.AddToRolesAsync(userIdentity, roles.Select(a => a.Name)).ConfigureAwait(false);
            await _Context.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(UserRepository)} encountered an exception attempting to {nameof(UpdateUserRoles)} for the {nameof(User)} with the Identifier: [{userId}];",
                ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task<bool> IsEmailUnique(string email)
    {
        try
        {
            return await _Context.Set<UserIdentity>().AnyAsync(a => a.Email == email).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(UserRepository)} encountered an exception attempting to invoke {nameof(IsEmailUnique)} for the {nameof(email)}: [{email}];", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task<User?> GetByEmailAsync(string email)
    {
        try
        {
            var userIdentity = await _Users.AsNoTracking().FirstOrDefaultAsync(a => a.Email == email).ConfigureAwait(false);

            return userIdentity is null ? null : new User(userIdentity.AsDto());
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(UserRepository)} encountered an exception attempting to invoke {nameof(GetByEmailAsync)} for the {nameof(email)}: [{email}];",
                ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task<User?> GetByIdAsync(string id)
    {
        try
        {
            var result = await _Users.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);

            return result is null ? null : new User(result.AsDto());
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(UserRepository)} encountered an exception attempting to invoke {nameof(GetByIdAsync)} for the {nameof(id)}: [{id}];", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task SaveAsync(User user)
    {
        try
        {
            UserIdentity userIdentity = new UserIdentity(user.AsDto());
            _Users.Update(userIdentity);
            await _Context.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(UserRepository)} encountered an exception attempting to invoke {nameof(SaveAsync)} for the {nameof(User)} with Id: [{user.GetId()}];",
                ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task RemoveAsync(User user)
    {
        try
        {
            _Users.Remove(new UserIdentity(user.AsDto()));
            await _Context.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(UserRepository)} encountered an exception attempting to invoke {nameof(RemoveAsync)} for the {nameof(User)} with Id: [{user.GetId()}];",
                ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public User? GetById(string id)
    {
        try
        {
            var result = _Users.AsNoTracking().FirstOrDefault(a => a.Id == id);

            return result is null ? null : new User(result.AsDto());
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(UserRepository)} encountered an exception attempting to invoke {nameof(GetById)} for the {nameof(User)} with Id: [{id}];", ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public void Save(User user)
    {
        try
        {
            UserIdentity userIdentity = new UserIdentity(user.AsDto());
            _Users.Update(userIdentity);
            _Context.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(UserRepository)} encountered an exception attempting to invoke {nameof(Save)} for the {nameof(User)} with Id: [{user.GetId()}];",
                ex);
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public void Remove(User user)
    {
        try
        {
            UserIdentity? userIdentity = new UserIdentity(user.AsDto());
            _Users.Remove(userIdentity);
            _Context.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                $"The {nameof(UserRepository)} encountered an exception attempting to invoke {nameof(Remove)} for the {nameof(User)} with Id: [{user.GetId()}];",
                ex);
        }
    }

    #endregion
}