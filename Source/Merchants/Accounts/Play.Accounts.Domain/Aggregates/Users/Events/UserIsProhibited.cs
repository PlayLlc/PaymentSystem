using Play.Domain.Aggregates;
using Play.Domain.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Accounts.Domain.Aggregates.Users.Events
{
    public record UserIsProhibited : BrokenBusinessRuleDomainEvent<User, string>
    {
        #region Instance Values

        public readonly User User;

        #endregion

        #region Constructor

        public UserIsProhibited(User user, IBusinessRule rule) : base(user, rule)
        {
            User = user;
        }

        #endregion
    }
}