using Play.Accounts.Domain.Aggregates.UserRegistration;
using Play.Domain.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Accounts.Domain.Aggregates.Users;

namespace Play.Accounts.Domain.Repositories
{
    public interface IUserRepository : IRepository<User, string>
    { }
}