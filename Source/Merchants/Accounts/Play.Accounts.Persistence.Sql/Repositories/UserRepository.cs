using AutoMapper;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Play.Accounts.Domain.Aggregates.Users;
using Play.Accounts.Persistence.Sql.Entities;
using Play.Domain.Aggregates;
using Play.Domain.Repositories;

namespace Play.Accounts.Persistence.Sql.Repositories
{
    public class UserRepository : IRepository<User, string>
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

        public async Task<User?> GetByIdAsync(string id)
        {
            UserIdentity userIdentity = await _UserManager.FindByIdAsync(id).ConfigureAwait(false);

            return _Mapper.Map<User>(userIdentity);
        }

        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="OperationCanceledException"></exception>
        public async Task SaveAsync(User user)
        {
            var userIdentity = _Mapper.Map<UserIdentity>(user);
            await _UserManager.UpdateAsync(userIdentity).ConfigureAwait(false);
            await _Context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="OperationCanceledException"></exception>
        public async Task RemoveAsync(User user)
        {
            var userIdentity = _Mapper.Map<UserIdentity>(user);
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
            var userIdentity = _Mapper.Map<UserIdentity>(user);
            Task.WhenAll(_UserManager.UpdateAsync(userIdentity));
            Task.WhenAll(_Context.SaveChangesAsync());
        }

        public void Remove(User user)
        {
            var userIdentity = _Mapper.Map<UserIdentity>(user);
            Task.WhenAll(_UserManager.DeleteAsync(userIdentity));
            Task.WhenAll(_Context.SaveChangesAsync());
        }

        #endregion
    }
}