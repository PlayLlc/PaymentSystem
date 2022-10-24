using AutoMapper;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Repositories;
using Play.Accounts.Persistence.Sql.Entities;
using Play.Domain.Exceptions;

namespace Play.Accounts.Persistence.Sql.Repositories;

public class UserRepository : IUserRepository
{
    #region Instance Values

    private readonly IMapper _Mapper;
    private readonly UserManager<UserIdentity> _UserManager;
    private readonly DbContext _Context;

    #endregion

    #region Constructor

    public UserRepository(IMapper mapper, UserManager<UserIdentity> userManager, DbContext context)
    {
        _Mapper = mapper;
        _UserManager = userManager;
        _Context = context;
    }

    #endregion

    #region Instance Members

    /// <exception cref="NotFoundException"></exception>
    public async Task UpdateUserRoles(string userId, params UserRole[] roles)
    {
        User? user = await GetByIdAsync(userId).ConfigureAwait(false);

        if (user is null)
            throw new NotFoundException(typeof(User), userId);

        UserIdentity userIdentity = _Mapper.Map<UserIdentity>(user);

        await _UserManager.AddToRolesAsync(userIdentity, roles.Select(a => a.Value)).ConfigureAwait(false);
    }

    /// <exception cref="OperationCanceledException"></exception>
    public async Task<bool> IsEmailUnique(string email)
    {
        return await _UserManager.Users.AnyAsync(a => a.Email == email).ConfigureAwait(false);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        UserIdentity? userIdentity = await _UserManager.FindByEmailAsync(email).ConfigureAwait(false);

        return userIdentity is null ? null : _Mapper.Map<User>(userIdentity);
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        UserIdentity userIdentity = await _UserManager.FindByIdAsync(id).ConfigureAwait(false);

        return _Mapper.Map<User>(userIdentity);
    }

    /// <exception cref="DbUpdateException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    public async Task SaveAsync(User user)
    {
        UserIdentity? userIdentity = _Mapper.Map<UserIdentity>(user);
        await _UserManager.UpdateAsync(userIdentity).ConfigureAwait(false);
        await _Context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <exception cref="DbUpdateException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    public async Task RemoveAsync(User user)
    {
        UserIdentity? userIdentity = _Mapper.Map<UserIdentity>(user);
        await _UserManager.DeleteAsync(userIdentity).ConfigureAwait(false);
        await _Context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <exception cref="AggregateException"></exception>
    public User? GetById(string id)
    {
        Task<UserIdentity> userTask = _UserManager.FindByIdAsync(id);
        Task.WhenAll(userTask);

        return _Mapper.Map<User>(userTask.Result);
    }

    public void Save(User user)
    {
        UserIdentity? userIdentity = _Mapper.Map<UserIdentity>(user);
        Task.WhenAll(_UserManager.UpdateAsync(userIdentity));
        Task.WhenAll(_Context.SaveChangesAsync());
    }

    public void Remove(User user)
    {
        UserIdentity? userIdentity = _Mapper.Map<UserIdentity>(user);
        Task.WhenAll(_UserManager.DeleteAsync(userIdentity));
        Task.WhenAll(_Context.SaveChangesAsync());
    }

    #endregion
}