using Play.Accounts.Domain.Aggregates.Users;
using Play.Domain.Repositories;

namespace Play.Accounts.Persistence.Sql.Repositories
{
    public class UserRepository : IRepository<User, string>
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