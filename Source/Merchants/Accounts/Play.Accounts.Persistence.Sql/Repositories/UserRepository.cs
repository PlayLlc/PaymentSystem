using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Accounts.Domain.Aggregates.Users;
using Play.Accounts.Domain.Repositories;

namespace Play.Accounts.Persistence.Sql.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Instance Members

        public Task<User?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(User aggregate)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(User id)
        {
            throw new NotImplementedException();
        }

        public User? GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void Save(User aggregate)
        {
            throw new NotImplementedException();
        }

        public void Remove(User entity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}