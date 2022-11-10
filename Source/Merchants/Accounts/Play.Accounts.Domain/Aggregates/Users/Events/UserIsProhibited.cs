using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

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