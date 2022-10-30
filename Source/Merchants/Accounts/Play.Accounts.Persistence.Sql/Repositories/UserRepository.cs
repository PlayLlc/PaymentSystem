using AutoMapper;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Repositories;
using Play.Accounts.Persistence.Sql.Entities;
using Play.Accounts.Persistence.Sql.Persistence;
using Play.Domain.Exceptions;

namespace Play.Accounts.Persistence.Sql.Repositories;

public class UserRepository : IUserRepository
{
    #region Instance Values

    private readonly IMapper _Mapper;

    //private readonly UserManager<UserIdentity> _UserManager;
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

    /// <exception cref="NotFoundException"></exception>
    public async Task UpdateUserRoles(string userId, params UserRole[] roles)
    {
        User? user = await GetByIdAsync(userId).ConfigureAwait(false);

        if (user is null)
            throw new NotFoundException(typeof(User));

        UserIdentity userIdentity = _Mapper.Map<UserIdentity>(user);

        await _UserManager.AddToRolesAsync(userIdentity, roles.Select(a => a.Name)).ConfigureAwait(false);
    }

    /// <exception cref="OperationCanceledException"></exception>
    public async Task<bool> IsEmailUnique(string email)
    {
        return await _Context.Set<UserIdentity>().AnyAsync(a => a.Email == email).ConfigureAwait(false);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        UserIdentity? userIdentity = await _UserManager.FindByEmailAsync(email).ConfigureAwait(false);

        var aa = await _Context.Set<UserIdentity>().AsNoTracking().FirstOrDefaultAsync(a => a.Email == email).ConfigureAwait(false);

        var bb = await _Context.Set<UserIdentity>()
            .AsNoTracking()
            .AsQueryable()
            .FirstOrDefaultAsync(a => a.Email == email)
            .ConfigureAwait(false); //.LoadAsync()

        //  var bb = await _Context.Set<UserIdentity>().AsQueryable().FirstOrDefaultAsync(a => a.Email == email).FirstOrDefaultAsync().ConfigureAwait(false);//.LoadAsync()

        return new User(userIdentity!.AsDto());

        //var test = await _Context.Set<UserIdentity>()
        //    .AsNoTracking()
        //    .Where(a => a.Email == email)
        //    .AsQueryable()
        //    .Include(x => x.Address)
        //    .Include(x => x.Password)
        //    .Include(x => x.PersonalDetail)
        //    .Include(x => x.Contact)
        //    .FirstOrDefaultAsync()
        //    .ConfigureAwait(false);

        //var dto = test.AsDto();

        //return new User(dto);

        //UserIdentity? userIdentity = await _UserManager.FindByEmailAsync(email).ConfigureAwait(false);

        //var dto = userIdentity.AsDto();

        //return new User(userIdentity.AsDto());
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        var result = await _Users.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);

        return result is null ? null : new User(result.AsDto());
    }

    /// <exception cref="DbUpdateException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    public async Task SaveAsync(User user)
    {
        UserIdentity userIdentity = new UserIdentity(user.AsDto());
        _Users.Update(userIdentity);
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