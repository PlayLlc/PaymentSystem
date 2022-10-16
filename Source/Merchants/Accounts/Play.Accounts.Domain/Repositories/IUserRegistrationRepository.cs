using Play.Accounts.Domain.Aggregates.Merchants;
using Play.Domain.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Accounts.Domain.Aggregates.UserRegistration;

namespace Play.Accounts.Domain.Repositories
{
    public interface IUserRegistrationRepository : IRepository<UserRegistration, string>
    { }
}